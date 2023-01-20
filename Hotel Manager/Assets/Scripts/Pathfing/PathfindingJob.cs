using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Collections;
using Unity.Burst;

[BurstCompile]
public struct PathfindingJob : IJob
{
    const int cost1 = 10;
    const int cost2 = 14;

    public int2 startPosition;
    public int2 endPosition;
    public NativeList<int2> paths;
    public int2 sizeGrid;
    [ReadOnly]public NativeArray<bool> nodes;


    public void Execute()
    {


        NativeArray<PathNode> pathNodeArray = new NativeArray<PathNode>(sizeGrid.x * sizeGrid.y, Allocator.Temp);

        for (int x = 0; x < sizeGrid.x; x++)
        {
            for (int y = 0; y < sizeGrid.y; y++)
            {
                PathNode pathNode = new PathNode();
                pathNode.x = x;
                pathNode.y = y;
                pathNode.gCost = int.MaxValue;
                pathNode.CalculateF();
                pathNode.index = GetIndex(x, y);
                pathNode.comeFromIndex = -1;
                pathNode.canMove = nodes[pathNode.index];

                pathNodeArray[pathNode.index] = pathNode;
            }
        }

        NativeArray<int2> Neighbors = new NativeArray<int2>(4, Allocator.Temp);
        Neighbors[0] = new int2(+1, 0);
        Neighbors[1] = new int2(-1, 0);
        Neighbors[2] = new int2(0, +1);
        Neighbors[3] = new int2(0, -1);


        PathNode startNode = pathNodeArray[GetIndex(startPosition.x, startPosition.y)];
        PathNode endNode = pathNodeArray[GetIndex(endPosition.x, endPosition.y)];
        startNode.gCost = 0;
        startNode.fCost = GetDistance(startNode, pathNodeArray[GetIndex(endPosition.x, endPosition.y)]);
        startNode.CalculateF();
        pathNodeArray[startNode.index] = startNode;

        NativeList<PathNode> openList = new NativeList<PathNode>(Allocator.Temp);
        NativeList<PathNode> closedList = new NativeList<PathNode>(Allocator.Temp);

        openList.Add(startNode);

        while (openList.Length > 0)
        {
            PathNode pathNode = GetLowestFPathNode(openList);
            if (pathNode.index == GetIndex(endPosition.x, endPosition.y))
            {
                break;
            }

            for (int i = 0; i < openList.Length; i++)
            {
                if (openList[i].index == pathNode.index)
                {
                    openList.RemoveAtSwapBack(i);
                    break;
                }
            }

            closedList.Add(pathNode);
            for (int i = 0; i < Neighbors.Length; i++)
            {
                int2 position = Neighbors[i];
                if (!IsInsideGrid(new int2(pathNode.x + position.x, pathNode.y + position.y)))
                {
                    continue;
                }

                int index = GetIndex(position.x + pathNode.x, position.y + pathNode.y);
                PathNode neighbourNode = pathNodeArray[index];

                if (IsInList(closedList, neighbourNode))
                {
                    continue;
                }

                if (!neighbourNode.canMove)
                {
                    continue;
                }

                int gCost = pathNode.gCost + GetDistance(pathNode, neighbourNode);
                if (gCost < neighbourNode.gCost)
                {
                    neighbourNode.gCost = gCost;
                    neighbourNode.comeFromIndex = pathNode.index;
                    neighbourNode.hCost = GetDistance(neighbourNode, endNode);
                    neighbourNode.CalculateF();
                    pathNodeArray[neighbourNode.index] = neighbourNode;

                    if (!IsInList(openList, neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }

                }

            }

        }


        endNode = pathNodeArray[endNode.index];

        GetPath(pathNodeArray, endNode);
        pathNodeArray.Dispose();
        openList.Dispose();
        closedList.Dispose();
        Neighbors.Dispose();
    }

    private void GetPath(NativeArray<PathNode> array, PathNode endNode)
    {
        if (endNode.comeFromIndex == -1)
        {
            return;
        }
        else
        {
            paths.Add(new int2(endNode.x, endNode.y));

            PathNode pathNode = endNode;
            int value = 1;
            while (pathNode.comeFromIndex != -1)
            {
                PathNode comeFromNode = array[pathNode.comeFromIndex];
                paths.Add(new int2(comeFromNode.x, comeFromNode.y));
                pathNode = comeFromNode;
                value++;
            }

            return;
        }

        }
        private bool IsInsideGrid(int2 position)
        {
            return position.x >= 0 && position.x < sizeGrid.x && position.y >= 0 && position.y < sizeGrid.y;
        }

        private bool IsInList(NativeList<PathNode> list, PathNode pathNode)
        {
            for (int i = 0; i < list.Length; i++)
            {
                if (pathNode.index == list[i].index)
                {
                    return true;
                }
            }
            return false;
        }

        private int GetDistance(PathNode pathNode1, PathNode pathNode2)
        {

            int x = Mathf.Abs(pathNode1.x - pathNode2.x);
            int y = Mathf.Abs(pathNode1.y - pathNode2.y);
            int value = Mathf.Abs(x - y);
            //    return cost2 * Mathf.Min(x, y) 
            return value * cost1;
        }

        private PathNode GetLowestFPathNode(NativeList<PathNode> pathNodes)
        {
            PathNode pathNode = pathNodes[0];
            for (int i = 0; i < pathNodes.Length; i++)
            {
                if (pathNodes[i].fCost < pathNode.fCost)
                {
                    pathNode = pathNodes[i];
                }
            }
            return pathNode;
        }

        private int GetIndex(int x, int y)
        {
            return x * sizeGrid.y + y;
        }

    }
