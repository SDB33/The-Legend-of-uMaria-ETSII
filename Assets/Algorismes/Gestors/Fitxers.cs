using System.Collections.Generic;
using System.Windows.Forms;
using UnityEngine;
using System.IO;

[System.Serializable]
public class ObjecteDadesList {
    public List<ObjecteDades> objectes = new List<ObjecteDades>();
}

[System.Serializable]
public class ObjecteDades {
    public string nom;
    public Vector3 posicio;
}

public class Fitxers {

    public void DesarContingut(ArrayCircular historial) {
        ObjecteDadesList PerAlSac = new ObjecteDadesList();
        historial.CopiarActius(PerAlSac);
        string json = JsonUtility.ToJson(PerAlSac);
        string ubicacio = string.Empty;
        
    #if UNITY_STANDALONE_WIN
            ubicacio = DesaALaFinestra();
    #else
            Debug.LogWarning("Guardar JSON ahora solo está disponible en Windows.");
    #endif
        
        if (ubicacio == string.Empty) { return; }
        
        File.WriteAllText(ubicacio, json);
    }

    public void CarregarContingut(ArrayCircular historial) {
        string ubicacio = string.Empty;
    #if UNITY_STANDALONE_WIN
            ubicacio = CarregaALaFinestra();
    #else
            Debug.LogWarning("Cargar JSON ahora solo está disponible en Windows.");
    #endif
        if (ubicacio == string.Empty) { return; }
        string json = File.ReadAllText(ubicacio);

        historial.Netejar();

        crearIDestruirObjecte cons =  new crearIDestruirObjecte();
        foreach (ObjecteDades dades in JsonUtility.FromJson<ObjecteDadesList>(json).objectes) {
            GameObject nou = Object.Instantiate(GSTRodanxes.Magatzem[dades.nom], dades.posicio, Quaternion.identity);
            nou.name = dades.nom;
            cons.ModificaM.Add(nou);
        } 
        if (cons.ModificaM.Count!=0) { historial.introduir(cons); }
    }

    private string CarregaALaFinestra() {
        OpenFileDialog dialeg = new OpenFileDialog();
        dialeg.Title = "Selecciona un archivo";
        dialeg.Filter = "Archivos JSON (*.json)|*.json"; 

        if (dialeg.ShowDialog() == DialogResult.OK) { return dialeg.FileName; }
        return string.Empty;  
    }

    private string DesaALaFinestra() {
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