using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceMember
{
    public SC_Character character;
    public SC_CharaterTurn characterTurn;

    public ForceMember(SC_Character character, SC_CharaterTurn characterTurn)
    {
        this.character = character;
        this.characterTurn = characterTurn;
    }
}

public class SC_ForceContainer : MonoBehaviour
{
    public Allegiance allegiance;
    public List<ForceMember> force;

    public void AddMe(SC_CharaterTurn chatacterTurn)
    {
        if (force == null) { force = new List<ForceMember>(); }
        force.Add(new ForceMember(chatacterTurn.GetComponent<SC_Character>(),chatacterTurn));
        chatacterTurn.transform.parent = transform;
    }

    public void GrantTurn()
    {
        for(int i = 0; i < force.Count; i++)
        {
            force[i].characterTurn.GrantTurn();
        }
    }

    public bool CheckDefeated()
    {
        for(int i = 0; i < force.Count; i++)
        {
            if(force[i].character.defeated == false)
            {
                return false;
            }
        }

        return true;
    }
}
