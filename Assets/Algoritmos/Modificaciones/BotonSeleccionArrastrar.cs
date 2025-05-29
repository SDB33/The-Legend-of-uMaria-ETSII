using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BotonSeleccionArrastrar : Button, IDragHandler {
    
    private static bool arrastrandoCompartido; 
    public static GestorUI gstUI;
    private static BotonSeleccionArrastrar ultimoBoton; // Último botón que fue presionado durante el arrastre.


    public override void OnPointerDown(PointerEventData datos) { 
        base.OnPointerDown(datos); 
        if (IsActive() && IsInteractable()) {
            ActualizarSeleccionVisual();
            arrastrandoCompartido = true;
            ultimoBoton = this;
        }
    }

    public override void OnPointerUp(PointerEventData datos) {
        base.OnPointerUp(datos); 
        if (IsActive() && IsInteractable()) {
            arrastrandoCompartido = false;
        }

        if (ultimoBoton != null && ultimoBoton != this) {
            PointerEventData nuevosDatos = new PointerEventData(EventSystem.current) { position = datos.position };

            // Simula el pointerUp en el último botón si aún está activo.
            ExecuteEvents.Execute(ultimoBoton.gameObject, nuevosDatos, ExecuteEvents.pointerUpHandler);

            // Si el ratón sigue sobre ese botón, también ejecuta el click.
            if (RectTransformUtility.RectangleContainsScreenPoint(ultimoBoton.GetComponent<RectTransform>(), Input.mousePosition, datos.enterEventCamera)) {
                ExecuteEvents.Execute(ultimoBoton.gameObject, nuevosDatos, ExecuteEvents.pointerClickHandler);
            }

            ultimoBoton = null;
        }
    }

    public override void OnPointerEnter(PointerEventData datos) {
        if (arrastrandoCompartido && IsActive() && IsInteractable()) {
            ActualizarSeleccionVisual();

            // Simula que este botón también fue presionado durante el arrastre.
            ExecuteEvents.Execute(this.gameObject, new PointerEventData(EventSystem.current) { position = datos.position }, ExecuteEvents.pointerDownHandler);
            ultimoBoton = this;
        }

        base.OnPointerEnter(datos);
    }

    public override void OnPointerExit(PointerEventData datos) {
        if (arrastrandoCompartido && IsActive() && IsInteractable()) {
            base.OnPointerUp(datos);
        }

        base.OnPointerExit(datos);
    }

    public override void OnPointerClick(PointerEventData eventData) {
        gstUI.esquerra(); // Llama a la función que gestiona la acción de selección.
    }

    // Métodos requeridos por la interfaz IDragHandler para que no se la lleve el scroll view
    public void OnBeginDrag(PointerEventData datos) {}
    public void OnDrag(PointerEventData datos) {}
    public void OnEndDrag(PointerEventData datos) {}

    // Cambia la imagen de selección visual.
    private void ActualizarSeleccionVisual() {
        Transform contenedor = this.transform.parent;
        Transform ultimaCasilla = contenedor.GetChild(contenedor.childCount - 1);
        Image imagenDestino = ultimaCasilla.GetChild(0).GetComponent<Image>();
        Image imagenOrigen = this.transform.GetChild(0).GetComponent<Image>();

        imagenDestino.sprite = imagenOrigen.sprite;
        ultimaCasilla.name = this.name;
    }
}