using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;


public class GSTRodanxes : MonoBehaviour {

    public HorizontalLayoutGroup botifler;
    public RectTransform contenidor;
    public RectTransform individu;

    public  float OnSoc;
    public  float OnVaig;
    public  float forca;

    public GameObject sector;
    public Sprite chisma;

    public string[] noms;
    public Color[] colors;

    public int divisor;

    public Dictionary<string, GameObject> Magatzem;

    public ModeDeu Thopasso;

    private GameObject[] rapida;

    public HorizontalLayoutGroup palet;

    private GameObject[] mares;

    void Start() {
        Magatzem = new Dictionary<string, GameObject>();
        rapida = new GameObject[12];
        for (int i=0; i<rapida.Length;i++) {
            rapida[i] = Instantiate(sector, Vector3.zero, Quaternion.identity, palet.transform);
            rapida[i].transform.GetChild(1).gameObject.SetActive(true);
            rapida[i].SetActive(false);
        }

        mares = new GameObject[50]; //hay que ponerle un limite
        

        for (int i=0; i<noms.Length;i++) { categoritzar(i); }

        mogudet();
        OnSoc = 3;
        OnVaig = 3;
        mares[3].transform.localScale = new Vector3(1f,1f,0f);
        BotoSelecArross.cap=this;
        MouSenseUI.cap=this;
        for (int j = 0; j < mares[3].transform.childCount-1; j++) { mares[3].transform.GetChild(j).GetComponent<Button>().interactable=true; } 
           
    }

    private void categoritzar(int vaig) {
        GameObject[] peces = Resources.LoadAll<GameObject>(noms[vaig]); 

        int inici = 0;
        int fi = divisor<peces.Length ? divisor-1:peces.Length;

        while (inici<fi) {

            GameObject mare = new GameObject("mare", typeof(RectTransform));
            mare.transform.SetParent(botifler.transform);
            mare.transform.localScale = new Vector3(1f, 1f, 1f);
            mares[Mathf.RoundToInt(OnSoc)]= mare;
            OnSoc++;
            
            for (int i=inici; i<fi; i++) {
                GameObject filla = Instantiate(sector, Vector3.zero, Quaternion.identity, mare.transform);
                individu = filla.GetComponent<RectTransform>();
                Magatzem.Add(peces[i].name, peces[i] ); 
                filla.name=peces[i].name;
                filla.transform.localScale = new Vector3(5f,5f,0f);
                filla.transform.Rotate(0f, 0.0f, (360f/(fi-inici))*(i-inici), Space.Self);
                filla.GetComponent<Image>().fillAmount= 1f/(fi-inici);
                filla.GetComponent<Image>().sprite=chisma; 
                filla.GetComponent<Image>().color=colors[vaig];
                filla.GetComponent<Image>().alphaHitTestMinimumThreshold=0.1f;
                filla.transform.GetChild(0).gameObject.GetComponent<Image>().sprite=peces[i].GetComponent<SpriteRenderer>().sprite;
                filla.transform.GetChild(0).gameObject.transform.localScale = new Vector3(0.2f,0.2f,0f);
                filla.transform.GetChild(0).gameObject.transform.localPosition = new Vector3(35f*Mathf.Cos((Mathf.PI)/(fi-inici)), 35f*Mathf.Sin((Mathf.PI)/(fi-inici)),0f); 
                filla.transform.GetChild(0).gameObject.transform.Rotate(0f, 0.0f, -(360f/(fi-inici))*(i-inici), Space.Self);
            }
            
            GameObject cercle = Instantiate(sector, Vector3.zero, Quaternion.identity, mare.transform);
            cercle.transform.localScale = new Vector3(1f,1f,0f);
            cercle.GetComponent<Image>().sprite=chisma;
            cercle.GetComponent<Image>().color=colors[vaig];
            cercle.transform.GetChild(0).gameObject.transform.localScale = new Vector3(0.5f,0.5f,0f);
            cercle.GetComponent<Button>().interactable=false;

            inici+=divisor;
            fi = divisor<peces.Length-inici ? inici+divisor-1:peces.Length;

        }
    }

    void Update() {}

    public void ActUbi() {
        OnSoc = (contenidor.sizeDelta.x/2f - contenidor.localPosition.x) / (individu.sizeDelta.x + botifler.spacing);
    }

    public void mogudet() {
        ActUbi();
 
        int i = 0;
        while (mares[i]!=null && i<mares.Length) {
            if (i!=Mathf.RoundToInt(OnSoc)) {
                mares[i].transform.localScale = new Vector3(0.7f,0.7f,0f); 
                for (int j = 0; j < mares[i].transform.childCount-1; j++) { mares[i].transform.GetChild(j).GetComponent<Button>().interactable=false; }
            }
            else {
                mares[i].transform.localScale = new Vector3(1f,1f,0f);
                for (int j = 0; j < mares[i].transform.childCount-1; j++) { mares[i].transform.GetChild(j).GetComponent<Button>().interactable=true; }
            }
            i++;
        }

         contenidor.localPosition =  new Vector3 (Mathf.MoveTowards(contenidor.localPosition.x, - ((individu.sizeDelta.x + botifler.spacing) * Mathf.Round(OnVaig) - contenidor.sizeDelta.x/2), forca * Time.deltaTime),
                                                  contenidor.localPosition.y,
                                                  contenidor.localPosition.z); 


    } 

    public void esquerra() { 
        GameObject futil;
        Magatzem.TryGetValue(mares[Mathf.RoundToInt(OnSoc)].transform.GetChild(mares[Mathf.RoundToInt(OnSoc)].transform.childCount-1).name, out futil);
        if (futil==null) {return;}
        Thopasso.objecte = futil;
        afegirRapida();
    }

    // Esto se debe hacer con una corrutina, en vez de usar currentSelectedGameObject, cuando este implementado esto de solo poder clickar en el circulo disponible, hacerlo con la referencia al circulo
    private void afegirRapida() { // esto se debería hacer con una corrutina
        Transform aux = mares[Mathf.RoundToInt(OnSoc)].transform.GetChild(mares[Mathf.RoundToInt(OnSoc)].transform.childCount-1);
        for (int i=0; i<rapida.Length; i++) {
            if (rapida[i].name==aux.name) {return;}
        }
        for (int i=rapida.Length-1; i>0; i--) {
            rapida[i].SetActive(rapida[i-1].activeSelf);
            rapida[i].name=rapida[i-1].name;
            rapida[i].transform.GetChild(0).gameObject.GetComponent<Image>().sprite=rapida[i-1].transform.GetChild(0).gameObject.GetComponent<Image>().sprite;
            rapida[i].transform.GetChild(1).gameObject.GetComponent<Image>().color=rapida[i-1].transform.GetChild(1).gameObject.GetComponent<Image>().color;
        }
        rapida[0].SetActive(true);
        rapida[0].name = aux.name;
        rapida[0].transform.GetChild(0).gameObject.GetComponent<Image>().sprite=aux.transform.GetChild(0).gameObject.GetComponent<Image>().sprite;
        rapida[0].transform.GetChild(1).gameObject.GetComponent<Image>().color=aux.GetComponent<Image>().color;
    }

    public void BotoTancar() {


    }


}