using System.Collections.Generic;
using UnityEngine;

public interface Executable {
    public void aplicar();
    public void revertir();
    public IList<GameObject> ModificaM  { get; }
    public IList<GameObject> DesactivaM { get; }

}

public class MoureObjecte : Executable {

    public IList<GameObject> ModificaM  { get; }
    public IList<GameObject> DesactivaM { get; }
    public Vector3 posIni, posFi;

    public MoureObjecte() {
        ModificaM  = new List<GameObject>();  
        DesactivaM = new List<GameObject>();  
    }

    public void aplicar() {
        foreach (GameObject modificat in ModificaM  ) { modificat.transform.position = posFi; }
        foreach (GameObject desactivat in DesactivaM) { desactivat.SetActive(false);  }        
    }
    public void revertir() {
        foreach (GameObject modificat in ModificaM  ) { modificat.transform.position = posIni; }
        foreach (GameObject desactivat in DesactivaM) { desactivat.SetActive(true);  } 
    }

}

public class crearIDestruirObjecte : Executable {

    public IList<GameObject> ModificaM  { get; }
    public IList<GameObject> DesactivaM { get; }

    public crearIDestruirObjecte() {
        ModificaM  = new List<GameObject>();  
        DesactivaM = new List<GameObject>();  
    }

    public void aplicar() {
        foreach (GameObject desactivat in DesactivaM) { desactivat.SetActive(false); }
        foreach (GameObject construit in ModificaM  ) { construit.SetActive(true); } 
    }
    public void revertir() {
        foreach (GameObject construit in ModificaM  ) {  construit.SetActive(false);  }
        foreach (GameObject desactivat in DesactivaM) {  desactivat.SetActive(true); }
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
        if (accions[fi] != null) { Rebutjar(accions[fi], accio); }
        accions[fi] = accio;
        fi = (fi + 1) % accions.Length;
        if (fi == inici) { inici = (inici + 1) % accions.Length;  }
        index = fi;
    }

    private void Rebutjar(Executable accioAntiga, Executable accioNova) {
        HashSet<GameObject> referenciats = new HashSet<GameObject>();
    
        int i = inici;
        while (i != fi) {
            Executable acc = accions[i];
            if (acc != null && acc != accioAntiga) {
                foreach (GameObject objecte in acc.ModificaM) { referenciats.Add(objecte); }
                foreach (GameObject objecte in acc.DesactivaM) { referenciats.Add(objecte); }
            }
            i = (i + 1) % accions.Length;
        }

        foreach (GameObject objecte in accioNova.ModificaM) { referenciats.Add(objecte); }
        foreach (GameObject objecte in accioNova.DesactivaM) { referenciats.Add(objecte); }

        foreach (GameObject objecte in accioAntiga.ModificaM) {
            if (objecte != null && !objecte.activeSelf && !referenciats.Contains(objecte)) {
                Object.Destroy(objecte);
            }
        }
        foreach (GameObject objecte in accioAntiga.DesactivaM) {
            if (objecte != null && !objecte.activeSelf && !referenciats.Contains(objecte)) {
                Object.Destroy(objecte);
            }
        }
    }


    public void Desfer() { 
        if (index == inici) {return;}  
        index = (index - 1 + accions.Length) % accions.Length;
        accions[index].revertir();
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