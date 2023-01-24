using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    public GridMesh floorGrid;
    public GridMesh wallGrid;
    public GridMesh iconGrid;
    public int height{ private set; get; }
    public int width { private set; get; }
    public float cellSize { private set; get; }
    private PathNode[,] gridArray;
    private MapCell[,] gridWalls;

    private GameObject obj;
    public Vector3 gridPosition{ private set; get; }

    public Grid(int width, int height, float cellSize, Vector3 gridPosition, Material material, Material material1,Material material2)
    {
        this.gridPosition = gridPosition;
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridArray = new PathNode[width, height];
        gridWalls = new MapCell[width, height];

        for (int i = 0; i < gridArray.GetLength(0); i++)
        {
            for (int j = 0; j < gridArray.GetLength(1); j++)
            {
                gridArray[i, j] = new PathNode { x = i, y = j, canMove = true,canBuild = true};
                gridWalls[i, j] = new MapCell();
            }
        }
        floorGrid = new GridMesh(width, height, cellSize, material, gridPosition + new Vector3(0, 0,1), true);
        wallGrid = new GridMesh(width, height, cellSize, material1, gridPosition , false);
        iconGrid = new GridMesh(width, height, cellSize, material2, gridPosition , false);


        List<Vector4> list = new List<Vector4>();
        for (int i = 0; i < width; i++)
        {
            list.Add(new Vector4(i, 1,4, 0));
            list.Add(new Vector4(i, 2,5, 0));
            list.Add(new Vector4(i, 3,5, 0));
            list.Add(new Vector4(i, 4,6, 0));
            list.Add(new Vector4(i, 5,5, 0));
            list.Add(new Vector4(i, 6,5, 0));
            list.Add(new Vector4(i, 7,4, 0));
            for (int j = 0; j < 8; j++)
            {
                gridArray[i, j].canBuild = false; 
            }
        }
        floorGrid.ChangeSprites(list,true);


        
    }

    public Vector3 GetWorldPosition(float x, float y)
    {
        return new Vector3(x, y) * cellSize + gridPosition;
    }

    public void GetXY(Vector3 vector3, out int x,out int y)
    {
        x = Mathf.FloorToInt((vector3.x - gridPosition.x) / cellSize);
        y = Mathf.FloorToInt((vector3.y  - gridPosition.y) / cellSize);
    }

    public void SetValue(int x, int y, PathNode value)
    {
        if(x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;
        }
    }

    public void SetValue(Vector3 vector3, PathNode value)
    {
        GetXY(vector3,out int x,out int y);
        SetValue(x, y, value);
    }

    public PathNode GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
          return gridArray[x, y];
        }
        else
        {
            return new PathNode { x = -1, y = -1 };
        }
    }

    public PathNode GetValue(Vector3 vector)
    {
        GetXY(vector, out int x, out int y);
        return GetValue(x, y);
    }

    public void SetCanMove(int x,int y,bool value)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
             gridArray[x, y].canMove = value;
        }
    }

    private bool GetIsWall(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
           return gridWalls[x, y].isWall;
        }
        else
        {
            return false;
        }
    }

    public Vector3 GetPosition(float x, float y)
    {
        return new Vector3(x + 0.5f, y + 0.5f) * cellSize + gridPosition;
    }

    public Vector2 GetLimit()
    {
        return GetPosition(width, height);
    }

    public void SetWall(int x,int y,int id)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridWalls[x, y].isWall = true;
            gridWalls[x, y].wallID = id;
            gridArray[x, y].canMove = false;
            SetWallSprite(x,y,id);
        }
        else
        {
            return;
        }
    }

    private bool[] GetNeighbours(int x,int y)
    {
        bool[] neighbours = new bool[4];
        neighbours[0] = GetIsWall(x + 1,y); //right
        neighbours[1] = GetIsWall(x - 1,y); //left
        neighbours[2] = GetIsWall(x, y + 1);//up
        neighbours[3] = GetIsWall(x, y - 1);//down

        return neighbours;
    }

    private void SetWallSprite(int x,int y,int id)
    {
        bool[] neighbours = GetNeighbours(x, y);
        int number = 0;
        List<Vector4> list = new List<Vector4>();


        for (int i = 0; i < 4; i++)
        {
            if (neighbours[i]) number++;
        }

        if(number == 0)
        {
            list.Add(new Vector4(x, y, 3, id));
        }
        else if (number == 1)
        {
            if (neighbours[0]) { list.Add(new Vector4(x, y, 1, id)); list.Add(SetWallSprite2(x + 1,  y));goto end; }
            if (neighbours[1]) { list.Add(new Vector4(x, y, 2, id)); list.Add(SetWallSprite2(x - 1,  y));goto end; }
            if (neighbours[2]) { list.Add(new Vector4(x, y, 8, id)); list.Add(SetWallSprite2(x , y + 1));goto end; }
            if (neighbours[3]) { list.Add(new Vector4(x, y, 6, id)); list.Add(SetWallSprite2(x , y - 1));goto end; }
        }
        else if(number == 2)
        {
            if(neighbours[0] && neighbours[1]) { list.Add(new Vector4(x, y, 0, id));  list.Add(SetWallSprite2(x + 1, y));  list.Add(SetWallSprite2(x - 1, y)); goto end;}
            if(neighbours[0] && neighbours[2]) { list.Add(new Vector4(x, y, 5, id));  list.Add(SetWallSprite2(x + 1 ,y));  list.Add(SetWallSprite2(x, y + 1)); goto end;}
            if(neighbours[0] && neighbours[3]) { list.Add(new Vector4(x, y, 10, id)); list.Add(SetWallSprite2(x + 1, y)); list.Add(SetWallSprite2(x, y - 1)); goto end;}
            
            if (neighbours[1] && neighbours[2]) { list.Add(new Vector4(x, y, 4, id)); list.Add(SetWallSprite2(x - 1, y)); list.Add(SetWallSprite2(x , y + 1)); goto end;}
            if (neighbours[1] && neighbours[3]) { list.Add(new Vector4(x, y, 9, id)); list.Add(SetWallSprite2(x - 1, y)); list.Add(SetWallSprite2(x , y - 1)); goto end;}

            if (neighbours[2] && neighbours[3]) { list.Add(new Vector4(x, y, 7, id)); list.Add(SetWallSprite2(x , y + 1)); list.Add(SetWallSprite2(x, y - 1)); goto end;}
        }
        else if(number == 3)
        {
            if (neighbours[0] && neighbours[1] && neighbours[2]) { list.Add(new Vector4(x, y, 15, id)); list.Add(SetWallSprite2(x + 1, y)); list.Add(SetWallSprite2(x - 1, y)); list.Add(SetWallSprite2(x , y + 1)); goto end; }
            if (neighbours[0] && neighbours[1] && neighbours[3]) { list.Add(new Vector4(x, y, 12, id)); list.Add(SetWallSprite2(x + 1, y)); list.Add(SetWallSprite2(x - 1, y)); list.Add(SetWallSprite2(x , y - 1)); goto end; }          
            if (neighbours[0] && neighbours[2] && neighbours[3]) { list.Add(new Vector4(x, y, 11, id)); list.Add(SetWallSprite2(x + 1, y)); list.Add(SetWallSprite2(x , y + 1)); list.Add(SetWallSprite2(x , y - 1)); goto end; }
            if (neighbours[1] && neighbours[2] && neighbours[3]) { list.Add(new Vector4(x, y, 14, id)); list.Add(SetWallSprite2(x - 1, y)); list.Add(SetWallSprite2(x, y + 1)); list.Add(SetWallSprite2(x , y - 1)); goto end; }

        }
        else if(number == 4)
        {
            list.Add(new Vector4(x, y, 13, id)); list.Add(SetWallSprite2(x - 1, y)); list.Add(SetWallSprite2(x + 1, y)); list.Add(SetWallSprite2(x , y + 1)); list.Add(SetWallSprite2(x, y - 1)); goto end;
        }

        end:
        {
            wallGrid.ChangeSprites(list,false);
        }
    }

    private Vector4 SetWallSprite2(int x, int y)
    {
        bool[] neighbours = GetNeighbours(x, y);
        int number = 0;
        int id = gridWalls[x, y].wallID;

        for (int i = 0; i < 4; i++)
        {
            if (neighbours[i]) number++;
        }

        if (number == 0)
        {
            return new Vector4(x, y, 3, id);
        }
        else if (number == 1)
        {
            if (neighbours[0]) { return new Vector4(x, y, 1, id); }
            if (neighbours[1]) { return new Vector4(x, y, 2, id); }
            if (neighbours[2]) { return new Vector4(x, y, 8, id); }
            if (neighbours[3]) { return new Vector4(x, y, 6, id); }
        }
        else if (number == 2)
        {
            if (neighbours[0] && neighbours[1]) return (new Vector4(x, y, 0, id));
            if (neighbours[0] && neighbours[2]) return (new Vector4(x, y, 5, id)); 
            if (neighbours[0] && neighbours[3]) return (new Vector4(x, y, 10, id));
            if (neighbours[1] && neighbours[2]) return (new Vector4(x, y, 4, id)); 
            if (neighbours[1] && neighbours[3]) return (new Vector4(x, y, 9, id)); 
            if (neighbours[2] && neighbours[3]) return (new Vector4(x, y, 7, id));
        }
        else if (number == 3)
        {
            if (neighbours[0] && neighbours[1] && neighbours[2]) return new Vector4(x, y, 15, id);
            if (neighbours[0] && neighbours[1] && neighbours[3]) return new Vector4(x, y, 12, id);
            if (neighbours[0] && neighbours[2] && neighbours[3]) return new Vector4(x, y, 11, id);
            if (neighbours[1] && neighbours[2] && neighbours[3]) return new Vector4(x, y, 14, id); 
        }
        else if (number == 4)
        {
            return new Vector4(x, y, 13, id);
        }


        return new Vector4(-1, -1, -1, -1);
    }

    public void RemoveWall(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            if (gridWalls[x, y].isWall)
            {
                gridWalls[x, y].isWall = false;
                gridWalls[x, y].wallID = -1;
                gridArray[x, y].canMove = true;
                SetWallSprite(x, y, -1);
                wallGrid.Clear(x, y);
            }
        }
        else
        {
            return;
        }
    }

}

