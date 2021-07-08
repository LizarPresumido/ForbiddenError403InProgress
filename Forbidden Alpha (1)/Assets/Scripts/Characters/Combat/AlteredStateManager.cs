using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlteredStateManager
{
    int code;
    int type;
    public int turn;

    public AlteredStateManager(int code, int turn) 
    {
        this.code = code;
        this.turn = turn;
        if (code == 1)
        {
            type = 1;
        }
        else type = 2;
    }
    /// <summary>
    /// ACTUALIZAR  
    /// </summary>
    /// <returns></returns>
    public bool isStackable()
    {
        return type == 1;
    }
    public override bool Equals(object obj)
    {
        
        return code == ((AlteredStateManager)obj).code;
    }

}
