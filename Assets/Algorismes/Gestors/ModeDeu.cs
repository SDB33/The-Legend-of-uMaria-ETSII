using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections;



public class ModeDeu : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    
    [HideInInspector] public GameObject objecte;

    private bool esticDestruint, estiConstruint;

    [SerializeField] private GSTRodanxes rodanxes;

    void Start() { Application.targetFrameRate = 60; } // borrar esto y ponerlo en algún archivo de configuracio 

    public void OnPointerDown(PointerEventData dades) {
        if (estiConstruint || esticDestruint || dades.button == PointerEventData.InputButton.Middle) {return;} 
        rodanxes.PicaParet();
        if (dades.button == PointerEventData.InputButton.Left && objecte != null) {
            if (objecte.GetComponent<Entitat>() != null) { StartCoroutine(ConstruirEntitat()); }
            else                                         { StartCoroutine(ConstruirTerreny()); }
        }
        else if (dades.button == PointerEventData.InputButton.Right) { StartCoroutine(Destruir());  }
    }
    public void OnPointerUp(PointerEventData dades) {
        if (dades.button == PointerEventData.InputButton.Middle) {return;}
        rodanxes.PicaParet();
        if      (dades.button == PointerEventData.InputButton.Left)   { Entitat.pucMourem = false; estiConstruint = false; } // Solución muy muy cutre. Esto es para objetos como Entitat que se construyen antes y como no les has dado click, no llaman a OnMouseDown ni OnMouseUp
        else if (dades.button == PointerEventData.InputButton.Right)  {                            esticDestruint = false; }
    }

    private IEnumerator ConstruirTerreny() {
        if (Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero,Mathf.Infinity, ~(1 << objecte.layer)).collider!=null) { yield break; }
        estiConstruint = true;
        crearIDestruirObjecte accions =  new crearIDestruirObjecte();
        while (estiConstruint) {
            RaycastHit2D cop = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero,Mathf.Infinity, 1 << objecte.layer);
            if (cop.collider!=null) {
                if (cop.collider.gameObject.name == objecte.name) {yield return new WaitForSeconds(.001f); continue;}
                cop.collider.gameObject.SetActive(false);  
                accions.DesactivaM.Add(cop.collider.gameObject);  
            }
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GameObject nou =  Instantiate(objecte,new Vector3(0.5f+Mathf.Floor(worldPosition.x),0.5f+Mathf.Floor(worldPosition.y),0f ), Quaternion.identity);
            nou.name = objecte.name;
            accions.ModificaM.Add(nou);
            
            yield return new WaitForSeconds(.001f);
        }
        if (accions.ModificaM.Count!=0) { Canvis.introduir(accions); }
    }

    private IEnumerator ConstruirEntitat() {
        if (Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, 1 << objecte.layer).collider!=null) { yield break; }
        estiConstruint = true;
        crearIDestruirObjecte accions =  new crearIDestruirObjecte();
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GameObject nou = Instantiate(objecte,new Vector3(0.5f+Mathf.Floor(worldPosition.x),0.5f+Mathf.Floor(worldPosition.y),-1f ), Quaternion.identity);
        nou.name = objecte.name;
        nou.GetComponent<Entitat>().OnMouseDown();
        accions.ModificaM.Add(nou);
        Canvis.introduir(accions);
        while (estiConstruint) { yield return new WaitForSeconds(.001f); }
    }

    private IEnumerator Destruir() { // Esborrar els objectes de la mateixa capa del primer que toquis
        int seleccionat = -1;
        esticDestruint = true;
        RaycastHit2D cop;
        while (seleccionat == -1 && esticDestruint) {
            cop = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (cop.collider!=null) { seleccionat = cop.collider.gameObject.layer; }
            yield return new WaitForSeconds(.01f);
        }
        crearIDestruirObjecte accions =  new crearIDestruirObjecte();
        while (esticDestruint) {
            cop = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, 1 << seleccionat);
            if (cop.collider!=null) { 
                cop.collider.gameObject.SetActive(false);  
                accions.DesactivaM.Add(cop.collider.gameObject);    
            }
            yield return new WaitForSeconds(.001f);
        }
        if (accions.DesactivaM.Count!=0) { Canvis.introduir(accions); }  
    }

    public void Desfer   (InputAction.CallbackContext canvi) { if (canvi.performed && !estiConstruint && !rodanxes.EsticAlMenu()) { Canvis.Desfer();            } }
    public void Refer    (InputAction.CallbackContext canvi) { if (canvi.performed && !estiConstruint && !rodanxes.EsticAlMenu()) { Canvis.Refer();             } }
    public void Desar    (InputAction.CallbackContext canvi) { if (canvi.performed && !estiConstruint && !rodanxes.EsticAlMenu()) { Canvis.DesarContingut();    } }
    public void Carregar (InputAction.CallbackContext canvi) { if (canvi.performed && !estiConstruint && !rodanxes.EsticAlMenu()) { Canvis.CarregarContingut(); } }

}