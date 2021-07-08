using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Habilidad1Hijacker : Ability
{
    public Habilidad1Hijacker()
    {
        nombre = "Formateo Rapido";
        codigo = 1;
        tipoHabilidad = 2;
        rangoHabilidad = new BoundsInt();
    }
    public override void ActivarHabilidad(List<GameObject> characters)
    {

    }

    public override void SetRangoHabilidad(Vector3Int position)
    {
        rangoHabilidad.position = new Vector3Int(position.x-2,position.y-2,position.z);
        rangoHabilidad.size = new Vector3Int(5, 5, 1);
    }
}
