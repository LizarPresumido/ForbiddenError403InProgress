using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class personajeDisplay : MonoBehaviour
{
    private Personajes personaje;
    public void setInventario(Personajes _personaje)
    {
        personaje = _personaje;
    }

    public Text textoNombre;
    public Text textoDescripcion;

    public Image imagen;

    public Text megas;
    public Text ataque;
    public Text defensa;
    public Text energia;
    public Text ram;
    public Text iniciativa;

    
    public void Update()
    {
        //personaje = inventario.personaje;
        textoNombre.text = personaje.nombre;
        textoDescripcion.text = personaje.descripcion;

        imagen.sprite = personaje.imagen;

        megas.text = personaje.megas;
        ataque.text = personaje.ataque;
        defensa.text = personaje.defensa;
        energia.text = personaje.energia;
        ram.text = personaje.ram;
        iniciativa.text = personaje.iniciativa;
    }

    
}
