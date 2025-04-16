using System.Collections.Generic;
using UnityEngine;

public interface Executable {
    public void aplicar();
    public void revertir();
    public void rebutjar();
}

public class MoureObjecte : Executable {

    public GameObject mogut, desactivat;
    public Vector3 posIni, posFi;

    public void aplicar() {
        mogut.transform.position = posFi;
        if (desactivat!=null) { desactivat.SetActive(true); }
        
    }
    public void revertir() {
        mogut.transform.position = posIni;
        if (desactivat!=null) { desactivat.SetActive(false); }
    }

    public void rebutjar() {
        if (desactivat!=null) { UnityEngine.Object.Destroy(desactivat); }
    }

}

public class ArrayCircular {

    private Executable[] accions;
    private int inici, fi, index;

    public ArrayCircular (int tam) {
        accions = new Executable[tam];
        inici = 0;
        fi = 0;
        index = 0;
    }

    public void introduir(Executable accio) {
        if (index != fi) { fi = index; }
        if (accions[fi] != null) { accions[fi].rebutjar(); }
        accions[fi] = accio;
        fi = (fi + 1) % accions.Length;
        if (fi == inici) { inici = (inici + 1) % accions.Length;  }
        index = fi;
    }

    public void Desfer() { 
        if (index == inici) {return;} 
        accions[index].revertir(); 
        index = (index - 1 + accions.Length) % accions.Length; 
    }
    public void Refer() { 
        if (index == fi ) {return;} 
        accions[index].aplicar();  
        index = (index + 1) % accions.Length; 
    }

}

public static class Canvis  {

    private static Dictionary<GameObject, int> objectes;

    private static ArrayCircular accions;

    static Canvis() {
        objectes = new Dictionary<GameObject, int>();
        accions = new ArrayCircular(20);
    }

    public static void introduir(Executable accio) {
        accions.introduir(accio);
    }

    public static void Desfer() { 
        accions.Desfer();
    }
    public static void Refer() { 
        accions.Refer();
    }



}
