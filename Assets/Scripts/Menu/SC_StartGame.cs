using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SC_StartGame : MonoBehaviour
{

    public void startGame()
    {
        SceneManager.LoadScene("WorldMap", LoadSceneMode.Single);
    }

    public void startMuliti()
    {
        SceneManager.LoadScene("Mulitiplayer", LoadSceneMode.Single);
    }
}
