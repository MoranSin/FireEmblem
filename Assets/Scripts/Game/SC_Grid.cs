using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SC_GridObject;

public class SC_Grid : MonoBehaviour
{
    SC_Node[,] grid; 
    public int width = 25;
    public int height = 25;
    [SerializeField] float cellSize = 3f;
    [SerializeField] LayerMask obstacleLayer;
    [SerializeField] LayerMask terrainLayer;

    private void Awake()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        grid = new SC_Node[height, width];

        for(int y = 0; y < width; y++)
        {
            for(int x= 0; x < height; x++)
            {
                grid[x,y] = new SC_Node();
            }
        }
        CalculateElevation();
        CheckPassableTerrain();

    }

    private void CalculateElevation()
    {
        for (int y = 0; y < width; y++)
        {
            for (int x =0; x < height; x++)
            {
                Ray ray = new Ray(GetWorldPosition(x, y) + Vector3.up * 100f, Vector3.down);
                RaycastHit hit;
                if(Physics.Raycast(ray, out hit,float.MaxValue, terrainLayer))
                {
                    grid[x, y].elevation = hit.point.y;
                }
            }
        }
    }

    public bool CheckBoundry(Vector2Int positionOnGrid)
    {
        if(positionOnGrid.x < 0 || positionOnGrid.x > width)
        {
            return false;
        }
        if (positionOnGrid.y < 0 || positionOnGrid.y > height)
        {
            return false;
        }
        return true;
    }

    internal bool CheckBoundry(int posX, int posY)
    {
        if (posX < 0 || posX > width)
        {
            return false;
        }
        if (posY < 0 || posY > height)
        {
            return false;
        }
        return true;
    }

    public bool CheckWalkable(int pos_x, int pos_y)
    {
        return grid[pos_x, pos_y].passable;
    }

    private void CheckPassableTerrain()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector3 worldPosition = GetWorldPosition(x, y);
                bool passable = !Physics.CheckBox(worldPosition,Vector3.one / 2 * cellSize, Quaternion.identity,obstacleLayer);
                grid[x, y].passable = passable;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if(grid == null)
        {
            for (int y = 0; y < width; y++)
            {
                for (int x = 0; x < height; x++)
                {
                    Vector3 pos = GetWorldPosition(x, y);
                    Gizmos.DrawCube(pos, Vector3.one / 4);
                }
            }
        }
        else
        {
            for(int y = 0; y < width; y++)
            {
                for(int x = 0; x < height; x++)
                {
                    Vector3 pos = GetWorldPosition(x, y,true);
                    Gizmos.color = grid[x,y].passable ? Color.white : Color.red;
                    Gizmos.DrawCube(pos, Vector3.one / 4);
                }
            }
        }
    }

    public Vector3 GetWorldPosition(int x, int y, bool elevation = false)
    {
        return new Vector3(x * cellSize,elevation == true ? grid[x,y].elevation : 0f ,y * cellSize);
    }

    internal Vector2Int GetGridPosition(Vector3 worldPosition)
    {
        worldPosition.x += cellSize / 2;
        worldPosition.z += cellSize / 2;

        Vector2Int positionOnGrid = new Vector2Int((int)(worldPosition.x/ cellSize), (int)(worldPosition.z/ cellSize));
        return positionOnGrid;
    }

    public void PlaceObject(Vector2Int positionOnGrid, SC_GridObject GridObject)
    {
        if(CheckBoundry(positionOnGrid) == true)
        {
        grid[positionOnGrid.x, positionOnGrid.y].gridObject = GridObject;
        }
        else
        {
            Debug.Log("Object outside of the bounderies!");
        }
    }

    public SC_GridObject GetPlacedObject(Vector2Int gridPosition)
    {
        if (CheckBoundry(gridPosition))
        {
        SC_GridObject gridObj = grid[gridPosition.x, gridPosition.y].gridObject;
        return gridObj;
        }
        return null;
    }

    public List<Vector3> ConvertPathNodesToWorldPositions(List<pathNode> path)
    {
        List<Vector3> worldPositions = new List<Vector3>();

        for(int i = 0; i < path.Count; i++)
        {
            worldPositions.Add(GetWorldPosition(path[i].pos_x, path[i].pos_y, true));
        }

        return worldPositions;
    }

    internal void RemoveObject(Vector2Int positionOnGrid, SC_GridObject gridObject)
    {
        if (CheckBoundry(positionOnGrid) == true)
        {
            grid[positionOnGrid.x, positionOnGrid.y].gridObject = null;
        }
        else
        {
            Debug.Log("Object outside of the bounderies!");
        }
    }

    internal bool CheckOccupied(Vector2Int positionOnGrid)
    {
        return GetPlacedObject(positionOnGrid) != null;
    }

    public bool CheckElevation(int from_pos_x, int from_pos_y, int to_pos_x, int to_pos_y, float characerClimb = 1.5f)
    {
        float from_elevation = grid[from_pos_x,from_pos_y].elevation;
        float to_elevation = grid[to_pos_x,to_pos_y].elevation;
        float elevation_diff = to_elevation - from_elevation;
        elevation_diff = Mathf.Abs(elevation_diff);

        return characerClimb > elevation_diff;
    }

    public string ToJson()
    {
        Dictionary<string, object> jsonData = new Dictionary<string, object>
        {
            { "width", width },
            { "height", height },
            { "cellSize", cellSize },
            { "obstacleLayer", obstacleLayer.value },
           { "terrainLayer", terrainLayer.value }
        };

       return MiniJSON.Json.Serialize(jsonData);
    }

    //public string ToJson()
    //{
    //    List<List<Dictionary<string, object>>> gridData = new List<List<Dictionary<string, object>>>();

    //    for (int x = 0; x < grid.GetLength(0); x++)
    //    {
    //        List<Dictionary<string, object>> row = new List<Dictionary<string, object>>();
    //        for (int y = 0; y < grid.GetLength(1); y++)
    //        {
    //            SC_Node node = grid[x, y];
    //            Dictionary<string, object> nodeData = new Dictionary<string, object>
    //            {
    //                { "passable", node.passable },
    //                { "elevation", node.elevation }
    //            };

    //            if (node.gridObject != null)
    //            {
    //                nodeData.Add("gridObject", node.gridObject.ToJson());
    //            }

    //            row.Add(nodeData);
    //        }
    //        gridData.Add(row);
    //    }

    //    return JsonConvert.SerializeObject(gridData);
    //}
}
