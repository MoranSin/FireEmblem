using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SC_GridObject;

public class SC_GridHighlight : MonoBehaviour
{
    SC_Grid grid;
    [SerializeField] GameObject highlightPoint;
    List<GameObject> highlightPointsToGo;
    [SerializeField] GameObject Container;


    void Awake()
    {
        grid = GetComponentInParent<SC_Grid>();
        highlightPointsToGo = new List<GameObject>();

       // Highlight(testTargetPosition);
    }

    private GameObject CreatePointHighlightObj()
    {
        GameObject go = Instantiate(highlightPoint);
        highlightPointsToGo.Add(go);
        go.transform.SetParent(Container.transform);
        return go;
    }

    public void Highlight(List<Vector2Int> positions)
    {
        for(int i = 0; i < positions.Count; i++)
        {
            Highlight(positions[i].x, positions[i].y, GetPointGO(i));
        }
    }

    public void Highlight(List<pathNode> positions)
    {
        for (int i = 0; i < positions.Count; i++)
        {
            Highlight(positions[i].pos_x, positions[i].pos_y, GetPointGO(i));
        }
    }

    private GameObject GetPointGO(int i)
    {
        if(highlightPointsToGo.Count > i)
        {
            return highlightPointsToGo[i];
        }
        GameObject newHighlightObj = CreatePointHighlightObj();
        return newHighlightObj;
    }

    public void Highlight(int posX, int posY, GameObject HighlightObj)
    {
        HighlightObj.SetActive(true);
        Vector3 position = grid.GetWorldPosition(posX, posY, true);
        position += Vector3.up * 0.2f;
       HighlightObj.transform.position = position;
    }

    internal void Hide()
    {
        for( int i = 0; i < highlightPointsToGo.Count; i++)
        {
            highlightPointsToGo[i].SetActive(false);
        }
    }
}
