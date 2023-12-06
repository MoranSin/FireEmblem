using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_LvlUpTestManager : MonoBehaviour
{
    public SC_Character targetCharacter;

    public void AddExperience(int exp)
    {
        targetCharacter.addExperience(exp);
    }
}
