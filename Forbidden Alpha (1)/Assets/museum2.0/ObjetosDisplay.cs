using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjetosDisplay : MonoBehaviour
{
    private objetos objeto;
    public void setInventario(objetos _objetos)
    {
        objeto = _objetos;
    }

    public Text textoNombre;
    public Text textoDescripcion;

    public Image imagen;


    public void Update()
    {
        textoNombre.text = objeto.nombre;
        textoDescripcion.text = objeto.descripcion;

        imagen.sprite = objeto.imagen;

    }
}
