using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_ClearUtility : MonoBehaviour
{
    SC_Pathfinding targetPF;
    SC_GridHighlight attackHighlight;
    SC_GridHighlight moveHighlight;

    private void Start()
    {
        SC_StageManager stageManager = FindObjectOfType<SC_StageManager>();
        targetPF = stageManager.pathfinding;
        attackHighlight = stageManager.attackHighlight;
        moveHighlight = stageManager.moveHighlight;
    }
    public void ClearPathfinding()
    {
        targetPF.Clear();
    }

    public void ClearGridHighlightAttack()
    {
        attackHighlight.Hide();
    }

    public void ClearGridHighlightMove()
    {
        moveHighlight.Hide();
    }
}
