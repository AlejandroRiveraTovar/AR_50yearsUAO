using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Controla el recorrido AR para un único stand activo.
/// - Busca los puntos dentro del stand instanciado.
/// - Recorre los puntos automáticamente.
/// - Se destruye junto con el stand.
/// - Espera al jugador antes de avanzar al siguiente punto.
/// </summary>
public class ARPathGuideSingleStand : MonoBehaviour
{
    [Header("Configuración")]
    [Tooltip("Etiqueta que usa el prefab del stand instanciado.")]
    public string standTag = "Stand";
    [Tooltip("Etiqueta que identifica los puntos de la ruta dentro del stand.")]
    public string pointTag = "RutaPoint";


    [Tooltip("Fragmento del nombre que identifica los puntos de ruta dentro del stand.")]
    public string pointNameContains = "RutaPoint";

    [Header("Movimiento")]
    public float moveSpeed = 0.5f;
    public float arrivalThreshold = 0.05f;

    [Header("Transformación")]
    public GameObject transformedPrefab;

    [Header("Jugador")]
    public Transform playerTransform;
    public float disappearDistance = 0.7f;

    [Header("Trail")]
    public TrailRenderer trailRenderer;

    [SerializeField] private List<Transform> routePoints = new List<Transform>();
    private int currentPointIndex = 0;
    private bool moving = false;
    private bool transformed = false;
    private GameObject transformedInstance;
    [SerializeField] private GameObject currentStand;

    void Start()
    {
        if (trailRenderer == null)
            trailRenderer = GetComponent<TrailRenderer>();

        StartCoroutine(FindActiveStandAndRoute());
    }

    /// <summary>
    /// Espera un breve tiempo y luego busca el stand activo y sus puntos de ruta.
    /// </summary>
    private IEnumerator FindActiveStandAndRoute()
    {
        yield return new WaitForSeconds(0.5f); // espera a que el stand sea instanciado
        FindRoutePoints();

        if (routePoints.Count > 0)
        {
            MoveToNextPoint();
        }
    }

    /// <summary>
    /// Busca el único stand activo y obtiene sus puntos de recorrido.
    /// </summary>
    private void FindRoutePoints()
    {
        routePoints.Clear();

        currentStand = GameObject.FindGameObjectWithTag(standTag);
        if (currentStand == null)
        {
            Debug.LogWarning("ARPathGuideSingleStand: No hay ningún stand activo.");
            return;
        }

        // Busca todos los objetos con la etiqueta del punto de ruta dentro del stand activo
        Transform[] allChildren = currentStand.GetComponentsInChildren<Transform>(true);
        foreach (Transform child in allChildren)
        {
            if (child.CompareTag(pointTag)) // Usa etiqueta en lugar de nombre parcial
            {
                routePoints.Add(child);
            }
        }

        // Ordena por nombre si los puntos siguen una secuencia (ej. RutaPoint1, RutaPoint2, ...)
        routePoints = routePoints.OrderBy(p => p.name).ToList();

        Debug.Log($"ARPathGuideSingleStand: Encontrados {routePoints.Count} puntos con tag '{pointTag}' en el stand '{currentStand.name}'.");
    }


    void Update()
    {
        if (moving && !transformed)
        {
            MoveTowardsTarget();
        }
        else if (transformed && transformedInstance != null)
        {
            CheckPlayerDistance();
        }
    }

    /// <summary>
    /// Inicia el movimiento hacia el siguiente punto.
    /// </summary>
    private void MoveToNextPoint()
    {
        if (currentPointIndex >= routePoints.Count)
        {
            Debug.Log("ARPathGuideSingleStand: Recorrido completado.");
            return;
        }

        moving = true;
        transformed = false;

        if (trailRenderer != null)
            trailRenderer.emitting = true;
    }

    /// <summary>
    /// Desplaza el objeto guía hacia el punto actual.
    /// </summary>
    private void MoveTowardsTarget()
    {
        Transform target = routePoints[currentPointIndex];
        if (target == null) return;

        transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) <= arrivalThreshold)
        {
            OnReachedDestination();
        }
    }

    /// <summary>
    /// Cuando llega al punto, se transforma.
    /// </summary>
    private void OnReachedDestination()
    {
        moving = false;

        if (trailRenderer != null)
            trailRenderer.emitting = false;

        if (transformedPrefab != null)
        {
            transformedInstance = Instantiate(transformedPrefab, transform.position, Quaternion.identity);
            transformedInstance.transform.SetParent(currentStand.transform);
            gameObject.SetActive(false); // Oculta el objeto guía
        }

        transformed = true;
    }

    /// <summary>
    /// Espera que el jugador se acerque al objeto transformado.
    /// </summary>
    private void CheckPlayerDistance()
    {
        if (playerTransform == null || transformedInstance == null) return;

        float distance = Vector3.Distance(playerTransform.position, transformedInstance.transform.position);
        if (distance <= disappearDistance)
        {
            Destroy(transformedInstance);
            transformed = false;
            currentPointIndex++;
            StartCoroutine(ContinueAfterDelay(0.5f));
        }
    }

    /// <summary>
    /// Espera antes de avanzar al siguiente punto o destruirse.
    /// </summary>
    private IEnumerator ContinueAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (currentPointIndex < routePoints.Count)
        {
            gameObject.SetActive(true);
            MoveToNextPoint();
        }
        else
        {
            Debug.Log("ARPathGuideSingleStand: Recorrido finalizado. Destruyendo guía junto con el stand...");
            Destroy(gameObject); // Destruye la guía
        }
    }

    /// <summary>
    /// Si el stand es destruido, la guía también.
    /// </summary>
    private void LateUpdate()
    {
        if (currentStand == null)
        {
            //Destroy(gameObject);
        }
    }
}
