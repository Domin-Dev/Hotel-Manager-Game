using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    private Grid grid;
    int id;
    int price;
    float ignoredCost;
    bool isLine;
    public IsSelected isSelected;
    public enum IsSelected
    {
        wall,
        floor,
        door,
    }



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

    public void SetWall(int id,int price)
    {
        this.id = id;
        this.price = price;
    }

    private void Update()
    {
        text.transform.position = Input.mousePosition;



        if(Input.GetMouseButton(1))
        {
            Vector3 vector3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            grid.GetXY(vector3, out int x, out int y);
            grid.floorGrid.ChangeSprites(new List<Vector4>() { new Vector4(x,y,id,0), },true);

        }

        if (Input.GetMouseButton(1) && startPosition != new Vector2Int(-1,-1))
        {
            startPosition = new Vector2Int(-1, -1);
            text.gameObject.SetActive(false);
            grid.iconGrid.Clear(list);
            list.Clear();
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (!CameraMovement.mouseIsOverUI())
            {
                text.gameObject.SetActive(true);
                Vector3 vector3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                grid.GetXY(vector3, out int x, out int y);
                startPosition = new Vector2Int(x, y);
            }
            else
            {
                startPosition = new Vector2Int(-1, -1);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            text.gameObject.SetActive(false);
            
            foreach (Vector4 item in list)
            {
                
                if (id != -1) grid.SetWall((int)item.x, (int)item.y, id);
                else grid.RemoveWall((int)item.x,(int)item.y);
            }
            grid.iconGrid.Clear(list);
            list.Clear();           
        }

        if(Input.GetMouseButton(0) && startPosition != new Vector2Int(-1,-1))
        {
            Vector3 vector3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            grid.GetXY(vector3, out int x, out int y);

            isLine = false;
            if (x < 0) x = 0;
            if (x > grid.width -1) x = grid.width -1;
            if (y <= 7) y = 8;
            if (y > grid.height -1) y = grid.height - 1;

            

             if (x != endPosition.x || y != endPosition.y)
               {

                ignoredCost = 0;
                grid.iconGrid.Clear(list);
                list.Clear();
                endPosition = new Vector2Int(x, y);

                int width = Mathf.Abs(endPosition.x - startPosition.x) + 1;
                int height = Mathf.Abs(endPosition.y - startPosition.y) + 1;

                if (width == 1 || height == 1) isLine = true;

                if (endPosition == startPosition)
                {
                    if (grid.GetValue(startPosition.x, startPosition.y).canBuild)
                    {
                        list.Add(new Vector4(startPosition.x, startPosition.y, 1, 0));
                    }
                    else
                    {
                        if (id == -1) list.Add(new Vector4(startPosition.x, startPosition.y, 2, 0));
                    }
                }

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

                int cost;

                if (isLine)
                {
                    cost = width * height * price;
                }
                else
                {
                    cost = (width * 2 + height * 2 - 4) * price;
                }



                text.text = width + " x " + height + "\n" + (cost - ignoredCost).ToString() + " $";
                grid.iconGrid.ChangeSprites(list, false);
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
                    else
                    {
                        if (i == 0 || i == end - 1) ignoredCost = ignoredCost + 0.5f * price;
                        else if (isLine) ignoredCost = ignoredCost + 0.5f * price;
                        else ignoredCost = ignoredCost + price;
                        if (id == -1) list.Add(new Vector4(position.x + i * k, position.y + i * (1 - k), 2, 0));
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < end; i++)
            {
                if (grid.GetValue(position.x - i * k, position.y - i * (1 - k)).canBuild)
                {
                    if (grid.GetValue(position.x - i * k, position.y - i * (1 - k)).canMove) list.Add(new Vector4(position.x - i * k, position.y - i * (1 - k), 0, 0));
                    else
                    {
                        if (i == 0 || i == end - 1) ignoredCost = ignoredCost + 0.5f * price;
                        else if(isLine) ignoredCost = ignoredCost + 0.5f * price;
                        else ignoredCost = ignoredCost + price;
                        if (id == -1) list.Add(new Vector4(position.x - i * k, position.y - i * (1 - k), 2, 0));
                    }
                }
            }
        }

    }    


}
    




