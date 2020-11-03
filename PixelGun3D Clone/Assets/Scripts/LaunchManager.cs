using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class LaunchManager : MonoBehaviourPunCallbacks
{
    public GameObject EnterGamePanel;
    public GameObject ConnectionStatusPanel;
    public GameObject LobbyPanel;

    #region Unity Methods
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true; // with MasterClient (the one who created the room)
        // So when MasterClient loads a new scene the other clients will follow..
    }



    // Start is called before the first frame update
    void Start()
    {
        EnterGamePanel.SetActive(true);
        ConnectionStatusPanel.SetActive(false);
        LobbyPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion

    #region Public Methods
    public void ConnectToPhotonServer()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            EnterGamePanel.SetActive(false);
            ConnectionStatusPanel.SetActive(true);

        }
    }

    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    #endregion

    #region Photon Callbacks
    public override void OnConnectedToMaster()
    {
        Debug.Log(PhotonNetwork.NickName + " Connected to photon server");
        LobbyPanel.SetActive(true);
        ConnectionStatusPanel.SetActive(false);
    }

    public override void OnConnected()
    {
        Debug.Log("Connected to Internet");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        CreateAndJoinRoom();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log(PhotonNetwork.NickName + " joined " + PhotonNetwork.CurrentRoom.Name);
        PhotonNetwork.LoadLevel("GameScene"); // for non Master clients they will load this scene even before this callback (OnJoinedRoom)gets called..
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("New Player : " + newPlayer.NickName + " joined this room (" + PhotonNetwork.CurrentRoom.Name + ")");
        Debug.Log("Player Count : " + PhotonNetwork.CurrentRoom.PlayerCount);

    }


    #endregion Photon Callbacks

    #region Private Methods
    void CreateAndJoinRoom()
    {
        string randomRoomName = "Room " + Random.Range(0, 10000);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = 20;

        PhotonNetwork.CreateRoom(randomRoomName, roomOptions);
    }
    #endregion
}
