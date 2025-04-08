using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;


public class ModeDeu : MonoBehaviour {

    public float alteracio; //+1 clic esquerre -1 clic dret

    RaycastHit2D cop;
    
    public GameObject objecte;

    public bool pucActuar;

    // Si al final lo hago con tilemap, se puede ver si el ratón está tocando un objeto y, si no, que mire el tilemap y para mover piezas del tilemap, se podría habilitar un gameobject que es el que realmente
    // se movera y donde se ponga se destruye el tile anterior para poner el nuevo
    void Start() { 
        Application.targetFrameRate = 60;  // borrar esto
        Terreny.deu = this;
        Entitat.deu = this; // Quitar esto y hacer todo hijo de un mismo padre
    } 


    void Update() { //Si el new input system me dejara quitar este update, seria genial

        if (EventSystem.current.IsPointerOverGameObject() || alteracio == 0 || !pucActuar ) {return;}

        cop = Physics2D.GetRayIntersection( Camera.main.ScreenPointToRay(Input.mousePosition) );
         
        if (alteracio ==  1) { 
            if (cop.collider!=null || objecte==null) { return; }
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Instantiate(objecte,new Vector3(0.5f+Mathf.Floor(worldPosition.x),0.5f+Mathf.Floor(worldPosition.y),0f ), Quaternion.identity);  
        }
        else {
            if (cop.collider==null) { return; }
            Destroy(cop.collider.gameObject);
        }
    }


    public void centre(InputAction.CallbackContext canvi) {
        alteracio = canvi.ReadValue<float>();
    }

}