using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PixelGunGameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject playerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        if(playerPrefab == null) { Debug.LogError("Player prefab not set!"); return; }

        int randomPoint = Random.Range(-20,20);
        if(PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(randomPoint, 0, randomPoint), Quaternion.identity);
        }
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
