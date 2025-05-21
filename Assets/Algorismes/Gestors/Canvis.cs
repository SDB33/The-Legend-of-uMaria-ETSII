using System.Collections.Generic;
using UnityEngine;

public static class Canvis {

    private static ArrayCircular accions;
    private static Fitxers fitxers;

    static Canvis() { Reiniciar(); }

    public static void Reiniciar() {
        accions = new ArrayCircular(20);
        fitxers = new Fitxers();
    }

    public static void introduir(Executable accio) { accions.introduir(accio); }
    public static void Desfer() { accions.Desfer(); }
    public static void Refer() { accions.Refer(); }
    public static void DesarContingut() { fitxers.DesarContingut(); }
    public static void CarregarContingut() { fitxers.CarregarContingut(); }

    public static void Rebutjar() { accions.Rebutjar(); }
    public static void CopiarActius(ObjecteDadesList PerAlSac) { accions.CopiarActius(PerAlSac); }
    public static void Jugar() {}
    public static void DeixarDeJugar() {}

}



//pulso el boton empezar a jugar
//se desactiva modeDeu y GSTRodanxes
//se activa el trigger de tamaño, la cámara
//se activa el comportamiento de todos los objetos que esten tocando un trigger que representa el pov de la camara
// se marca con 1 a todos aquellos objetos que se hayan modificado (que estén tocando el trigger)


//pulso el botón para dejar de jugar o gano o pierdo
//se reinician todos los objetos (posición, rotación y toda clase de variables)
//se desactiva el comportamiento de todos los objetos
