using UnityEngine;
using UnityEngine.UI;

public class GSTRodanxes : MonoBehaviour {

    public HorizontalLayoutGroup botifler;
    public RectTransform contenidor;
    public RectTransform tamanoIndividual;

    void Start() {
        
    }

    void Update() {
        Debug.Log( (contenidor.sizeDelta.x/2 - contenidor.localPosition.x) / (tamanoIndividual.sizeDelta.x + botifler.spacing) ); 
        
    }
}
