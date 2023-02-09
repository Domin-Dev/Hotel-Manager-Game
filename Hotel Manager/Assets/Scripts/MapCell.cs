using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCell 
{

    public bool isWall;
    public int ID;
    public int floorID;
    public InteractiveObject interactiveObject;
    public bool canBuild;

    public MapCell()
    {
        canBuild = true;
        floorID = 0;
        ID = -1;
    }

    public bool CanBuild()
    {        
        return canBuild && interactiveObject == null;
    }
}
