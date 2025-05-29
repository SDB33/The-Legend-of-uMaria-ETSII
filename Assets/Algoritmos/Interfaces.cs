using System.Collections.Generic;
using UnityEngine;

// Acciones que se pueden aplicar y revertir
public interface Ejecutable {
    public void aplicar();    
    public void revertir();
    
    // Lista de objetos que serán modificados o afectados por la acción
    public IList<GameObject> modificaM { get; }
    
    // Lista de objetos que serán desactivados durante la acción
    public IList<GameObject> desactivaM { get; }
}

// Acción: Movimiento de Objetos
public class MoverObjeto : Ejecutable {

    // Lista para los objetos movidos
    public IList<GameObject> modificaM { get; }
    // Lista para los objetos sobre los que se ha movido otro encima y se han desactivado
    public IList<GameObject> desactivaM { get; }

    // Posiciones inicial y final para la acción de mover
    public Vector3 posIni, posFin;

    // Constructor inicializa las listas vacías
    public MoverObjeto() {
        modificaM = new List<GameObject>();
        desactivaM = new List<GameObject>();
    }

    // Aplica la acción: mueve los objetos a la posición final y desactiva otros objetos
    public void aplicar() {
        foreach (GameObject modificado in modificaM) {
            modificado.transform.position = posFin;
        }
        foreach (GameObject desactivado in desactivaM) {
            desactivado.SetActive(false);
        }
    }

    // Revierte la acción: mueve los objetos a la posición inicial y reactiva los objetos desactivados
    public void revertir() {
        foreach (GameObject modificado in modificaM) {
            modificado.transform.position = posIni;
        }
        foreach (GameObject desactivado in desactivaM) {
            desactivado.SetActive(true);
        }
    }
}

// Clase que implementa la acción de crear y destruir (activar y desactivar) objetos
public class CrearYDestruirObjeto : Ejecutable {

    // Listas para los objetos activados y desactivados
    public IList<GameObject> modificaM { get; }
    public IList<GameObject> desactivaM { get; }

    // Constructor inicializa las listas vacías
    public CrearYDestruirObjeto() {
        modificaM = new List<GameObject>();
        desactivaM = new List<GameObject>();
    }

    // Aplica la acción: desactiva ciertos objetos y activa otros
    public void aplicar() {
        foreach (GameObject desactivado in desactivaM) {
            desactivado.SetActive(false);
        }
        foreach (GameObject construido in modificaM) {
            construido.SetActive(true);
        }
    }

    // Revierte la acción: desactiva los objetos que se activaron y reactiva los que se desactivaron
    public void revertir() {
        foreach (GameObject construido in modificaM) {
            construido.SetActive(false);
        }
        foreach (GameObject desactivado in desactivaM) {
            desactivado.SetActive(true);
        }
    }
}

// Estado: Guardar y restaurar
public interface IReiniciable {
    // Guarda el estado actual (por ejemplo, posición, variables)
    void DesarEstat();

    // Restaura el estado guardado previamente
    void RestablirEstat();
}
