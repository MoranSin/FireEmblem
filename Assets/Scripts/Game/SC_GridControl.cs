using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static SC_GridObject;

public class SC_GridControl : MonoBehaviour
{
    [SerializeField] SC_Grid targetGrid;
    [SerializeField] LayerMask terrainLayerMask;
    Vector2Int currentGridPos = new Vector2Int(-1, -1);

    [SerializeField] SC_GridObject hoveringOver;
    [SerializeField] SC_SelectableGridObj selectedObj;

    void Update()
    {
        HoverOverObjCheck();

        selectObj();

        DeselectObj();
    }

    private void DeselectObj()
    {
        if (Input.GetMouseButtonDown(1))
        {
            selectedObj = null;
        }
    }

    private void selectObj()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (hoveringOver == null) { return; }
            selectedObj = hoveringOver.GetComponent<SC_SelectableGridObj>();
        }
    }

    private void HoverOverObjCheck()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, float.MaxValue, terrainLayerMask))
        {
            Vector2Int gridPosition = targetGrid.GetGridPosition(hit.point);
            if (gridPosition == currentGridPos) { return; }
            currentGridPos = gridPosition;
            SC_GridObject gridObject = targetGrid.GetPlacedObject(gridPosition);
            hoveringOver = gridObject;
        }
    }
}
