using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SC_CommandInput : MonoBehaviour
{
    SC_CommandManager commandManager;
    SC_MouseInput mouseInput;
    SC_MoveCharacter moveCharacter;
    SC_CharaAttack attackCharacter;
    SC_SelectCharacter selectCharacter;
    SC_ClearUtility clearUtility;

    private void Awake()
    {
        commandManager = GetComponent<SC_CommandManager>();
        mouseInput = GetComponent<SC_MouseInput>();
        moveCharacter = GetComponent<SC_MoveCharacter>();
        attackCharacter = GetComponent<SC_CharaAttack>();
        selectCharacter = GetComponent<SC_SelectCharacter>();
        clearUtility = GetComponent<SC_ClearUtility>();

    }

    public CommandType currentCommand;
    bool isInputCommand;

    public void setCommandType(CommandType commandType)
    {
        currentCommand = commandType;
    }

    public void InitCommand()
    {
        isInputCommand = true;
        switch (currentCommand)
        {
            case CommandType.MoveTo:
                HighlightWalkableTerrain();
                break;
            case CommandType.Attack:
                attackCharacter.CalculateAttackArea(selectCharacter.selected.GetComponent<SC_GridObject>().positionOnGrid, selectCharacter.selected.GetIntValue(CharacterStats.AttackRange));
                break;
        }
    }

    private void Update()
    {
        if(isInputCommand == false) { return; }
        switch (currentCommand)
        {
            case CommandType.MoveTo:
                MoveCommandInput();
                break;
            case CommandType.Attack:
                AttackCommandInput();
                break;
        }

    }

    private void AttackCommandInput()
    {
        if (Input.GetMouseButtonDown(0) && SC_RoundManager.instance.isMyTurn)
        {
            if(attackCharacter.Check(mouseInput.positionOnGrid) == true)
            {
                SC_GridObject gridObject = attackCharacter.GetAttackTarget(mouseInput.positionOnGrid);
                if(gridObject == null) { return; }
                commandManager.AddAttackCommand(selectCharacter.selected, mouseInput.positionOnGrid, gridObject);
                StopCommandInput();
            }
        }
            if (Input.GetMouseButtonDown(1))
            {
                StopCommandInput();
                clearUtility.ClearGridHighlightAttack();
            }
    }

    public void HighlightWalkableTerrain()
    {
        moveCharacter.CheckWalkableTerrain(selectCharacter.selected);
    }


    private void MoveCommandInput()
    {
        if (Input.GetMouseButtonDown(0) && SC_RoundManager.instance.isMyTurn)
        {
            if(moveCharacter.CheckOccupied(mouseInput.positionOnGrid) == true) { return;}
            List<pathNode> path = moveCharacter.GetPath(mouseInput.positionOnGrid);
            if (path == null) { return; }
            if (path.Count == 0) { return; }
            commandManager.AddMoveCommand(selectCharacter.selected, mouseInput.positionOnGrid, path);
            StopCommandInput();
        }

        if (Input.GetMouseButtonDown(1))
        {
            StopCommandInput();
            clearUtility.ClearGridHighlightMove();
            clearUtility.ClearPathfinding();
        }
    }

    private void StopCommandInput()
    {
        selectCharacter.Deselect();
        selectCharacter.enabled = true;
        isInputCommand = false;
    }
}
