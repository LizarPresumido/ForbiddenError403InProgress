using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetosInventario : MonoBehaviour
{
    public objetos objeto;
    public GameObject objetodisplay;
    public ObjetosDisplay display;

    public void Start()
    {
        objetodisplay.gameObject.SetActive(false);
    }

    private void OnMouseDown()
    {
        display.setInventario(objeto);
        objetodisplay.gameObject.SetActive(true);

    }
}
