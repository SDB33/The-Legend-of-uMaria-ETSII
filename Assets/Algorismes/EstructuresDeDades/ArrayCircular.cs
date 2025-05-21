using System.Collections.Generic;
using UnityEngine;

public class ArrayCircular {

    private Executable[] accions;
    private Dictionary<GameObject, int> referenciats;
    
    private int inici, fi, index;

    public ArrayCircular (int tam) {
        accions = new Executable[tam];
        referenciats = new Dictionary<GameObject, int>();
        inici = 0;
        fi = 0;
        index = 0;
    }

    public void introduir(Executable accio) {
        if (index != fi) { fi = index; }

        foreach (GameObject objecte in accio.ModificaM)  { referenciats[objecte] = referenciats.TryGetValue(objecte, out int comptador) ? comptador + 1 : 1; }
        foreach (GameObject objecte in accio.DesactivaM) { referenciats[objecte] = referenciats.TryGetValue(objecte, out int comptador) ? comptador + 1 : 1; }

        if (accions[fi] != null) { Rebutjar(accions[fi]); }
        accions[fi] = accio;
        fi = (fi + 1) % accions.Length;
        if (fi == inici) { inici = (inici + 1) % accions.Length;  }
        index = fi;
    }

    private void Rebutjar(Executable accioAntiga) {
        foreach (GameObject objecte in accioAntiga.ModificaM) {
            if (referenciats.ContainsKey(objecte)) {
                referenciats[objecte]--;
                if (referenciats[objecte] == 0) {
                    referenciats.Remove(objecte);
                    if (!objecte.activeSelf) { Object.Destroy(objecte);}
                }
            }
        }

        foreach (GameObject objecte in accioAntiga.DesactivaM) {
            if (referenciats.ContainsKey(objecte)) {
                referenciats[objecte]--;
                if (referenciats[objecte] == 0) {
                    referenciats.Remove(objecte);
                    if (!objecte.activeSelf) { Object.Destroy(objecte); }
                }
            }
        }
    }

    public void Rebutjar() { foreach (KeyValuePair<GameObject, int> objecte in referenciats) { Object.Destroy(objecte.Key); } }

    public void Desfer() { 
        if (index == inici) {return;}  
        index = (index - 1 + accions.Length) % accions.Length;
        accions[index].revertir();
    }
    
    public void Refer() { 
        if (index == fi) {return;} 
        accions[index].aplicar();  
        index = (index + 1) % accions.Length; 
    }

    public void CopiarActius(ObjecteDadesList desti) {
        foreach (KeyValuePair<GameObject, int> parella in referenciats) {
            if (parella.Key.activeSelf) { desti.objectes.Add(new ObjecteDades { nom = parella.Key.name, posicio = parella.Key.transform.position }); }
        }
    }

}