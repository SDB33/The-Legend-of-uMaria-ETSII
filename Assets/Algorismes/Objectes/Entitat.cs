using UnityEngine;
using System.Collections;

public class Entitat : Objecte {

    void Start () { base.OnMouseDown(); }

    public override void joControloAra () { ModeDeu.estiConstruint = false; }

}
