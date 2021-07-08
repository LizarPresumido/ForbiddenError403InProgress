using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class Ability
{
    protected string nombre;
    protected int codigo, tipoHabilidad;
    protected BoundsInt rangoHabilidad;

    public abstract void ActivarHabilidad(List<GameObject> characters);

    public abstract void SetRangoHabilidad(Vector3Int position);

    public string GetNombre() {
        return nombre;
    }

    public BoundsInt GetRangoHabilidad()
    {
        return rangoHabilidad;
    }

    public int GetTipo()
    {
        return tipoHabilidad;
    }

    public int GetCodigo()
    {
        return codigo;
    }

    
}
