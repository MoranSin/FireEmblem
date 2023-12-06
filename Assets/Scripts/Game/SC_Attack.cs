using AssemblyCSharp;
using com.shephertz.app42.gaming.multiplayer.client;
using com.shephertz.app42.gaming.multiplayer.client.events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using Newtonsoft.Json;

public enum AttackType
{
    Physical,
    Magic
}

public class SC_Attack : MonoBehaviour
{
    SC_GridObject gridObj;
    SC_CharaAnimator charaAnimator;
    SC_Character character;
    SC_CommandInput CommandInput;

    [SerializeField] private AudioSource attackSoundEffect;
    [SerializeField] private AudioSource spellSoundEffect;
    [SerializeField] private AudioSource missSoundEffect;
    [SerializeField] private AudioSource critSoundEffect;



    private void Awake()
    {
        gridObj = GetComponent<SC_GridObject>();
        charaAnimator = GetComponentInChildren<SC_CharaAnimator>();
        character = GetComponent<SC_Character>();
        CommandInput = FindObjectOfType<SC_CommandInput>();
    }

    private void OnEnable()
    {
       // Listener.OnMoveCompleted += OnMoveCompleted;
       // Listener.OnPrivateChatReceived += OnPrivateChatReceived;
        Listener.OnSendChat += OnSendChat;

    }

    private void OnDisable()
    {
       // Listener.OnMoveCompleted -= OnMoveCompleted;
       // Listener.OnPrivateChatReceived -= OnPrivateChatReceived;
        Listener.OnSendChat -= OnSendChat;

    }

    public void AttackGridObject(SC_GridObject targetObj)
    {
        SendMove(targetObj);
        Attack(targetObj);
    }

    private void SendMove(SC_GridObject targetObj)
    {
       // Dictionary<string, string> toSend = new Dictionary<string, string>() { { "target", targetObj.ToJson() } };
        string toJson = targetObj.ToJson();
        Debug.Log("sending move " + toJson);

        WarpClient.GetInstance().sendMove(toJson);

        if (SC_RoundManager.instance.isMyTurn)
        {
            WarpClient.GetInstance().sendPrivateChat(GlobalVariables.opponnentId, toJson);
        }
        else
        {
            WarpClient.GetInstance().sendPrivateChat(GlobalVariables.userId, toJson);
        }
    }

    public void Attack(SC_GridObject targetObj)
    {
        RotateCharacter(targetObj.transform.position);
        charaAnimator.Attack();

        if (UnityEngine.Random.value >= character.GetFloatValue(CharacterStats.Accuracy)) {
            DamagePopUpGenerator.Instance.CreatePopUp(transform.position, "Miss!", Color.white);
            missSoundEffect.Play();
            return; }

        SC_Character target = targetObj.GetComponent<SC_Character>();

        if (UnityEngine.Random.value <= target.GetFloatValue(CharacterStats.Dodge)) {
            DamagePopUpGenerator.Instance.CreatePopUp(target.transform.position, "Dodge!",Color.white);
            missSoundEffect.Play();
            return; }

        int damage = character.getDamage();

        if (UnityEngine.Random.value <= character.GetFloatValue(CharacterStats.CritChance))
        {
            damage = (int)(damage * character.GetFloatValue(CharacterStats.CritDmgMultiplicator));
            DamagePopUpGenerator.Instance.CreatePopUp(transform.position, "CRITICAL ATTACK!", Color.yellow);
            critSoundEffect.Play();
        }

        damage -= target.getDefenceValue(character.attackType);

        if (damage <= 0)
        {
            damage = 1;
        }
        if(character.attackType == AttackType.Physical)
        {
            attackSoundEffect.Play();
        }
        else
        {
            spellSoundEffect.Play();
        }
        target.TakeDamage(damage);
           DamagePopUpGenerator.Instance.CreatePopUp(target.transform.position, damage.ToString(), Color.white);


    }

    private void RotateCharacter(Vector3 towards)
    {
        Vector3 direction = (towards - transform.position).normalized;
        direction.y = 0;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    private void OnMoveCompleted(MoveEvent _Move)
    {
       if(CommandInput.currentCommand == CommandType.Attack)
       {
            Debug.Log("OnMoveCompleted Attack: " + _Move.getMoveData() + ",Sender: " + _Move.getSender() + ",Next Turn: " + _Move.getNextTurn());
            if (_Move.getSender() != GlobalVariables.userId && _Move.getMoveData() != null)
            {
                string senderJson = _Move.getMoveData();
                SC_GridObject target = MiniJSON.Json.Deserialize(senderJson) as SC_GridObject;
                Attack(target);
                //    Dictionary<string, SC_GridObject> data = MiniJSON.Json.Deserialize(senderJson) as Dictionary<string, SC_GridObject>;
                //    if (data.ContainsKey("target"))
                //    {
                //        SC_GridObject target = data["target"];
                //        Attack(target);
                //    }
            }
           // SC_RoundManager.instance.CheckIfEndTurn();
       }

   }


    private void OnPrivateChatReceived(string sender, string message)
    {
       Debug.Log("OnPrivateChatReceived " + message);
                string senderJson = message;
                SC_GridObject target = MiniJSON.Json.Deserialize(senderJson) as SC_GridObject;
                    Attack(target);
    }

    private void OnSendChat(string _Sender, string _Message)
    {
        Debug.Log("OnPrivateChatReceived " + _Message + " Sender: " + _Sender);
        string senderJson = _Message;
        SC_GridObject target = MiniJSON.Json.Deserialize(senderJson) as SC_GridObject;
        Attack(target);
    }

}
