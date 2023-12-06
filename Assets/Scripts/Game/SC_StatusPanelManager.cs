using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_StatusPanelManager : MonoBehaviour
{
    bool isActive;

    [SerializeField] bool fixedCharacter;
    [SerializeField]  SC_Character currentCharacterStatus;
    [SerializeField] GameObject statusPanelGO;
    SC_SelectCharacter selectCharacter;
    [SerializeField] SC_StatusPanel statusPanel;


    private void Awake()
    {
        selectCharacter = GetComponent<SC_SelectCharacter>();
    }

    private void Update()
    {
        if(fixedCharacter == true)
        {
            statusPanel.UpdateStatus(currentCharacterStatus);
        }
        else
        {
        MouseHoverOverObj();

        }
    }

    private void MouseHoverOverObj()
    {
        if (isActive == true)
        {
            statusPanel.UpdateStatus(currentCharacterStatus);
            if (selectCharacter.hoverOverChara == null)
            {
                HideStatusPanel();
                return;
            }
            if (selectCharacter.hoverOverChara != currentCharacterStatus)
            {
                currentCharacterStatus = selectCharacter.hoverOverChara;
                statusPanel.UpdateStatus(currentCharacterStatus);
                return;
            }

        }
        else
        {
            if (selectCharacter.hoverOverChara != null)
            {
                currentCharacterStatus = selectCharacter.hoverOverChara;
                ShowStatusPanel();
                return;
            }
        }
    }

    private void ShowStatusPanel()
    {
        statusPanelGO.SetActive(true);
        isActive = true;
        statusPanel.UpdateStatus(selectCharacter.hoverOverChara);
    }



    private void HideStatusPanel()
    {
        statusPanelGO.SetActive(false);
        isActive = false;
    }

}
