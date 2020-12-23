using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MobileFPSGameManager : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    // Start is called before the first frame update
    void Start()
    {
        if(playerPrefab != null)
        {
            int randomPoint = Random.Range(-10,10);
            if(PhotonNetwork.IsConnectedAndReady)
            {
                PhotonNetwork.Instantiate(playerPrefab.name,new Vector3(randomPoint,0,randomPoint), Quaternion.identity);
            }
        }
        else 
        {
            Debug.Log("PlayerPrefab is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
