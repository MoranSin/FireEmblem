using AssemblyCSharp;
using com.shephertz.app42.gaming.multiplayer.client;
using com.shephertz.app42.gaming.multiplayer.client.events;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;

public class SC_Movement : MonoBehaviour
{
    SC_GridObject gridObject;
    SC_CharaAnimator characterAnimator;
    List<Vector3> pathWorldPositions;
    [SerializeField] float moveSpeed = 1f;
    SC_CommandInput CommandInput;
    [SerializeField] private AudioSource moveSoundEffect;

    private void OnEnable()
    {
        //Listener.OnMoveCompleted += OnMoveCompleted;
       // Listener.OnPrivateChatReceived += OnPrivateChatReceived;
        //Listener.OnChatReceived += OnChatReceived;
        Listener.OnSendChat += OnSendChat;

    }

    private void OnDisable()
    {
        //Listener.OnMoveCompleted -= OnMoveCompleted;
       // Listener.OnPrivateChatReceived -= OnPrivateChatReceived;
       // Listener.OnChatReceived -= OnChatReceived;
        Listener.OnSendChat -= OnSendChat;

    }


    public bool IS_MOVING
    {
        get
        {
            if(pathWorldPositions == null) { return false; }
            return pathWorldPositions.Count > 0;
        }
    }

    private void Awake()
    {
        gridObject = GetComponent<SC_GridObject>();
        characterAnimator = GetComponentInChildren<SC_CharaAnimator>();
        CommandInput = FindObjectOfType<SC_CommandInput>();
    }
    public void Move(List<pathNode> path)
    {
        SendMove(path);
        Moving(path);
    }

    public void Moving(List<pathNode> path)
    {
         moveSoundEffect.Play();
         if (IS_MOVING)
         {
            SkipAnimation();
         }
          pathWorldPositions = gridObject.targetGrid.ConvertPathNodesToWorldPositions(path);
          gridObject.targetGrid.RemoveObject(gridObject.positionOnGrid, gridObject);

          gridObject.positionOnGrid.x = path[path.Count - 1].pos_x;
          gridObject.positionOnGrid.y = path[path.Count - 1].pos_y;

          gridObject.targetGrid.PlaceObject(gridObject.positionOnGrid, gridObject);
          RotateCharacter(transform.position, pathWorldPositions[0]);
          characterAnimator.StartMoving();
          moveSoundEffect.Pause();
    }

    public void SendMove(List<pathNode> path)
    {

        string pathJson = JsonConvert.SerializeObject(path);

        string gridObjectJson = gridObject.ToJson();

        string combinedJson = $"{{\"Path\": {pathJson}, \"GridObject\": {gridObjectJson}}}";

            Debug.Log("sending chat " + combinedJson);

            WarpClient.GetInstance().SendChat(combinedJson);
    }

    private void RotateCharacter(Vector3 originPos, Vector3 destinationPos)
    {
        Vector3 direction = (destinationPos - originPos).normalized;
        direction.y = 0;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    private void Update()
    {
        if(pathWorldPositions == null) { return; }
        if(pathWorldPositions.Count == 0) { return; }

            transform.position = Vector3.MoveTowards(transform.position, pathWorldPositions[0], moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, pathWorldPositions[0]) < 0.05f)
            {
                pathWorldPositions.RemoveAt(0);
                if (pathWorldPositions.Count == 0) { characterAnimator.StopMoving(); }
                else
                {
                    RotateCharacter(transform.position, pathWorldPositions[0]);
                }
            }
    }

    public void SkipAnimation()
    {
        if(pathWorldPositions.Count < 2) { return; }
        transform.position = pathWorldPositions[pathWorldPositions.Count - 1];
        Vector3 originPosition = pathWorldPositions[pathWorldPositions.Count - 2];
        Vector3 destinationPosition = pathWorldPositions[pathWorldPositions.Count - 1];
        RotateCharacter(originPosition,destinationPosition);
        pathWorldPositions.Clear();
        characterAnimator.StopMoving();
    }

