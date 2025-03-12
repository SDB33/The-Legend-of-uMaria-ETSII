using UnityEngine;
using UnityEngine.InputSystem;

public class ModeDeu : MonoBehaviour {

    public IRemovible bestiesa;

    private float alteracio; //+1 clic esquerre -1 clic dret

    RaycastHit2D cop;
    
    private GameObject mateix;
    private GameObject avaluador;
    private bool sonDiferents;
    private float temps;


    // Si al final lo hago con tilemap, se puede ver si el ratón está tocando un objeto y, si no, que mire el tilemap y para mover piezas del tilemap, se podría habilitar un gameobject que es el que realmente
    // se movera y donde se ponga se destruye el tile anterior para poner el nuevo
    void Start() { Application.targetFrameRate = 60; } // borrar esto


    void Update() {
         
        if      (alteracio ==  1) { Organitzar(); }
        else if (alteracio == -1) {
            cop = Physics2D.GetRayIntersection( Camera.main.ScreenPointToRay(Input.mousePosition) );
            if (cop.collider!=null) { Destroy(cop.collider.gameObject); }
        }

    }

    private void Organitzar() {
        cop = Physics2D.GetRayIntersection( Camera.main.ScreenPointToRay(Input.mousePosition) );
        if (cop.collider==null) { sonDiferents=true; temps=Time.time+3f; if (bestiesa==null) {return;} bestiesa.Concebre();  }
        else {
            if (sonDiferents) {return;}
            avaluador=cop.collider.gameObject;
            if (mateix==null) {mateix=cop.collider.gameObject;}
            if (mateix==avaluador) {
                if (Time.time>temps) { cop.collider.gameObject.GetComponent<SpriteRenderer>().color = Color.cyan; }

            }
            else { sonDiferents=true; }


        }
        
    }

    public void Alteracio(InputAction.CallbackContext valor) { alteracio = valor.ReadValue<float>(); sonDiferents=false; temps=Time.time+3f; mateix=null; }

}


//        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//        Instantiate(mao,  new Vector3(0.5f+Mathf.Floor(worldPosition.x),0.5f+Mathf.Floor(worldPosition.y),0f )    , Quaternion.identity);