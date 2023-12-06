using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static SC_GridObject;

public class SC_CharaAttack : MonoBehaviour
{
    SC_Grid targetGrid;
    SC_GridHighlight highlight;

    List<Vector2Int> attackPos;

    private void Start()
    {
        SC_StageManager stageManager = FindObjectOfType<SC_StageManager>();
        targetGrid = stageManager.stageGrid;
        highlight = stageManager.attackHighlight;
    }

    public void CalculateAttackArea(Vector2Int characterPositionOnGrid, int attackRange, bool selfTargetable = false)
    {
        if (attackPos == null)
        {
            attackPos = new List<Vector2Int>();
        }
        else
        {
            attackPos.Clear();
        }

        for (int x = -attackRange; x <= attackRange; x++)
            {
                for(int y = -attackRange; y <= attackRange; y++)
                {
                    if (Mathf.Abs(x) + Mathf.Abs(y) > attackRange) { continue; }
                    if(selfTargetable == false)
                    {
                        if(x == 0 && y == 0) { continue; }
                    }
                    if(targetGrid.CheckBoundry(characterPositionOnGrid.x + x, characterPositionOnGrid.y + y) == true)
                    {
                        attackPos.Add(new Vector2Int(characterPositionOnGrid.x + x, characterPositionOnGrid.y + y));
                    }
                }
            }

            highlight.Highlight(attackPos);
        }

    internal bool Check(Vector2Int positionOnGrid)
    {
        return attackPos.Contains(positionOnGrid);
    }

    internal SC_GridObject GetAttackTarget(Vector2Int positionOnGrid)
    {
        SC_GridObject target = targetGrid.GetPlacedObject(positionOnGrid);
        return target;
    }

    /*private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, float.MaxValue, terrainLayerMask))
            {
                Vector2Int gridPosition = targetGrid.GetGridPosition(hit.point);

                if (attackPos.Contains(gridPosition))
                {
                    SC_GridObject gridObject = targetGrid.GetPlacedObject(gridPosition);
                    if(gridObject == null) { return; }
                    selectedChara.GetComponent<SC_Attack>().AttackPosition(gridObject);
                }
            }
        }
    }*/
}
