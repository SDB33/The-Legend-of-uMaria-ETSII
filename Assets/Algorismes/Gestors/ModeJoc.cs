using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ModeJoc : MonoBehaviour {

    private HashSet<GameObject> modificats;

    [SerializeField] private GSTRodanxes rodanxes;

    void Start() {
        modificats = new HashSet<GameObject>();
    }

    void OnEnable() {
        //modificats.Clear();
    }

    void OnTriggerEnter2D(Collider2D altre) {
        if (modificats.Add(altre.gameObject)){
            altre.gameObject.GetComponent<Entitat>().enabled = false;
            altre.gameObject.GetComponent<IReiniciable>().DesarEstat();
        }
        
        ((MonoBehaviour) altre.gameObject.GetComponent<IReiniciable>()).enabled = true;
    }

    void OnTriggerExit2D(Collider2D altre) {
        ((MonoBehaviour)altre.gameObject.GetComponent<IReiniciable>()).enabled = false;
        altre.attachedRigidbody.linearVelocity = Vector2.zero; // esto no es una buena solución, habría que ver como lo hace originalmente el juego
    }
    
    void OnDisable() {
        if (modificats == null) { return; }
        foreach (GameObject objecte in modificats) {
            /*
            El problema de esto es que queremos que cuando termine el juego, se desactiven los que tenemos en pantalla 
            (puesto que el resto ya ha sido desactivado al salir de esta) pero estamos desactivando todos, TODOS, y si hay un montonazo, 
            estás desperdiciando tiempo. Se podria hacer otro hashset o una lista o así solo con los que están en pantalla para desactivarlos al final
            */
            ((MonoBehaviour)objecte.GetComponent<IReiniciable>()).enabled = false;
            objecte.GetComponent<IReiniciable>().RestablirEstat();
        }
    }
    
    
    public void Editar (InputAction.CallbackContext canvi) {
        if (canvi.performed) {
            transform.parent.GetComponent<PlayerInput>().SwitchCurrentActionMap("Prometeu");
            rodanxes.enabled = true;
            this.gameObject.transform.parent.GetChild(0).gameObject.SetActive(true);
            this.gameObject.SetActive(false);
        } 
    }
        
}
