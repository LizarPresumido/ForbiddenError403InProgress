using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public InventoryObject inventario;

    public int espacioXEntreObjetos;
    public int numeroColumnas;
    public int espacioYEntreObjetos;
    Dictionary<InventorySlot, GameObject> obgejtosDisplay = new Dictionary<InventorySlot, GameObject>();
    void Start()
    {
        CrearDisplay();
    }

    public void CrearDisplay()
    {
        for(int i=0; i<inventario.Container.Count; i++)
        {
            var obj = Instantiate(inventario.Container[i].objeto, Vector3.zero, Quaternion.identity, transform);
        }
    }
}
