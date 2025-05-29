using UnityEngine;
using System.Collections;

public class Entidad : MonoBehaviour {

    public static Vector3 posRatoli;       // Posición actual del ratón en el mundo
    public static ModoEdicion modoEdicion; 
    public static bool puedoMoverme;      

    // Coroutine para mover la entidad mientras puedoMoverme sea true
    public IEnumerator Moverme() {
        Vector3 posicionInicial = transform.position;
        while (puedoMoverme) {
            movimientoConstante();
            yield return new WaitForSeconds(.0001f);
        }
        movimientoFinal(posicionInicial);
    }

    public void OnMouseDown() {
        if (!enabled) return;
        puedoMoverme = true;
        this.GetComponent<SpriteRenderer>().sortingOrder = 1;  // Traer al frente
        StartCoroutine(Moverme());
    }

    public void OnMouseUp() {
        puedoMoverme = false;
    }

    // Movimiento continuo mientras se arrastra la entidad
    private void movimientoConstante() {
        Vector3 posMundo = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(posMundo.x, posMundo.y, 0f);
    }

    // Movimiento final y registro de la acción para deshacer/rehacer
    private void movimientoFinal(Vector3 posicionInicial) {
        MoverObjeto accion = new MoverObjeto();

        accion.modificaM.Add(this.gameObject);
        accion.posIni = posicionInicial;

        posRatoli = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(0.5f + Mathf.Floor(posRatoli.x), 0.5f + Mathf.Floor(posRatoli.y), -1f);

        accion.posFin = transform.position;

        this.GetComponent<SpriteRenderer>().sortingOrder = 0;

        // Desactivar objetos que estén debajo
        foreach (RaycastHit2D rayo in Physics2D.RaycastAll(transform.position, Vector2.right, 0.2f, 1 << gameObject.layer)) {
            if (rayo.collider.gameObject != gameObject) {
                rayo.collider.gameObject.SetActive(false);
                accion.desactivaM.Add(rayo.collider.gameObject);
            }
        }

        if (posicionInicial != transform.position) {
            modoEdicion.Introducir(accion);
        }
    }
}
