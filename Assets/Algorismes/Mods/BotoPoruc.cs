using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BotoPoruc : Button {

    private int Orientacio; // 0 = dalt, 1 = dreta, 2 = baix, 3 = esquerra
    private bool EsticFora;

    protected override void Start() {
        base.Start();

        Vector2 desplacament = RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position) - new Vector2(Screen.width, Screen.height) / 2f;

        if (Mathf.Abs(desplacament.x) > Mathf.Abs(desplacament.y)) { Orientacio = desplacament.x > 0 ? 1 : 3; } 
        else                                                       { Orientacio = desplacament.y > 0 ? 0 : 2; }
    }

    public override void OnPointerDown(PointerEventData dades) {
        base.OnPointerDown(dades);

        RectTransform rt = transform.parent.GetComponent<RectTransform>();
        int factor = EsticFora ? 1 : -1;

        switch (Orientacio) {
            case 0: rt.anchoredPosition += new Vector2(0f, factor * 2 * rt.rect.y); break;
            case 1: rt.anchoredPosition += new Vector2(factor * 2 * rt.rect.x, 0f); break;
            case 2: rt.anchoredPosition -= new Vector2(0f, factor * 2 * rt.rect.y); break;
            case 3: rt.anchoredPosition -= new Vector2(factor * 2 * rt.rect.x, 0f); break;
        }
        EsticFora = !EsticFora;
    }


}
