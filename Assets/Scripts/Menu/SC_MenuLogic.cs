using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SC_MenuLogic : MonoBehaviour
{

    public enum Screens
    {
        MainMenu, Loading, Options, Info, Muliti
    };

    #region Variables
    private Stack<Screens> History;
    private Dictionary<string, GameObject> unityObjects;
    private Screens curScreen;
    private Screens prevScreen;

    #endregion

    #region MonoBehaviour

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        InitLogic();
    }

    #endregion

    #region Logic

    private void Init()
    {
        curScreen = Screens.MainMenu;
        prevScreen = Screens.MainMenu;
        unityObjects = new Dictionary<string, GameObject>();
        History = new Stack<Screens>();

        GameObject[] _unityObj = GameObject.FindGameObjectsWithTag("UnityObject");
        foreach(GameObject g in _unityObj)
        {
            if (unityObjects.ContainsKey(g.name) == false)
                unityObjects.Add(g.name, g);
            else Debug.LogError("This key " + g.name + " Is Already inside the Dictionary!!!");
        }

        GlobalVariables.gameState = GlobalVariables.GameState.SinglePlayer;
    }

    private void InitLogic()
    {
        if (unityObjects.ContainsKey("Screen_Loading"))
            unityObjects["Screen_Loading"].SetActive(false);
        if (unityObjects.ContainsKey("Screen_Options"))
            unityObjects["Screen_Options"].SetActive(false);
        if (unityObjects.ContainsKey("Screen_Muliti"))
            unityObjects["Screen_Muliti"].SetActive(false);
        if (unityObjects.ContainsKey("Screen_Info"))
            unityObjects["Screen_Info"].SetActive(false);
    }

    public void ChangeScreen(Screens _ToScreen)
    {
        History.Push(curScreen);
        prevScreen = curScreen;
        curScreen = _ToScreen;

        if (unityObjects.ContainsKey("Screen_" + prevScreen))
        {
            unityObjects["Screen_" + prevScreen].SetActive(false);
        }

            if (unityObjects.ContainsKey("Screen_" + curScreen))
        {
            unityObjects["Screen_" + curScreen].SetActive(true);
        }
    }

    public void GoBack()
    {
        if(History.Count <= 2)
        {
            if (unityObjects.ContainsKey("Screen_" + curScreen))
            {
                unityObjects["Screen_" + curScreen].SetActive(false);
            }

            prevScreen = History.Pop();
            curScreen = prevScreen;

            if (unityObjects.ContainsKey("Screen_" + prevScreen))
            {
                unityObjects["Screen_" + prevScreen].SetActive(true);

            }


        }
    }

    #endregion

    #region Controller


    public void Btn_BackLogic()
    {
        Debug.Log("Btn_BackLogic");
        GoBack();
    }

    public void Btn_MainMenu_PlayLogic()
    {
        Debug.Log("Btn_MainMenu_PlayLogic");
        ChangeScreen(Screens.Loading);
    }

    public void Btn_MainMenu_OptionsLogic()
    {
        Debug.Log("Btn_MainMenu_OptionsLogic");
        ChangeScreen(Screens.Options);
    }

    public void Btn_MainMenu_MulitiLogic()
    {
        Debug.Log("Btn_MainMenu_OptionsLogic");
        ChangeScreen(Screens.Muliti);
    }

    public void Btn_MainMenu_InfoLogic()
    {
        Debug.Log("Btn_MainMenu_OptionsLogic");
        ChangeScreen(Screens.Info);
    }

    public void Slider_Muliti()
    {
        GameObject.Find("txt_Number").GetComponent<TextMeshProUGUI>().text =
        GameObject.Find("slider_Number").GetComponent<Slider>().value.ToString();
    }
    #endregion
}


