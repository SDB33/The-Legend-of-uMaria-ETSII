using UnityEngine;

public class Terreny : MonoBehaviour {

    public static ModeDeu deu;

    void OnMouseDrag() {
        deu.estArros = true;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(worldPosition.x,worldPosition.y,0f)  ;
    }


    void OnMouseUp(){
        transform.position = new Vector3(0.5f+Mathf.Floor(transform.position.x),0.5f+Mathf.Floor(transform.position.y),0f );
        deu.estArros = false;
    }


    void OnTriggerEnter2D(Collider2D collisio) {
        Debug.Log(collisio.gameObject.name + " : " + gameObject.name + " : " + Time.time);
    }

}
