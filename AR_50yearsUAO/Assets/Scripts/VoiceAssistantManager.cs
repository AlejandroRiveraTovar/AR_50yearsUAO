using UnityEngine;
using System.Collections;
using StarterAssets;


public class VoiceAssistantManager : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private ARAssistantController assistantController;
    [SerializeField] private SpeechTextManager speechManager;

    [Header("Mensajes de Historia por Época")]
    [SerializeField] private string historia1970 = " La historia de la Universidad Autónoma de Occidente comenzó a forjarse a finales de la década de 1960, en un contexto de transformación educativa y social en Colombia. Un grupo de visionarios —profesionales, educadores y empresarios del Valle del Cauca— identificó la necesidad de crear una institución universitaria moderna, abierta a la innovación y capaz de ofrecer oportunidades a una nueva generación de jóvenes con deseos de progreso. En 1970, ese sueño tomó forma con la fundación de la Corporación Autónoma de Occidente, la cual sentó las bases de lo que más tarde se consolidaría como la Universidad Autónoma de Occidente.Desde sus inicios, la institución asumió el compromiso de brindar una educación de calidad, con un enfoque humanista y una marcada responsabilidad social.Su propósito era formar profesionales competentes y conscientes de su entorno, capaces de contribuir activamente al desarrollo regional. Durante sus primeros años, la universidad operó en instalaciones modestas, pero con una gran visión institucional. Se comenzaron a diseñar los primeros programas académicos y se establecieron lineamientos claros sobre la docencia, la investigación y la extensión como ejes fundamentales de su labor educativa. Este período se caracterizó por la búsqueda de autonomía, no solo en su nombre, sino también en su filosofía: la independencia académica y la libertad de pensamiento fueron pilares esenciales de su identidad. A lo largo de la década, la institución fue ganando reconocimiento en la ciudad de Cali y en la región del suroccidente colombiano.Su modelo educativo se destacó por promover el pensamiento crítico, el liderazgo y la creatividad, en un ambiente que fomentaba tanto el rigor académico como la participación activa de los estudiantes en proyectos comunitarios. El espíritu pionero de la UAO también se manifestó en la incorporación temprana de la tecnología y los nuevos métodos de enseñanza, con un enfoque interdisciplinario que integraba las ciencias, la comunicación y la ingeniería.Así, poco a poco, la universidad se posicionó como un espacio de formación integral, orientado a preparar a los jóvenes para los desafíos de una sociedad en constante cambio. Hacia el final de la década de 1970, la Universidad Autónoma de Occidente ya había consolidado sus cimientos institucionales.Su crecimiento, aunque progresivo, fue firme, sostenido por la convicción de que la educación debía ser un instrumento de transformación social.El compromiso, la pasión y la visión de sus fundadores permitieron que lo que comenzó como una idea se convirtiera en una realidad sólida, marcando el inicio de una trayectoria que, con el tiempo, la posicionaría como una de las universidades más importantes del país.";
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