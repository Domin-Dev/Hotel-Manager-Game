using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    public int x, y;
    public int ID;


    public void SetValue(int x,int y,int ID)
    {
        this.x = x;
        this.y = y;
        this.ID = ID;
    }

    public void Remove()
    {
        Destroy(this.gameObject);
    }
}
