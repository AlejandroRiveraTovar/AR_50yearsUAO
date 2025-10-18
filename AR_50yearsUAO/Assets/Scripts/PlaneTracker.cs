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
    public GameObject guia;
    [Header("Índice del objeto a instanciar")]
    [Tooltip("Selecciona el índice del objeto en la lista para instanciar.")]
    [Range(0, 10)]
    public int indiceObjeto = 0;


    [Header("Opciones de reconocimiento")]
    [Tooltip("Si está activo, buscará planos y podrá colocar objetos.")]
    public bool reconocimientoActivo = true;

    // Control interno
    private bool objetoInstanciado = false;
    private GameObject objetoActual;
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
    /// Instancia el objeto seleccionado sobre el centro del plano detectado,
    /// con una pequeña elevación para evitar que se hunda en el plano.
    /// </summary>
    private void InstanciarObjeto(ARPlane plano)
    {
        if (indiceObjeto < 0 || indiceObjeto >= objetosDisponibles.Count)
        {
            Debug.LogWarning("Índice de objeto fuera de rango.");
            return;
        }

        // Si ya hay un objeto, lo destruimos antes de crear otro
        if (objetoActual != null)
            Destroy(objetoActual);

        // Altura de elevación sobre el plano (ajustable)
        float altura = 0.1f; // 10 cm por encima
        Vector3 posicion = plano.center + Vector3.up * altura;

        GameObject prefab = objetosDisponibles[indiceObjeto];
        objetoActual = Instantiate(prefab, posicion, Quaternion.identity);
        objetoActual.name = prefab.name;

        // Ajuste opcional según el nombre del prefab
        string name = prefab.name;
        switch (name)
        {
            case "Stand1": name = "1970-1"; break;
            case "Stand2": name = "1980-1"; break;
            case "Stand3": name = "1990-1"; break;
            case "Stand4": name = "2000-1"; break;
            case "Stand5": name = "2010-1"; break;
        }

        // Instancia la guía un poco más arriba para evitar solapamiento visual
        Instantiate(guia, posicion + Vector3.up * 0.05f, Quaternion.identity);

        reconocimientoActivo = false;
        Debug.Log($"Objeto '{objetoActual.name}' instanciado sobre el plano a {altura} m de altura.");
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

    public void nextStand() 
    {
        indiceObjeto++;
    }

    public void previusStand()
    {
        indiceObjeto--;
    }
    private void Update()
    {
        if(indiceObjeto > 4) 
        {
            indiceObjeto = 4;
        }
        if (indiceObjeto < 0)
        {
            indiceObjeto = 0;
        }
    }

}
