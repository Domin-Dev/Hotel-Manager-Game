using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New BuildingObject", menuName = "BuildingObject")]
public class BuildingObject : ScriptableObject
{
    public int Id;
    public string name; 
    public int price;
    public TypesOfInteractiveObjects.TypeOfIO typeOfIO;
    [Space(height: 30f)]
    public List<Sprite> images = new List<Sprite>();
    public List<UIManager.ObjectTag> Tags = new List<UIManager.ObjectTag>();
    public List<Vector2> interactionSites = new List<Vector2>();
    
     
    BuildingObject()
    {
        Tags.Add(UIManager.ObjectTag.none);
    }
}
