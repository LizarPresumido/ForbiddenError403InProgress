using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickViruses : MonoBehaviour
{
    //public GameObject virus;
    public GameObject[] viruses = new GameObject[4];
    public GameObject[] listVirus = new GameObject[3];
    public GameObject bigSquare, blckSquare;
    public GameObject info;
    private void OnMouseDown()
    {
        if (this.GetComponent<SpriteRenderer>().enabled)
        {
            bigSquare.GetComponent<SpriteRenderer>().enabled = false;
            blckSquare.GetComponent<SpriteRenderer>().enabled = false;
            

            for (int i = 0; i < viruses.Length; i++)
            {
                viruses[i].GetComponent<SpriteRenderer>().enabled = false;
                viruses[i].GetComponent<BoxCollider2D>().enabled = false;
            }

            for (int i = 0; i < listVirus.Length; i++)
            {
                listVirus[i].GetComponent<SpriteRenderer>().enabled = true;
                listVirus[i].GetComponent<BoxCollider2D>().enabled = true;
            }

            if (this.gameObject == GameObject.Find("l"))
            {
                info.GetComponent<SpriteRenderer>().enabled = true;
            }

        }
    }
}
