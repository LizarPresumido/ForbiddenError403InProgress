using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotonController : MonoBehaviour
{
    public GameObject seleccionado;
    public GameObject noseleccionado1;
    public GameObject noseleccionado2;

    public GameObject clasePadre;
    public GameObject noClasePadre;

    public GameObject descripcionObjetos;
    public GameObject descripcionPersonajes;
    private void OnMouseDown()
    {
        seleccionado.gameObject.SetActive(true);
        clasePadre.gameObject.SetActive(true);

        noseleccionado1.gameObject.SetActive(false);
        noseleccionado2.gameObject.SetActive(false);
        noClasePadre.gameObject.SetActive(false);


        descripcionObjetos.gameObject.SetActive(false);
        descripcionPersonajes.gameObject.SetActive(false);
    }
}
