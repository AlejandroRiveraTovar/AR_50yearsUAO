using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Controla un recorrido educativo en RA basado en imágenes.
/// Cada imagen (stand) activa contenido diferente (modelo, video, texto, escena, etc.).
/// Compatible con Unity 6 y AR Foundation 6.
/// </summary>
[RequireComponent(typeof(ARTrackedImageManager))]
public class ImageTracker : MonoBehaviour
{
    [Header("Componentes AR")]
    [SerializeField] private ARTrackedImageManager arManager;

    [Header("Contenidos por stand")]
    [SerializeField] private GameObject[] contenidosRA;

    [Header("UI opcional")]
    [SerializeField] private Canvas infoCanvas;
    [SerializeField] private TMP_Text infoText;

    private Dictionary<string, GameObject> contenidoActivo = new();
    private Dictionary<string, bool> mostrado = new();

    void Awake()
    {
        if (arManager == null)
            arManager = GetComponent<ARTrackedImageManager>();
    }

    void Start()
    {
        // Instanciamos los contenidos pero los ocultamos al inicio
        foreach (var prefab in contenidosRA)
        {
            var instancia = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            instancia.name = prefab.name;
            instancia.SetActive(false);
            contenidoActivo.Add(prefab.name, instancia);
            mostrado.Add(prefab.name, false);
        }

        if (infoCanvas != null)
            infoCanvas.enabled = true;
    }

    void OnEnable() => arManager.trackablesChanged.AddListener(OnTrackedImagesChanged);
    void OnDisable() => arManager.trackablesChanged.RemoveListener(OnTrackedImagesChanged);

    private void OnTrackedImagesChanged(ARTrackablesChangedEventArgs<ARTrackedImage> args)
    {
        foreach (var img in args.added)
            MostrarContenido(img);

        foreach (var img in args.updated)
            MostrarContenido(img);
    }

    /// <summary>
    /// Muestra o actualiza el contenido según la imagen detectada.
    /// </summary>
    private void MostrarContenido(ARTrackedImage trackedImage)
    {
        string nombre = trackedImage.referenceImage.name;

        if (!contenidoActivo.ContainsKey(nombre))
        {
            Debug.LogWarning($"No hay contenido asignado para: {nombre}");
            return;
        }

        GameObject contenido = contenidoActivo[nombre];
        contenido.transform.position = trackedImage.transform.position;
        contenido.transform.rotation = trackedImage.transform.rotation;

        // Si no se había mostrado antes, lo activamos y mostramos información
        if (!mostrado[nombre])
        {
            contenido.SetActive(true);
            mostrado[nombre] = true;
            Debug.Log($"Contenido activado para: {nombre}");

            //MostrarInfoEnUI(nombre);
            //EjecutarAccionEspecial(nombre);
        }
    }


    public void SimularDeteccion(string nombre, Transform ubicacion)
    {
        if (!contenidoActivo.ContainsKey(nombre)) return;

        GameObject contenido = contenidoActivo[nombre];
        contenido.transform.position = ubicacion.position;
        contenido.transform.rotation = ubicacion.rotation;
        contenido.SetActive(true);
        Debug.Log($"[Simulación manual] Imagen detectada: {nombre}");
    }

    /// <summary>
    /// Muestra texto o descripción educativa según el stand.
    /// </summary>
    private void MostrarInfoEnUI(string nombre)
    {
        if (infoText == null) return;

        string descripcion = nombre switch
        {
            "Stand1_Biodiversidad" => "Descubre la biodiversidad local y cómo proteger los ecosistemas.",
            "Stand2_Agua" => "Aprende sobre el ciclo del agua y su conservación.",
            "Stand3_Energía" => "Explora fuentes de energía renovable y su impacto.",
            "Stand4_Residuos" => "Comprende cómo separar residuos y reciclar correctamente.",
            _ => "Contenido educativo RA"
        };

        infoText.text = descripcion;
    }

    /// <summary>
    /// Ejecuta una acción especial (animación, escena, sonido) según el stand detectado.
    /// </summary>
    private void EjecutarAccionEspecial(string nombre)
    {
        switch (nombre)
        {
            case "Stand1_Biodiversidad":
                // Animación de crecimiento de plantas
                contenidoActivo[nombre].GetComponent<Animator>()?.SetTrigger("Crecimiento");
                break;

            case "Stand2_Agua":
                // Reproducir sonido de agua
                contenidoActivo[nombre].GetComponent<AudioSource>()?.Play();
                break;

            case "Stand3_Energía":
                // Mostrar partículas o efectos
                ParticleSystem ps = contenidoActivo[nombre].GetComponentInChildren<ParticleSystem>();
                if (ps != null) ps.Play();
                break;

            case "Stand4_Residuos":
                // Cambiar de escena al final del recorrido
                SceneManager.LoadScene("EscenaFinal");
                break;
        }
    }
}

    

