using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    [HideInInspector]
    public Room room;
    [HideInInspector]
    public int direction;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject.FindGameObjectWithTag("CombatManager").GetComponent<CombatManager>().Spawn(room);
        room.combat = true;
    }
}
