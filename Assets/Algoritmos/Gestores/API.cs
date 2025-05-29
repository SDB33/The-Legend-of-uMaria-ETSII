using UnityEngine;

public class API : MonoBehaviour {
    public Mariank personaje;

    public void EnviarComando(string comando) {
        switch (comando.ToLower()) {
            case "arriba":
                personaje.Mover(Vector2.up);
                break;
            case "abajo":
                personaje.Mover(Vector2.down);
                break;
            case "izquierda":
                personaje.Mover(Vector2.left);
                break;
            case "derecha":
                personaje.Mover(Vector2.right);
                break;
            case "detener":
                personaje.Detener();
                break;
            default:
                Debug.LogWarning($"[API] Comando desconocido: {comando}");
                break;
        }
    }
}
