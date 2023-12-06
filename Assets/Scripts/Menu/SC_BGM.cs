using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_BGM : MonoBehaviour
{
    private static SC_BGM BackgroundMusic;
    private void Awake()
    {
        if(BackgroundMusic == null)
        {
            BackgroundMusic = this;
            //DontDestroyOnLoad(BackgroundMusic);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
