using UnityEngine;
using System.Collections;

public class Entitat : Objecte {

    void Start () {  this.GetComponent<SpriteRenderer>().sortingOrder = 1; deu.pucActuar = false; StartCoroutine(MouMe()); }


}
