using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SC_Sliders : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI sliderTxt;

    private void Awake()
    {
        if(slider != null && sliderTxt != null)
        {
        slider.onValueChanged.AddListener((v) =>
        {
            int wholeNumber = Mathf.RoundToInt(Mathf.Lerp(0, 10, v));
            sliderTxt.text = wholeNumber.ToString();
            GlobalVariables.password = wholeNumber;
        });
        }
    }

    public void getPassword(float v)
    {
        int wholeNumber = Mathf.RoundToInt(Mathf.Lerp(0, 10, v));
        GlobalVariables.password = wholeNumber;
    }

}
