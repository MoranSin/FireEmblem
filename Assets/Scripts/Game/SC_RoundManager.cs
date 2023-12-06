using AssemblyCSharp;
using com.shephertz.app42.gaming.multiplayer.client;
using com.shephertz.app42.gaming.multiplayer.client.events;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class SC_RoundManager : MonoBehaviour
{
    public static SC_RoundManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        UpdateTextOnScreen();
    }

    private void OnEnable()
    {
        Listener.OnGameStarted += OnGameStarted;
        Listener.OnMoveCompleted += OnMoveCompleted;
    }


    private void OnDisable()
    {
        Listener.OnGameStarted -= OnGameStarted;
        Listener.OnMoveCompleted -= OnMoveCompleted;
    }

    public void SendMove()
    {
        if(GlobalVariables.gameState == GlobalVariables.GameState.MultiPlayer)
        WarpClient.GetInstance().sendMove(null);
    }

    private void OnMoveCompleted(MoveEvent _Move)
    {
        NextTurn();
    }

    [SerializeField] SC_ForceContainer playerForceContainer;
    [SerializeField] SC_ForceContainer opponentForceContainer;

    int round = 1;

    [SerializeField] TMPro.TextMeshProUGUI turnCountText;
    [SerializeField] TMPro.TextMeshProUGUI forceRoundText;
    [SerializeField] TMPro.TextMeshProUGUI MySign;


    [SerializeField] SC_MouseInput mouseInput;

    private float startTime;
    private string nextTurn;
    public bool isMyTurn;

    public void AddMe(SC_CharaterTurn character)
    {
        if(character.allegiance == Allegiance.Opponent)
        {
            opponentForceContainer.AddMe(character);
        }
        if(character.allegiance == Allegiance.Player)
        {
            playerForceContainer.AddMe(character);
        }
    }

    public Allegiance currentTurn;
    public void NextTurn()
    {
        switch (currentTurn)
        {
            case Allegiance.Opponent:
                isMyTurn = true;
                currentTurn = Allegiance.Player;
                EnablePlayerInput();
                break;
            case Allegiance.Player:
                isMyTurn = false;
                currentTurn = Allegiance.Opponent;
                EnablePlayerInput();
                break;
        }
        GrantTurnToForce();
        NextRound();
        UpdateTextOnScreen();
        if(GlobalVariables.gameState == GlobalVariables.GameState.SinglePlayer)
        OpponentTurn();
    }

    private void EnablePlayerInput()
    {
        if (isMyTurn)
        {
        mouseInput.enabled = true;
        }
        else
        {
        mouseInput.enabled = false;
        }
    }


    private void GrantTurnToForce()
    {
        switch (currentTurn)
        {
            case Allegiance.Opponent:
                opponentForceContainer.GrantTurn();
                break;
            case Allegiance.Player:
                playerForceContainer.GrantTurn();
                break;
        }
    }

    public void NextRound()
    {
        round += 1;

    }

    void UpdateTextOnScreen()
    {
        turnCountText.text = "Turn: " + round.ToString();
        forceRoundText.text = currentTurn.ToString();
    }


    public void OnGameStarted(string Sender, string RoomId, string _nextTurn)
    {
        Debug.Log("Sender: " + Sender + ",Room Id: " + RoomId + ",Next Turn: " + _nextTurn);
        nextTurn = _nextTurn;
        startTime = Time.time;
        
        if(GlobalVariables.userId == _nextTurn)
        {
            isMyTurn = true;
            MySign.text = Allegiance.Player.ToString();
            currentTurn = Allegiance.Player;
        }
        else
        {
            isMyTurn = false;
            MySign.text = Allegiance.Opponent.ToString();
            currentTurn = Allegiance.Player;
        }
    }

    private void OpponentTurn()
    {
        if(currentTurn == Allegiance.Opponent)
        {
            Invoke("NextTurn", 2.0f);
        }
    }


    public void CheckIfEndTurn()
    {
        if (currentTurn == Allegiance.Player)
        {
            int done = 0;
            for (int i = 0; i < playerForceContainer.force.Count; i++)
            {
                if ((playerForceContainer.force[i].characterTurn.canAct = false) && (playerForceContainer.force[i].characterTurn.canWalk = false))
                {
                    done++;
                }
            }
            if (done == playerForceContainer.force.Count)
            {
                NextTurn();
            }
        }
        else if (currentTurn == Allegiance.Opponent)
        {
            int done = 0;
            for (int i = 0; i < opponentForceContainer.force.Count; i++)
            {
                if ((opponentForceContainer.force[i].characterTurn.canAct = false) && (opponentForceContainer.force[i].characterTurn.canWalk = false))
                {
                    done++;
                }
            }
            if (done == opponentForceContainer.force.Count)
            {
                NextTurn();
            }
        }
    }
}
