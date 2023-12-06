using com.shephertz.app42.gaming.multiplayer.client;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SC_VictoryConditionManager : MonoBehaviour
{
    [SerializeField] SC_ForceContainer enemyForce;
    [SerializeField] SC_ForceContainer playerForce;

    [SerializeField] GameObject victoryPanel;
    [SerializeField] TextMeshProUGUI clearMessage;
    [SerializeField] SC_MouseInput mouseInput;

    private string message;
    public void CheckPlayerVictory()
    {
        if(enemyForce.CheckDefeated() == true)
        {
            message = "Yillisian force won!";
            Victory();
        }

        if(playerForce.CheckDefeated() == true)
        {
            message = "Grima's force won!";
            Victory();
        }
    }

    private void Victory()
    {
        mouseInput.enabled = false;
        victoryPanel.SetActive(true);
        clearMessage.text = message;

        if (GlobalVariables.gameState == GlobalVariables.GameState.MultiPlayer)
            WarpClient.GetInstance().stopGame();

    }

    public void ReturnToWorldMap()
    {
        SceneManager.LoadScene("WorldMap",LoadSceneMode.Single);
    }
}
