using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Habilidad2Troyano : Ability
{
    public Habilidad2Troyano()
    {
        nombre = "Caballo de Troya";
        codigo = 2;
        tipoHabilidad = 1;
        rangoHabilidad = new BoundsInt();
    }
    public override void ActivarHabilidad(List<GameObject> characters)
    {

    }

    public override void SetRangoHabilidad(Vector3Int position)
    {
        rangoHabilidad.position = position;
        rangoHabilidad.size = new Vector3Int(1, 1, 1);
    }
}
