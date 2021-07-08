using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Habilidad1Troyano : Ability
{
    public Habilidad1Troyano()
    {
        nombre = "¡A las armas!";
        codigo = 1;
        tipoHabilidad = 1;
        rangoHabilidad = new BoundsInt();
    }
    public override void ActivarHabilidad(List<GameObject> characters)
    {
       
    }

    public override void SetRangoHabilidad(Vector3Int position)
    {
        rangoHabilidad.position = position;
        rangoHabilidad.size = new Vector3Int(1,1,1);
    }
}
