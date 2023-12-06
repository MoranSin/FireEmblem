using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

[Serializable]
public class Int2Val
{
    public int current;
    public int max;

    public bool canGoNegative;

    public Int2Val(int current, int max)
    {
        this.current = current;
        this.max = max;
    }

    internal void Subtract(int amount)
    {
        current -= amount;

        if(canGoNegative == false)
        {
            if(current < 0) { current = 0;}
        }
    }

    public string ToJson()
    {
        Dictionary<string, object> jsonData = new Dictionary<string, object>
        {
            { "current", current },
            { "max", max },
            { "canGoNegative", canGoNegative.ToString() }
        };

        return MiniJSON.Json.Serialize(jsonData);
    }
}

public enum CharacterAttributesEnum
{
    Strength,
    Magic,
    Skill,
    Speed,
    Defence,
    Resistance
}

[Serializable]
public class CharacterAttributes
{
    public const int AttributesCount = 6;
    public int strength;
    public int magic;
    public int skill;
    public int speed;
    public int defence;
    public int resistance;

    public CharacterAttributes() { }

    public void Sum(CharacterAttributesEnum attribute, int val)
    {
        switch (attribute)
        {
            case CharacterAttributesEnum.Strength:
                strength += val;
                break;
            case CharacterAttributesEnum.Magic:
                magic += val;
                break;
            case CharacterAttributesEnum.Skill:
                skill += val;
                break;
            case CharacterAttributesEnum.Speed:
                speed += val;
                break;
            case CharacterAttributesEnum.Defence:
                defence += val;
                break;
            case CharacterAttributesEnum.Resistance:
                resistance += val;
                break;
        }
    }

    public int Get(CharacterAttributesEnum i)
    {
        switch (i)
        {
            case CharacterAttributesEnum.Strength:
                return strength;
            case CharacterAttributesEnum.Magic:
                return magic;
            case CharacterAttributesEnum.Skill:
                return skill;
            case CharacterAttributesEnum.Speed:
                return speed;
            case CharacterAttributesEnum.Defence:
                return defence;
            case CharacterAttributesEnum.Resistance:
                return resistance;
        }
        Debug.Log("Added attribute that wasn't implemented!");
        return -1;
    }

    //public Dictionary<string, object> ToJson()
    //{
    //    Dictionary<string, object> jsonData = new Dictionary<string, object>
    //    {
    //        { "strength", strength },
    //        { "magic", magic },
    //        { "skill", skill },
    //        { "speed", speed },
    //        { "defence", defence },
    //        { "resistance", resistance }
    //    };

    //    return jsonData;
    //}
}

[Serializable]
public class Level
{
    public int RequiredExperienceToLevelUp
    {
        get { return level * 1000; }
    }
    public int level = 1;
    public int experience = 0;

    public void AddExperience(int exp)
    {
        experience += exp;
    }

    public bool CheckLevelUp()
    {
        return experience >= RequiredExperienceToLevelUp; 
    }

    public void LevelUp()
    {
        experience -= RequiredExperienceToLevelUp;
        level += 1;
    }

}

public enum CharacterStats
{
    HP,
    AttackRange,
    Accuracy,
    Dodge,
    CritChance,
    CritDmgMultiplicator,
    Speed
}

[Serializable]
public class Stats
{
    public float speed = 50f;
    public int HP = 100;
    public int AttackRange = 1;
    public float accuracy = 0.75f;
    public float dodge = 0.1f;
    public float critChance = 0.1f;
    public float critDmgMul = 1.5f;

    public float GetFloatValue(CharacterStats characterStats)
    {
        switch (characterStats)
        {
            case CharacterStats.Accuracy:
                return accuracy;
            case CharacterStats.Dodge:
                return dodge;
            case CharacterStats.CritChance:
                return critChance;
            case CharacterStats.CritDmgMultiplicator:
                return critDmgMul;
            case CharacterStats.Speed:
                return speed;
        }
        Debug.Log("Incorrect stat type");
        return 0f;
    }

    public int GetIntValue(CharacterStats characterStats)
    {
        switch (characterStats)
        {
            case CharacterStats.HP:
                return HP;
            case CharacterStats.AttackRange:
                return AttackRange;
        }

        Debug.Log("Incorrect stat type");
        return 0;
    }

    public Dictionary<string, object> ToJson()
    {
        Dictionary<string, object> jsonData = new Dictionary<string, object>
        {
            { "speed", speed },
            { "HP", HP },
            { "AttackRange", AttackRange },
            { "accuracy", accuracy },
            { "dodge", dodge },
            { "critChance", critChance },
            { "critDmgMul", critDmgMul }
        };

        return jsonData;
    }
}

    public class SC_Character : MonoBehaviour
{

    public SC_CharacterData characterData;
    public CharacterAttributes attributes;
    public AttackType attackType;
    public bool defeated;
    public Int2Val HP = new Int2Val(100, 100);

    private float fadeOutTime = 10.0f;
    public int getDefenceValue(AttackType at)
    {
     
        int def = 0;
        def = characterData.getDamage(at);
        return def;
    }

    public int getDamage()
    {
        int Damage = 0;
        Damage = characterData.getDamage(attackType);
        return Damage;
    }

    public void Init()
    {
        attributes = new CharacterAttributes();
      //  characterData.level = new Level();
    }

    private void Start()
    {
        if (attributes == null)
        {
            Init();
        }
    }

    public void TakeDamage(int damage)
    {
        HP.Subtract(damage);
        CheckDefeat();
    }

    private void CheckDefeat()
    {
        if(HP.current <= 0)
        {
            Defeated();
        }
    }

    private void Defeated()
    {
        defeated = true;
        StartCoroutine(DoFadeIn(GetComponentInChildren<SpriteRenderer>()));
        Destroy(this,3f);
       
    }

    IEnumerator DoFadeIn(SpriteRenderer character)
    {
        yield return new WaitForSeconds(1f);
        Color Alive = character.color;
        while (Alive.a > 0)
        {
            Alive.a -= Time.deltaTime / fadeOutTime;
            character.color = Alive;
        }
        yield return null;
    }

    internal void addExperience(int exp)
    {
        characterData.addExperience(exp);
    }

    public int GetIntValue(CharacterStats chatacterStat)
    {
        return characterData.GetIntValue(chatacterStat);
    }

    public float GetFloatValue(CharacterStats characterStat)
    {
        return characterData.GetFloatValue(characterStat);

    }

    public void SetCharacterData(SC_CharacterData characterData)
    {
        this.characterData = characterData;
    }

    public string ToJson()
    {
        Dictionary<string, object> jsonData = new Dictionary<string, object>
        {
            { "characterData", characterData.ToJson() },
            //{ "attributes", attributes.ToJson() },
            { "attackType", attackType.ToString() },
            { "defeated", defeated.ToString() },
            { "HP", HP.ToJson() }
        };

        // return JsonConvert.SerializeObject(jsonData);
        return MiniJSON.Json.Serialize(jsonData);
    }
}
