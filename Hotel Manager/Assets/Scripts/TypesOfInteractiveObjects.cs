using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class TypesOfInteractiveObjects
{
    public enum TypeOfIO
    { 
        door,
        desk,
    }

    static public Type GetTypeInteractiveObjects(TypeOfIO typeOfIO)
    {
        switch(typeOfIO)
        {
            case TypeOfIO.door: return typeof(Door);
            case TypeOfIO.desk: 
                break;
        }
        return null;
    }
}
