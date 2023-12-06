using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using static SC_GridObject;

public class SC_SelectCharacter : MonoBehaviour
{
    SC_MouseInput mouseInput;
    SC_CommandMenu commandMenu;
    SC_GameMenu gameMenu;

    private void Awake()
    {
        mouseInput = GetComponent<SC_MouseInput>();
        commandMenu = GetComponent<SC_CommandMenu>();
        gameMenu = GetComponent<SC_GameMenu>();
        selected = GetComponent<SC_Character>();
    }


    public SC_Character selected;
    public bool isSelected;
    SC_GridObject hoverOverGridObj;
    public SC_Character hoverOverChara;
    Vector2Int positionOnGrid = new Vector2Int(-1, -1);
    SC_Grid targetGrid;

    private void Start()
    {
        targetGrid = FindObjectOfType<SC_StageManager>().stageGrid;
    }

    private void Update()
    {
        if (positionOnGrid != mouseInput.positionOnGrid)
        {
            HoverOverObj();
        }

        SelectCharacter();

        DeselectCharacter();
    }

    private void LateUpdate()
    {
        if(selected != null)
        {
            if(isSelected == false)
            {
                selected = null;
            }
        }
    }
    private void HoverOverObj()
    {
            positionOnGrid = mouseInput.positionOnGrid;
            hoverOverGridObj = targetGrid.GetPlacedObject(positionOnGrid);
            if (hoverOverGridObj != null)
            {
                hoverOverChara = hoverOverGridObj.GetComponent<SC_Character>();
            }
            else
            {
                hoverOverChara = null;
            }
    }

    private void DeselectCharacter()
    {
        if (Input.GetMouseButtonDown(1))
        {
            selected = null;
            UpdatePanel();
        }
    }

    private void UpdatePanel()
    {
        if(selected != null)
        {
            commandMenu.OpenPanel(selected.GetComponent<SC_CharaterTurn>());
        }
        else
        {
            commandMenu.ClosePanel();
        }
    }

    private void SelectCharacter()
    {
        
        HoverOverObj();
        if(selected != null) { return; }
        if(gameMenu.panel.activeInHierarchy == true) { return; }
        if (Input.GetMouseButtonDown(0)
            //&& SC_RoundManager.instance.isMyTurn
            )
        {
            if (hoverOverChara != null && selected == null)
            {
                selected = hoverOverChara;
                isSelected = true;
            }
            UpdatePanel();
        }
    }

    public void Deselect()
    {
        isSelected = false;
    }
}
