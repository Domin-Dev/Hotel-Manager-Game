using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public struct PathNode
{
    public int x, y;
    public int index;
    public int comeFromIndex;
    public bool canMove;
    public bool isDoor;




    public int fCost;
    public int gCost;
    public int hCost;

    public void CalculateF()
    {
        fCost = gCost + hCost;
    }

    public override string ToString()
    {
        return x + " " + y;
    }



}
