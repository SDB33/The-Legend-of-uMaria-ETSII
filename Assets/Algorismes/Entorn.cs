using UnityEngine;

public class Entorn : Abstraccio {

    public GameObject mao;

    public ModeDeu bestiesa;


    public override void Concebre(){ 
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Instantiate(mao,  new Vector3(0.5f+Mathf.Floor(worldPosition.x),0.5f+Mathf.Floor(worldPosition.y),0f )    , Quaternion.identity); 
    }


    public void PremerBoto() { bestiesa.bestiesa=this; }

    
}
