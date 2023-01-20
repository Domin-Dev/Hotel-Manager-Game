using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class AI : MonoBehaviour
{

     List<Vector2> path;
     int index;

    private void Update()
    {
        
        if (path != null && index < path.Count)
        {
            Vector2 vector = path[index];
            Vector2 vector2 = Vector2.MoveTowards(transform.position, vector, Time.deltaTime * 6);
            gameObject.transform.position = vector2;
            if (vector2.x == vector.x && vector2.y == vector.y)
            {
                index++;
            }
        }
        else
        {
            Pathfinding.instance.GetPath(gameObject.transform.position,this);  
        }   
    }

    public void SetNewPath(List<Vector2> path)
    {
        index = 0;
        path.Reverse();
        this.path = path;
    }



}
