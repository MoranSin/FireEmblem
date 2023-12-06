using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SC_CharacterData : ScriptableObject
{
    public string Name = "Nameless";
    public Level level;
    public CharacterAttributes attributes;
    public CharacterAttributes levelUpRates;

    public Stats stats;

    public int getDefenceValue(AttackType at)
    {
        int def = 0;
        switch (at)
        {
            case AttackType.Physical:
                def += attributes.defence;
                break;
            case AttackType.Magic:
                def += attributes.resistance;
                break;
        }

        return def;
    }

    public int getDamage(AttackType attackType)
    {
        int Damage = 0;

        switch (attackType)
        {
            case AttackType.Physical:
                Damage += attributes.strength;
                break;
            case AttackType.Magic:
                Damage += attributes.magic;
                break;
        }

        return Damage;
    }

    public void addExperience(int exp)
    {
        level.AddExperience(exp);
        if (level.CheckLevelUp())
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        level.LevelUp();
        LevelUpAttributes();
    }

    private void LevelUpAttributes()
    {
        for (int i = 0; i < CharacterAttributes.AttributesCount; i++)
        {
            int rate = levelUpRates.Get((CharacterAttributesEnum)i);
            rate += UnityEngine.Random.Range(0, 100);
            rate /= 100;
            if (rate > 0)
            {
                attributes.Sum((CharacterAttributesEnum)i, rate);
            }
        }
    }

    public int GetIntValue(CharacterStats chatacterStat)
    {
        return stats.GetIntValue(chatacterStat);
    }

    internal float GetFloatValue(CharacterStats characterStat)
    {
        return stats.GetFloatValue(characterStat);
    }

    public string ToJson()
    {
        Dictionary<string, object> jsonData = new Dictionary<string, object>
        {
            { "Name", Name },
           // { "level", level.ToJson() },
           // { "attributes", attributes.ToJson() },
          //  { "levelUpRates", levelUpRates.ToJson() },
            { "stats", stats.ToJson() }
        };

        return MiniJSON.Json.Serialize(jsonData);
    }
}
