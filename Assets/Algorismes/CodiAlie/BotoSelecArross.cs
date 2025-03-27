using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BotoSelecArross : Button, IDragHandler {
    private static bool compartit;
    public static GSTRodanxes cap;
    private static BotoSelecArross ultimBoto; 

    public override void OnPointerDown(PointerEventData dades) { 
        base.OnPointerDown(dades); 
        if (IsActive() && IsInteractable()) { exemple(); compartit = true; ultimBoto = this; }
    }

    public override void OnPointerUp(PointerEventData dades) {
        base.OnPointerUp(dades); 
        if (IsActive() && IsInteractable()) { compartit = false; }
        if (ultimBoto != null && ultimBoto != this) {
            PointerEventData nuevoDades = new PointerEventData(EventSystem.current) { position = dades.position };
            ExecuteEvents.Execute(ultimBoto.gameObject, nuevoDades, ExecuteEvents.pointerUpHandler);
            if (RectTransformUtility.RectangleContainsScreenPoint(ultimBoto.GetComponent<RectTransform>(), Input.mousePosition, dades.enterEventCamera))  {
                ExecuteEvents.Execute(ultimBoto.gameObject, nuevoDades, ExecuteEvents.pointerClickHandler);
            }
            ultimBoto = null;
        }
    }

    public override void OnPointerEnter(PointerEventData dades) {
        if (compartit && IsActive() && IsInteractable()) { 
            exemple();
            ExecuteEvents.Execute(this.gameObject, new PointerEventData(EventSystem.current) { position = dades.position}, ExecuteEvents.pointerDownHandler);
            ultimBoto = this; 
        }    
        base.OnPointerEnter(dades);
    }

    public override void OnPointerExit(PointerEventData dades) {
        if (compartit && IsActive() && IsInteractable()) { base.OnPointerUp(dades); }
        base.OnPointerExit(dades); 
    }

    public override void OnPointerClick(PointerEventData eventData) { cap.esquerra(); }

    public void OnBeginDrag(PointerEventData dades) {}
    public void OnDrag(PointerEventData dades) {}
    public void OnEndDrag(PointerEventData dades) {}

    private void exemple() {
        this.transform.parent.GetChild(this.transform.parent.childCount - 1).GetChild(0).GetComponent<Image>().sprite = this.transform.GetChild(0).GetComponent<Image>().sprite;
        this.transform.parent.GetChild(this.transform.parent.childCount - 1).name = this.name;
    }
}
