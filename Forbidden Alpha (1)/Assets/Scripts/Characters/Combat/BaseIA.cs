using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BaseIA : MonoBehaviour
{
    Animator animator;
    BattleBehaviour bb;
    Vector3Int currentPosition;
    Tilemap tilemap;
    GameObject target = null;
    Vector3Int targetPos;
    BoundsInt actionArea;
    GameObject[] characters;
    public bool inMovement;

    public enum iaState
    {
        attack,
        move,
        ability,
        endTurn
    }

    public iaState state;

    private void Start()
    {
        bb = GetComponent<BattleBehaviour>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(inMovement)
        {
            Move();
        }
    }
    protected void Movement()
    {
        Vector3Int movePos;
        BoundsInt.PositionEnumerator tiles = actionArea.allPositionsWithin;
        tiles = tiles.GetEnumerator();
        tiles.MoveNext();
        movePos = Vector3Int.zero;
        while (tiles.MoveNext())
        {
            if (Vector3Int.Distance(targetPos, tiles.Current) < Vector3Int.Distance(targetPos, movePos) && availableTile(tiles.Current) && ((currentPosition.y-tiles.Current.y) == 0 || (currentPosition.x - tiles.Current.x) == 0))
                movePos = tiles.Current;
        }
        if ((currentPosition.y - movePos.y) == 1)
        {
            animator.SetInteger("direccion", 0);
            animator.SetLayerWeight(1, 1);
        }
        else if ((currentPosition.x - movePos.x) == 1)
        {
            animator.SetInteger("direccion", 2);
            animator.SetLayerWeight(1, 1);
        }
        else if ((currentPosition.y - movePos.y) == -1)
        {
            animator.SetInteger("direccion", 3);
            animator.SetLayerWeight(1, 1);
        }
        else
        {
            animator.SetInteger("direccion", 1);
            animator.SetLayerWeight(1, 1);
        }
        if ( movePos != Vector3Int.zero && bb.currentRam > 0 && availableTile(movePos))
        {
            currentPosition = movePos;
            bb.inAction = true;
            inMovement = true;
            actionArea = bb.getMoveArea(currentPosition);
            --bb.currentRam;
        }
        else
        {
            state = iaState.endTurn;
        }
    }

    public void Move()
    {
        transform.position = Vector3.Lerp(transform.position, tilemap.GetCellCenterWorld(currentPosition), Time.deltaTime * 20f);
        if (Vector3.Distance(transform.position, tilemap.GetCellCenterWorld(currentPosition)) < 0.001f)
        {
            inMovement = false;
            bb.inAction = false;
        }
    }

    protected void Attack()
    {
        if (bb.currentRam > 0)
        {
            target.GetComponent<BattleBehaviour>().GetHit(bb.attack);
            if (target.GetComponent<BattleBehaviour>().currentMegas <= 0)
                SetNewTarget();
            --bb.currentRam;
        } else
            state = iaState.endTurn;
    }

    protected void CastAbility(Vector3Int characterPosition /*Lista de habilidades*/, int ram)
    {
        if (ram >= 0) return;
    }

    public void setAction()
    {
        if (Vector3Int.Distance(currentPosition, tilemap.WorldToCell(target.transform.position)) > 1)
            Movement();
        else
            Attack();
    }

    public void SetTarget(GameObject[] chars, Tilemap t)
    {
        characters = chars;
        tilemap = t;
        currentPosition = t.WorldToCell(transform.position);
        actionArea = bb.getMoveArea(currentPosition);
        for (int i = 0; i < characters.Length; ++i)
        {
            if(characters[i] != null)
            {
                if (!characters[i].GetComponent<BattleBehaviour>().isEnemy)
                {
                    if (target == null)
                        target = characters[i];
                    else if(Vector3.Distance(transform.position, characters[i].transform.position) < Vector3.Distance(transform.position, target.transform.position))
                        target = characters[i];
                    else if(target != characters[i] && Vector3.Distance(transform.position, characters[i].transform.position) == Vector3.Distance(transform.position, target.transform.position))
                        if(Random.Range(0,1) == 0) target = characters[i];
                }
            }
        }
        targetPos = tilemap.WorldToCell(target.transform.position);
    }
    public void SetNewTarget()
    {
        actionArea = bb.getMoveArea(currentPosition);
        for (int i = 0; i < characters.Length; ++i)
        {
            if (characters[i] != null)
            {
                if (!characters[i].GetComponent<BattleBehaviour>().isEnemy)
                {
                    if (target == null)
                        target = characters[i];
                    else if (Vector3.Distance(transform.position, characters[i].transform.position) < Vector3.Distance(transform.position, target.transform.position))
                        target = characters[i];
                    else if (target != characters[i] && Vector3.Distance(transform.position, characters[i].transform.position) == Vector3.Distance(transform.position, target.transform.position))
                        if (Random.Range(0, 1) == 0) target = characters[i];
                }
            }
        }
        targetPos = tilemap.WorldToCell(target.transform.position);
    }

    private bool availableTile(Vector3Int p1)
    {
        //Añadir condiciones de no pegar desde muros a camino y viceversa(?)
        bool available = true;
        for (int i = 0; i < characters.Length && available; ++i)
        {
            if (characters[i] != null && (Vector3.Distance(characters[i].transform.position,tilemap.GetCellCenterWorld(p1)) < 0.5f || !tilemap.HasTile(p1)))
                available = false;
        }
        return available;
    }
}
