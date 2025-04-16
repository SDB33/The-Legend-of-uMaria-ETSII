using UnityEngine;
using System.Collections;

public class Objecte : MonoBehaviour {

    public static ModeDeu deu;

    protected static Vector3 posRatoli;

    protected IEnumerator MouMe()  {
        while (deu.alteracio == 1) { 
            movimentConstant(); 
            yield return new WaitForSeconds(.0001f); 
        } 
        movimentFinal();
    }    

    void OnMouseDown() {   this.GetComponent<SpriteRenderer>().sortingOrder = 1; deu.pucActuar = false; StartCoroutine(MouMe()); }

    protected void movimentConstant() { posRatoli = Camera.main.ScreenToWorldPoint(Input.mousePosition); transform.position = new Vector3(posRatoli.x,posRatoli.y,0f); }

    protected void movimentFinal() {
        posRatoli = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(0.5f+Mathf.Floor(posRatoli.x),0.5f+Mathf.Floor(posRatoli.y),0f);
        deu.pucActuar = true; 
        this.GetComponent<SpriteRenderer>().sortingOrder = 0;
        foreach (RaycastHit2D hit in Physics2D.RaycastAll(transform.position, Vector2.right, 0.2f, 1 << gameObject.layer)) { if (hit.collider.gameObject != gameObject) { Destroy(hit.collider.gameObject); } }
    }


}
