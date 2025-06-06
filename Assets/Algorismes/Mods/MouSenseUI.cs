using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MouSenseUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
  
    public float veloDesp;  

    public static GSTRodanxes cap;

    private bool PotSer;  
    private bool Es;
    private float comencament;  
    private float moviment;

    void Update() {
        if (!PotSer) { 
            if (!cap.esCanviant && Mathf.Abs(cap.OnSoc - Mathf.RoundToInt(cap.OnSoc)) != 0f) { StartCoroutine(cap.EnCanviarValor()); }
            return; 
        }

        moviment = Mathf.Clamp(Input.mousePosition.x - comencament, -75f, 75f);

        if (!Es && Mathf.Abs(moviment) < 10f) { return; }
        Es = true;

        if (cap.esCanviant) {return;}  

        transform.localPosition += new Vector3(moviment * veloDesp * Time.deltaTime, 0f, 0f);

        if      (Input.mousePosition.x <                5f) { Mouse.current.WarpCursorPosition(new Vector2(Screen.width - 7f, Input.mousePosition.y)); comencament += Screen.width - 12f;   } 
        else if (Input.mousePosition.x > Screen.width - 6f) { Mouse.current.WarpCursorPosition(new Vector2(6f, Input.mousePosition.y));                comencament -= Screen.width - 12f;   } 
        else                                                {                                                                                          comencament = Input.mousePosition.x; }

        cap.ActUbi();
        if (Mathf.Abs(cap.OnSoc - cap.OnVaig) > 0.2f) { cap.OnVaig += Mathf.Sign(cap.OnSoc - cap.OnVaig); StartCoroutine(cap.EnCanviarValor()); }
    }

    public void OnPointerDown(PointerEventData dades) {
        if (dades.button == PointerEventData.InputButton.Right) {return;}
        PotSer = true;
        Es = false;
        comencament = Input.mousePosition.x;
    }

    public void OnPointerUp(PointerEventData dades) {
        if (dades.button == PointerEventData.InputButton.Right) {return;}
        PotSer = false;
        if (!Es) { cap.OnVaig+=Input.mousePosition.x >  Screen.width/2f ? 1f : -1f; StartCoroutine(cap.EnCanviarValor()); }
        Es = false;
    }

    
}