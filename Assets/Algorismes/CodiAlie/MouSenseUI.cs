using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MouSenseUI : ScrollRect { 

    public static GSTRodanxes cap;
    private bool esClica; 

    public override void OnBeginDrag(PointerEventData dades) { 
        if (dades.pointerPress!=null) { return; }
        esClica=false; 
        base.OnBeginDrag(dades); 
    } 

    public override void OnDrag(PointerEventData dades) {
        if (cap.esCanviant) {
            dades.position = Input.mousePosition;
            dades.button = PointerEventData.InputButton.Left;
            base.OnBeginDrag(dades);
        }
        base.OnDrag(dades);
        if (cap.esCanviant) {return;}
        cap.ActUbi();
        if (Mathf.Abs(cap.OnSoc - cap.OnVaig) > 0.2f) { 
            cap.OnVaig += Mathf.Sign(cap.OnSoc - cap.OnVaig);
            StartCoroutine(cap.EnCanviarValor());    
        }
    }
    
    public override void OnEndDrag  (PointerEventData dades) { 
        base.OnEndDrag(dades); 
         if (cap.esCanviant) {return;} StartCoroutine(cap.EnCanviarValor());
    }

    public override void OnInitializePotentialDrag(PointerEventData dades) { 
        if (dades.pointerPress==null) { esClica=true; }
        base.OnInitializePotentialDrag(dades);    
    }

    public void deixarClic (InputAction.CallbackContext canvi) { 
        if (!canvi.canceled || !esClica) {return;}
        esClica=false;
        if (cap.esCanviant) {return;} 
        cap.OnVaig+=Input.mousePosition.x >  Screen.width/2f ? 1f : -1f;
        StartCoroutine(cap.EnCanviarValor());            
    }
    
}