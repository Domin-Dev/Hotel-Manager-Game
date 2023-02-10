using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Collections;
using Unity.Burst;


public class Pathfinding : MonoBehaviour
{
    
    static public Pathfinding instance;

    private Grid grid;
    [SerializeField] private GameObject ai;

    List<DataToPathfinding> list = new List<DataToPathfinding>();
    int number = 0;

    public int x;

    int count = 0;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            for (int i = 0; i < x; i++)
            {
                int x = UnityEngine.Random.RandomRange(0, grid.width);
                int y = UnityEngine.Random.RandomRange(0, grid.height);

                if (grid.GetValue(x, y).canMove)
                {
                   GameObject obj = Instantiate(ai, grid.GetPosition(x, y), Quaternion.identity);
                    FindObjectOfType<CustomerGenerator>().GetRandomLookCustomer(obj.transform);
                    count++;
                }

            }
        }


      if(number != 0)
        {
            int2[] startPositions = new int2[number]; 
            int2[] endPositions = new int2[number];
            AI[] AIs = new AI[number];

            for (int i = 0; i < number; i++)
            {
                DataToPathfinding dataToPathfinding = this.list[i];
                startPositions[i] = dataToPathfinding.startPosition;
                endPositions[i] = dataToPathfinding.endPosition;
                AIs[i] = dataToPathfinding.aI;
            }

            this.list.Clear();
            FindPath(startPositions, endPositions, number, AIs);
            number = 0;
        }
    }

    private void Awake()
    {
        if(instance == null)
        {
            DontDestroyOnLoad(this);
            instance = this;
        }
        else
        {
            Destroy(this); 
        }
    }

    private void Start()
    {
        grid = GameManager.instance.gameGrid;
    }




    private void FindPath(int2[] startPositions,int2[] endPositions,int number,AI[] AIs)
    {
        float time = Time.realtimeSinceStartup;
        NativeArray<JobHandle> jobHandles = new NativeArray<JobHandle>(number,Allocator.Temp);
        PathfindingJob[] jobs = new PathfindingJob[number];
        NativeArray<bool>  nodes = new NativeArray<bool>(grid.width * grid.height,Allocator.TempJob);     


        for (int i = 0; i < grid.width; i++)
        {
            for (int j = 0; j < grid.height; j++)
            {
                nodes[i * grid.height + j] = grid.GetValue(i, j).canMove;
            }
        }



        for (int i = 0; i < number; i++)
        {
            PathfindingJob pathfinding = new PathfindingJob()
            {
                startPosition = startPositions[i],
                endPosition = endPositions[i],
                sizeGrid = new int2(grid.width, grid.height),
                paths = new NativeList<int2>(Allocator.TempJob),
                nodes = nodes,
            };

            jobHandles[i] = pathfinding.Schedule();
            jobs[i] = pathfinding;
        }
        JobHandle.CompleteAll(jobHandles);
        nodes.Dispose();

        bool[,] doorsArray = grid.GetDoorsArray();


        for (int i = 0; i < jobHandles.Length ; i++)
        {
            List<Vector2> vectors = new List<Vector2>();
            List<bool> doorsList = new List<bool>();
            for (int k = 0; k < jobs[i].paths.Length; k++)
            {
                int2 position = jobs[i].paths[k];
                vectors.Add(GetVector3(position.x,position.y));
                doorsList.Add(doorsArray[position.x, position.y]);

            }
            jobs[i].paths.Dispose();
            AIs[i].SetNewPath(vectors,doorsList);
        }

        jobHandles.Dispose();
    }

    private Vector3 GetVector3(int x,int y)
    {
        return new Vector3((x + 0.5f) * grid.cellSize, (y + 0.5f) * grid.cellSize, 0) + grid.gridPosition;
    }

    private int2 GetXY(Vector3 vector3)
    {
        grid.GetXY(vector3, out int x, out int y);
        int2 position = new int2(x, y);
        return position;
       
    }

    public void GetPath(Vector2 position,AI aI)
    {
        int2 positionint2 = GetXY(position);
        int x = UnityEngine.Random.Range(0, grid.width);
        int y = UnityEngine.Random.Range(0, grid.height);

        list.Add(new DataToPathfinding(positionint2, new int2(x, y), aI));
        number++;
    }

    
}
