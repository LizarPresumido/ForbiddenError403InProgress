using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject objetodisplay;
    public GameObject personajedisplay;

    public GameObject descripcionObjetos;
    public GameObject descripcionPersonajes;
    public void Start()
    {
        objetodisplay.gameObject.SetActive(false);
        personajedisplay.gameObject.SetActive(false);

        descripcionObjetos.gameObject.SetActive(false);
        descripcionPersonajes.gameObject.SetActive(false);
    }
}
