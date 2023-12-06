using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class SC_StatusPanel : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI characterName;
    [SerializeField] Slider hpBar;
    [SerializeField] TMPro.TextMeshProUGUI LevelTxt;
    [SerializeField] Slider expBar;

    [SerializeField] SC_AttributeText strAttributeText;
    [SerializeField] SC_AttributeText magAttributeText;
    [SerializeField] SC_AttributeText sklAttributeText;
    [SerializeField] SC_AttributeText spdAttributeText;
    [SerializeField] SC_AttributeText defAttributeText;
    [SerializeField] SC_AttributeText resAttributeText;


    public void UpdateStatus(SC_Character character)
    {
        characterName.text = character.characterData.Name;
        hpBar.maxValue = character.HP.max;
        hpBar.value = character.HP.current;

        LevelTxt.text = "LVL " + character.characterData.level.level.ToString();
        expBar.maxValue = character.characterData.level.RequiredExperienceToLevelUp;
        expBar.value = character.characterData.level.experience;

        strAttributeText.UpdateText(character.characterData.attributes.Get(CharacterAttributesEnum.Strength));
        magAttributeText.UpdateText(character.characterData.attributes.Get(CharacterAttributesEnum.Magic));
        sklAttributeText.UpdateText(character.characterData.attributes.Get(CharacterAttributesEnum.Skill));
        spdAttributeText.UpdateText(character.characterData.attributes.Get(CharacterAttributesEnum.Speed));
        defAttributeText.UpdateText(character.characterData.attributes.Get(CharacterAttributesEnum.Defence));
        resAttributeText.UpdateText(character.characterData.attributes.Get(CharacterAttributesEnum.Resistance));
    }
}
