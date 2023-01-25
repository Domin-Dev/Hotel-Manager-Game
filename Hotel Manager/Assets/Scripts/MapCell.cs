using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCell 
{
    public bool isWall;
    public int ID;
    public int floorID;
    public MapCell()
    {
        floorID = 0;
        ID = -1;
    }
}
