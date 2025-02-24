using UnityEngine;
using UnityEngine.Tilemaps;

public class Entorn : Contenidor {

    public Tilemap Terra;
    public Tilemap Paret;
    public Tile Zigot;
    public bool EsPasejable;

    public ModeDeu bestiesa;


    public override void Concebre(){
        if (EsPasejable) {
            Terra.SetTile(Vector3Int.FloorToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition)) ,Zigot);
            Paret.SetTile(Vector3Int.FloorToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition)) ,null);
        }
        else {
            Terra.SetTile(Vector3Int.FloorToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition)) ,null);
            Paret.SetTile(Vector3Int.FloorToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition)) ,Zigot);          
        }
    }

    public override void Desfer(){
        Terra.SetTile(Vector3Int.FloorToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition)) ,null);
        Paret.SetTile(Vector3Int.FloorToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition)) ,null);
    }

    public void PremerBoto() { bestiesa.bestiesa=this; }

    
}
