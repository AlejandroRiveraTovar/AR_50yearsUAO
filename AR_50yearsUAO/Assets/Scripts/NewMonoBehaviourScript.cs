using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

/// <summary>
/// Controla el recorrido AR para un único stand activo.
/// - Busca los puntos dentro del stand instanciado.
/// - Recorre los puntos automáticamente.
/// - Cambia de forma (prefab) en cada punto.
/// - Espera al jugador antes de avanzar al siguiente.
/// - Se destruye junto con el stand.
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
    [Tooltip("Velocidad de desplazamiento del guía entre puntos.")]
    public float moveSpeed = 0.5f;
    [Tooltip("Distancia mínima para considerar que llegó al punto.")]
    public float arrivalThreshold = 0.05f;

    [Header("Transformación")]
    [Tooltip("Lista de prefabs que el guía instanciará o alternará al llegar a cada punto.")]
    public GameObject transformedPrefab;

    [Header("Jugador")]
    [Tooltip("Transform del jugador que se moverá en la escena.")]
    public Transform playerTransform;
    [Tooltip("Distancia a la que el jugador debe acercarse para que desaparezca el objeto transformado.")]
    public float disappearDistance = 0.7f;

    [Header("Trail")]
    public TrailRenderer trailRenderer;

    [SerializeField] private List<Transform> routePoints = new List<Transform>();
    private int currentPointIndex = 0;
    private bool moving = false;
    private bool transformed = false;
    private GameObject currentStand;
    private GameObject currentFormInstance;
    private GameObject transformedInstance;

    void Start()
    {
        if (trailRenderer == null)
            trailRenderer = GetComponent<TrailRenderer>();

        StartCoroutine(FindActiveStandAndRoute());
    }

    /// <summary>
    /// Espera un breve tiempo y luego busca el stand activo y sus puntos de ruta ordenados correctamente.
    /// </summary>
    private IEnumerator FindActiveStandAndRoute()
    {
        yield return new WaitForSeconds(0.5f); // espera a que el stand sea instanciado
        FindRoutePoints();

        if (routePoints.Count > 0)
            MoveToNextPoint();
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

        // Busca los puntos dentro del stand actual
        Transform[] allChildren = currentStand.GetComponentsInChildren<Transform>(true);
        foreach (Transform child in allChildren)
        {
            if (child.CompareTag(pointTag) || child.name.Contains(pointNameContains))
                routePoints.Add(child);
        }
        // Ordena los puntos por número al final del nombre (RutaPoint1 → 1, RutaPoint12 → 12)
        routePoints = routePoints
            .OrderBy(p => ExtractRouteNumber(p.name))
            .ToList();

        Debug.Log($"ARPathGuideSingleStand: {routePoints.Count} puntos encontrados y ordenados correctamente en '{currentStand.name}'.");
    }

    /// <summary>
    /// Extrae el número al final del nombre del punto (RutaPoint12 → 12).
    /// </summary>
    private int ExtractRouteNumber(string name)
    {
        Match match = Regex.Match(name, @"(\d+)$");
        return match.Success ? int.Parse(match.Value) : 0;
    }

    void Update()
    {
        if (moving && !transformed)
        {
            MoveTowardsTarget();
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
    /// Cuando llega al punto, se transforma temporalmente y oculta visualmente el guía.
    /// </summary>
    private void OnReachedDestination()
    {
        moving = false;

        if (trailRenderer != null)
            trailRenderer.emitting = false;

        if (transformedPrefab != null)
        {
            // Instancia el objeto transformado
            transformedInstance = Instantiate(transformedPrefab, transform.position, Quaternion.identity);
            transformedInstance.transform.SetParent(currentStand.transform);
        }

        transformed = true;

        // Oculta visualmente el guía (pero no desactiva el script)
        foreach (var r in GetComponentsInChildren<Renderer>())
            r.enabled = false;

        if (trailRenderer != null)
            trailRenderer.emitting = false;

        // Espera antes de continuar
        StartCoroutine(WaitBeforeContinue(5f));
    }

    /// <summary>
    /// Espera un tiempo fijo después de transformarse antes de continuar al siguiente punto.
    /// </summary>
    private IEnumerator WaitBeforeContinue(float waitTime = 2f)
    {
        yield return new WaitForSeconds(waitTime);

        // Destruye la forma transformada
        if (transformedInstance != null)
        {
            Destroy(transformedInstance);
            transformedInstance = null;
        }

        transformed = false;
        currentPointIndex++;

        // Reactiva visualmente el guía
        foreach (var r in GetComponentsInChildren<Renderer>())
            r.enabled = true;

        if (trailRenderer != null)
            trailRenderer.emitting = true;

        yield return new WaitForSeconds(0.5f);

        // Si aún hay puntos, continúa el movimiento
        if (currentPointIndex < routePoints.Count)
        {
            moving = true;
            MoveToNextPoint();
        }
        else
        {
            Debug.Log("ARPathGuideSingleStand: Recorrido finalizado. Destruyendo guía...");
            Destroy(gameObject);
        }
    }
    private float standCheckDelay = 1f;
    private float standCheckTimer = 0f;

    private void LateUpdate()
    {
        if (currentStand == null)
        {
            standCheckTimer += Time.deltaTime;
            if (standCheckTimer >= standCheckDelay)
            {
                Debug.LogWarning("ARPathGuide: Stand no encontrado, destruyendo guía.");
                Destroy(gameObject);
            }
        }
        else
        {
            standCheckTimer = 0f;
        }
    }

}
