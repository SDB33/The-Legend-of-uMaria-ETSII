using UnityEngine;
using UnityEngine.InputSystem;

public class ModeDeu : MonoBehaviour {

    public IRemovible bestiesa;

    private float alteracio;

    void Update() {
        if (bestiesa==null) {return;}
        if      (alteracio ==  1) { 


            bestiesa.Concebre(); 

            
        }
        else if (alteracio == -1) { bestiesa.Desfer();   }

    }

    public void Alteracio(InputAction.CallbackContext valor) { alteracio = valor.ReadValue<float>(); }


}
