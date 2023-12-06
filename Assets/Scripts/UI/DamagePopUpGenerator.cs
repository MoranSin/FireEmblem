using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopUpGenerator : MonoBehaviour
{
    public static DamagePopUpGenerator Instance;
    public GameObject prefab;

    void Awake()
    {
        Instance = this;
    }


    public void CreatePopUp(Vector3 position, string text, Color color)
    {
        var popup = Instantiate(prefab, position, Quaternion.identity);
        var temp = popup.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        temp.text = text;
        temp.faceColor = color;

        Destroy(popup, 2f);
    }
}
