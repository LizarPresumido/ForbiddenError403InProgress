using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName ="inventario")]
public class InventoryObject : ScriptableObject
{
    public List<InventorySlot> Container = new List<InventorySlot>();
}

public class InventorySlot
{

    public objetos objeto;
    public InventorySlot(objetos _objeto)
    {
        objeto = _objeto;
    }
}