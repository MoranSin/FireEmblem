using AssemblyCSharp;
using com.shephertz.app42.gaming.multiplayer.client;
using com.shephertz.app42.gaming.multiplayer.client.events;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;
using UnityEngine.SceneManagement;

public class SC_MulitiplayerLogic : MonoBehaviour
{

    #region Singleton
    private static SC_MulitiplayerLogic instance;
    public static SC_MulitiplayerLogic Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.Find("MulitiplayerLogic").GetComponent<SC_MulitiplayerLogic>();
            }
                return instance;
        }
    }
    #endregion

    #region AppWarp Keys
    private string apiKey = "7889f957f7668a607a8e0a26a64d90930d1f784b043355e8160b7bd5bd1a2078";
    private string secretKey = "57b9ff37412acc710de5b9d347c89926cf0fc802cdcfa4bc6d292b65b731d8ca";

    public Listener listener;
    #endregion

    #region Variables
    Dictionary<string, GameObject> unityObjects;
    private Dictionary<string, object> passedParams;
    private List<string> roomsIds;
    private int numOfPlayers = 2;
    public string roomName = "FE Room ";
    private string roomId;
    private int roomIndex = 0;
    #endregion

    #region MonoBehaviour

    private void OnEnable()
    {
        Listener.OnConnect += OnConnect;
        Listener.OnRoomsInRange += OnRoomsInRange;
        Listener.OnCreateRoom += OnCreateRoom;
        Listener.OnJoinRoom += OnJoinRoom;
        Listener.OnGetLiveRoomInfo += OnGetLiveRoomInfo;
        Listener.OnUserJoinRoom += OnUserJoinRoom;
        Listener.OnGameStarted += OnGameStarted;
    }

    private void OnDisable()
    {
        Listener.OnConnect -= OnConnect;
        Listener.OnRoomsInRange -= OnRoomsInRange;
        Listener.OnCreateRoom -= OnCreateRoom;
        Listener.OnJoinRoom -= OnJoinRoom;
        Listener.OnGetLiveRoomInfo -= OnGetLiveRoomInfo;
        Listener.OnUserJoinRoom -= OnUserJoinRoom;
        Listener.OnGameStarted -= OnGameStarted;

    }


    void Awake()
    {
        InitAwake();
    }

    private void Start()
    {
        InitStart();
    }

    public static string RandomNames()
    {
        string[] names = { "Yillisian", "Pelgian", "Robin", "Lucina", "Marth", "Chrom", "Grima", "Lissa", "Anna", "Gaius", "Emmeryn", "Henry", "Inigo", "Morgan", "Lon'qu", "Owain", "Sully", "Severa" };
        return names[UnityEngine.Random.Range(0,names.Length)];
    }

    #endregion

    #region Logics

    private void InitAwake()
    {
        GlobalVariables.gameState = GlobalVariables.GameState.MultiPlayer;
        unityObjects = new Dictionary<string, GameObject>();
        GameObject[] obj = GameObject.FindGameObjectsWithTag("UnityObject");
        foreach (GameObject g in obj)
            unityObjects.Add(g.name, g);

        passedParams = new Dictionary<string, object>()
        {
            {"Password", GlobalVariables.password }
        };

        Debug.Log(GlobalVariables.password);

        listener = new Listener();

        WarpClient.initialize(apiKey, secretKey);
        WarpClient.GetInstance().AddConnectionRequestListener(listener);
        WarpClient.GetInstance().AddChatRequestListener(listener);
        WarpClient.GetInstance().AddUpdateRequestListener(listener);
        WarpClient.GetInstance().AddLobbyRequestListener(listener);
        WarpClient.GetInstance().AddNotificationListener(listener);
        WarpClient.GetInstance().AddRoomRequestListener(listener);
        WarpClient.GetInstance().AddTurnBasedRoomRequestListener(listener);
        WarpClient.GetInstance().AddZoneRequestListener(listener);


        GlobalVariables.userId = RandomNames();

    }

    private void InitStart()
    {
        unityObjects["Game"].SetActive(false);
        unityObjects["Menu"].SetActive(true);

        unityObjects["FindGame"].GetComponent<Button>().interactable = false;
        unityObjects["UserText"].GetComponent<TextMeshProUGUI>().text = "User: " + GlobalVariables.userId;

        WarpClient.GetInstance().Connect(GlobalVariables.userId);
        UpdateStatus("Open Connection...");
    }

    private void UpdateStatus(string status)
    {
        unityObjects["ConnectStatus"].GetComponent<TextMeshProUGUI>().text = status;
    }

    private void DoRoomSearchLogic()
    {
        if(roomIndex < roomsIds.Count)
        {
            UpdateStatus("Bring room info (" + roomsIds[roomIndex] + ")");
            WarpClient.GetInstance().GetLiveRoomInfo(roomsIds[roomIndex]);

        }
        else
        {
            UpdateStatus("Creating Room..");
            int randNum = UnityEngine.Random.Range(100000, 999999);
            WarpClient.GetInstance().CreateTurnRoom(roomName + randNum, GlobalVariables.userId, numOfPlayers, passedParams, GlobalVariables.TurnTime);
        }
    }


    #endregion

    #region Server Callbacks
    private void OnConnect(bool IsSuccess)
    {
        if (IsSuccess)
        {
            UpdateStatus("Connected.");
            unityObjects["FindGame"].GetComponent<Button>().interactable = true;
        }
        else
        {
            UpdateStatus("Failed to connect.");
        }
    }

    private void OnRoomsInRange(bool IsSuccess, MatchedRoomsEvent eventObj)
    {
        Debug.Log("OnRoomInRange " + IsSuccess);
        if (IsSuccess)
        {
            UpdateStatus("Parsing Rooms..");
            roomsIds = new List<string>();
            foreach(var RoomData in eventObj.getRoomsData())
            {
                Debug.Log("Room Id: " + RoomData.getId());
                Debug.Log("Room Owner " + RoomData.getRoomOwner());
                roomsIds.Add(RoomData.getId());
            }

                roomIndex = 0;
                DoRoomSearchLogic();

        }
    }


    private void OnCreateRoom(bool IsSuccess, string RoomId)
    {
        if (IsSuccess)
        {
            roomId = RoomId;
            UpdateStatus("Room Has Been Created, Room ID: " + RoomId);
            WarpClient.GetInstance().JoinRoom(roomId);
            WarpClient.GetInstance().SubscribeRoom(roomId);

        }
        else
        {
            Debug.Log("Cant Create Room..");
            roomIndex++;
            DoRoomSearchLogic();
        }
    }

    private void OnJoinRoom(bool IsSuccess, string RoomId)
    {
        if (IsSuccess)
        {
            UpdateStatus("Joined Room, ID: " + RoomId);
            unityObjects["RoomText"].GetComponent<TextMeshProUGUI>().text = "Room: " + RoomId;

        }
        else
        {
            UpdateStatus("Failed to join Room ID: " + RoomId);
        }
    }

    private void OnGetLiveRoomInfo(LiveRoomInfoEvent eventObj)
    {
        Debug.Log("OnGetLiveInfo");
        UpdateStatus("Received Data OnGetLiveRoomInfo");
        if(eventObj != null && eventObj.getProperties() != null)
        {
            Dictionary<string,object> properties = eventObj.getProperties();
            if(properties.ContainsKey("Password") && properties["Password"].ToString() == passedParams["Password"].ToString())
            {
                roomId = eventObj.getData().getId();
                UpdateStatus("Received room info, joining room: " + roomId);
                WarpClient.GetInstance().JoinRoom(roomId);
                WarpClient.GetInstance().SubscribeRoom(roomId);

            }
            else
            {
                roomIndex++;
                DoRoomSearchLogic();
            }
        }

    }

    private void OnUserJoinRoom(RoomData eventObj, string UserName)
    {
        UpdateStatus(UserName + " had joined the room.");
        if(eventObj.getRoomOwner() == GlobalVariables.userId && GlobalVariables.userId != UserName)
        {
            GlobalVariables.opponnentId = UserName;
            UpdateStatus("Starting game..");
            WarpClient.GetInstance().startGame();
        }

    }

    private void OnGameStarted(string Sender, string RoomId, string NextTurn)
    {
            UpdateStatus("Game Started.");  
            unityObjects["Menu"].SetActive(false);
            unityObjects["Game"].SetActive(true);
            SC_RoundManager.instance.OnGameStarted(Sender, RoomId, NextTurn);

    }

    #endregion

    #region Controller
    public void FindGameLogic()
    {
        unityObjects["FindGame"].GetComponent<Button>().interactable = false;
        WarpClient.GetInstance().GetRoomsInRange(1, 2);
        UpdateStatus("Searching for a rooms..");

    }


    #endregion
}
