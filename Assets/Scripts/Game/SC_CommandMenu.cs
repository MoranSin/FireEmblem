using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_CommandMenu : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] GameObject moveButton;
    [SerializeField] GameObject attackButton;
    SC_CommandInput commandInput;
    SC_SelectCharacter selectCharacter;
    SC_RoundManager currentRound;

    private void Awake()
    {
        commandInput = GetComponent<SC_CommandInput>();
        selectCharacter = GetComponent<SC_SelectCharacter>();
        currentRound = GetComponent<SC_RoundManager>();
    }


    private void TogglePanel()
    {
        if(selectCharacter.selected != null)
        {
            OpenPanel();
        }
        else
        {
            ClosePanel();
        }
    }

    public void OpenPanel()
    {
       selectCharacter.enabled = false;
       panel.SetActive(true);
    }

    public void ClosePanel()
    {
        panel.SetActive(false);
    }

    public void MoveCommandSelected()
    {
        if (selectCharacter.selected.GetComponent<SC_CharaterTurn>().canWalk)
        {
        commandInput.setCommandType(CommandType.MoveTo);
        commandInput.InitCommand();
        ClosePanel();
        }
    }

    public void AttackCommandSelected()
    {
        if (selectCharacter.selected.GetComponent<SC_CharaterTurn>().canAct)
        {
        commandInput.setCommandType(CommandType.Attack);
        commandInput.InitCommand();
        ClosePanel();
        }
    }

    internal void OpenPanel(SC_CharaterTurn CharaterTurn)
    {
       // selectCharacter.enabled = false;
            panel.SetActive(true);
        
        if (CharaterTurn.allegiance != currentRound.currentTurn)
        {
            moveButton.SetActive(false);
            attackButton.SetActive(false);

        }
        else
        {
            if (CharaterTurn.canAct)
            {
                attackButton.SetActive(true);
            }
            else
            {
                attackButton.SetActive(false);
            }

            if (CharaterTurn.canWalk)
            {
                moveButton.SetActive(true);
            }
            else
            {
                moveButton.SetActive(false);
            }
        }
    }
}
