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
    [SerializeField] private string historia1980 = "La década de 1980 marcó un punto de inflexión en la historia de la Universidad Autónoma de Occidente. Fue un período de transformación profunda, donde la institución pasó de ser una organización en formación a una universidad madura, estructurada y con visión estratégica. En un contexto nacional de cambios sociales y educativos, la UAO asumió el desafío de profesionalizar su gestión, fortalecer su calidad académica y planificar su desarrollo a largo plazo. Durante estos años, se sentaron las bases de una gestión moderna: se implementaron procesos administrativos y académicos sistemáticos, se crearon los primeros planes de desarrollo institucional y se adoptó una cultura organizacional centrada en la excelencia, la planificación y la innovación.Fue una década dedicada a organizar el presente y pensar el futuro. Entre 1980 y 1982, la universidad inició un proceso de estructuración interna sin precedentes. Bajo el lema “Organizar el presente y pensar el futuro”, se establecieron sistemas de planeación estratégica, procedimientos administrativos más sólidos y estructuras académicas que garantizaron estabilidad y crecimiento. La UAO fortaleció sus programas existentes, modernizó sus laboratorios y adoptó las primeras tecnologías educativas, reflejando su interés por la formación práctica y colaborativa.Las imágenes de estudiantes trabajando en equipo y utilizando equipos de cómputo primitivos para la época son testimonio de este espíritu innovador. El año 1983 se convirtió en un hito histórico para la UAO: el reconocimiento oficial de su autonomía universitaria.Este logro transformó profundamente a la institución, dándole la capacidad de autogobernarse, crear sus propias políticas académicas y definir su rumbo de manera independiente. Más que un cambio jurídico, la autonomía significó una mayor responsabilidad institucional, impulsando el fortalecimiento de los sistemas de gestión, control y planeación.Las ceremonias y actos conmemorativos de la época reflejaron el orgullo de toda la comunidad universitaria ante la consolidación de su identidad autónoma. 1983 a 1987: Consolidación Académica y Expansión Institucional Durante este período, la universidad vivió una etapa de expansión integral. Se fortalecieron los laboratorios, se amplió la oferta de programas académicos y se estrecharon los vínculos con el sector productivo. La docencia adoptó enfoques más participativos, con énfasis en el aprendizaje activo y el trabajo en equipo. La matrícula estudiantil creció de manera sostenida, y con ella, la complejidad administrativa y académica.En paralelo, la incorporación de tecnología computacional marcó un cambio decisivo: se adquirieron nuevos equipos, se formó al profesorado en su uso y se crearon espacios especializados que modernizaron la enseñanza. La UAO también promovió una formación integral que abarcaba la cultura, el deporte y la acción social, consolidando así su compromiso con el desarrollo humano en todas sus dimensiones. 1988 a 1989: Desarrollo de Infraestructura y Modernización Tecnológica A finales de la década, la universidad entró en una fase de expansión física y tecnológica acelerada. Se ejecutó un Plan de Desarrollo Institucional que contemplaba la ampliación del campus, la construcción de nuevos edificios y la mejora de las instalaciones existentes. En esta etapa se implementó el sistema de transporte institucional, representado por las emblemáticas busetas universitarias, que facilitaban el acceso de los estudiantes y simbolizaban el compromiso con la inclusión y la equidad educativa. En paralelo, los laboratorios y espacios académicos fueron equipados con tecnología de punta para la época, consolidando a la UAO como una institución moderna y preparada para los desafíos tecnológicos emergentes. Los años 1989 y 1990 representaron el culmen del proceso de madurez institucional iniciado una década atrás. Este período fue descrito como “el primer gran reto de la década”: lograr la consolidación integral en lo académico, lo administrativo y lo financiero. La UAO fortaleció su sostenibilidad económica mediante la diversificación de fuentes de ingreso y la optimización de su gestión interna. Este equilibrio entre crecimiento académico y estabilidad financiera fue clave para sustentar los proyectos de expansión y modernización que definirían su futuro. Las imágenes de la época muestran una universidad sólida, con infraestructura consolidada, procesos eficientes y una comunidad académica comprometida con su misión educativa. La década de 1980 dejó un legado invaluable. No solo por los logros tangibles —edificios, laboratorios, programas—, sino por haber construido la identidad institucional que guiaría el desarrollo posterior de la universidad. Los valores cultivados en estos años —autonomía, calidad, planeación estratégica, innovación y compromiso social— se convirtieron en pilares permanentes de la cultura UAO.Este período demostró que la transformación universitaria requiere visión, continuidad y trabajo colectivo. Gracias a los cimientos establecidos en los años ochenta, la UAO entraría a la década siguiente preparada para dar el salto hacia la consolidación definitiva, la internacionalización y la creación de su campus moderno en el Valle del Lili.";
    [SerializeField] private string historia1990 = "El inicio de la década de 1990 marcó un punto de inflexión en la historia de la Universidad Autónoma de Occidente. Este período estuvo caracterizado por la consolidación institucional, la modernización académica y la proyección internacional. En un contexto nacional de transformaciones políticas y económicas impulsadas por la apertura de mercados y la Constitución de 1991, la UAO asumió el reto de fortalecer su papel como institución de educación superior comprometida con la innovación, la calidad y la responsabilidad social. Durante estos años, la universidad amplió significativamente su oferta académica, integrando programas que respondían a las nuevas demandas del entorno productivo y tecnológico.Se fortaleció la investigación y se crearon espacios para la reflexión académica, consolidando una cultura universitaria más integral y crítica. En 1990 y 1991, la UAO celebró sus 20 años de existencia con múltiples actividades institucionales, entre ellas la producción de su primer video institucional y la inauguración del Auditorio Múltiple de la sede Champagnat, símbolo del crecimiento físico y académico de la universidad.Estos eventos representaron no solo una mirada retrospectiva, sino también una proyección hacia el futuro. Entre 1993 y 1995 se promovieron nuevas estrategias de desarrollo institucional.Se realizaron actos administrativos claves, como la posesión del nuevo Vicerrector Administrativo, el fortalecimiento del área audiovisual y la creación de materiales académicos y de divulgación que marcaron el inicio de una etapa de modernización tecnológica. Además, la universidad inició un proceso de renovación curricular y administrativa que buscaba responder a las tendencias globales de la educación. En 1996 y 1997, la UAO vivió un periodo de expansión en su infraestructura y una mayor participación en redes académicas nacionales e internacionales. Se registraron avances en el proyecto de construcción del nuevo campus en el Valle del Lili, una iniciativa que simbolizaba la madurez institucional alcanzada. Se desarrollaron informes técnicos y de planeación que detallaban el avance del campus, reflejando una visión de largo plazo enfocada en la excelencia académica y la sostenibilidad. Finalmente, hacia 1998 y 1999, la universidad entró en una fase de consolidación de su identidad y de fortalecimiento del vínculo con la comunidad.Se construyeron los primeros edificios del nuevo campus y se inauguró el Video Centro de Servicios, reafirmando el compromiso con la innovación tecnológica.En paralelo, la universidad amplió sus redes de cooperación y comenzó a posicionarse como un referente educativo en el suroccidente colombiano. La década culminó con un profundo sentido de transformación.Más allá de los logros materiales y académicos, los años noventa quedaron grabados en la memoria de la comunidad universitaria como la época de transición hacia la modernidad, marcada por la construcción e inauguración del nuevo campus en el Valle del Lili, un símbolo de crecimiento, proyección y compromiso con el futuro.";
    [SerializeField] private string historia2000 = "Durante la primera década del siglo XXI, la Universidad Autónoma de Occidente (UAO) consolidó su crecimiento académico, investigativo y social. En estos años se fortalecieron los programas de formación profesional, se crearon nuevos posgrados y se fomentó la investigación aplicada al desarrollo regional. La Universidad afianzó su compromiso con la responsabilidad social, la innovación educativa y la proyección comunitaria, promoviendo una educación integral centrada en el ser humano, la ética y la sostenibilidad. Este periodo marcó también la modernización de la infraestructura física y tecnológica, con la adecuación de laboratorios, espacios de aprendizaje y áreas verdes que respondían a las necesidades de una universidad en expansión.Asimismo, se fortalecieron los vínculos con el sector empresarial y las instituciones públicas, afianzando el papel de la UAO como referente académico del suroccidente colombiano.";
    [SerializeField] private string historia2010 = "A partir de 2010, la UAO vivió una etapa de crecimiento y transformación profunda, orientada a la calidad, la sostenibilidad y la internacionalización. En 2012, la Universidad obtuvo por primera vez la Acreditación Institucional de Alta Calidad, ingresando al grupo de las mejores universidades del país. Durante la década siguiente, se crearon nuevas facultades, como Humanidades y Artes(2018) y Arquitectura, Diseño y Urbanismo(2022), además de dos doctorados(en Regiones Sostenibles y en Sostenibilidad), consolidando su oferta académica con más de 70 programas activos. La UAO se destacó por sus logros en sostenibilidad ambiental, alcanzando el primer lugar en Colombia y el tercero en Latinoamérica en el Green Metric World University Ranking (2018), y por su compromiso con la innovación, la investigación interdisciplinaria y la formación ética. Entre 2020 y 2021, la Universidad enfrentó los retos de la pandemia fortaleciendo la educación virtual y multimodal, adaptándose con éxito a la enseñanza remota sin perder calidad ni continuidad.Posteriormente, recibió acreditaciones internacionales(ABET y CINDA) y consolidó su sistema de gestión de calidad y sostenibilidad financiera. En 2023, al cumplir 50 años de historia, la UAO celebró su trayectoria con múltiples actividades culturales, académicas y deportivas, reafirmando su misión de ser una universidad de y para la comunidad, comprometida con el medio ambiente, la equidad de género, la investigación de impacto y la formación integral.";

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