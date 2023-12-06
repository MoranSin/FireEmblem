using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables
{
    public static string userId = string.Empty;
    public static string opponnentId = string.Empty;
    public static int TurnTime = 30;
    public static int password = 0;
    public static GameState gameState;


    public enum GameState
    {
        SinglePlayer,
        MultiPlayer
    }

}

