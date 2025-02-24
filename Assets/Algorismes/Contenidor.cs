using UnityEngine;

public interface IRemovible { 
    void Concebre(); 
    void Desfer(); 
}

public class Contenidor : MonoBehaviour, IRemovible {

    public Sprite follet;
    public int restants;
    public virtual void Concebre(){}
    public virtual void Desfer(){}
    
}
