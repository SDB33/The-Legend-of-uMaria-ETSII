using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections;



public class ModeDeu : MonoBehaviour {

    public float alteracio; //+1 clic esquerre -1 clic dret

    RaycastHit2D cop;
    
    public GameObject objecte;

    public bool pucActuar;
    // Si al final lo hago con tilemap, se puede ver si el ratón está tocando un objeto y, si no, que mire el tilemap y para mover piezas del tilemap, se podría habilitar un gameobject que es el que realmente
    // se movera y donde se ponga se destruye el tile anterior para poner el nuevo
    void Start() { 
        Application.targetFrameRate = 60;  // borrar esto
        Objecte.deu = this; 
    } 


    void Update() { //Si el new input system me dejara quitar este update, seria genial

        if (EventSystem.current.IsPointerOverGameObject() || alteracio == 0 || !pucActuar ) {return;}

        if (alteracio ==  1) {
            if ( objecte==null) {return;}

            cop = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero,Mathf.Infinity, 1 << objecte.layer);

            if (cop.collider!=null) {
                if (cop.collider.gameObject.name == objecte.name + "(Clone)" ) {return;}
                Destroy(cop.collider.gameObject);
            }
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Instantiate(objecte,new Vector3(0.5f+Mathf.Floor(worldPosition.x),0.5f+Mathf.Floor(worldPosition.y),0f ), Quaternion.identity);  
        }
        else {
            cop = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (cop.collider==null) { return; }
            Destroy(cop.collider.gameObject);
        }
    }

    public void centre(InputAction.CallbackContext canvi) { alteracio = canvi.ReadValue<float>(); }

}