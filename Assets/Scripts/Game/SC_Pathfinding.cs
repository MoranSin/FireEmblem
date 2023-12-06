using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pathNode
{
    public int pos_x;
    public int pos_y;

    public float gValue;
    public float hValue;
    public pathNode parentNode;

    public float fValue
    {
        get { return gValue + hValue; }
    }

    public pathNode(int posX, int posY)
    {
        pos_x = posX;
        pos_y = posY;
    }

    public void Clear()
    {
        gValue = 0f;
        hValue = 0f;
        parentNode = null;

    }
}
[RequireComponent(typeof(SC_Grid))]
public class SC_Pathfinding : MonoBehaviour
{
    SC_Grid gridMap;
    pathNode[,] pathNodes;
    private void Start()
    {
        Init();
    }

    private void Init()
    {
        if (gridMap == null) { gridMap = GetComponent<SC_Grid>(); }
        pathNodes = new pathNode[gridMap.width, gridMap.height];
        for (int x = 0; x < gridMap.width; x++)
        {
            for(int y = 0; y < gridMap.height; y++)
            {
                pathNodes[x, y] = new pathNode(x, y);
            }
        }
    }

    public void CalculateWalkableNodes(int startX, int startY, float range, ref List<pathNode> toHighlight)
    {
        pathNode startNode = pathNodes[startX, startY];

        List<pathNode> openList = new List<pathNode>();
        List<pathNode> closedList = new List<pathNode>();

        openList.Add(startNode);

        while (openList.Count > 0)
        {
            pathNode currentNode = openList[0];

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            List<pathNode> neighbourNodes = new List<pathNode>();
            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    if (x == 0 && y == 0) { continue; }
                    if (gridMap.CheckBoundry(currentNode.pos_x + x, currentNode.pos_y + y) == false) { continue; }

                    neighbourNodes.Add(pathNodes[currentNode.pos_x + x, currentNode.pos_y + y]);
                }
            }

            for (int i = 0; i < neighbourNodes.Count; i++)
            {
                if (closedList.Contains(neighbourNodes[i])) { continue; }
                if (gridMap.CheckWalkable(neighbourNodes[i].pos_x, neighbourNodes[i].pos_y) == false) { continue; }
                if(gridMap.CheckElevation(currentNode.pos_x, currentNode.pos_y , neighbourNodes[i].pos_x, neighbourNodes[i].pos_y) == false) { continue; }

                float movementCost = currentNode.gValue + CalculateDistance(currentNode, neighbourNodes[i]);
                if(movementCost > range) { continue; }

                if (openList.Contains(neighbourNodes[i]) == false || movementCost < neighbourNodes[i].gValue)
                {
                    neighbourNodes[i].gValue = movementCost;
                    neighbourNodes[i].parentNode = currentNode;

                    if (openList.Contains(neighbourNodes[i]) == false)
                    {
                        openList.Add(neighbourNodes[i]);
                    }
                }
            }
        }

       if(toHighlight != null)
        toHighlight.AddRange(closedList);

    }

        public List<pathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        pathNode startNode = pathNodes[startX, startY];
        pathNode endNode = pathNodes[endX, endY];

        List<pathNode> openList = new List<pathNode>();
        List<pathNode> closedList = new List<pathNode>();

        openList.Add(startNode);

        while(openList.Count > 0)
        {
            pathNode currentNode = openList[0];
            
            for(int i = 0; i < openList.Count; i++)
            {
                if(currentNode.fValue > openList[i].fValue)
                {
                    currentNode = openList[i];
                }

                if(currentNode.fValue == openList[i].fValue && currentNode.hValue > openList[i].hValue)
                {
                    currentNode = openList[i];
                }
            }
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if(currentNode == endNode)
            {
                return RetracePath(startNode, endNode);
            }

            List<pathNode> neighbourNodes = new List<pathNode>();
            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    if (x == 0 && y == 0){ continue; }
                    if (gridMap.CheckBoundry(currentNode.pos_x + x, currentNode.pos_y + y) == false) { continue; }

                    neighbourNodes.Add(pathNodes[currentNode.pos_x + x, currentNode.pos_y + y]);
                }
            }

            for(int i = 0; i < neighbourNodes.Count; i++)
            {
                if (closedList.Contains(neighbourNodes[i])) { continue; }
                if (gridMap.CheckWalkable(neighbourNodes[i].pos_x, neighbourNodes[i].pos_y) == false) { continue; }

                float movementCost = currentNode.gValue + CalculateDistance(currentNode, neighbourNodes[i]);

                if (openList.Contains(neighbourNodes[i]) == false || movementCost < neighbourNodes[i].gValue)
                {
                    neighbourNodes[i].gValue = movementCost;
                    neighbourNodes[i].hValue = CalculateDistance(neighbourNodes[i], endNode);
                    neighbourNodes[i].parentNode = currentNode;

                    if (openList.Contains(neighbourNodes[i]) == false)
                    {
                        openList.Add(neighbourNodes[i]);
                    }
                }
            }
        }
        return null;
    }

    private int CalculateDistance(pathNode currentNode, pathNode targetNode)
    {
        int distX = Mathf.Abs(currentNode.pos_x - targetNode.pos_x);
        int distY = Mathf.Abs(currentNode.pos_y - targetNode.pos_y);

        if(distX > distY) { return 14 * distY + 10 * (distX - distY); }
        return 14 * distX + 10 * (distY - distX);
    }

    private List<pathNode> RetracePath(pathNode startNode, pathNode endNode)
    {
        List<pathNode> path = new List<pathNode>();
        pathNode currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parentNode;
        }
        path.Reverse();

        return path;
    }

    public List<pathNode> TracedBackPath(int x , int y)
    {
        if(gridMap.CheckBoundry(x,y) == false) { return null; }
        List<pathNode> path = new List<pathNode>();
        pathNode currentNode = pathNodes[x, y];
        while(currentNode.parentNode != null)
        {
            path.Add(currentNode);
            currentNode = currentNode.parentNode;
        }

        return path;
    }

    internal void Clear()
    {
        for (int x = 0; x < gridMap.width; x++)
        {
            for (int y = 0; y < gridMap.height; y++)
            {
                   pathNodes[x, y].Clear();
            }
        }
    }
}
