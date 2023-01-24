using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New BuildingElement", menuName = "BuildingElement")]
public class BuildingElement: ScriptableObject
{  
    public int Id;
    public string name;
    public Sprite image;
    public int price;
    public UIManager.FloorTag floorTag;
}
