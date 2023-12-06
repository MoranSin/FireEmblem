using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SC_MenuLogic;

public class SC_MenuController : MonoBehaviour
{
    public SC_MenuLogic curMenuLogic;

    public void Btn_BackLogic()
    {
        if (curMenuLogic != null)
            curMenuLogic.Btn_BackLogic();
    }
    public void Btn_MainMenu_Play()
    {
        if (curMenuLogic != null)
            curMenuLogic.Btn_MainMenu_PlayLogic();
    }
    
    public void Btn_MainMenu_Options()
    {
        if (curMenuLogic != null)
            curMenuLogic.Btn_MainMenu_OptionsLogic();
    }

    public void Btn_MainMenu_Info()
    {
        if (curMenuLogic != null)
            curMenuLogic.Btn_MainMenu_InfoLogic();
    }

    public void Btn_MainMenu_Muliti()
    {
        if (curMenuLogic != null)
            curMenuLogic.Btn_MainMenu_MulitiLogic();
    }

    public void Btn_ChangeScreen(string _ScreenName)
    {
        if (curMenuLogic != null)
        {
            try
            {
                Screens _toScreen = (Screens)Enum.Parse(typeof(Screens), _ScreenName);
                curMenuLogic.ChangeScreen(_toScreen);
            }
            catch (Exception e)
            {
                Debug.LogError("Fail to convert: " + e.ToString());
            }
        }
    }

}
