using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Allegiance
{
    Player,
    Opponent
}
public class SC_CharaterTurn : MonoBehaviour
{
    public Allegiance allegiance;
    public bool canWalk;
    public bool canAct;

    private void Start()
    {
        AddToRoundManager();
        GrantTurn();
    }

    private void AddToRoundManager()
    {
        SC_RoundManager.instance.AddMe(this);
    }

    public void GrantTurn()
    {
        canAct = true;
        canWalk = true;
    }
}
