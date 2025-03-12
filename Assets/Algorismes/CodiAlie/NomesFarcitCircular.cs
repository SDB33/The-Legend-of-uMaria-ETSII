using UnityEngine;
using UnityEngine.UI;

//https://github.com/Satoshi-Hirazawa  Agraïments a aquest usuari per mostrar-me la solució al problema
                                    // Thanks to that user for showing me the solution to the problem

public class NomesFarcitCircular : Image {
    public override bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera) {
        if (!base.IsRaycastLocationValid(screenPoint, eventCamera)) { return false; }
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPoint, eventCamera, out localPoint);
        float clickAngle = Vector2.SignedAngle(localPoint, Vector2.down) + 90f * fillOrigin;
        if (clickAngle < 0) clickAngle += 360f;
        if (!fillClockwise) clickAngle = 360f - clickAngle;
        return clickAngle >= 0 && clickAngle < (360f * fillAmount);
    }
}
