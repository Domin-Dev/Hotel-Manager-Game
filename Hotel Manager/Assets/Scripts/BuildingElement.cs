using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New BuildingElement", menuName = "BuildingElement")]
public class BuildingElement: ScriptableObject
{  
    public int Id;
    public string name;
    [HideInInspector]  public Sprite image;
    public int price;
    public List<UIManager.FloorTag> Tags = new List<UIManager.FloorTag>();


    BuildingElement()
    {
        Tags.Add(UIManager.FloorTag.none);
    }
}
