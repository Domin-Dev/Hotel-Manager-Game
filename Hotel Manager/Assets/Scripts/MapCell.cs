using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCell 
{

    public bool isWall;
    public int wallID;
    public int floorID;
    public InteractiveObject interactiveObject;
    public bool canBuild;

    public MapCell()
    {
        canBuild = true;
        floorID = 0;
        wallID = -1;
    }

    public bool CanBuildObj(bool isDoor)
    {        
        if(isDoor)
        {
            return canBuild && interactiveObject == null;
        }else
        {
            return canBuild && !isWall && interactiveObject == null;
        }

    }
    public bool CanBuildWall(int id)
    {
        return canBuild && id != wallID && interactiveObject == null;
    }
}
