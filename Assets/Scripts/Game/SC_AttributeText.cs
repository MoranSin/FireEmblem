using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SC_AttributeText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI valueText;

    public void UpdateText(int val)
    {
        valueText.text = val.ToString();
    }
}
