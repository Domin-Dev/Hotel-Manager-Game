using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class AI : MonoBehaviour
{

   public   List<Vector2> path;
     List<bool> doors;

    
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
                if (index + 1 < doors.Count && doors[index + 1])
                {
                    Door door = GameManager.instance.gameGrid.GetMapCell(path[index + 1]).interactiveObject as Door;
                    if(door != null) door.Open();
                }
                index++;
            }
        }
        else
        {
            Pathfinding.instance.GetPath(gameObject.transform.position,this);  
        }   
    }

    public void SetNewPath(List<Vector2> path, List<bool> doors)
    {
        index = 0;
        path.Reverse();
        doors.Reverse();
        this.path = path;
        this.doors = doors;
        if(doors.Count > 0 && doors[0])
        {
            Door door =  GameManager.instance.gameGrid.GetMapCell(path[0]).interactiveObject as Door;
            if(door != null) door.Open();
        }
    }



}
