using System.Collections.Generic;
using UnityEngine;

public class ModeJoc : MonoBehaviour {

    private HashSet<GameObject> modificats;

    void Start() {
        modificats = new HashSet<GameObject>();
    }

    void OnTriggerEnter2D(Collider2D altre) {
        modificats.Add(altre.gameObject);
        ((MonoBehaviour) altre.gameObject.GetComponent<IReiniciable>()).enabled = true;
    }
    
    void OnDisable() {
        foreach (GameObject objecte in modificats) {
            objecte.GetComponent<IReiniciable>().RestablirEstat();
            ((MonoBehaviour) objecte.GetComponent<IReiniciable>()).enabled = false;
        }
    }
        
}


