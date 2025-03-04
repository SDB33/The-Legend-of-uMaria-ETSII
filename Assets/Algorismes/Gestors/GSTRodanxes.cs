using UnityEngine;
using UnityEngine.UI;

public class GSTRodanxes : MonoBehaviour {

    public HorizontalLayoutGroup botifler;
    public RectTransform paco;
    public RectTransform tamanoIndividual;

    void Start() {
        
    }

    void Update() {
        Debug.Log( (paco.sizeDelta.x/2 - paco.localPosition.x) / (tamanoIndividual.sizeDelta.x + botifler.spacing) ); 
        
    }
}
