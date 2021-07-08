using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MovementOOC : MonoBehaviour
{
    // Segidores del primer personaje para manejar el movimiento de la cola
    public Transform[] seguidores = new Transform[3];
    // Ultima posición del object que se ha movido el ultimo
    Vector3 lastPos;
    // Posicion del jugador en la tilemap
    private Vector3Int tilemapPos;
    //habria que programar que el controlador mandara la orden al jugador de moverse
    private int[,] mapa;
    public Tilemap grid;
    bool canMove = true;

    // Start is called before the first frame update
    void Start()
    {
        // Ajustamos al jugador a la grid para que este centrado
        tilemapPos = grid.WorldToCell(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        //Se checkea que el jugador este en una situación en la que se puede mover
        if (canMove)
            Mover();

    }

    /// <summary>
    /// Movimiento del jugador fuera de combate que se obtiene al comparar la posiciom +1 con la posicion en la grid y ajustandolo a ello
    /// y por ultimo actualizando la cola de seguidores y la posicion en la grid
    /// </summary>
    void Mover()
    {

        if (Input.GetKeyDown(KeyCode.D))
        {
            if (mapa[tilemapPos.x + 1, tilemapPos.y] != 0)
            {
                lastPos = transform.position;
                tilemapPos += new Vector3Int(1, 0, 0);
                Cola();
            }

        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (mapa[tilemapPos.x - 1, tilemapPos.y] != 0)
            {
                lastPos = transform.position;
                tilemapPos -= new Vector3Int(1, 0, 0);
                Cola();
            }
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (mapa[tilemapPos.x, tilemapPos.y + 1] != 0)
            {
                lastPos = transform.position;
                tilemapPos += new Vector3Int(0, 1, 0);
                Cola();
            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (mapa[tilemapPos.x, tilemapPos.y - 1] != 0)
            {
                lastPos = transform.position;
                tilemapPos -= new Vector3Int(0, 1, 0);
                Cola();
            }
        }


        transform.position = grid.GetCellCenterWorld(tilemapPos);

    }

    /// <summary>
    /// Actualiza el array de seguidores tomando la posicion del de adelante y actualizando la suya propia en base a ello
    /// </summary>
    void Cola()
    {
        for (int i = 0; i < seguidores.Length; ++i)
        {
            Vector3 temp = seguidores[i].position;
            seguidores[i].position = lastPos;
            lastPos = temp;
            transform.position = grid.GetCellCenterWorld(tilemapPos);
        }
    }

    public void setMapa(int[,] numMap)
    {
        mapa = numMap;
    }
    public void setPos(Vector3Int pos)
    {
        tilemapPos = pos;
    }



    public void setCanMove(bool state)
    {
        canMove = state;
    }

    public Vector3Int getPosition()
    {
        return tilemapPos;
    }
}