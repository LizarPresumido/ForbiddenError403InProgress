using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CombatManager : MonoBehaviour
{
    public GameObject virusLeader;
    public GameObject[] viruses;
    public Tilemap tilemap;
    private TileBase[] moveArea;
    // Start is called before the first frame update
    void Start()
    {
        orderVirusesByInciative();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            virusLeader.GetComponent<MovementOOC>().setCanMove(false);
            startCombat();
        }
    }

    private void startCombat()
    {
        BoundsInt.PositionEnumerator tiles  = viruses[0].GetComponent<VirusBehaviour>().getMoveArea(tilemap.WorldToCell(viruses[0].transform.position)).allPositionsWithin;
        tiles = tiles.GetEnumerator();
        print(tilemap.WorldToCell(viruses[0].transform.position));
        while (tiles.MoveNext())
        {
            tilemap.SetTileFlags(tiles.Current, TileFlags.None);
            tilemap.SetColor(tiles.Current, new Color(1,0,0,1));
        }
    }

    public void orderVirusesByInciative()
    {
        GameObject virusAux;
        for (int i = 0; i < viruses.Length; ++i)
        {
            for (int j = 0; j < viruses.Length; ++j)
            {
                if (viruses[i].GetComponent<VirusBehaviour>().getIniciative() > viruses[j].GetComponent<VirusBehaviour>().getIniciative())
                {                    
                    virusAux = viruses[i];
                    viruses[i] = viruses[j];
                    viruses[j] = virusAux;
                }
            } 
        }
    }

}
