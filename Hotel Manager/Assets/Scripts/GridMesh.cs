using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMesh
{


    const float ws = 32;
    const float hs = 32f;
    Vector3[] vertices;
    Vector2[] uv;
    int[] triangles;
    Mesh mesh;
    int width, height;
    float cellSize;
    Vector3 position;
    GameObject obj;
    Material material;

    public GridMesh(int width, int height, float cellSize, Material material, Vector3 position, bool fill)
    {
        this.material = material;
        this.position = position;
        this.height = height;
        this.width = width;
        this.cellSize = cellSize;
        vertices = new Vector3[4 * width * height];
        uv = new Vector2[4 * width * height];
        triangles = new int[6 * width * height];
        mesh = new Mesh();
        int index;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                index = i * height + j;


                if (fill)
                {
                    vertices[index * 4 + 0] = new Vector3(cellSize * i, cellSize * (j + 1)) + position;
                    vertices[index * 4 + 1] = new Vector3(cellSize * (i + 1), cellSize * (j + 1)) + position;
                    vertices[index * 4 + 2] = new Vector3(cellSize * i, cellSize * j) + position;
                    vertices[index * 4 + 3] = new Vector3(cellSize * (i + 1), cellSize * j) + position;
                } else
                {
                    vertices[index * 4 + 0] = new Vector3(cellSize * i, 0 * (j + 1)) + position;
                    vertices[index * 4 + 1] = new Vector3(cellSize * (i + 1), 0 * (j + 1)) + position;
                    vertices[index * 4 + 2] = new Vector3(cellSize * i, 0 * j) + position;
                    vertices[index * 4 + 3] = new Vector3(cellSize * (i + 1), 0 * j) + position;
                }


                uv[index * 4 + 0] = PixelsToUV(1 , 1 + hs);
                uv[index * 4 + 1] = PixelsToUV(1 + ws, 1 + hs);
                uv[index * 4 + 2] = PixelsToUV(1 , 1);
                uv[index * 4 + 3] = PixelsToUV(1 + ws,  1);



                triangles[index * 6 + 0] = index * 4 + 0;
                triangles[index * 6 + 1] = index * 4 + 1;
                triangles[index * 6 + 2] = index * 4 + 2;

                triangles[index * 6 + 3] = index * 4 + 2;
                triangles[index * 6 + 4] = index * 4 + 1;
                triangles[index * 6 + 5] = index * 4 + 3;

            }
        }
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        obj = new GameObject("Mesh", typeof(MeshFilter), typeof(MeshRenderer));
        obj.GetComponent<MeshRenderer>().material = material;
        obj.GetComponent<MeshFilter>().mesh = mesh;
        mesh.RecalculateBounds();
    }

    private Vector2 PixelsToUV(float x, float y)
    {
        return new Vector2((float)x / material.mainTexture.width, (float)y / material.mainTexture.height);
    }

    public void ChangeSprite(int x, int y, int idX, int idY)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            int index = x * height + y;


            vertices[index * 4 + 0] = new Vector3(cellSize * x, cellSize * (y + 1)) + position;
            vertices[index * 4 + 1] = new Vector3(cellSize * (x + 1), cellSize * (y + 1)) + position;
            vertices[index * 4 + 2] = new Vector3(cellSize * x, cellSize * y) + position;
            vertices[index * 4 + 3] = new Vector3(cellSize * (x + 1), cellSize * y) + position;

            uv[index * 4 + 0] = PixelsToUV(idX * 32, idY * 32 + hs);
            uv[index * 4 + 1] = PixelsToUV(idX * 32 + ws, idY * 32 + hs);
            uv[index * 4 + 2] = PixelsToUV(idX * 32, idY * 32);
            uv[index * 4 + 3] = PixelsToUV(idX * 32 + ws, idY * 32);

            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.triangles = triangles;
            obj.GetComponent<MeshFilter>().mesh = mesh;
            mesh.RecalculateBounds();
        }
    }


    public void ChangeSprites(List<Vector4> list, bool isfloor)
    {
        if (!isfloor)
        {
            foreach (var vector4 in list)
            {
                int x, y, idX, idY;
                x = (int)vector4.x;
                y = (int)vector4.y;
                idX = (int)vector4.z;
                idY = (int)vector4.w;


                if (x >= 0 && y >= 0 && x < width && y < height)
                {
                    int index = x * height + y;

                    vertices[index * 4 + 0] = new Vector3(cellSize * x, cellSize * (y + 1)) + position;
                    vertices[index * 4 + 1] = new Vector3(cellSize * (x + 1), cellSize * (y + 1)) + position;
                    vertices[index * 4 + 2] = new Vector3(cellSize * x, cellSize * y) + position;
                    vertices[index * 4 + 3] = new Vector3(cellSize * (x + 1), cellSize * y) + position;

                    uv[index * 4 + 0] = PixelsToUV(idX * 32, idY * 32 + hs);
                    uv[index * 4 + 1] = PixelsToUV(idX * 32 + ws, idY * 32 + hs);
                    uv[index * 4 + 2] = PixelsToUV(idX * 32, idY * 32);
                    uv[index * 4 + 3] = PixelsToUV(idX * 32 + ws, idY * 32);
                }
            }
        } else
        {
            foreach (var vector4 in list)
            {
                int x, y, idX, idY;
                x = (int)vector4.x;
                y = (int)vector4.y;
                idX = (int)vector4.z;
                idY = (int)vector4.w;

                if (x >= 0 && y >= 0 && x < width && y < height)
                {
                    int index = x * height + y;

                    vertices[index * 4 + 0] = new Vector3(cellSize * x, cellSize * (y + 1)) + position;
                    vertices[index * 4 + 1] = new Vector3(cellSize * (x + 1), cellSize * (y + 1)) + position;
                    vertices[index * 4 + 2] = new Vector3(cellSize * x, cellSize * y) + position;
                    vertices[index * 4 + 3] = new Vector3(cellSize * (x + 1), cellSize * y) + position;

                    uv[index * 4 + 0] = PixelsToUV(idX * 32 + (idX * 2) + 1, idY * 32 + hs + 1);
                    uv[index * 4 + 1] = PixelsToUV(idX * 32 + ws + (idX * 2) + 1, idY * 32 + hs +1);
                    uv[index * 4 + 2] = PixelsToUV(idX * 32 + (idX * 2) +1 , idY * 32 + 1);
                    uv[index * 4 + 3] = PixelsToUV(idX * 32 + ws + (idX * 2 + 1), idY * 32 + 1);
                }
            }
        }
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        obj.GetComponent<MeshFilter>().mesh = mesh;
        mesh.RecalculateBounds();


    }

    public void Clear(List<Vector4> list)
    {
        foreach (var vector4 in list)
        {
            int x, y, idX, idY;
            x = (int)vector4.x;
            y = (int)vector4.y;
            idX = (int)vector4.z;
            idY = (int)vector4.w;

            if (x >= 0 && y >= 0 && x < width && y < height)
            {
                int index = x * height + y;

                vertices[index * 4 + 0] = new Vector3(0, 0) + position;
                vertices[index * 4 + 1] = new Vector3(0, 0) + position;
                vertices[index * 4 + 2] = new Vector3(0, 0) + position;
                vertices[index * 4 + 3] = new Vector3(0, 0) + position;

                uv[index * 4 + 0] = PixelsToUV(idX * 32, idY * 32 + hs);
                uv[index * 4 + 1] = PixelsToUV(idX * 32 + ws, idY * 32 + hs);
                uv[index * 4 + 2] = PixelsToUV(idX * 32, idY * 32);
                uv[index * 4 + 3] = PixelsToUV(idX * 32 + ws, idY * 32);
            }
        }
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        obj.GetComponent<MeshFilter>().mesh = mesh;
        mesh.RecalculateBounds();


    }

    public void Clear(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            int index = x * height + y;

            vertices[index * 4 + 0] = new Vector3(0, 0) + position;
            vertices[index * 4 + 1] = new Vector3(0, 0) + position;
            vertices[index * 4 + 2] = new Vector3(0, 0) + position;
            vertices[index * 4 + 3] = new Vector3(0, 0) + position;

            uv[index * 4 + 0] = PixelsToUV(0 * 32, 0 * 32 + hs);
            uv[index * 4 + 1] = PixelsToUV(0 * 32 + ws, 0 * 32 + hs);
            uv[index * 4 + 2] = PixelsToUV(0 * 32, 0 * 32);
            uv[index * 4 + 3] = PixelsToUV(0 * 32 + ws, 0 * 32);
        }
    
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        obj.GetComponent<MeshFilter>().mesh = mesh;
        mesh.RecalculateBounds();
    }
}
   


