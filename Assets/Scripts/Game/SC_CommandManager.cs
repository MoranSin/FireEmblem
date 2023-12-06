using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SC_GridObject;

public enum CommandType
{
    MoveTo,
    Attack
}
public class Command
{
    public SC_Character character;
    public Vector2Int selectedGrid;
    public CommandType commandType;

    public Command(SC_Character character, Vector2Int selectedGrid, CommandType commandType)
    {
        this.character = character;
        this.selectedGrid = selectedGrid;
        this.commandType = commandType;
    }

    public List<pathNode> path;
    public SC_GridObject target;

}
public class SC_CommandManager : MonoBehaviour
{
    SC_ClearUtility clearUtility;
    SC_VictoryConditionManager victoryConditionManager;

    private void Awake()
    {
        clearUtility = GetComponent<SC_ClearUtility>();
        victoryConditionManager = GetComponent<SC_VictoryConditionManager>();
    }

    Command currentCommand;
    SC_CommandInput commandInput;

    private void Start()
    {
        commandInput = GetComponent<SC_CommandInput>();
        
    }

    private void Update()
    {
        if (currentCommand != null)
        {
            ExecuteCommand();
        }
    }

    public void ExecuteCommand()
    {
        switch (currentCommand.commandType)
        {
            case CommandType.MoveTo:
                MovementCommandExecute();
                break;
            case CommandType.Attack:
                AttackCommandExecute();
                break;
        }
    }

    private void AttackCommandExecute()
    {
        SC_Character recciver = currentCommand.character;
        if (GlobalVariables.gameState == GlobalVariables.GameState.MultiPlayer)
        {
            recciver.GetComponent<SC_Attack>().AttackGridObject(currentCommand.target);
        }
        else
        {
            recciver.GetComponent<SC_Attack>().Attack(currentCommand.target);
        }
        recciver.GetComponent<SC_CharaterTurn>().canAct = false;
        recciver.GetComponent<SC_CharaterTurn>().canWalk = false;

        victoryConditionManager.CheckPlayerVictory();
        currentCommand = null;
        clearUtility.ClearGridHighlightAttack();
    }

    private void MovementCommandExecute()
    {
        SC_Character recciver = currentCommand.character;
        if (GlobalVariables.gameState == GlobalVariables.GameState.MultiPlayer)
        {
        recciver.GetComponent<SC_Movement>().Move(currentCommand.path);
        }
        else
        {
        recciver.GetComponent<SC_Movement>().Moving(currentCommand.path);
        }
        recciver.GetComponent<SC_CharaterTurn>().canWalk = false;
        currentCommand = null;
        clearUtility.ClearPathfinding();
        clearUtility.ClearGridHighlightMove();
    }

    internal void AddMoveCommand(SC_Character selectedCharacter, Vector2Int positionOnGrid, List<pathNode> path)
    {
        currentCommand = new Command(selectedCharacter, positionOnGrid, CommandType.MoveTo);
        currentCommand.path = path;
    }

    internal void AddAttackCommand(SC_Character attacker, Vector2Int selectedGrid, SC_GridObject target)
    {
        currentCommand = new Command(attacker, selectedGrid, CommandType.Attack);
        currentCommand.target = target;
    }
}
