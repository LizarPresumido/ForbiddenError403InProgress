using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MovementOOC : MonoBehaviour
{
    public MoveState moveState;

    // Ultima posición del object que se ha movido el ultimo
    private Vector3Int lastPos;
    // Posicion del jugador en la tilemap
    private Vector3Int tilemapPos;
    //habria que programar que el controlador mandara la orden al jugador de moverse
    [HideInInspector]
    public int[,] mapa;
    public Tilemap grid;
    [HideInInspector]
    public bool canMove = true;
    public bool combatMove = false;

    //parte animaciones
    private Animator animator;
    public bool isLeader;


    // Start is called before the first frame update
    void Start()
    {
        moveState = MoveState.stand;
        // parte animaciones
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (moveState == MoveState.moving)
        {
            Move();
            if (Vector3.Distance(transform.position, grid.GetCellCenterWorld(tilemapPos)) < 0.001f)
                moveState = MoveState.stand;
        }
        else
        {
            animator.SetLayerWeight(1, 0);
        }
        if (combatMove)
            LastMove();

    }

    /// <summary>
    /// Movimiento del jugador fuera de combate que se obtiene al comparar la posiciom +1 con la posicion en la grid y ajustandolo a ello
    /// y por ultimo actualizando la cola de seguidores y la posicion en la grid
    /// </summary>
    public Vector3 SetMove(int code)
    {
        if (moveState == MoveState.stand)
        {
            switch (code)
            {
                case 0:
                    if (mapa[tilemapPos.x, tilemapPos.y + 1] != 0 && notWallTraspassing(mapa[tilemapPos.x, tilemapPos.y + 1]))
                    {
                        tilemapPos += new Vector3Int(0, 1, 0);
                        animator.SetInteger("direccion", 3);
                        animator.SetLayerWeight(1, 1);
                    }
                    break;
                case 1:
                    if (mapa[tilemapPos.x + 1, tilemapPos.y] != 0 && notWallTraspassing(mapa[tilemapPos.x + 1, tilemapPos.y]))
                    {
                        tilemapPos += new Vector3Int(1, 0, 0);
                        animator.SetInteger("direccion", 1);
                        animator.SetLayerWeight(1, 1);
                    }
                    break;
                case 2:
                    if (mapa[tilemapPos.x, tilemapPos.y - 1] != 0 && notWallTraspassing(mapa[tilemapPos.x, tilemapPos.y - 1]))
                    {
                        tilemapPos -= new Vector3Int(0, 1, 0);
                        animator.SetInteger("direccion", 0);
                        animator.SetLayerWeight(1, 1);
                    }
                    break;
                case 3:
                    if (mapa[tilemapPos.x - 1, tilemapPos.y] != 0 && notWallTraspassing(mapa[tilemapPos.x - 1, tilemapPos.y]))
                    {
                        tilemapPos -= new Vector3Int(1, 0, 0);
                        animator.SetInteger("direccion", 2);
                        animator.SetLayerWeight(1, 1);
                    }
                    break;
            }
            moveState = MoveState.moving;
        }
        
        return transform.position;


    }

    private void Move()
    {
        transform.position = Vector3.Lerp(transform.position, grid.GetCellCenterWorld(tilemapPos), Time.deltaTime * 20f);
    }

    public Vector3 GetNewPos()
    {
        return grid.GetCellCenterWorld(tilemapPos);
    }

    private void LastMove()
    {
        if (transform.position != grid.GetCellCenterWorld(tilemapPos))
            transform.position = Vector3.Lerp(transform.position, grid.GetCellCenterWorld(tilemapPos), Time.deltaTime * 20f);
        else
            combatMove = false;
    }


    /// <summary>
    /// Actualiza el array de seguidores tomando la posicion del de adelante y actualizando la suya propia en base a ello
    /// </summary>
    public Vector3 Cola(Vector3 newPos)
    {
        if (moveState == MoveState.stand)
        {
            int code = 999;
            Vector3 direction = newPos - transform.position;
            if (Mathf.Abs(direction.x) >= Mathf.Abs(direction.y))
            {
                if (direction.x > 0)
                    code = 1;
                else
                    code = 3;
            }
            else if (direction.y != direction.x)
            {
                if (direction.y > 0)
                    code = 0;
                else
                    code = 2;
            }
            if (code != 999)
                SetMove(code);

        }
        return transform.position;
    }

    /// <summary>
    /// Setter de una instancia del mapa para comprobar mas tarde si puede moverse
    /// </summary>
    /// <param name="numMap"></param>
    public void setMapa(int[,] numMap)
    {
        mapa = numMap;
    }
    /// <summary>
    /// Setter de control de la posicion del personaje en el tilemap
    /// </summary>
    /// <param name="pos"></param>
    public void setPos(Vector3Int pos)
    {
        tilemapPos = pos;
    }

    public void StartPos()
    {
        tilemapPos = grid.WorldToCell(transform.position);
    }


    /// <summary>
    /// Setter para el control de tipo de movimiento entre el normal y el de combate
    /// </summary>
    /// <param name="state"></param>
    public void setCanMove(bool state)
    {
        canMove = state;
    }

    /// <summary>
    /// getter para obtener la posicion del personaje
    /// </summary>
    /// <returns></returns>
    public Vector3Int getPosition()
    {
        return tilemapPos;
    }

    private bool notWallTraspassing(int movingTile)
    {
        return !((movingTile == 4 && (mapa[tilemapPos.x, tilemapPos.y] != 1 && mapa[tilemapPos.x, tilemapPos.y] != 4)) || ((movingTile != 1 && movingTile != 4) && mapa[tilemapPos.x, tilemapPos.y] == 4));
    }
    public void forceMove()
    {
        combatMove = true;
        canMove = false;
    }
}
    
  
    
