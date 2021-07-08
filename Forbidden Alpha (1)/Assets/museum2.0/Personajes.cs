using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Nuevo personaje", menuName = "Museo/Personaje")]
public class Personajes : MuseoPadre
{
    
    public void Awake()
    {
        tipo = museoTipo.personaje;
        
    }

    //estadisticas
    public string megas;
    public string ataque;
    public string defensa;
    public string energia;
    public string ram;
    public string iniciativa;

}
