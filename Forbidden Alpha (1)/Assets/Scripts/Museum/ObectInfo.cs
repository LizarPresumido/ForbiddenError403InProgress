using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObectInfo : MonoBehaviour
{
    public GameObject wahoo;
    public GameObject objectList;
    public GameObject bigSquare, blckSquare;

    private void OnMouseDown()
    {
        if (this.GetComponent<SpriteRenderer>().enabled)
        {
            bigSquare.GetComponent<SpriteRenderer>().enabled = false;
            blckSquare.GetComponent<SpriteRenderer>().enabled = false;

            this.GetComponent<SpriteRenderer>().enabled = false;

            wahoo.GetComponent<SpriteRenderer>().enabled = true;

            objectList.GetComponent<SpriteRenderer>().enabled = true;

        }
    }
}
