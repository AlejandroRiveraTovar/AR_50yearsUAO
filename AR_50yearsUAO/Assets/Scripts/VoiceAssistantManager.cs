using UnityEngine;
using System.Collections;
using StarterAssets;


public class VoiceAssistantManager : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private ARAssistantController assistantController;
    [SerializeField] private SpeechTextManager speechManager;

    [Header("Mensajes de Historia por Época")]
    [SerializeField] private string historia1970 = "";
    [SerializeField] private string historia1980 = "";
    [SerializeField] private string historia1990 = "";
    [SerializeField] private string historia2000 = "";
    [SerializeField] private string historia2010 = "";

    [Header("Configuración")]
    [SerializeField] private float tiempoEsperaNarrar = 1f;

    private bool narrando = false;

    private void Start()
    {
        if (assistantController == null)
        {
            assistantController = FindObjectOfType<ARAssistantController>();
        }

        if (speechManager == null)
        {
            speechManager = FindObjectOfType<SpeechTextManager>();
        }
    }

    // ========== MÉTODOS PÚBLICOS PARA CONECTAR CON VOICE COMMANDS ==========

    /// <summary>
    /// Ir a la época de 1970
    /// </summary>
    public void IrA1970()
    {
        IrAEpocaYNarrar(1970, historia1970);
    }

    /// <summary>
    /// Ir a la época de 1980
    /// </summary>
    public void IrA1980()
    {
        IrAEpocaYNarrar(1980, historia1980);
    }

    /// <summary>
    /// Ir a la época de 1990
    /// </summary>
    public void IrA1990()
    {
        IrAEpocaYNarrar(1990, historia1990);
    }

    /// <summary>
    /// Ir a la época de 2000
    /// </summary>
    public void IrA2000()
    {
        IrAEpocaYNarrar(2000, historia2000);
    }

    /// <summary>
    /// Ir a la época de 2010
    /// </summary>
    public void IrA2010()
    {
        IrAEpocaYNarrar(2010, historia2010);
    }

    /// <summary>
    /// Detener la narración actual
    /// </summary>
    public void DetenerNarracion()
    {
        StopAllCoroutines();
        narrando = false;

        if (speechManager != null)
        {
            speechManager.StopSpeaking();
        }

        if (assistantController != null)
        {
            assistantController.Señalar(false);
        }
    }

    /// <summary>
    /// Iniciar el recorrido desde el principio
    /// </summary>
    public void IniciarRecorrido()
    {
        StartCoroutine(RecorridoCompleto());
    }

    // ========== MÉTODOS PRIVADOS ==========

    private void IrAEpocaYNarrar(int año, string historia)
    {
        if (assistantController == null || speechManager == null)
        {
            Debug.LogError("Faltan referencias en VoiceAssistantManager");
            return;
        }

        // Detener cualquier narración anterior
        DetenerNarracion();

        // Iniciar el proceso
        StartCoroutine(ProcesoIrYNarrar(año, historia));
    }

    private IEnumerator ProcesoIrYNarrar(int año, string historia)
    {
        narrando = true;

        // 1. Mover al asistente hacia la época
        assistantController.IrAEpoca(año);

        // 2. Esperar a que llegue al destino
        while (assistantController.EstaMoviendo())
        {
            yield return null;
        }

        // 3. Pequeña pausa antes de señalar
       //yield return new WaitForSeconds(tiempoEsperaSeñalar);

        // 4. Señalar si está configurado
        //if (señalarDuranteNarracion)
        {
            assistantController.Señalar(true);
        }

        // 5. Iniciar la narración
        if (speechManager != null)
        {
            speechManager.StartSpeaking(historia);
        }

        // 6. Esperar a que termine de hablar
        // Como TextToSpeech tiene callback onDoneCallback, 
        // podríamos esperar a que termine, pero por ahora estimamos el tiempo
        float duracionEstimada = EstimarDuracionTexto(historia);
        yield return new WaitForSeconds(duracionEstimada);

        // 7. Bajar el brazo al terminar
        //if (señalarDuranteNarracion)
        {
            assistantController.Señalar(false);
        }

        narrando = false;
    }

    private IEnumerator RecorridoCompleto()
    {
        // Recorrido automático por todas las épocas
        yield return ProcesoIrYNarrar(1970, historia1970);
        yield return new WaitForSeconds(2f);

        yield return ProcesoIrYNarrar(1980, historia1980);
        yield return new WaitForSeconds(2f);

        yield return ProcesoIrYNarrar(1990, historia1990);
        yield return new WaitForSeconds(2f);

        yield return ProcesoIrYNarrar(2000, historia2000);
        yield return new WaitForSeconds(2f);

        yield return ProcesoIrYNarrar(2010, historia2010);

        Debug.Log("Recorrido completo finalizado");
    }

    private float EstimarDuracionTexto(string texto)
    {
        // Estimar ~3 palabras por segundo en español
        int palabras = texto.Split(' ').Length;
        float duracion = palabras / 3f;
        return Mathf.Max(duracion, 2f); // Mínimo 2 segundos
    }

    // ========== MÉTODOS PARA DEBUGGING ==========

    public bool EstaNavegando()
    {
        return assistantController != null && assistantController.EstaMoviendo();
    }

    public bool EstaNarrando()
    {
        return narrando;
    }

    public int GetEpocaActual()
    {
        return assistantController != null ? assistantController.GetEpocaActual() : 0;
    }
}