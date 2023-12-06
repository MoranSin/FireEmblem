using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static SC_GridObject;

public class SC_MoveCharacter : MonoBehaviour
{
    SC_Grid targetGrid;

    SC_Pathfinding pathfinding;

    SC_GridHighlight gridHighlight;

    private void Start()
    {
        SC_StageManager stageManager = FindObjectOfType<SC_StageManager>();
        targetGrid = stageManager.stageGrid;
        gridHighlight = stageManager.moveHighlight;
        pathfinding = targetGrid.GetComponent<SC_Pathfinding>();
    }

    public void CheckWalkableTerrain(SC_Character targetCharacter)
    {
        SC_GridObject gridObject = targetCharacter.GetComponent<SC_GridObject>();
        List<pathNode> walkableNodes = new List<pathNode>();
        pathfinding.Clear();
        pathfinding.CalculateWalkableNodes(gridObject.positionOnGrid.x, gridObject.positionOnGrid.y, targetCharacter.GetComponent<SC_Character>().GetFloatValue(CharacterStats.Speed), ref walkableNodes);
        gridHighlight.Hide();
        gridHighlight.Highlight(walkableNodes);
    }

    public List<pathNode> GetPath(Vector2Int from)
    {
        List<pathNode> path = pathfinding.TracedBackPath(from.x, from.y);
        if (path == null) { return null; }
        if (path.Count == 0) { return null; }
        path.Reverse();

        return path;
    }

    public bool CheckOccupied(Vector2Int positionOnGrid)
    {
       return targetGrid.CheckOccupied(positionOnGrid);

    }
}
