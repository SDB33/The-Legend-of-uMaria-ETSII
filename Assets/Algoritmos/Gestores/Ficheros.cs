using System.Collections.Generic;
using System.Windows.Forms;
using UnityEngine;
using System.IO;

// Clase serializable que representa una lista de datos de objetos
[System.Serializable]
public class ListaDatosObjetos {
    public List<DatoObjeto> objetos = new List<DatoObjeto>();
}

// Clase serializable que representa los datos de un objeto individual
[System.Serializable]
public class DatoObjeto {
    public string nombre;
    public Vector3 posicion;
}

// Clase encargada de guardar y cargar contenido desde/hacia disco
public class GestorFicheros {

    // Guarda el contenido actual del historial en un archivo JSON
    public void GuardarContenido(ArrayCircular historial) {
        ListaDatosObjetos datosParaGuardar = new ListaDatosObjetos();
        historial.CopiarActivos(datosParaGuardar);
        string json = JsonUtility.ToJson(datosParaGuardar);
        string ruta = string.Empty;

        #if UNITY_STANDALONE_WIN
            ruta = GuardarWindows();
        #else
            Debug.LogWarning("Guardar JSON ahora solo está disponible en Windows.");
        #endif

        if (ruta == string.Empty) { return; }

        File.WriteAllText(ruta, json);
    }

    // Carga contenido desde un archivo JSON y lo inserta en el historial
    public void CargarContenido(ArrayCircular historial) {
        string ruta = string.Empty;

        #if UNITY_STANDALONE_WIN
            ruta = CargarWindows();
        #else
            Debug.LogWarning("Cargar JSON ahora solo está disponible en Windows.");
        #endif

        if (ruta == string.Empty) { return; }

        string json = File.ReadAllText(ruta);
        historial.Limpiar();

        CrearYDestruirObjeto accion = new CrearYDestruirObjeto();
        foreach (DatoObjeto dato in JsonUtility.FromJson<ListaDatosObjetos>(json).objetos) {
            GameObject nuevoObjeto = Object.Instantiate(GestorUI.Magatzem[dato.nombre], dato.posicion, Quaternion.identity);
            nuevoObjeto.name = dato.nombre;
            accion.modificaM.Add(nuevoObjeto);
        }

        if (accion.modificaM.Count != 0) {
            historial.introducir(accion);
        }
    }

    // Abre un cuadro de diálogo para seleccionar el archivo a cargar
    private string CargarWindows() {
        OpenFileDialog dialogo = new OpenFileDialog();
        dialogo.Title = "Selecciona un archivo";
        dialogo.Filter = "Archivos JSON (*.json)|*.json";

        if (dialogo.ShowDialog() == DialogResult.OK) {
            return dialogo.FileName;
        }
        return string.Empty;
    }

    // Abre un cuadro de diálogo para seleccionar la ubicación donde guardar el archivo
    private string GuardarWindows() {
        SaveFileDialog dialogo = new SaveFileDialog();
        dialogo.Title = "Guardar archivo JSON";
        dialogo.Filter = "Archivos JSON (*.json)|*.json";
        dialogo.DefaultExt = "json";
        dialogo.AddExtension = true;
        dialogo.FileName = "datos.json";
        dialogo.OverwritePrompt = true;

        if (dialogo.ShowDialog() == DialogResult.OK) {
            return dialogo.FileName;
        }
        return string.Empty;
    }
}
