using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

/// <summary>
/// Gestiona el reconocimiento de planos horizontales en RA.
/// Permite activarlo o desactivarlo, y al detectar un plano horizontal,
/// instancia un objeto seleccionado de una lista (una sola vez).
/// Se asume que el prefab instanciado ya contiene un componente
/// SimulatedTrackedImage configurado para XR Simulation.
/// </summary>
[RequireComponent(typeof(ARPlaneManager))]
public class PlaneTracker : MonoBehaviour
{
    [Header("Configuración del Sistema AR")]
    private ARPlaneManager planeManager;

    [Header("Prefabs disponibles para instanciar")]
    [Tooltip("Lista de objetos que pueden ser instanciados al detectar un plano.")]
    public List<GameObject> objetosDisponibles = new List<GameObject>();

    [Header("Índice del objeto a instanciar")]
    [Tooltip("Selecciona el índice del objeto en la lista para instanciar.")]
    [Range(0, 10)]
    public int indiceObjeto = 0;

    [Header("Opciones de reconocimiento")]
    [Tooltip("Si está activo, buscará planos y podrá colocar objetos.")]
    public bool reconocimientoActivo = true;

    // Control interno
    private bool objetoInstanciado = false;

    private void Awake()
    {
        planeManager = GetComponent<ARPlaneManager>();
    }

    private void OnEnable()
    {
        planeManager.trackablesChanged.AddListener(OnPlanesChanged);
    }

    private void OnDisable()
    {
        planeManager.trackablesChanged.RemoveListener(OnPlanesChanged);
    }

    /// <summary>
    /// Se ejecuta cada vez que se detectan, actualizan o eliminan planos.
    /// </summary>
    private void OnPlanesChanged(ARTrackablesChangedEventArgs<ARPlane> args)
    {
        if (!reconocimientoActivo || objetoInstanciado || objetosDisponibles.Count == 0)
            return;

        foreach (var plano in args.added)
        {
            if (plano.alignment == PlaneAlignment.HorizontalUp)
            {
                InstanciarObjeto(plano);
                break;
            }
        }
    }

    /// <summary>
    /// Instancia el objeto seleccionado sobre el centro del plano detectado.
    /// </summary>
    private void InstanciarObjeto(ARPlane plano)
    {
        if (indiceObjeto < 0 || indiceObjeto >= objetosDisponibles.Count)
        {
            Debug.LogWarning("Índice de objeto fuera de rango.");
            return;
        }

        Vector3 posicion = plano.center;
        GameObject prefab = objetosDisponibles[indiceObjeto];

        GameObject instancia = Instantiate(prefab, posicion, Quaternion.identity);
        instancia.name = prefab.name;
        string name = prefab.name;
        Debug.Log($"Objeto '{instancia.name}' instanciado sobre el plano.");
        switch (name)
        {
            case "Stand1":
                name = "1970-1";
                break;
            case "Stand2":
                name = "1980-1";
                break;
            case "Stand3":
                name = "1990-1";
                break;
            case "Stand4":
                name = "2000-1";
                break;
            case "Stand5":
                name = "2010-1";
                break;
        }
        // El prefab ya tiene su SimulatedTrackedImage
        // XR Simulation lo detectará automáticamente
        objetoInstanciado = true;
        // Dentro de PlaneTracker.cs al instanciar
        var imageTracker = gameObject.GetComponent<ImageTracker>();
        if (imageTracker != null)
        {
            imageTracker.SimularDeteccion(name, instancia.transform);
            Debug.Log("funciona");
        }

        //desactivar la detección de planos después de colocar
        //ToggleReconocimiento(false);
    }

    /// <summary>
    /// Activa o desactiva el reconocimiento de planos y visibilidad de los detectados.
    /// </summary>
    public void ToggleReconocimiento(bool estado)
    {
        reconocimientoActivo = estado;
        planeManager.requestedDetectionMode = estado
            ? PlaneDetectionMode.Horizontal
            : PlaneDetectionMode.None;

        foreach (var plano in planeManager.trackables)
            plano.gameObject.SetActive(estado);

        Debug.Log($"Reconocimiento de planos: {(estado ? "Activado" : "Desactivado")}");
    }
}
