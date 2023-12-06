using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SC_GridObject;

public class SC_MouseInput : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    SC_Grid grid;
    [SerializeField] LayerMask terrainLayerMask;

    public Vector2Int positionOnGrid;
    public bool active;



    private void Start()
    {
        grid = FindObjectOfType<SC_StageManager>().stageGrid;
    }
    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit , float.MaxValue, terrainLayerMask))
        {
            active = true;
            Vector2Int hitPosition = grid.GetGridPosition(hit.point);
            if(hitPosition != positionOnGrid)
            {
                positionOnGrid = hitPosition;
                
            }
        }
        else
        {
            active = false;
        }

    }
}
