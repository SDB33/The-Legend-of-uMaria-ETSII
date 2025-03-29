using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections;
using TMPro;

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
    public Sprite[] follets;
    public int[] posicio;

    public int divisor;

    public Dictionary<string, GameObject> Magatzem;

    public ModeDeu Thopasso;

    private GameObject[] rapida;

    public HorizontalLayoutGroup palet;

    private GameObject[] mares;

    public HorizontalLayoutGroup saltadorMare;
    public GameObject saltadorPrefab;

    public bool esCanviant;

    void Start() {
        BotoSelecArross.cap=this;
        MouSenseUI.cap=this;
        Magatzem = new Dictionary<string, GameObject>();
        rapida = new GameObject[12];
        for (int i=0; i<rapida.Length;i++) {
            rapida[i] = Instantiate(sector, Vector3.zero, Quaternion.identity, palet.transform);
            rapida[i].transform.GetChild(1).gameObject.SetActive(true);
            rapida[i].SetActive(false);
        }

        mares = new GameObject[50]; //hay que ponerle un limite
        for (int i=0; i<noms.Length;i++) { categoritzar(i); } 
        GameObject[] efimer = new GameObject[Mathf.FloorToInt(OnSoc)];    
        for (int i=0; i<efimer.Length;i++) { efimer[i] = mares[i]; }
        mares = efimer;


        contenidor.localPosition =  new Vector3 (322.137f, contenidor.localPosition.y, contenidor.localPosition.z); 
        OnSoc=3f;
        OnVaig=3f;
        mares[3].transform.localScale = new Vector3(1.5f,1.5f,0f);
        for (int k = 0; k < mares[3].transform.childCount-1; k++) { mares[3].transform.GetChild(k).GetComponent<Button>().interactable=true; } 
    }

    private void categoritzar(int vaig) {
        GameObject[] peces = Resources.LoadAll<GameObject>(noms[vaig]); 

        GameObject saltador = Instantiate(saltadorPrefab, Vector3.zero, Quaternion.identity, saltadorMare.transform);
        saltador.GetComponent<Image>().color=colors[vaig];
        saltador.transform.GetChild(1).gameObject.GetComponent<Image>().sprite=follets[vaig];
        saltador.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text=noms[vaig];
        saltador.GetComponent<Button>().onClick.AddListener(() => botar(saltador));
        posicio[vaig] = Mathf.FloorToInt(OnSoc);

        int inici = 0;
        int fi = divisor<peces.Length ? divisor:peces.Length;

        while (inici<fi) {

            GameObject mare = new GameObject("mare", typeof(RectTransform));
            mare.transform.SetParent(botifler.transform);
            mare.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            mares[Mathf.RoundToInt(OnSoc)]= mare;
            OnSoc++;
            
            for (int i=inici; i<fi; i++) {
                GameObject filla = Instantiate(sector, Vector3.zero, Quaternion.identity, mare.transform);
                individu = filla.GetComponent<RectTransform>();
                Magatzem.Add(peces[i].name, peces[i] ); 
                filla.name=peces[i].name;
                filla.transform.localScale = new Vector3(5f,5f,0f);
                filla.transform.Rotate(0f, 0.0f, (360f/(fi-inici))*(i-inici), Space.Self);
                filla.GetComponent<Button>().interactable=false;
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
            fi = divisor<peces.Length-inici ? inici+divisor:peces.Length;

        }
    }

    void Update() {}

    public void ActUbi() { OnSoc = (contenidor.sizeDelta.x/2f - contenidor.localPosition.x) / (individu.sizeDelta.x + botifler.spacing); }

    public IEnumerator EnCanviarValor()  {
        esCanviant=true;

        float signe = Mathf.Sign(OnVaig - OnSoc);

        while (Mathf.Abs(OnSoc-OnVaig) > 0.001f) {
            ActUbi();

            if (OnSoc<0) {
                contenidor.localPosition = new Vector3 (-contenidor.sizeDelta.x/2f+200.01299999999f,contenidor.localPosition.y, contenidor.localPosition.z); 
                OnSoc=mares.Length-1;
                OnVaig=mares.Length-1;
            }
            else if (OnSoc>mares.Length-1) {
                contenidor.localPosition = new Vector3 (contenidor.sizeDelta.x/2f,contenidor.localPosition.y, contenidor.localPosition.z); 
                OnSoc=0;
                OnVaig=0;
            }

            DeCopIVolta(Mathf.RoundToInt(OnSoc-signe),Mathf.RoundToInt(OnSoc));
            contenidor.localPosition =  new Vector3 (Mathf.MoveTowards(contenidor.localPosition.x, - ((individu.sizeDelta.x + botifler.spacing) * Mathf.Round(OnVaig) - contenidor.sizeDelta.x/2), forca * Time.deltaTime),
                                                                       contenidor.localPosition.y,
                                                                       contenidor.localPosition.z); 

            yield return new WaitForSeconds(Time.deltaTime);
        }
        esCanviant=false;
    }

    private void DeCopIVolta (int tancar, int obrir) {
        if (tancar==-1) {tancar=mares.Length-1;}
        else if (tancar==mares.Length) {tancar=0;}
        mares[tancar].transform.localScale = Vector3.MoveTowards(mares[tancar].transform.localScale,new Vector3(0.7f,0.7f,0f), forca * Time.deltaTime); 
        for (int j = 0; j < mares[tancar].transform.childCount-1; j++) { mares[tancar].transform.GetChild(j).GetComponent<Button>().interactable=false; }
        mares[obrir].transform.localScale = Vector3.MoveTowards(mares[obrir].transform.localScale,new Vector3(1.3f,1.3f,0f), forca * Time.deltaTime); 
        for (int j = 0; j < mares[obrir].transform.childCount-1; j++) { mares[obrir].transform.GetChild(j).GetComponent<Button>().interactable=true; }
    }

    public void esquerra() { 
        GameObject futil;
        Magatzem.TryGetValue(mares[Mathf.RoundToInt(OnSoc)].transform.GetChild(mares[Mathf.RoundToInt(OnSoc)].transform.childCount-1).name, out futil);
        if (futil==null) {return;}
        Thopasso.objecte = futil;
        afegirRapida();
    }

    // Esto se debe hacer con una corrutina,
    private void afegirRapida() { 
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

    public void botar(GameObject  boto) {
        if (esCanviant) {return;} 
         OnVaig = posicio[boto.transform.GetSiblingIndex()];
         StartCoroutine(EnCanviarValor());
    }


}