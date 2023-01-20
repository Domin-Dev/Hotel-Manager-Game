using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="New Wall",menuName ="Wall")]
public class Wall : ScriptableObject
{  
    public int wallId;
    public string name;
    public Sprite Image;
    public int price;
}
