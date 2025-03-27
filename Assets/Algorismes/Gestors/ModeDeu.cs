using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class ModeDeu : MonoBehaviour {

    public IRemovible bestiesa;

    private float alteracio; //+1 clic esquerre -1 clic dret

    RaycastHit2D cop;
    
    private GameObject mateix;
    private GameObject avaluador;
    private bool sonDiferents;
    private float temps;


    public GameObject objecte;


    // Si al final lo hago con tilemap, se puede ver si el ratón está tocando un objeto y, si no, que mire el tilemap y para mover piezas del tilemap, se podría habilitar un gameobject que es el que realmente
    // se movera y donde se ponga se destruye el tile anterior para poner el nuevo
    void Start() { Application.targetFrameRate = 60; } // borrar esto


    void Update() { //Si encuentro una forma de hacer que no se pueda tocar la ui, quitar el boton de InterDusuari y por otro lado si este proceso lo pudiese quitar de update, seria genial

        if (EventSystem.current.currentSelectedGameObject!=null || alteracio == 0 ) {return;}

        cop = Physics2D.GetRayIntersection( Camera.main.ScreenPointToRay(Input.mousePosition) );
         
        if (alteracio ==  1) {  
            if (cop.collider!=null || objecte==null ) { return; }
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Instantiate(objecte,  new Vector3(0.5f+Mathf.Floor(worldPosition.x),0.5f+Mathf.Floor(worldPosition.y),0f )    , Quaternion.identity);
        }
        else if (alteracio == -1) {
            if (cop.collider==null) { return; }
            Destroy(cop.collider.gameObject);
        }
    }

     public void esquerra(InputAction.CallbackContext canvi) {
        if (canvi.performed) {alteracio+=1;}
        else if (canvi.canceled) {alteracio-=1;} 
    }

    public void dreta(InputAction.CallbackContext canvi) { 
        if (canvi.performed) {alteracio-=1;}
        else if (canvi.canceled) {alteracio+=1;} 
    }

}


//        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//        Instantiate(mao,  new Vector3(0.5f+Mathf.Floor(worldPosition.x),0.5f+Mathf.Floor(worldPosition.y),0f )    , Quaternion.identity);