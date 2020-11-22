using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;


public class NetworkManager : MonoBehaviourPunCallbacks
{
    [Header("Connection Status")]
    public Text connectionStatusText;

    [Header("Login UI panel")]
    public InputField playerNameInput;
    public GameObject Login_UI_Panel;

    [Header("Game Options UI Panel")]
    public GameObject GameOptions_UI_Panel;


    [Header("Create Room UI Panel")]
    public GameObject CreateRoom_UI_Panel;
    public InputField roomNameInputField;

    public InputField maxPlayerInputField;

    [Header("Inside Room UI Panel")]
    public GameObject InsideRoom_UI_Panel;

    [Header("Room List UI Panel")]
    public GameObject RoomList_UI_Panel;
    public GameObject RoomListEntryPrefab;
    public GameObject roomListParentGameobject;

    [Header("Join Random Room UI Panel")]
    public GameObject JoinRandomRoom_UI_Panel;

    private Dictionary<string, RoomInfo> cachedRoomList;
    private Dictionary<string, GameObject> roomListGameobjects;

    ///////////////////////////////////////////////////////////////////////////

    #region Unity Methods

    // Start is called before the first frame update
    void Start()
    {
        ActivatePanel(Login_UI_Panel.name);
        cachedRoomList = new Dictionary<string, RoomInfo>();
        roomListGameobjects = new Dictionary<string, GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        connectionStatusText.text = "Connection Status: " + PhotonNetwork.NetworkClientState;
    }

    #endregion Unity Methods

    ///////////////////////////////////////////////////////////////////////////

    #region UI callbacks
    public void OnLoginButtonClicked()
    {
        string playerName = playerNameInput.text;
        if(!string.IsNullOrEmpty(playerName))
        {
            PhotonNetwork.LocalPlayer.NickName = playerName;
            PhotonNetwork.ConnectUsingSettings();
        }
        else 
        {
            Debug.Log("Player Name is invalid");
        }
    }

    public void OnCreateRoomButtonClicked()
    {
        string roomName = roomNameInputField.text;

        if(string.IsNullOrEmpty(roomName))
        {
            roomName = "Room " + Random.Range(1000, 10000);
        }

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = (byte)int.Parse(maxPlayerInputField.text); 

        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }

    public void OnCancelButtonClicked()
    {
        ActivatePanel(GameOptions_UI_Panel.name);
    }

    public void OnShowRoomListButtonClicked()
    {
        if(!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }

        ActivatePanel(RoomList_UI_Panel.name);
    }

    #endregion UI callbacks

    /////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////

    #region Photon Callbacks
    public override void OnConnected()
    {
        Debug.Log("Connected to Internet");
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " is connected to Photon");
        ActivatePanel(GameOptions_UI_Panel.name);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log(PhotonNetwork.CurrentRoom.Name + " is created.");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " joined " + PhotonNetwork.CurrentRoom.Name);
        ActivatePanel(InsideRoom_UI_Panel.name);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        ClearRoomListView();

        // Update cached room list
        foreach(RoomInfo room in roomList)
        {
            Debug.Log(room.Name);
            // A room that is Full, Closed, or Hidden is marked as "removed from list"
            if(!room.IsOpen || !room.IsVisible || room.RemovedFromList) 
            {
                cachedRoomList.Remove(room.Name);
            }
            else 
            {
                if(cachedRoomList.ContainsKey(room.Name))
                {
                    cachedRoomList[room.Name] = room; // Update room in list
                }
                else
                {
                    cachedRoomList.Add(room.Name, room);
                }
            }
        }
        // Instantiate GameObject
        foreach (RoomInfo room in cachedRoomList.Values)
        {
            GameObject roomListEntryGameObject = Instantiate(RoomListEntryPrefab);
            roomListEntryGameObject.transform.SetParent(roomListParentGameobject.transform);
            roomListEntryGameObject.transform.localScale = Vector3.one;

            roomListEntryGameObject.transform.Find("RoomNameText").GetComponent<Text>().text = room.Name;
            roomListEntryGameObject.transform.Find("RoomPlayersText").GetComponent<Text>().text = room.PlayerCount + " / " + room.MaxPlayers;
            roomListEntryGameObject.transform.Find("JoinRoomButton").GetComponent<Button>().onClick.AddListener( ()=> OnJoinRoomButtonClicked(room.Name) ); // check Unity Docu..

            roomListGameobjects.Add(room.Name, roomListEntryGameObject);
        }
    }
    
    #endregion Photon Callbacks

    /////////////////////////////////////////////////////////////////////////////////

    #region Public Methods
    public void ActivatePanel(string panelToBeActivated)
    {
        Login_UI_Panel.SetActive(panelToBeActivated.Equals(Login_UI_Panel.name));
        GameOptions_UI_Panel.SetActive(panelToBeActivated.Equals(GameOptions_UI_Panel.name));
        CreateRoom_UI_Panel.SetActive(panelToBeActivated.Equals(CreateRoom_UI_Panel.name));
        InsideRoom_UI_Panel.SetActive(panelToBeActivated.Equals(InsideRoom_UI_Panel.name));
        RoomList_UI_Panel.SetActive(panelToBeActivated.Equals(RoomList_UI_Panel.name));
        JoinRandomRoom_UI_Panel.SetActive(panelToBeActivated.Equals(JoinRandomRoom_UI_Panel.name));
    }
    #endregion Public Methods

    #region Private Methods
    private void OnJoinRoomButtonClicked(string _roomName) // listener for room Prefab button
    {
        if(PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }
        PhotonNetwork.JoinRoom(_roomName);
    }
    private void ClearRoomListView()
    {
        foreach (var roomListGameobject in roomListGameobjects.Values)
        {
            Destroy(roomListGameobject);
        }
        roomListGameobjects.Clear();
    }


    #endregion Private Methods
}
