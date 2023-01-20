using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] Transform player;
    Grid grid;
    Pathfinding pathfinding;
    [SerializeField] GameObject x;
    [SerializeField] Material material;
    List<PathNode> list;
    int index = 0;

    void Start()
    {
       // pathfinding = new Pathfinding(30, 30, 5f,out grid,material);
    }

    private void Update()
    {
        


        if (Input.GetMouseButtonDown(0))
        {
            if (list != null)
            {
                foreach (PathNode item in list)
                {
                //    grid.gridMesh.ChangeSprite(item.x, item.y, 0);
                }
            }

            Vector3 vector3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            grid.GetXY(vector3, out int x, out int y);
            grid.GetXY(player.position, out int x2, out int y2);
         //   list = pathfinding.FindPath(x2,y2, x, y);
            if (list != null)
            {
                foreach (PathNode item in list)
                {
              //      grid.gridMesh.ChangeSprite(item.x, item.y, 3);                 
                }
            }
            index = 0;
        }
        
      if(Input.GetMouseButton(1))
       {
            Vector3 vector3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            grid.GetXY(vector3, out int x, out int y);
       
     //       grid.gridMesh.ChangeSprite(x, y, 4);
            grid.SetCanMove(x, y,false);
       }


   }
    public TextMesh Make(Vector3 vector3)
    {
       GameObject obj =  Instantiate(x, vector3, Quaternion.identity);
        TextMesh textMesh =  obj.GetComponent<TextMesh>();
        return textMesh;
    }





}
