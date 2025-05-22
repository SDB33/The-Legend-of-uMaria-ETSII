using System.Collections.Generic;
using UnityEngine;

public class ModeJoc : MonoBehaviour {

    private HashSet<GameObject> modificats;

    void Start() {
        modificats = new HashSet<GameObject>();
    }

    void OnEnable() {
        //modificats.Clear();
    }

    void OnTriggerEnter2D(Collider2D altre) {
        if (modificats.Add(altre.gameObject)){
            altre.gameObject.GetComponent<Entitat>().enabled = false;
        }
        
        ((MonoBehaviour) altre.gameObject.GetComponent<IReiniciable>()).enabled = true;
    }
    
    void OnTriggerExit2D(Collider2D altre) {
        //altre.gameObject.SetActive(false);
    }
    
    void OnDisable() {
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
        
}


/*
Hay dos temas diferentes, cuando el juego empieza y se toca al objeto por primera vez
Activamos su modo juego y desactivamos su modo ser editado
activamos el objeto

Cuando sale de la pantalla, desactivamos todo el objeto

Cuando vamos a dejar de jugar,
desactivamos su modo juego, activamos su modo ser editado
*/