using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Nuevo objeto", menuName = "Museo/objeto")]
public class objetos : MuseoPadre
{
    public void Awake()
    {
        tipo = museoTipo.objeto;
    }
}

