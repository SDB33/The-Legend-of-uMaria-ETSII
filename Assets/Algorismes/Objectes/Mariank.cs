using UnityEngine;
using UnityEngine.InputSystem;

public class Mariank : MonoBehaviour, IReiniciable {
    public SpriteRenderer renderizadorSprite;

    public Sprite arribaQuieto, arribaPaso1, arribaPaso2;
    public Sprite abajoQuieto, abajoPaso1, abajoPaso2;
    public Sprite derechaQuieto, derechaPaso1, derechaPaso2;

    public float velocidad = 2f;
    public float ritmoAnimacion = 0.1f;

    private Vector2 direccionEntrada;
    private Vector2 ultimaDireccion = Vector2.down;
    private float temporizadorAnimacion;
    private int faseAnimacion;

    void Update() {
        Vector2 movimiento = Vector2.zero;

        if (direccionEntrada.sqrMagnitude > 0.01f) {
            movimiento = Mathf.Abs(direccionEntrada.x) > Mathf.Abs(direccionEntrada.y)
                ? new Vector2(Mathf.Sign(direccionEntrada.x), 0)
                : new Vector2(0, Mathf.Sign(direccionEntrada.y));

            ultimaDireccion = movimiento;
            transform.position += (Vector3)(movimiento * velocidad * Time.deltaTime);

            temporizadorAnimacion += Time.deltaTime;
            if (temporizadorAnimacion >= ritmoAnimacion) {
                faseAnimacion = (faseAnimacion + 1) % 3;
                temporizadorAnimacion = 0f;
            }
        } else {
            faseAnimacion = 0;
        }

        ActualizarSprite();
    }

    void ActualizarSprite() {
        Sprite sprite;

        if (ultimaDireccion == Vector2.up) {
            sprite = new[] { arribaQuieto, arribaPaso1, arribaPaso2 }[faseAnimacion];
        } else if (ultimaDireccion == Vector2.down) {
            sprite = new[] { abajoQuieto, abajoPaso1, abajoPaso2 }[faseAnimacion];
        } else {
            sprite = new[] { derechaQuieto, derechaPaso1, derechaPaso2 }[faseAnimacion];
            renderizadorSprite.flipX = (ultimaDireccion == Vector2.left);
        }

        renderizadorSprite.sprite = sprite;
    }

    public void AlMover(InputAction.CallbackContext contexto) {
        if (contexto.performed || contexto.canceled)
            direccionEntrada = contexto.ReadValue<Vector2>();
    }

    public void Mover(Vector2 direccion) {
        direccionEntrada = direccion;
    }

    public void Detener() {
        direccionEntrada = Vector2.zero;
    }

    public void DesarEstat() { }

    public void RestablirEstat() { renderizadorSprite.sprite = abajoQuieto; }
}
