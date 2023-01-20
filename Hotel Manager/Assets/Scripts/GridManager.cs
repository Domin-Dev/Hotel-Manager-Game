using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    private Grid grid;
    public int id;

    Vector2Int startPosition;
    Vector2Int endPosition;
    Text text;


    List<Vector4> list = new List<Vector4>();

    public GridManager()
    {
        text = GameManager.instance.text;
        grid = GameManager.instance.gameGrid;
    }

    int index;
    private void Update()
    {

        if(Input.GetKeyDown(KeyCode.E))
        {
            id++;
        }
        text.transform.position = Input.mousePosition;

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 vector3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            grid.GetXY(vector3, out int x, out int y);
            startPosition = new Vector2Int(x, y);
        }

        if (Input.GetMouseButtonUp(0))
        {
            text.text = "";

            foreach (Vector4 item in list)
            {
                grid.SetWall((int)item.x,(int)item.y,id);
            }


            grid.iconGrid.Clear(list);
            list.Clear();
        }

        if(Input.GetMouseButton(1))
        {
            Vector3 vector3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            grid.GetXY(vector3, out int x, out int y);
            grid.floorGrid.ChangeSprite(x, y, 3, 0);
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 vector3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            grid.GetXY(vector3, out int x, out int y);
            if (x != endPosition.x || y != endPosition.y)
            {

                
                grid.iconGrid.Clear(list);
                list.Clear();
                endPosition = new Vector2Int(x, y);

                int width = Mathf.Abs(endPosition.x - startPosition.x) + 1;
                int height = Mathf.Abs(endPosition.y - startPosition.y) + 1;

                text.text = width + " x " + height + "\n" + (width * height * 20).ToString() + " $";

                if (endPosition.x > startPosition.x)
                {
                    DrawLine(true, startPosition, width, true);
                    DrawLine(false, endPosition, width, true);
                }
                else if (endPosition.x < startPosition.x)
                {
                    DrawLine(false, startPosition, width, true);
                    DrawLine(true, endPosition, width, true);
                }

                if (endPosition.y > startPosition.y)
                {
                    DrawLine(true, startPosition, height, false);
                    DrawLine(false, endPosition, height, false);
                }
                else if (endPosition.y < startPosition.y)
                {
                    DrawLine(false, startPosition, height, false);
                    DrawLine(true, endPosition, height, false);
                }

                grid.iconGrid.ChangeSprites(list,false);
            }
        }

    }

    private void DrawLine(bool isPositive,Vector2Int position,int end,bool isX)
    {
        int k;

        if (isX) k = 1;
        else k = 0;

        if (isPositive)
        {
            for (int i = 0; i < end; i++)
            {
                if (grid.GetValue(position.x + i * k, position.y + i * (1 - k)).canBuild)
                {
                    if (grid.GetValue(position.x + i * k, position.y + i * (1 - k)).canMove) list.Add(new Vector4(position.x + i * k, position.y + i * (1 - k), 0, 0));
                    else list.Add(new Vector4(position.x + i * k, position.y + i * (1 - k), 2, 0));
                }
            }
        }
        else
        {
            for (int i = 0; i < end; i++)
            {
                if (grid.GetValue(position.x - i * k, position.y - i * (1 - k)).canBuild)
                {
                    if (grid.GetValue(position.x - i * k, position.y - i * (1 - k)).canMove) list.Add(new Vector4(position.x - i * k, position.y - i * (1 - k),0,0));
                    else list.Add(new Vector4(position.x - i * k, position.y - i * (1 - k), 2, 0));
                }
            }
        }
    }    



}




