using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SC_MissionSelect : MonoBehaviour
{
    public void LoadMission(string missionName)
    {
        SceneManager.LoadScene("StageBasics", LoadSceneMode.Single);
        SceneManager.LoadScene(missionName, LoadSceneMode.Additive);
    }
}
