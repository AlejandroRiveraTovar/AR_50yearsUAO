using UnityEngine;
using System.Collections;

namespace StarterAssets
{
    /// <summary>
    /// Controla el movimiento del asistente mediante toques en pantalla en RA
    /// Se complementa con ThirdPersonController sin reemplazarlo
    /// </summary>
    public class ARAssistantController : MonoBehaviour
    {
        [Header("Referencias")]
        private Animator animator;
        private CharacterController characterController;
        private ThirdPersonController thirdPersonController;

        [Header("Puntos de Referencia por Época")]
        [SerializeField] private Transform punto1970;
        [SerializeField] private Transform punto1980;
        [SerializeField] private Transform punto1990;
        [SerializeField] private Transform punto2000;
        [SerializeField] private Transform punto2010;

        [Header("Parámetros de Movimiento")]
        [SerializeField] private float velocidadCaminata = 2f;
        [SerializeField] private float velocidadGiro = 5f;
        [SerializeField] private float distanciaMinima = 0.5f;

        [Header("Control")]
        [SerializeField] private bool modoRA = true; // Activar/desactivar control por toques

        [Header("Parámetros de Animación")]
        private int hashSeñalar = Animator.StringToHash("Señalar");

        [Header("Estados")]
        private Vector3 destino;
        private bool moviendoseADestino = false;
        private bool señalando = false;
        private Transform standActual;
        private int epocaActual = 1970;

        private StarterAssetsInputs inputSimulado;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            characterController = GetComponent<CharacterController>();
            thirdPersonController = GetComponent<ThirdPersonController>();
            inputSimulado = GetComponent<StarterAssetsInputs>();

            // Si no existe StarterAssetsInputs, agregarlo
            if (inputSimulado == null)
            {
                inputSimulado = gameObject.AddComponent<StarterAssetsInputs>();
            }
        }

        private void Start()
        {
            // Posicionar en el primer punto al iniciar
            if (punto1970 != null)
            {
                transform.position = punto1970.position;
                transform.rotation = punto1970.rotation;
            }
        }

        private void Update()
        {
            if (modoRA && moviendoseADestino)
            {
                MoverHaciaDestinoRA();
            }
        }

        /// <summary>
        /// Mueve el asistente al stand de una época específica
        /// </summary>
        public void IrAEpoca(int año)
        {
            if (!modoRA) return;

            StopAllCoroutines();
            epocaActual = año;

            Transform puntoDestino = ObtenerPuntoDeEpoca(año);

            if (puntoDestino != null)
            {
                standActual = puntoDestino;
                destino = puntoDestino.position;
                moviendoseADestino = true;
                señalando = false;

                // Desactivar input del jugador mientras navega automáticamente
                if (thirdPersonController != null)
                {
                    thirdPersonController.enabled = false;
                }

                // Resetear animación de señalar
                //if (animator != null && animator.HasParameter(hashSeñalar))
                {
                    animator.SetBool(hashSeñalar, false);
                }
            }
        }

        /// <summary>
        /// Mueve a una posición específica (para toques personalizados)
        /// </summary>
        public void IrAPosicion(Vector3 posicion)
        {
            if (!modoRA) return;

            destino = posicion;
            moviendoseADestino = true;

            if (thirdPersonController != null)
            {
                thirdPersonController.enabled = false;
            }
        }

        private Transform ObtenerPuntoDeEpoca(int año)
        {
            switch (año)
            {
                case 1970: return punto1970;
                case 1980: return punto1980;
                case 1990: return punto1990;
                case 2000: return punto2000;
                case 2010: return punto2010;
                default:
                    Debug.LogWarning("Época no reconocida: " + año);
                    return null;
            }
        }

        private void MoverHaciaDestinoRA()
        {
            float distancia = Vector3.Distance(transform.position, destino);

            // Si llegó al destino
            if (distancia < distanciaMinima)
            {
                moviendoseADestino = false;

                // Simular que dejó de moverse en el input
                if (inputSimulado != null)
                {
                    inputSimulado.MoveInput(Vector2.zero);
                }

                if (standActual != null)
                {
                    StartCoroutine(GirarHaciaCamaraYReactivar());
                }
                return;
            }

            // Calcular dirección hacia el destino
            Vector3 direccion = (destino - transform.position).normalized;
            direccion.y = 0; // Mantener movimiento horizontal

            // Calcular el ángulo hacia el destino
            Quaternion rotacionDestino = Quaternion.LookRotation(direccion);

            // Girar suavemente hacia el destino
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                rotacionDestino,
                velocidadGiro * Time.deltaTime
            );

