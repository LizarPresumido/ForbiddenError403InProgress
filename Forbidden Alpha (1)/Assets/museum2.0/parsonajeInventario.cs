using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parsonajeInventario : MonoBehaviour
{
    public  Personajes personaje;
    public GameObject personajedisplay;
    public personajeDisplay display;

    public void Start()
    {
        personajedisplay.gameObject.SetActive(false);
    }

    private void OnMouseDown()
    {
        display.setInventario(personaje);
        personajedisplay.gameObject.SetActive(true);
       
    }
    
}