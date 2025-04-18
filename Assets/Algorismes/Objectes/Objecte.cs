using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class Objecte : MonoBehaviour {

    protected static Vector3 posRatoli;

    public static bool pucMourem;

    public virtual void joControloAra () {}

    protected IEnumerator MouMe()  {
        Vector3 posIni = transform.position; // no sé si deberia ser transform.position "esté donde esté" o convertirlo a lo de los cuadrados
        while (pucMourem) { 
            movimentConstant(); 
            yield return new WaitForSeconds(.0001f); 
        } 
        movimentFinal(posIni);
    }    

    protected void OnMouseDown() { pucMourem = true; this.GetComponent<SpriteRenderer>().sortingOrder = 1;  StartCoroutine(MouMe()); }
    protected void OnMouseUp()   { pucMourem = false;                                                                                }

    protected void movimentConstant() { transform.position = Vector3.Scale(Camera.main.ScreenToWorldPoint(Input.mousePosition), new Vector3(1f, 1f, 0f)); }

    protected void movimentFinal(Vector3 posIni) {
        MoureObjecte accio =  new MoureObjecte();
        accio.mogut = this.gameObject; 
        accio.posIni = posIni;

        posRatoli = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(0.5f+Mathf.Floor(posRatoli.x),0.5f+Mathf.Floor(posRatoli.y),0f);

        accio.posFi = transform.position;
         
        this.GetComponent<SpriteRenderer>().sortingOrder = 0;
        
        foreach (RaycastHit2D hit in Physics2D.RaycastAll(transform.position, Vector2.right, 0.2f, 1 << gameObject.layer)) { 
            if (hit.collider.gameObject != gameObject) { 
                hit.collider.gameObject.SetActive(false);
                accio.desactivat = hit.collider.gameObject;
            } 
        }

        Canvis.introduir(accio);
    }
}
