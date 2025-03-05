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

        Sprite[] peces = Resources.LoadAll<Sprite>("Terreny");
        GameObject cercle = Instantiate(sector, Vector3.zero, Quaternion.identity, botifler.transform);
        cercle.transform.localScale = new Vector3(3f,3f,0f);
        cercle.GetComponent<Image>().sprite=chisma;
        cercle.GetComponent<Image>().color=Color.cyan;
        cercle.transform.GetChild(0).gameObject.transform.localScale = new Vector3(0.5f,0.5f,0f);

        int sobrante = peces.Length;
        int inicio = 0;
        int fin = sobrante>divisor ? divisor:sobrante;

        for (int i=inicio; i<fin; i++) {
            
            Debug.Log(fin);
            GameObject paco = Instantiate(sector, Vector3.zero, Quaternion.identity, cercle.transform);

            paco.GetComponent<Image>().fillAmount= 1f/fin  ;



            paco.transform.localScale = new Vector3(2f,2f,0f);
            paco.transform.Rotate(0f, 0.0f, (360f/fin)*i, Space.Self);
            paco.GetComponent<Image>().sprite=chisma;
            paco.GetComponent<Image>().color=Color.cyan;
            paco.GetComponent<Image>().alphaHitTestMinimumThreshold=0.1f;
            paco.transform.GetChild(0).gameObject.GetComponent<Image>().sprite=peces[i];
            paco.transform.GetChild(0).gameObject.transform.localScale = new Vector3(0.2f,0.2f,0f);
            paco.transform.GetChild(0).gameObject.transform.localPosition = new Vector3(-17.47f,-24.41f,0f);
            paco.transform.GetChild(0).gameObject.transform.Rotate(0f, 0.0f, -(360f/fin)*i, Space.Self);

        }

        

        //while (sobrante>divisor) {

        //}

        
    }


    // Quitar input.GetMouseButton y poner el new input system

    void Update() {
        index = (contenidor.sizeDelta.x/2f - contenidor.localPosition.x) / (individu.sizeDelta.x + botifler.spacing);


        if (Input.GetMouseButton(0)) { return; }

         contenidor.localPosition =  new Vector3 (Mathf.MoveTowards(contenidor.localPosition.x, - ( (individu.sizeDelta.x + botifler.spacing) * Mathf.Round(index) - contenidor.sizeDelta.x/2 ), forca * Time.deltaTime),
                                                  contenidor.localPosition.y,
                                                  contenidor.localPosition.z);  
        
    }
}

