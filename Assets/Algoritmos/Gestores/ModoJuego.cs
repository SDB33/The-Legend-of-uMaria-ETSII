using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ModoJuego : MonoBehaviour {

    private HashSet<GameObject> modificados;

    // Referencia al gestor de UI para habilitar/deshabilitar en cambios de modo
    [SerializeField] private GestorUI gestorUI;

    void Start() {
        modificados = new HashSet<GameObject>();
    }

    void OnEnable() {
        // modificados.Clear();
    }

    // Cuando un objeto entra en el trigger
    void OnTriggerEnter2D(Collider2D otro) {
        // Si el objeto no estaba modificado antes, lo añadimos y deshabilitamos su Entidad
        if (modificados.Add(otro.gameObject)) {
            otro.gameObject.GetComponent<Entidad>().enabled = false;
            otro.gameObject.GetComponent<IReiniciable>().DesarEstat(); // guardamos el estado actual
        }
        
        // Activamos el componente IReiniciable para que pueda controlar el objeto en modo juego
        ((MonoBehaviour)otro.gameObject.GetComponent<IReiniciable>()).enabled = true;
    }

    // Cuando un objeto sale del trigger
    void OnTriggerExit2D(Collider2D otro) {
        // Desactivamos el IReiniciable y detenemos el movimiento del objeto
        ((MonoBehaviour)otro.gameObject.GetComponent<IReiniciable>()).enabled = false;
        otro.attachedRigidbody.linearVelocity = Vector2.zero; // Detener la velocidad (no ideal, pero sirve)
    }
    
    // Cuando se desactiva este modo
    void OnDisable() {
        if (modificados == null) { return; }

        // Restaurar el estado original y desactivar el control IReiniciable de todos los objetos modificados
        foreach (GameObject obj in modificados) {
            ((MonoBehaviour)obj.GetComponent<IReiniciable>()).enabled = false;
            obj.GetComponent<IReiniciable>().RestablirEstat();
        }
    }
    
    // Método para cambiar al modo edición mediante Input System
    public void Editar(InputAction.CallbackContext contexto) {
        if (contexto.performed) {
            // Cambiar el mapa de acciones a "Edicion"
            transform.parent.GetComponent<PlayerInput>().SwitchCurrentActionMap("Edicion");

            // Habilitar el gestor UI para la edición
            gestorUI.enabled = true;

            // Activar el primer hijo del padre (por ejemplo UI edición)
            this.gameObject.transform.parent.GetChild(0).gameObject.SetActive(true);

            // Desactivar el modo juego
            this.gameObject.SetActive(false);
        } 
    }
}
