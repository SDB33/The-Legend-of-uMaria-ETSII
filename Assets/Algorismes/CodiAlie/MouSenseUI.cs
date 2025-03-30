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
        if (!PotSer) { return; }

        moviment = Mathf.Clamp(Input.mousePosition.x - comencament, -100, 100);

        if (!Es && Mathf.Abs(moviment) < 50f) { return; }
        Es = true;

        if (cap.esCanviant) {return;}  

        transform.localPosition += new Vector3(moviment * veloDesp * Time.deltaTime, 0f, 0f);

        if      (Input.mousePosition.x <                5f) { Mouse.current.WarpCursorPosition(new Vector2(Screen.width - 7f, Input.mousePosition.y)); comencament += Screen.width - 12f;   } 
        else if (Input.mousePosition.x > Screen.width - 6f) { Mouse.current.WarpCursorPosition(new Vector2(6f, Input.mousePosition.y));                comencament -= Screen.width - 12f;   } 
        else                                                {                                                                                          comencament = Input.mousePosition.x; }

        cap.ActUbi();
        if (Mathf.Abs(cap.OnSoc - cap.OnVaig) > 0.2f) { cap.OnVaig += Mathf.Sign(cap.OnSoc - cap.OnVaig); StartCoroutine(cap.EnCanviarValor()); }
    }

    public void OnPointerDown(PointerEventData eventData) {
        PotSer = true;
        Es = false;
        comencament = Input.mousePosition.x;
    }

    public void OnPointerUp(PointerEventData eventData) {
        PotSer = false;
        if (!Es) { cap.OnVaig+=Input.mousePosition.x >  Screen.width/2f ? 1f : -1f; StartCoroutine(cap.EnCanviarValor()); }
        Es = false;
    }

    
}