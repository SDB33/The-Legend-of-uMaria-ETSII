using System.Collections.Generic;
using UnityEngine;

public class ArrayCircular {

    private Ejecutable[] acciones;                          // Array circular de acciones
    private Dictionary<GameObject, int> objetosReferenciados; // Conteo de referencias a objetos activos

    private int inicio, fin, indice;                        // Índices de control de la cola

    public ArrayCircular(int tamaño) {
        acciones = new Ejecutable[tamaño];
        objetosReferenciados = new Dictionary<GameObject, int>();
        inicio = 0;
        fin = 0;
        indice = 0;
    }

    // Introduce una acción en el historial, gestionando los objetos afectados
    public void introducir(Ejecutable accion) {
        if (indice != fin) { fin = indice; }

        foreach (GameObject objeto in accion.modificaM) {
            objetosReferenciados[objeto] = objetosReferenciados.TryGetValue(objeto, out int contador) ? contador + 1 : 1;
        }
        foreach (GameObject objeto in accion.desactivaM) {
            objetosReferenciados[objeto] = objetosReferenciados.TryGetValue(objeto, out int contador) ? contador + 1 : 1;
        }

        if (acciones[fin] != null) {
            Rechazar(acciones[fin]);
        }

        acciones[fin] = accion;
        fin = (fin + 1) % acciones.Length;
        if (fin == inicio) {
            inicio = (inicio + 1) % acciones.Length;
        }
        indice = fin;
    }

    // Elimina una acción del historial y gestiona referencias de los objetos implicados
    private void Rechazar(Ejecutable accionAntigua) {
        foreach (GameObject objeto in accionAntigua.modificaM) {
            if (objetosReferenciados.ContainsKey(objeto)) {
                objetosReferenciados[objeto]--;
                if (objetosReferenciados[objeto] == 0) {
                    objetosReferenciados.Remove(objeto);
                    if (!objeto.activeSelf) { Object.Destroy(objeto); }
                }
            }
        }

        foreach (GameObject objeto in accionAntigua.desactivaM) {
            if (objetosReferenciados.ContainsKey(objeto)) {
                objetosReferenciados[objeto]--;
                if (objetosReferenciados[objeto] == 0) {
                    objetosReferenciados.Remove(objeto);
                    if (!objeto.activeSelf) { Object.Destroy(objeto); }
                }
            }
        }
    }

    // Elimina todos los objetos del historial
    public void RechazarTodo() {
        foreach (KeyValuePair<GameObject, int> par in objetosReferenciados) {
            Object.Destroy(par.Key);
        }
    }

    // Revierte la última acción ejecutada
    public void Deshacer() {
        if (indice == inicio) { return; }
        indice = (indice - 1 + acciones.Length) % acciones.Length;
        acciones[indice].revertir();
    }

    // Reaplica la siguiente acción en el historial
    public void Rehacer() {
        if (indice == fin) { return; }
        acciones[indice].aplicar();
        indice = (indice + 1) % acciones.Length;
    }

    // Copia los objetos activos en una lista de datos serializable
    public void CopiarActivos(ListaDatosObjetos destino) {
        foreach (KeyValuePair<GameObject, int> par in objetosReferenciados) {
            if (par.Key.activeSelf) {
                destino.objetos.Add(new DatoObjeto {
                    nombre = par.Key.name,
                    posicion = par.Key.transform.position
                });
            }
        }
    }

    // Borra completamente el historial y las referencias
    public void Limpiar() {
        RechazarTodo();
        objetosReferenciados.Clear();
        for (int i = 0; i < acciones.Length; i++) {
            acciones[i] = null;
        }
        inicio = 0;
        fin = 0;
        indice = 0;
    }
}
