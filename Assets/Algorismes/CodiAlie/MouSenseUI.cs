using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System.Collections;

public class MouSenseUI : ScrollRect { 

    public static GSTRodanxes cap;
    private bool esClica;
    private bool esCanviant; 

    public override void OnBeginDrag(PointerEventData dades) { 
        if (dades.pointerPress!=null) { return; }

        esClica=false; 
        base.OnBeginDrag(dades); 
    } 

    public override void OnDrag(PointerEventData dades) {
        if (esCanviant) {
            dades.position = Input.mousePosition;
            dades.button = PointerEventData.InputButton.Left;
            base.OnBeginDrag(dades);
        }

        base.OnDrag(dades);
        if (esCanviant) {return;}
        cap.ActUbi();
        if (cap.OnSoc-cap.OnVaig> 0.5f) { StartCoroutine(EnCanviarValor(1f)); }
        if (cap.OnSoc-cap.OnVaig<-0.5f) { StartCoroutine(EnCanviarValor(-1f)); }
    }
    
    public override void OnEndDrag  (PointerEventData dades) { 
        base.OnEndDrag(dades); 
         if (esCanviant) {return;} StartCoroutine(EnCanviarValor(0f));
    }

    private IEnumerator EnCanviarValor(float pos)  {
        esCanviant=true;
        cap.OnVaig= cap.OnVaig + pos ;
        while (Mathf.Abs(cap.OnSoc-cap.OnVaig) > 0.001f) {
            cap.mogudet();
            yield return new WaitForSeconds(Time.deltaTime);
        }
        esCanviant=false;
    }


    public override void OnInitializePotentialDrag(PointerEventData dades) { 
        if (dades.pointerPress==null) { esClica=true; }
        base.OnInitializePotentialDrag(dades);    
    }

    public void deixarClic (InputAction.CallbackContext canvi) { 
        if (!canvi.canceled || !esClica) {return;}
        esClica=false;
        if (esCanviant) {return;} 
        StartCoroutine(EnCanviarValor(Input.mousePosition.x >  Screen.width/2f ? 1f : -1f));           
    }
    
}


