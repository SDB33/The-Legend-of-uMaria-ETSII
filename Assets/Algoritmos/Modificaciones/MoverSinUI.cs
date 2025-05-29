using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MoverSinUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

    public float veloDesp;  // Velocidad de desplazamiento horizontal del objeto

    public static GestorUI gstUI; // Referencia global al gestor de UI

    private bool mePuedoMover;    // Indica si el usuario tiene pulsado el ratón y se puede mover
    private bool meMuevo;         // Marca si el usuario ya ha movido el ratón lo suficiente
    private float inicioMovimiento; // Posición X del ratón cuando empezó el arrastre
    private float desplazamiento;   // Cuánto se ha movido desde el punto inicial

    void Update() {
        // Si no estamos en modo de mover, salimos
        if (!mePuedoMover) {
            // Si no se está animando y estamos entre dos posiciones, iniciamos la animación
            if (!gstUI.esCanviant && Mathf.Abs(gstUI.OnSoc - Mathf.RoundToInt(gstUI.OnSoc)) != 0f) {
                StartCoroutine(gstUI.EnCanviarValor());
            }
            return;
        }

        // Calculamos cuánto se ha desplazado el ratón desde que empezó el arrastre
        desplazamiento = Mathf.Clamp(Input.mousePosition.x - inicioMovimiento, -75f, 75f);

        // Si aún no ha superado el umbral mínimo, ignoramos el movimiento
        if (!meMuevo && Mathf.Abs(desplazamiento) < 10f) return;
        meMuevo = true;

        // Si se está cambiando de valor, no hacemos nada más
        if (gstUI.esCanviant) return;

        // Movemos el objeto visualmente
        transform.localPosition += new Vector3(desplazamiento * veloDesp * Time.deltaTime, 0f, 0f);

        // Si el ratón se va del borde izquierdo, lo teletransportamos al derecho
        if (Input.mousePosition.x < 5f) {
            Mouse.current.WarpCursorPosition(new Vector2(Screen.width - 7f, Input.mousePosition.y));
            inicioMovimiento += Screen.width - 12f;
        }
        // Si se va del borde derecho, lo mandamos al izquierdo
        else if (Input.mousePosition.x > Screen.width - 6f) {
            Mouse.current.WarpCursorPosition(new Vector2(6f, Input.mousePosition.y));
            inicioMovimiento -= Screen.width - 12f;
        }
        // Si sigue en pantalla, actualizamos el punto de referencia
        else {
            inicioMovimiento = Input.mousePosition.x;
        }

        // Actualizamos la posición lógica del cursor
        gstUI.ActUbi();

        // Si estamos lejos del objetivo, lo acercamos de uno en uno (paso a paso)
        if (Mathf.Abs(gstUI.OnSoc - gstUI.OnVaig) > 0.2f) {
            gstUI.OnVaig += Mathf.Sign(gstUI.OnSoc - gstUI.OnVaig);
            StartCoroutine(gstUI.EnCanviarValor());
        }
    }

    public void OnPointerDown(PointerEventData datos) {
        if (datos.button == PointerEventData.InputButton.Right) return;

        // Marcamos que se puede empezar a mover
        mePuedoMover = true;
        meMuevo = false;
        inicioMovimiento = Input.mousePosition.x;
    }

    public void OnPointerUp(PointerEventData datos) {
        if (datos.button == PointerEventData.InputButton.Right) return;

        mePuedoMover = false;

        // Si no se ha llegado a mover, interpretamos como un clic para cambiar de opción
        if (!meMuevo) {
            gstUI.OnVaig += Input.mousePosition.x > Screen.width / 2f ? 1f : -1f;
            StartCoroutine(gstUI.EnCanviarValor());
        }

        meMuevo = false;
    }
}
