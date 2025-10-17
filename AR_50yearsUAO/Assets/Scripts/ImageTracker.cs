using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;

/// <summary>
/// Controla un solo objeto RA que se instancia una vez al detectar la primera imagen.
/// Si se detecta una nueva imagen, el objeto se mueve a su posición.
/// No destruye el objeto.
/// Compatible con Unity 6 y AR Foundation 6.
/// </summary>
[RequireComponent(typeof(ARTrackedImageManager))]
public class ImageTracker : MonoBehaviour
{
    [Header("Componentes AR")]
    [SerializeField] private ARTrackedImageManager arManager;

    [Header("Objeto RA único (prefab)")]
    [SerializeField] private GameObject objetoPrefab;

    [Header("UI opcional")]
    [SerializeField] private Canvas infoCanvas;
    [SerializeField] private TMP_Text infoText;

    // Referencia al objeto instanciado actualmente
    private GameObject objetoInstanciado;

    void Awake()
    {
        if (arManager == null)
            arManager = GetComponent<ARTrackedImageManager>();
    }

    void OnEnable() => arManager.trackablesChanged.AddListener(OnTrackedImagesChanged);
    void OnDisable() => arManager.trackablesChanged.RemoveListener(OnTrackedImagesChanged);

    private void OnTrackedImagesChanged(ARTrackablesChangedEventArgs<ARTrackedImage> args)
    {
        foreach (var img in args.added)
            ActualizarObjeto(img);

        foreach (var img in args.updated)
            ActualizarObjeto(img);
    }

    /// <summary>
    /// Instancia el objeto si no existe, o lo mueve si ya fue creado.
    /// </summary>
    private void ActualizarObjeto(ARTrackedImage trackedImage)
    {
        if (objetoPrefab == null)
        {
            Debug.LogWarning("ImageTracker: No se ha asignado un prefab.");
            return;
        }

        // Solo instanciar una vez
        if (objetoInstanciado == null)
        {
            Vector3 posicion = trackedImage.transform.position + new Vector3(0, 0.05f, 0);
            objetoInstanciado = Instantiate(objetoPrefab, posicion, trackedImage.transform.rotation);
            objetoInstanciado.name = objetoPrefab.name;

            Debug.Log($"ImageTracker: Instanciado '{objetoInstanciado.name}' en '{trackedImage.referenceImage.name}'.");
        }
        else
        {
            // Solo moverlo si la imagen cambia o se actualiza
            objetoInstanciado.transform.position = trackedImage.transform.position + new Vector3(0, 0.05f, 0);
            objetoInstanciado.transform.rotation = trackedImage.transform.rotation;
        }

        // Mostrar información opcional en la UI
        if (infoText != null)
            infoText.text = $"Imagen detectada: {trackedImage.referenceImage.name}";
    }

    /// <summary>
    /// Permite simular detección manual desde el editor.
    /// </summary>
    public void SimularDeteccion()
    {
        if (objetoPrefab == null)
        {
            Debug.LogWarning("Simulación: No se ha asignado un prefab.");
            return;
        }

        if (objetoInstanciado == null)
        {
            objetoInstanciado = Instantiate(objetoPrefab, objetoPrefab.transform.position, objetoPrefab.transform.rotation);
            objetoInstanciado.name = objetoPrefab.name;
            Debug.Log($"[Simulación] Objeto instanciado en '{objetoPrefab.name}'.");
        }
        else
        {
            Debug.Log("[Simulación] El objeto ya está instanciado.");
        }
    }
}