   private void OnMoveCompleted(MoveEvent _Move)
   {
       if(CommandInput.currentCommand == CommandType.MoveTo)
      {
            if (_Move.getSender() != GlobalVariables.userId && _Move.getMoveData() != null)
           {
                   Debug.Log("OnMoveCompleted Move: " + _Move.getMoveData() + ",Sender: " + _Move.getSender() + ",Next Turn: " + _Move.getNextTurn());
                    string senderJson = _Move.getMoveData();
                    var combinedData = MiniJSON.Json.Deserialize(senderJson) as Dictionary<string, object>;
                    string pathJson = combinedData["Path"].ToString();
                    List<pathNode> receivedPath = JsonConvert.DeserializeObject<List<pathNode>>(pathJson);

                    string gridObjectJson = combinedData["GridObject"].ToString();
                    SC_GridObject receivedGridObject = MiniJSON.Json.Deserialize(gridObjectJson) as SC_GridObject;

                    Debug.Log(receivedPath);
                    Debug.Log(receivedGridObject);

                    Moving(receivedPath, receivedGridObject);
            }

       }
   }


    private void OnPrivateChatReceived(string sender, string message)
    {
        Debug.Log("OnPrivateChatReceived " + message);

        if (CommandInput.currentCommand == CommandType.MoveTo)
        {
                var combinedData = MiniJSON.Json.Deserialize(message) as Dictionary<string, object>;
                string pathJson = combinedData["Path"].ToString();
                List<pathNode> receivedPath = JsonConvert.DeserializeObject<List<pathNode>>(pathJson);

                string gridObjectJson = combinedData["GridObject"].ToString();
                SC_GridObject receivedGridObject = MiniJSON.Json.Deserialize(gridObjectJson) as SC_GridObject;

                Debug.Log(receivedPath);
                Debug.Log(receivedGridObject);

                Moving(receivedPath, receivedGridObject);
        }
    }

    private void OnChatReceived(ChatEvent eventObj)
    {
        Debug.Log("OnChatReceived " + eventObj.getMessage() + ", sent by: "+ eventObj.getSender());

        if (eventObj.getSender() != GlobalVariables.userId && eventObj.getMessage() != null)
        {
            var combinedData = MiniJSON.Json.Deserialize(eventObj.getMessage()) as Dictionary<string, object>;
            string pathJson = combinedData["Path"].ToString();
            List<pathNode> receivedPath = JsonConvert.DeserializeObject<List<pathNode>>(pathJson);

            string gridObjectJson = combinedData["GridObject"].ToString();
            SC_GridObject receivedGridObject = MiniJSON.Json.Deserialize(gridObjectJson) as SC_GridObject;

            Debug.Log(receivedPath);
            Debug.Log(receivedGridObject);

            Moving(receivedPath, receivedGridObject);
        }
    }

    private void OnSendChat(string _Sender, string _Message)
    {
        if(_Sender != GlobalVariables.userId && _Message != null)
        {
        var combinedData = MiniJSON.Json.Deserialize(_Message) as Dictionary<string, object>;
        string pathJson = combinedData["Path"].ToString();
        List<pathNode> receivedPath = JsonConvert.DeserializeObject<List<pathNode>>(pathJson);

        string gridObjectJson = combinedData["GridObject"].ToString();
        SC_GridObject receivedGridObject = MiniJSON.Json.Deserialize(gridObjectJson) as SC_GridObject;

        Debug.Log(receivedPath);
        Debug.Log(receivedGridObject);

        Moving(receivedPath, receivedGridObject);
        }
    }

    public void Moving(List<pathNode> path, SC_GridObject gridObject)
    {
        if (IS_MOVING)
        {
            SkipAnimation();
        } 
            pathWorldPositions = gridObject.targetGrid.ConvertPathNodesToWorldPositions(path);
            gridObject.targetGrid.RemoveObject(gridObject.positionOnGrid, gridObject);

            gridObject.positionOnGrid.x = path[path.Count - 1].pos_x;
            gridObject.positionOnGrid.y = path[path.Count - 1].pos_y;

            gridObject.targetGrid.PlaceObject(gridObject.positionOnGrid, gridObject);
            RotateCharacter(transform.position, pathWorldPositions[0]);
            characterAnimator.StartMoving();
    }
}
