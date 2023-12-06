using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagePopUpAnimation : MonoBehaviour
{
    public AnimationCurve opacityCurve;
    public AnimationCurve ScaleCurve;
    public AnimationCurve heightCurve;


    private TextMeshProUGUI tmp;
    private float time = 0;
    private Vector3 origin;
    void Awake()
    {
        tmp = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        origin = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        tmp.color = new Color(1,1,1, opacityCurve.Evaluate(time));
        transform.localScale = Vector3.one * ScaleCurve.Evaluate(time);
        transform.position = origin + new Vector3(0, 1 + heightCurve.Evaluate(time), 0);
        time += Time.deltaTime;
    }
}