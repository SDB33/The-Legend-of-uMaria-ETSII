using UnityEngine;
using System.Collections;

public class Entitat : MonoBehaviour {
    public static Vector3 posRatoli;

    public static bool pucMourem;
        
    public IEnumerator MouMe()  {
        Vector3 posIni = transform.position; 
        while (pucMourem) { 
            movimentConstant(); 
            yield return new WaitForSeconds(.0001f); 
        } 
        movimentFinal(posIni);
    }    

    public void OnMouseDown() { pucMourem = true; this.GetComponent<SpriteRenderer>().sortingOrder = 1;  StartCoroutine(MouMe()); }
    public void OnMouseUp()   { pucMourem = false;                                                                                }

    private void movimentConstant() { transform.position = Vector3.Scale(Camera.main.ScreenToWorldPoint(Input.mousePosition), new Vector3(1f, 1f, 0f)); }

    private void movimentFinal(Vector3 posIni) {
        MoureObjecte accio =  new MoureObjecte();
        accio.ModificaM.Add(this.gameObject);
        accio.posIni = posIni;

        posRatoli = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(0.5f+Mathf.Floor(posRatoli.x),0.5f+Mathf.Floor(posRatoli.y),-1f);

        accio.posFi = transform.position;
         
        this.GetComponent<SpriteRenderer>().sortingOrder = 0;
        
        foreach (RaycastHit2D hit in Physics2D.RaycastAll(transform.position, Vector2.right, 0.2f, 1 << gameObject.layer)) { 
            if (hit.collider.gameObject != gameObject) { 
                hit.collider.gameObject.SetActive(false);
                accio.DesactivaM.Add( hit.collider.gameObject);
            } 
        }

        if (posIni!=transform.position) { Canvis.introduir(accio); }
    }

}
