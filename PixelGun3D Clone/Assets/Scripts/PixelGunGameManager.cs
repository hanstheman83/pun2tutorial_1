using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PixelGunGameManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Photon Callbacks
    public override void OnJoinedRoom()
    {
        Debug.Log("Player Name : " + PhotonNetwork.NickName + " Joined " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("New Player " + newPlayer.NickName + " entered " + PhotonNetwork.CurrentRoom.Name);
        Debug.Log("Player Count : " + PhotonNetwork.CurrentRoom.PlayerCount);
    }
    #endregion Photon Callbacks
}
