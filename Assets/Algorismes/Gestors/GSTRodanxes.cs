using UnityEngine;
using UnityEngine.UI;

public class GSTRodanxes : MonoBehaviour {

    public HorizontalLayoutGroup botifler;
    public RectTransform contenidor;
    public RectTransform individu;

    private float index;
    public  float forca;

    public GameObject sector;
    public Sprite chisma;

    public string[] noms;
    public Color[] colors;

    public int divisor;

    void Start() {
        for (int i=0; i<noms.Length;i++) { categoritzar(i); }
                 
    }


    private void categoritzar(int vaig) {
        Sprite[] peces = Resources.LoadAll<Sprite>(noms[vaig]); // aixo no s'hauria de fer aixi a causa de "ARBITRARY CODE EXECUTION"

        int inici = 0;
        int fi = divisor<peces.Length ? divisor-1:peces.Length;

        while (inici<fi) {

            GameObject mare = Instantiate(new GameObject("mare", typeof(RectTransform)), Vector3.zero, Quaternion.identity, botifler.transform);
            
            for (int i=inici; i<fi; i++) {
                GameObject filla = Instantiate(sector, Vector3.zero, Quaternion.identity, mare.transform);
                individu = filla.GetComponent<RectTransform>();
                filla.transform.localScale = new Vector3(5f,5f,0f);
                filla.transform.Rotate(0f, 0.0f, (360f/(fi-inici))*(i-inici), Space.Self);
                filla.GetComponent<Image>().fillAmount= 1f/(fi-inici);
                filla.GetComponent<Image>().sprite=chisma; 
                filla.GetComponent<Image>().color=colors[vaig];
                filla.GetComponent<Image>().alphaHitTestMinimumThreshold=0.1f;
                filla.transform.GetChild(0).gameObject.GetComponent<Image>().sprite=peces[i];
                filla.transform.GetChild(0).gameObject.transform.localScale = new Vector3(0.2f,0.2f,0f);
                filla.transform.GetChild(0).gameObject.transform.localPosition = new Vector3(-17.47f,-24.41f,0f); // esto no es valido, ponerlo en el centro
                filla.transform.GetChild(0).gameObject.transform.Rotate(0f, 0.0f, -(360f/(fi-inici))*(i-inici), Space.Self);
            }
            
            GameObject cercle = Instantiate(sector, Vector3.zero, Quaternion.identity, mare.transform);
            cercle.transform.localScale = new Vector3(1f,1f,0f);
            cercle.GetComponent<Image>().sprite=chisma;
            cercle.GetComponent<Image>().color=colors[vaig];
            cercle.transform.GetChild(0).gameObject.transform.localScale = new Vector3(0.5f,0.5f,0f);

            inici+=divisor;
            fi = divisor<peces.Length-inici ? inici+divisor-1:peces.Length;

        }
    }


    // Quitar input.GetMouseButton y poner el new input system

    void Update() {
        index = (contenidor.sizeDelta.x/2f - contenidor.localPosition.x) / (individu.sizeDelta.x + botifler.spacing);

        Debug.Log(index);

        if (Input.GetMouseButton(0)) { return; }

         contenidor.localPosition =  new Vector3 (Mathf.MoveTowards(contenidor.localPosition.x, - ( (individu.sizeDelta.x + botifler.spacing) * Mathf.Round(index) - contenidor.sizeDelta.x/2 ), forca * Time.deltaTime),
                                                  contenidor.localPosition.y,
                                                  contenidor.localPosition.z);  
        
    }
}

