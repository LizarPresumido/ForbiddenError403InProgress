using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum museoTipo
{
    personaje,
    objeto
}

public abstract class MuseoPadre : ScriptableObject
{
    public museoTipo tipo;

    public string nombre;
    [TextArea(5,10)]
    public string descripcion;

    public Sprite imagen;
}
