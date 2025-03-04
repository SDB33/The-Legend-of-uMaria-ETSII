using UnityEngine;

public interface IRemovible { 
    void Concebre(); 
}

public class Abstraccio : MonoBehaviour, IRemovible {

    public Sprite follet;
    public int restants;
    public virtual void Concebre(){}
    
}
