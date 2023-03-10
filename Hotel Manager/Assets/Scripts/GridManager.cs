using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class GridManager : MonoBehaviour
{

    private Grid grid;
    private UIManager uIManager;
    private Material material;
    private Sprite interactionSprite;


    Color color;
    int id;
    int price;

    Transform Objects;
    public int rotationIndex = 0;
    GameObject shadowObject;
    List<GameObject> listInteraction = new List<GameObject>();

    List<BuildingObject> buildingObjects = new List<BuildingObject>();
     


    bool isLine;
    public IsSelected isSelected;
    public enum IsSelected
    {
        none,
        wall,
        floor,
        door,
    }

    Vector2Int startPosition;
    Vector2Int endPosition;
    Text text;
    Shader shader;


    List<Vector4> list = new List<Vector4>();
    public GridManager()
    {
        shader = GameManager.instance.shader;
        text = GameManager.instance.text;
        grid = GameManager.instance.gameGrid;
        material = GameManager.instance.spriteMaterial;
        interactionSprite = GameManager.instance.interactionSprite;
        color = Color.white;
    }

    private void Start()
    {
        Objects = new GameObject("Objects").transform;
        uIManager = GameManager.instance.gameObject.GetComponent<UIManager>();
    }
    public void SetStats(int id,int price)
    {
        this.id = id;
        this.price = price;
    }

    public void SetColor(Color color)
    {
        this.color = color;
        if (shadowObject != null) shadowObject.GetComponent<SpriteRenderer>().material.SetColor("color1", color);
    }
    private void Update()
    {
        text.transform.position = Input.mousePosition;

        switch (isSelected)
        {
            case IsSelected.none:               
                text.gameObject.SetActive(false);
                ClearShadows();
                break;
            case IsSelected.wall:
                ClearShadows();
                BuildWall();
                break;
            case IsSelected.floor:
                ClearShadows();
                BuildFloor();
                break;
            case IsSelected.door:
                BuildObject();
                break;
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
                if (grid.GetMapCell(position.x + i * k, position.y + i * (1 - k)).CanBuildWall(id))
                {
                    if(id == -1)
                    {
                        if(!grid.GetValue(position.x + i * k, position.y + i * (1 - k)).canMove)
                        {
                            list.Add(new Vector4(position.x + i * k, position.y + i * (1 - k), 2, 0));
                        }
                        else
                        {
                            list.Add(new Vector4(position.x + i * k, position.y + i * (1 - k), 0, 0));
                        }
                    }
                    else 
                    {

                        list.Add(new Vector4(position.x + i * k, position.y + i * (1 - k), 0, 0));
                    }
                }
                else
                {
                    if (i == 0 || i == end - 1) costSkipped += 0.5f * price;
                    else if (isLine) costSkipped += 0.5f * price;
                    else costSkipped += price;
                }
            }
        }
        else
        {
            for (int i = 0; i < end; i++)
            {
                if (grid.GetMapCell(position.x - i * k, position.y - i * (1 - k)).CanBuildWall(id))
                {
                    if (id == -1)
                    {
                        if (!grid.GetValue(position.x - i * k, position.y - i * (1 - k)).canMove)
                        {
                            list.Add(new Vector4(position.x - i * k, position.y - i * (1 - k), 2, 0));
                        }
                        else
                        {
                            list.Add(new Vector4(position.x - i * k, position.y - i * (1 - k), 0, 0));
                        }
                    }
                    else
                    {

                        list.Add(new Vector4(position.x - i * k, position.y - i * (1 - k), 0, 0));
                    }

                }else
                {
                    if (i == 0 || i == end - 1) costSkipped += 0.5f * price;
                    else if (isLine) costSkipped += 0.5f * price;
                    else costSkipped += price;
                }

            }
        }

    }
    float costSkipped;
    private void BuildWall()
    {
        text.gameObject.SetActive(true);

        if (Input.GetMouseButton(1) && startPosition != new Vector2Int(-1, -1))
        {
            startPosition = new Vector2Int(-1, -1);
            grid.iconGrid.Clear(list);
            list.Clear();
        }
        else if(Input.GetMouseButton(1))
        {
            isSelected = IsSelected.none;
            uIManager.SwitchOff();
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (!CameraMovement.mouseIsOverUI())
            {
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
            text.text = text.text = 0 + " x " + 0 + "\n" + "$" + 0;
            foreach (Vector4 item in list)
            {

                if (id != -1) grid.SetWall((int)item.x, (int)item.y, id);
                else grid.RemoveWall((int)item.x, (int)item.y);
            }
            grid.iconGrid.Clear(list);
            list.Clear();
        }

        if (Input.GetMouseButton(0) && startPosition != new Vector2Int(-1, -1))
        {
            Vector3 vector3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            grid.GetXY(vector3, out int x, out int y);

            isLine = false;
            if (x < 0) x = 0;
            if (x > grid.width - 1) x = grid.width - 1;
            if (y <= 7) y = 8;
            if (y > grid.height - 1) y = grid.height - 1;

            if (x != endPosition.x || y != endPosition.y)
            {

                grid.iconGrid.Clear(list);
                list.Clear();
                endPosition = new Vector2Int(x, y);
                costSkipped = 0;



                int width = Mathf.Abs(endPosition.x - startPosition.x) + 1;
                int height = Mathf.Abs(endPosition.y - startPosition.y) + 1;

                if (width == 1 || height == 1) isLine = true;

                if (endPosition == startPosition)
                {
                    if (grid.GetMapCell(startPosition.x, startPosition.y).CanBuildObj(isSelected == IsSelected.door))
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
                    cost = width * height * price - Mathf.RoundToInt(costSkipped);
                }
                else
                {
                    cost = (width * 2 + height * 2 - 4) * price - Mathf.RoundToInt(costSkipped);
                }



                text.text = width + " x " + height + "\n" + "$" + (cost).ToString();
                grid.iconGrid.ChangeSprites(list, false);
            }

        }    
    }
    private void BuildFloor()
    {
        text.gameObject.SetActive(true);

        if (Input.GetMouseButton(1) && startPosition != new Vector2Int(-1, -1))
        {
            startPosition = new Vector2Int(-1, -1);
            grid.iconGrid.Clear(list);
            list.Clear();
        }
        else if (Input.GetMouseButton(1))
        {
            isSelected = IsSelected.none;
            uIManager.SwitchOff();
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (!CameraMovement.mouseIsOverUI())
            {
                Vector3 vector3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                grid.GetXY(vector3, out int x, out int y);
                if (x >= 0 && x < grid.width && y > 7 && y < grid.height) startPosition = new Vector2Int(x, y);
                else startPosition = new Vector2Int(-1, -1);
            }
            else
            {
                startPosition = new Vector2Int(-1, -1);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            text.text = text.text = 0 + " x " + 0 + "\n" + "$" + 0;

            for (int i = 0; i < list.Count; i++)
            {
                Vector4 vector4 = list[i];
                list[i] = new Vector4(vector4.x, vector4.y, id, 0);

                grid.ChangeFloor(id, (int)vector4.x, (int)vector4.y);

            }

            grid.floorGrid.ChangeSprites(list, true);
            grid.iconGrid.Clear(list);
            list.Clear();

        }

        if (Input.GetMouseButton(0) && startPosition != new Vector2Int(-1, -1))
        {
            Vector3 vector3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            grid.GetXY(vector3, out int x, out int y);

            isLine = false;
            if (x < 0) x = 0;
            if (x > grid.width - 1) x = grid.width - 1;
            if (y <= 7) y = 8;
            if (y > grid.height - 1) y = grid.height - 1;

            if (x != endPosition.x || y != endPosition.y)
            {
                grid.iconGrid.Clear(list);
                list.Clear();
                endPosition = new Vector2Int(x, y);

                int width = endPosition.x - startPosition.x + 1;
                int height = endPosition.y - startPosition.y + 1;

                if (Mathf.Abs(width) == 1 || Mathf.Abs(height) == 1) isLine = true;

                if (endPosition == startPosition)
                {
                    if (grid.GetMapCell(startPosition.x, startPosition.y).canBuild)
                    {
                        list.Add(new Vector4(startPosition.x, startPosition.y, 1, 0));
                    }
                    else
                    {
                        if (id == -1) list.Add(new Vector4(startPosition.x, startPosition.y, 2, 0));
                    }
                }

                int vx = 0;
                int vy = 0;
                int number = 0;

                if (width < 1)
                {
                    vx = -1;
                    width = Mathf.Abs(width) + 2;
                }
                else
                {
                    vx = 1;
                }

                if (height < 1)
                {
                    vy = -1;
                    height = Mathf.Abs(height) + 2;
                }
                else
                {
                    vy = 1;
                }

                for (int i = 0; i < Mathf.Abs(width); i++)
                {
                    for (int j = 0; j < Mathf.Abs(height); j++)
                    {
                        int px = startPosition.x + i * vx;
                        int py = startPosition.y + j * vy;
                        if (!grid.CheckFloor(id, px, py))
                        {
                            number++;
                            list.Add(new Vector4(px, py, 3, 0));
                        }
                        else
                        {
                            list.Add(new Vector4(px, py, 0, 0));
                        }
                    }
                }
                text.text = Mathf.Abs(width) + " x " + Mathf.Abs(height) + "\n" + "$" + Mathf.Abs(width * height - number).ToString();
                grid.iconGrid.ChangeSprites(list, false);
            }
        }  
    }
    private void BuildObject()
    {
        if (Input.GetMouseButton(1))
        {                  
            isSelected = IsSelected.none;
            uIManager.SwitchOff();
        }
        Vector3 vector3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        grid.GetXY(vector3, out int x, out int y);

        if (id == -1)
        {
            text.gameObject.SetActive(false);
            

            if (Input.GetMouseButton(0) && !CameraMovement.mouseIsOverUI() && grid.CheckObject(x,y))
            {
                grid.RemoveObject(x, y);
            }
        }
        else
        {
            text.gameObject.SetActive(true);
            text.text = "$" + price;

            if (shadowObject == null)
            {
                shadowObject = new GameObject("Object " + id, typeof(SpriteRenderer));
                shadowObject.transform.localScale = new Vector3(3, 3, 1);

                shadowObject.transform.position = grid.GetPosition(x, y);
                shadowObject.GetComponent<SpriteRenderer>().sprite = uIManager.doors[id].images[rotationIndex];
                Material material =  new Material(shader);
                material.SetColor("color1", color);
                shadowObject.GetComponent<SpriteRenderer>().material = material; 
                shadowObject.GetComponent<SpriteRenderer>().color = Color.white;
                shadowObject.GetComponent<SpriteRenderer>().sortingOrder = 10;
            }
            else
            {
                shadowObject.SetActive(true);
                shadowObject.transform.position = grid.GetPosition(x, y);
                shadowObject.GetComponent<SpriteRenderer>().sprite = uIManager.doors[id].images[rotationIndex];
            }

            Vector3 rotation = new Vector3(1, 1, 0);
            switch (rotationIndex)
            {
                case 0:
                    rotation = new Vector3(1, 1, 0);
                    break;
                case 1:
                    rotation = new Vector3(1, -1, 0);
                    break;
                case 2:
                    rotation = new Vector3(-1, -1, 0);
                    break;
                case 3:
                    rotation = new Vector3(-1, 1, 0);
                    break;
            }

            for (int i = 0; i < uIManager.doors[id].interactionSites.Count; i++)
            {
                Vector3 position = (Vector3)uIManager.doors[id].interactionSites[i] * grid.cellSize;
                if (rotationIndex == 1 || rotationIndex == 3)
                {
                    position = new Vector3(position.y, position.x, 0);
                }


                if (listInteraction.Count - 1 < i)
                {
                    GameObject gObject;
                    gObject = new GameObject("Interaction " + id, typeof(SpriteRenderer));
                    gObject.transform.localScale = new Vector3(3, 3, 1);
                    gObject.transform.position = grid.GetPosition(x, y) + new Vector3(position.x * rotation.x, position.y * rotation.y, 0);
                    gObject.GetComponent<SpriteRenderer>().sprite = interactionSprite;
                    gObject.GetComponent<SpriteRenderer>().material = material;
                    listInteraction.Add(gObject);
                }
                else
                {
                    GameObject gObject = listInteraction[i];
                    gObject.SetActive(true);
                    gObject.transform.position = grid.GetPosition(x, y) + new Vector3(position.x * rotation.x, position.y * rotation.y, 0);
                }
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                if (rotationIndex == 3)
                {
                    rotationIndex = 0;
                }
                else
                {
                    rotationIndex++;
                }
            }

            if (Input.GetMouseButton(0) && !CameraMovement.mouseIsOverUI() && grid.CheckBuildObj(x, y,isSelected == IsSelected.door))
            {
                Type type = TypesOfInteractiveObjects.GetTypeInteractiveObjects(uIManager.doors[id].typeOfIO);
                grid.RemoveWall(x, y);
                GameObject obj = new GameObject("Object " + id, typeof(SpriteRenderer), type);
                obj.transform.localScale = new Vector3(3, 3, 1);
                obj.transform.position = grid.GetPosition(x, y);
                obj.transform.parent = Objects;
                obj.GetComponent<Door>().SetValue(x, y, id);
                grid.SetObject(x, y, (InteractiveObject)obj.GetComponent(type));

                obj.GetComponent<SpriteRenderer>().sprite = uIManager.doors[id].images[rotationIndex];
                Material material = new Material(shader);
                material.SetColor("color1", color);
                obj.GetComponent<SpriteRenderer>().material = material;

            }
        }
    }
    private void ClearShadows()
    {
       
       if(shadowObject != null) shadowObject.SetActive(false);

        foreach (GameObject item in listInteraction)
        {
            item.SetActive(false);
        }

    }

}
    




