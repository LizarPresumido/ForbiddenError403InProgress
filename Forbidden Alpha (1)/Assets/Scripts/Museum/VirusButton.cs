using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusButton : MonoBehaviour
{
    public SpriteRenderer button;
    public BoxCollider2D buttonHit;
    public GameObject bigSquare, bigSquareBlack;

    public GameObject[] viruses = new GameObject[4];

    public bool virusIsActive;

    /// <summary>
    /// Activacion de las opciones del menu
    /// </summary>
    private void OnMouseDown()
    {
        if (this.GetComponent<SpriteRenderer>().enabled)
        {
            virusIsActive = true;
            /*bigSquare.GetComponent<SpriteRenderer>().enabled = false;
            bigSquareBlack.GetComponent<SpriteRenderer>().enabled = false;*/
            this.GetComponent<SpriteRenderer>().color = Color.red;

            for (int i = 0; i < viruses.Length; i++)
            {
                viruses[i].GetComponent<SpriteRenderer>().enabled = true;
                viruses[i].GetComponent<BoxCollider2D>().enabled = true;
            }

        }
    }
}
