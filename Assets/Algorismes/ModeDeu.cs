using UnityEngine;
using UnityEngine.InputSystem;

public class ModeDeu : MonoBehaviour {

    public IRemovible bestiesa;



    public void Concebre(InputAction.CallbackContext accio) {
        if (!accio.performed) {return;}
        if (bestiesa==null) {return;}
        Debug.Log("a la Laura li agrada sonic");
        bestiesa.Concebre();
        

    }

    public void Desfer(InputAction.CallbackContext accio) {
        if (!accio.performed) {return;}
        if (bestiesa==null) {return;}
        Debug.Log("a la Carla li agrada Tami");
        bestiesa.Desfer();

    }
}
