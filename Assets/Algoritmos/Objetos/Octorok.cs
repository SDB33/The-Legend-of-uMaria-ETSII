using UnityEngine;

public class Octorok : MonoBehaviour, IReiniciable {

    // Guarda la posición para poder restaurarla (implementación de IReiniciable)
    private Vector3 pos;

    // Velocidad de movimiento
    public float vel;

    // Tiempo mínimo y máximo entre cambios de dirección
    public float tiempoMinCambio;
    public float tiempoMaxCambio;

    // Rigidbody2D para mover el objeto físicamente
    private Rigidbody2D rb;

    // Dirección actual de movimiento
    private Vector2 dir;

    // Temporizador para controlar cuándo cambiar de dirección
    private float tempCambio;

    // Temporizador para controlar el cambio de estado del pico (corto/largo)
    private float tempPico;

    // Tiempo que dura cada estado del pico antes de cambiar
    public float tiempoCambioPico;

    // Sprites para representar el pico corto y largo mirando hacia abajo
    public Sprite abajoPicoCorto;
    public Sprite abajoPicoLargo;

    // Sprites para pico corto y largo mirando hacia la izquierda
    public Sprite izquierdaPicoCorto;
    public Sprite izquierdaPicoLargo;

    // Componente SpriteRenderer para cambiar la imagen visible
    private SpriteRenderer spriteRenderer;

    // Estado actual del pico: true = corto, false = largo
    private bool picoCorto;

    // Guarda la posición actual para poder restaurarla luego
    public void DesarEstat() {
        pos = transform.position;
    }

    // Restaura la posición guardada
    public void RestablirEstat() {
        transform.position = pos;
    }

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        picoCorto = true;

        tempPico = tiempoCambioPico;

        ElegirNuevaDireccion();

        ActualizarSprite();
    }

    void Update() {
        // Reducimos el temporizador para cambiar de dirección
        tempCambio -= Time.deltaTime;

        // Cuando el temporizador llega a cero, elegimos una nueva dirección
        if (tempCambio <= 0f)
            ElegirNuevaDireccion();

        // Reducimos el temporizador para el cambio del pico
        tempPico -= Time.deltaTime;

        // Cuando toca cambiar el estado del pico, alternamos y actualizamos sprite
        if (tempPico <= 0f) {
            picoCorto = !picoCorto;
            tempPico = tiempoCambioPico;
            ActualizarSprite();
        }
    }

    void FixedUpdate() {
        // Aplicamos la velocidad en la dirección actual usando física
        rb.linearVelocity = dir * vel;
    }

    // Método para elegir una nueva dirección aleatoria
    private void ElegirNuevaDireccion() {
        int n = Random.Range(0, 5);

        // Asignamos la dirección según el valor aleatorio
        switch (n) {
            case 0: dir = Vector2.up; break;
            case 1: dir = Vector2.down; break;
            case 2: dir = Vector2.left; break;
            case 3: dir = Vector2.right; break;
            default: dir = Vector2.zero; break; // Quieto (sin moverse)
        }

        // Reiniciamos el temporizador de cambio de dirección con un valor aleatorio entre mínimo y máximo
        tempCambio = Random.Range(tiempoMinCambio, tiempoMaxCambio);

        // Actualizamos el sprite para reflejar la nueva dirección
        ActualizarSprite();
    }

    // Actualiza el sprite mostrado según la dirección y el estado del pico
    private void ActualizarSprite() {
        if (dir == Vector2.up) {
            // Para arriba usamos el sprite de abajo pero con flip vertical
            spriteRenderer.sprite = picoCorto ? abajoPicoCorto : abajoPicoLargo;
            spriteRenderer.flipY = true;
            spriteRenderer.flipX = false;
        }
        else if (dir == Vector2.down) {
            // Para abajo usamos el sprite correspondiente sin flip
            spriteRenderer.sprite = picoCorto ? abajoPicoCorto : abajoPicoLargo;
            spriteRenderer.flipY = false;
            spriteRenderer.flipX = false;
        }
        else if (dir == Vector2.left) {
            // Para izquierda usamos el sprite correspondiente sin flip
            spriteRenderer.sprite = picoCorto ? izquierdaPicoCorto : izquierdaPicoLargo;
            spriteRenderer.flipX = false;
            spriteRenderer.flipY = false;
        }
        else if (dir == Vector2.right) {
            // Para derecha usamos el sprite de izquierda con flip horizontal
            spriteRenderer.sprite = picoCorto ? izquierdaPicoCorto : izquierdaPicoLargo;
            spriteRenderer.flipX = true;
            spriteRenderer.flipY = false;
        }
        else {
            // Cuando la dirección es Vector2.zero (quieto), no hacemos nada
        }
    }

    // Al chocar con otro collider, elegimos una nueva dirección
    void OnCollisionEnter2D(Collision2D col) {
        ElegirNuevaDireccion();
    }
}
