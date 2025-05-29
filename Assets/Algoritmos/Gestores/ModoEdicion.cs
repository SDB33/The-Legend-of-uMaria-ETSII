using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections;

public class ModoEdicion : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    
    [HideInInspector] public GameObject objetoAEditar;

    private bool estoyDestruyendo, estoyConstruyendo;

    [SerializeField] private GestorUI gstUI;

    private ArrayCircular historial;
    private GestorFicheros gestorFicheros;

    void Start() {
        Application.targetFrameRate = 60; // Mover a archivo de configuración 
        historial = new ArrayCircular(20);
        gestorFicheros = new GestorFicheros();

        Entidad.modoEdicion = this;
    }

    public void OnPointerDown(PointerEventData datos) {
        if (estoyConstruyendo || estoyDestruyendo || datos.button == PointerEventData.InputButton.Middle) return;
         gstUI.PicaParet();
       

        if (datos.button == PointerEventData.InputButton.Left && objetoAEditar != null)
        {
            if (objetoAEditar.GetComponent<Entidad>() != null)
                StartCoroutine(ConstruirEntidad());
            else
                StartCoroutine(ConstruirTerreno());
        }
        else if (datos.button == PointerEventData.InputButton.Right)
        {
            StartCoroutine(Destruir());
        }
    }

    public void OnPointerUp(PointerEventData datos) {
        if (datos.button == PointerEventData.InputButton.Middle) return;
         gstUI.PicaParet();

        if (datos.button == PointerEventData.InputButton.Left) {
            Entidad.puedoMoverme = false;
            estoyConstruyendo = false;
        } else if (datos.button == PointerEventData.InputButton.Right) {
            estoyDestruyendo = false;
        }
    }

    private IEnumerator ConstruirTerreno() {
        if (Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, ~(1 << objetoAEditar.layer)).collider != null)
            yield break;

        estoyConstruyendo = true;
        CrearYDestruirObjeto accion = new CrearYDestruirObjeto();

        while (estoyConstruyendo) {
            RaycastHit2D golpe = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, 1 << objetoAEditar.layer);
            if (golpe.collider != null && golpe.collider.gameObject.name == objetoAEditar.name) {
                yield return new WaitForSeconds(0.001f);
                continue;
            }

            if (golpe.collider != null) {
                golpe.collider.gameObject.SetActive(false);
                accion.desactivaM.Add(golpe.collider.gameObject);
            }

            Vector3 posicionMundo = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GameObject nuevo = Instantiate(objetoAEditar, new Vector3(0.5f + Mathf.Floor(posicionMundo.x), 0.5f + Mathf.Floor(posicionMundo.y), 0f), Quaternion.identity);
            nuevo.name = objetoAEditar.name;
            accion.modificaM.Add(nuevo);

            yield return new WaitForSeconds(0.001f);
        }

        if (accion.modificaM.Count != 0) historial.introducir(accion);
    }

    private IEnumerator ConstruirEntidad() {
        if (Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, 1 << objetoAEditar.layer).collider != null)
            yield break;

        estoyConstruyendo = true;
        CrearYDestruirObjeto accion = new CrearYDestruirObjeto();
        Vector3 posicionMundo = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        GameObject nuevo = Instantiate(objetoAEditar, new Vector3(0.5f + Mathf.Floor(posicionMundo.x), 0.5f + Mathf.Floor(posicionMundo.y), -1f), Quaternion.identity);
        nuevo.name = objetoAEditar.name;
        nuevo.GetComponent<Entidad>().OnMouseDown();
        accion.modificaM.Add(nuevo);

        historial.introducir(accion);

        while (estoyConstruyendo) {
            yield return new WaitForSeconds(0.001f);
        }
    }

    private IEnumerator Destruir() {
        int capaSeleccionada = -1;
        estoyDestruyendo = true;
        RaycastHit2D golpe;

        while (capaSeleccionada == -1 && estoyDestruyendo) {
            golpe = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (golpe.collider != null) capaSeleccionada = golpe.collider.gameObject.layer;
            yield return new WaitForSeconds(0.01f);
        }

        CrearYDestruirObjeto accion = new CrearYDestruirObjeto();

        while (estoyDestruyendo) {
            golpe = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, 1 << capaSeleccionada);
            if (golpe.collider != null) {
                golpe.collider.gameObject.SetActive(false);
                accion.desactivaM.Add(golpe.collider.gameObject);
            }
            yield return new WaitForSeconds(0.001f);
        }

        if (accion.desactivaM.Count != 0) historial.introducir(accion);
    }

    public void Deshacer(InputAction.CallbackContext cambio) {
        if (cambio.performed && !estoyConstruyendo && !gstUI.EsticAlMenu())
            historial.Deshacer();
    }

    public void Rehacer(InputAction.CallbackContext cambio) {
        if (cambio.performed && !estoyConstruyendo && !gstUI.EsticAlMenu())
            historial.Rehacer();
    }

    public void Guardar(InputAction.CallbackContext cambio) {
        if (cambio.performed && !estoyConstruyendo && !gstUI.EsticAlMenu())
            gestorFicheros.GuardarContenido(historial);
    }

    public void Cargar(InputAction.CallbackContext cambio) {
        if (cambio.performed && !estoyConstruyendo && !gstUI.EsticAlMenu())
            gestorFicheros.CargarContenido(historial);
    }

    public void Jugar(InputAction.CallbackContext cambio) {
        if (cambio.performed && !estoyConstruyendo && !gstUI.EsticAlMenu()) {
            transform.parent.GetComponent<PlayerInput>().SwitchCurrentActionMap("Juego");
            gstUI.enabled = false;
            transform.parent.GetChild(1).gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    public void Introducir(Ejecutable accion) {
        historial.introducir(accion);
    }
}
