using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class MouSenseUI : ScrollRect { 

    public override void OnBeginDrag(PointerEventData dades) { 
        if (dades.pointerPress==null) { base.OnBeginDrag(dades); } 
    } 
    
}