            // Mover el personaje
            Vector3 movimiento = direccion * velocidadCaminata * Time.deltaTime;
            characterController.Move(movimiento + new Vector3(0, -9.81f * Time.deltaTime, 0));

            // Simular input para que las animaciones funcionen
            if (inputSimulado != null)
            {
                // Crear un vector de movimiento en espacio local
                Vector3 movimientoLocal = transform.InverseTransformDirection(direccion);
                inputSimulado.MoveInput(new Vector2(movimientoLocal.x, movimientoLocal.z));
            }
        }

        private IEnumerator GirarHaciaCamaraYReactivar()
        {
            // Obtener la cámara principal
            Camera camara = Camera.main;
            if (camara != null)
            {
                Vector3 direccionCamara = (camara.transform.position - transform.position).normalized;
                direccionCamara.y = 0;

                Quaternion rotacionDestino = Quaternion.LookRotation(direccionCamara);
                float tiempoGiro = 0.5f;
                float tiempoTranscurrido = 0f;

                while (tiempoTranscurrido < tiempoGiro)
                {
                    tiempoTranscurrido += Time.deltaTime;
                    transform.rotation = Quaternion.Slerp(
                        transform.rotation,
                        rotacionDestino,
                        tiempoTranscurrido / tiempoGiro
                    );
                    yield return null;
                }
            }

            // Reactivar el control del jugador
            if (thirdPersonController != null)
            {
                thirdPersonController.enabled = true;
            }
        }

        /// <summary>
        /// Hace que el asistente levante el brazo para señalar
        /// </summary>
        public void Señalar(bool activar)
        {
            //if (animator != null && animator.HasParameter(hashSeñalar))
            {
                animator.SetBool(hashSeñalar, activar);
                señalando = activar;
            }
        }

        /// <summary>
        /// Detiene el movimiento automático y devuelve el control
        /// </summary>
        public void DetenerYDevolverControl()
        {
            moviendoseADestino = false;

            if (inputSimulado != null)
            {
                inputSimulado.MoveInput(Vector2.zero);
            }

            if (thirdPersonController != null)
            {
                thirdPersonController.enabled = true;
            }
        }

        /// <summary>
        /// Cambia entre modo RA (toques) y modo manual (teclado)
        /// </summary>
        public void SetModoRA(bool activar)
        {
            modoRA = activar;

            if (thirdPersonController != null)
            {
                thirdPersonController.enabled = !activar || !moviendoseADestino;
            }
        }

        public bool EstaMoviendo() => moviendoseADestino;
        public bool EstaSeñalando() => señalando;
        public int GetEpocaActual() => epocaActual;

        // Visualización de puntos en el editor
        private void OnDrawGizmos()
        {
            DibujarPuntoGizmo(punto1970, Color.red, "1970");
            DibujarPuntoGizmo(punto1980, Color.yellow, "1980");
            DibujarPuntoGizmo(punto1990, Color.green, "1990");
            DibujarPuntoGizmo(punto2000, Color.cyan, "2000");
            DibujarPuntoGizmo(punto2010, Color.magenta, "2010");

            if (moviendoseADestino)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(transform.position, destino);
                Gizmos.DrawWireSphere(destino, distanciaMinima);
            }
        }

        private void DibujarPuntoGizmo(Transform punto, Color color, string etiqueta)
        {
            if (punto != null)
            {
                Gizmos.color = color;
                Gizmos.DrawWireSphere(punto.position, 0.3f);
                Gizmos.DrawLine(punto.position, punto.position + punto.forward * 1f);

#if UNITY_EDITOR
                UnityEditor.Handles.Label(punto.position + Vector3.up * 0.5f, etiqueta);
#endif
            }
        }
    }
}
