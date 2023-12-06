using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_MousePosOnScreen : MonoBehaviour
{

    [SerializeField] TMPro.TextMeshProUGUI positionOnScreen;
    SC_MouseInput mouseInput;
    private void Awake()
    {
        mouseInput = GetComponent<SC_MouseInput>(); 
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (mouseInput.active == true)
        {
            positionOnScreen.text = "Position " + mouseInput.positionOnGrid.x.ToString() + ":" + mouseInput.positionOnGrid.y.ToString();

        }
        else
        {
            positionOnScreen.text = "Outside";
        }
    }
}
