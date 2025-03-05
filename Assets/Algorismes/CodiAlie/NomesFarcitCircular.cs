using UnityEngine;
using UnityEngine.UI;

//https://github.com/Satoshi-Hirazawa  Agraïments a aquest usuari per mostrar-me la solució al problema
                                    // Thanks to that user for showing me the solution to the problem


public class NomesFarcitCircular : Image {
    public override bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera) {
        bool result = base.IsRaycastLocationValid(screenPoint, eventCamera);
        if (!result) { return false; }
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPoint, eventCamera, out localPoint);
        float clickAngle = Vector2.SignedAngle(localPoint, new Vector2(0, -1));
        if (clickAngle < 0) clickAngle += 360;
        return (clickAngle >= 0) && (clickAngle < (360f * fillAmount));
    }
}