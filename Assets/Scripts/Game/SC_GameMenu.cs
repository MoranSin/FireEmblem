using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_GameMenu : MonoBehaviour
{
    public GameObject panel;
    SC_SelectCharacter selectCharacter;

    private void Awake()
    {
        selectCharacter = GetComponent<SC_SelectCharacter>();
    }
    void Update()
    {
        if(selectCharacter.enabled == false) { return; }
        if (Input.GetMouseButtonDown(1))
        {
            panel.SetActive(!panel.activeInHierarchy);
        }
        
    }
}
