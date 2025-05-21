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

    public IList<GameObject> ModificaM { get; }
    public IList<GameObject> DesactivaM { get; }

    public crearIDestruirObjecte()
    {
        ModificaM = new List<GameObject>();
        DesactivaM = new List<GameObject>();
    }

    public void aplicar()
    {
        foreach (GameObject desactivat in DesactivaM) { desactivat.SetActive(false); }
        foreach (GameObject construit in ModificaM) { construit.SetActive(true); }
    }
    public void revertir()
    {
        foreach (GameObject construit in ModificaM) { construit.SetActive(false); }
        foreach (GameObject desactivat in DesactivaM) { desactivat.SetActive(true); }
    }

}


public interface IReiniciable {
    void RestablirEstat();
}