using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SC_GridObject;

public class SC_Marker : MonoBehaviour
{
    [SerializeField] Transform marker;
    SC_MouseInput mouseInput;
    bool active;
    Vector2Int currentPos;
    SC_Grid targetGrid;
    [SerializeField] float elevation = 2f;

    private void Awake()
    {
        mouseInput = GetComponent<SC_MouseInput>();
    }

    private void Start()
    {
        targetGrid = FindObjectOfType<SC_StageManager>().stageGrid;
    }

    private void Update()
    {
        if (active != mouseInput.active)
        {
            active = mouseInput.active;
            marker.gameObject.SetActive(active);
        }

        if (active == false) { return; }

        if(currentPos != mouseInput.positionOnGrid)
        {
            currentPos = mouseInput.positionOnGrid;
            UpdateMarker();
        }

    }

    private void UpdateMarker()
    {
        if(targetGrid.CheckBoundry(currentPos) == false) { return;}
        Vector3 worldPosition = targetGrid.GetWorldPosition(currentPos.x, currentPos.y , true);
        worldPosition.y += elevation;
        marker.position = worldPosition;
    }
}
