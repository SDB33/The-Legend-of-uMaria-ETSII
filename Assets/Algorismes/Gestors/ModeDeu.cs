using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections;



public class ModeDeu : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    
    public GameObject objecte;

    private bool esticDestruint;

    public static bool estiConstruint;
    // Si al final lo hago con tilemap, se puede ver si el ratón está tocando un objeto y, si no, que mire el tilemap y para mover piezas del tilemap, se podría habilitar un gameobject que es el que realmente
    // se movera y donde se ponga se destruye el tile anterior para poner el nuevo
    void Start() { Application.targetFrameRate = 60; } // borrar esto y ponerlo en algún archivo de configuracio 

    public void OnPointerDown(PointerEventData dades) {
        if (estiConstruint || esticDestruint) {return;}
        if      (dades.button == PointerEventData.InputButton.Left  && objecte != null) { StartCoroutine(Construir()); }
        else if (dades.button == PointerEventData.InputButton.Right                   ) { StartCoroutine(Destruir());  }
    }
    public void OnPointerUp(PointerEventData dades) {
        Objecte.pucMourem = false;  // Solución muy muy cutre. Esto es para objetos como Entitat que se construyen antes y como no les has dado click, no llaman a OnMouseDown ni OnMouseUp
        if      (dades.button == PointerEventData.InputButton.Left)   { estiConstruint = false; }
        else if (dades.button == PointerEventData.InputButton.Right)  { esticDestruint = false; }
    }

    private IEnumerator Construir() {
        if (Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero,Mathf.Infinity, 1 << objecte.layer).collider==null) { estiConstruint = true; }  // el tema está en que la ui en la que está este script recibe el click aunque tenga un objeto delante entonces si tiene un objeto delante del mismo tipo del que va a construir, no hace nada
        crearIDestruirObjecte accions =  new crearIDestruirObjecte();
        while (estiConstruint) {
            RaycastHit2D cop = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero,Mathf.Infinity, 1 << objecte.layer);

            if (cop.collider!=null) {
                if (cop.collider.gameObject.name == objecte.name + "(Clone)" ) {yield return new WaitForSeconds(.001f); continue;}
                cop.collider.gameObject.SetActive(false);  
                accions.DesactivaM.Add(cop.collider.gameObject);  
            }
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GameObject breu = Instantiate(objecte,new Vector3(0.5f+Mathf.Floor(worldPosition.x),0.5f+Mathf.Floor(worldPosition.y),0f ), Quaternion.identity);
            breu.GetComponent<Objecte>().joControloAra();
            accions.ModificaM.Add(breu);
            yield return new WaitForSeconds(.001f);
        }
        if (accions.ModificaM.Count!=0) { Canvis.introduir(accions); } 
    }

    private IEnumerator Destruir() {
        esticDestruint = true;
        crearIDestruirObjecte accions =  new crearIDestruirObjecte();
        while (esticDestruint) {
            RaycastHit2D cop = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (cop.collider!=null) { 
                cop.collider.gameObject.SetActive(false);  
                accions.DesactivaM.Add(cop.collider.gameObject);    
            }
            yield return new WaitForSeconds(.001f);
        }
        if (accions.DesactivaM.Count!=0) { Canvis.introduir(accions); }  
    }

    public void Desfer (InputAction.CallbackContext canvi) { if (canvi.performed) { Canvis.Desfer(); } }

    public void Refer (InputAction.CallbackContext canvi) { if (canvi.performed) {Canvis.Refer(); } }


}