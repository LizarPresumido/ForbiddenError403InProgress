using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusBehaviour : MonoBehaviour
{
    public int iniciativa;
    public int moveRange;
    private BoundsInt moveArea;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int getIniciative()
    {
        return iniciativa;
    }

    public BoundsInt getMoveArea(Vector3Int position)
    {
        //print(position);
        moveArea.position = position;
        moveArea.size = new Vector3Int(moveRange, moveRange, 1);
        return moveArea;
    }
}
