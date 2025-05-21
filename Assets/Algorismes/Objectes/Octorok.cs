using UnityEngine;

public class Octorok : MonoBehaviour, IReiniciable {
    public float velocitat;
    public float tempsMinimCanvi;
    public float tempsMaximCanvi;
    private Rigidbody2D cosRigida;
    private Vector2 direccioMoviment;
    private float temporitzadorCanvi;
    private float temporitzadorBec;
    public float tempsCanviBec;
    public Sprite[] follets; // creo que esto se podria hacer static ya que va a ser la misma referencia para todos los objetos
    private SpriteRenderer spriteRenderer;
    private bool becCurt;

    public void RestablirEstat() {
        
    }

    void Start() {
        cosRigida = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        becCurt = false;
        temporitzadorBec = tempsCanviBec;
        TriarNovaDireccio();
        ActualitzarSprite();
    }

    void Update() {
        temporitzadorCanvi -= Time.deltaTime;
        if (temporitzadorCanvi <= 0f) { TriarNovaDireccio(); }

        temporitzadorBec -= Time.deltaTime;
        if (temporitzadorBec <= 0f) {
            becCurt = !becCurt;
            temporitzadorBec = tempsCanviBec;
            ActualitzarSprite();
        }
    }

    void FixedUpdate() { cosRigida.linearVelocity = direccioMoviment * velocitat; }

    private void TriarNovaDireccio() {
        int dir = Random.Range(0, 5);
        switch (dir) {
            case 0: direccioMoviment = Vector2.up; break;
            case 1: direccioMoviment = Vector2.down; break;
            case 2: direccioMoviment = Vector2.left; break;
            case 3: direccioMoviment = Vector2.right; break;
            default: direccioMoviment = Vector2.zero; break;
        }
        temporitzadorCanvi = Random.Range(tempsMinimCanvi, tempsMaximCanvi);
        ActualitzarSprite();
    }

    private void ActualitzarSprite() {
        if (direccioMoviment == Vector2.up) {
            spriteRenderer.sprite = becCurt ? follets[1] : follets[0];
            spriteRenderer.flipY = true;
        } 
        else if (direccioMoviment == Vector2.down) {
            spriteRenderer.sprite = becCurt ? follets[1] : follets[0];
            spriteRenderer.flipY = false;
        } 
        else if (direccioMoviment == Vector2.left) {
            spriteRenderer.sprite = becCurt ? follets[3] : follets[2];
            spriteRenderer.flipX = false;
        } 
        else if (direccioMoviment == Vector2.right) {
            spriteRenderer.sprite = becCurt ? follets[3] : follets[2];
            spriteRenderer.flipX = true;
        } 
        else {}
    }

    void OnCollisionEnter2D(Collision2D colisio) { TriarNovaDireccio(); }


}
