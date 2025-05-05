using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Windows.Forms;

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

    public void Rebutjar() {
        foreach (KeyValuePair<GameObject, int> objecte in referenciats) { Object.Destroy(objecte.Key); }
    }


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

    public void CopiarActius(Dictionary<GameObject, int> desti) {
        foreach (KeyValuePair<GameObject, int> parella in referenciats) {
            if (parella.Key.activeSelf) { desti.Add(parella.Key, 0); }
        }
    }

    public void CopiarActius(ObjecteDadesList desti) {
        foreach (KeyValuePair<GameObject, int> parella in referenciats) {
            if (parella.Key.activeSelf) { desti.objectes.Add(new ObjecteDades { nom = parella.Key.name, posicio = parella.Key.transform.position }); }
        }
    }

}

public static class Canvis {

    private static Dictionary<GameObject, int> objectes;

    private static ArrayCircular accions;

    static Canvis() { Reiniciar(); }

    public static void Reiniciar() {
        objectes = new Dictionary<GameObject, int>();
        accions = new ArrayCircular(20);
    }

    public static void introduir(Executable accio) { accions.introduir(accio); }
    public static void Desfer() { accions.Desfer(); }
    public static void Refer() { accions.Refer(); }

    public static void DesarContingut() {
        ObjecteDadesList PerAlSac = new ObjecteDadesList();
        accions.CopiarActius(PerAlSac);
        string json = JsonUtility.ToJson(PerAlSac);
        string ubicacio = string.Empty;
        
    #if UNITY_STANDALONE_WIN
            ubicacio = DesaALaFinestra();
    #else
            Debug.LogWarning("Guardar JSON ahora solo está disponible en Windows.");
    #endif
        
        if (ubicacio == string.Empty) { return;}
        
        File.WriteAllText(ubicacio, json);
    }

    public static void CarregarContingut() {
        string ubicacio = string.Empty;
    #if UNITY_STANDALONE_WIN
            ubicacio = CarregaALaFinestra();
    #else
            Debug.LogWarning("Cargar JSON ahora solo está disponible en Windows.");
    #endif
        if (ubicacio == string.Empty) { return;}
        string json = File.ReadAllText(ubicacio);

        accions.Rebutjar();
        Reiniciar();

        crearIDestruirObjecte cons =  new crearIDestruirObjecte();
        foreach (ObjecteDades dades in JsonUtility.FromJson<ObjecteDadesList>(json).objectes) {
            GameObject nou = Object.Instantiate(GSTRodanxes.Magatzem[dades.nom], dades.posicio, Quaternion.identity);
            nou.name = dades.nom;
            cons.ModificaM.Add(nou);
        } 
        if (cons.ModificaM.Count!=0) { introduir(cons); }
    }

    private static string CarregaALaFinestra() {
        OpenFileDialog dialeg = new OpenFileDialog();
        dialeg.Title = "Selecciona un archivo";
        dialeg.Filter = "Archivos JSON (*.json)|*.json"; 

        if (dialeg.ShowDialog() == DialogResult.OK) { return dialeg.FileName; }
        return string.Empty;  
    }

    private static string DesaALaFinestra() {
        SaveFileDialog dialeg = new SaveFileDialog();
        dialeg.Title = "Guardar archivo JSON";
        dialeg.Filter = "Archivos JSON (*.json)|*.json";
        dialeg.DefaultExt = "json";
        dialeg.AddExtension = true;
        dialeg.FileName = "datos.json"; 
        dialeg.OverwritePrompt = true;  

        if (dialeg.ShowDialog() == DialogResult.OK) { return dialeg.FileName; }
        return string.Empty;
    } 

}

[System.Serializable]
public class ObjecteDadesList {
    public List<ObjecteDades> objectes = new List<ObjecteDades>();
}

[System.Serializable]
public class ObjecteDades {
    public string nom;
    public Vector3 posicio;
}