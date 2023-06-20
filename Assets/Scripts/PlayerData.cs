using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class PlayerData 
{
    public VectorValue position;

    public PlayerData(Hero player)
    {
        position = player.pos;
    }
}
