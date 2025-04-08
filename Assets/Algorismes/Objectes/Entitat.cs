using UnityEngine;
using System.Collections;

public class Entitat : MonoBehaviour {

    public static ModeDeu deu;

    private static Vector3 posRatoli;

    void Start () {  this.GetComponent<SpriteRenderer>().sortingOrder = 1; deu.pucActuar = false; StartCoroutine(MouMe()); }

    private IEnumerator MouMe()  {
        while (deu.alteracio == 1) { movimentConstant(); yield return new WaitForSeconds(.0001f); } 
        movimentFinal();
    }    

    void OnMouseDown() {  this.GetComponent<SpriteRenderer>().sortingOrder = 1; deu.pucActuar = false; }
    void OnMouseDrag() { movimentConstant(); }
    void OnMouseUp()   { movimentFinal(); }

    private void movimentConstant() { posRatoli = Camera.main.ScreenToWorldPoint(Input.mousePosition); transform.position = new Vector3(posRatoli.x,posRatoli.y,0f); }
    
    private void movimentFinal() {
        posRatoli = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(0.5f+Mathf.Floor(posRatoli.x),0.5f+Mathf.Floor(posRatoli.y),0f);
        deu.pucActuar = true; 
        this.GetComponent<SpriteRenderer>().sortingOrder = 0;
        foreach (RaycastHit2D hit in Physics2D.RaycastAll(transform.position, Vector2.right, 0.2f, 1 << gameObject.layer)) { if (hit.collider.gameObject != gameObject) { Destroy(hit.collider.gameObject); } }
    }

}
