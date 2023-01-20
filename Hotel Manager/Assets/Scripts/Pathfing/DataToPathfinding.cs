using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class DataToPathfinding
{
    public int2 startPosition;
    public int2 endPosition;
    public AI aI;

    public DataToPathfinding(int2 startPosition,int2 endPosition,AI aI)
    {
        this.startPosition = startPosition;
        this.endPosition = endPosition;
        this.aI = aI;
    }

}
