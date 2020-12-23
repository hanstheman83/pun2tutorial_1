using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSetup : MonoBehaviourPunCallbacks
{
    public GameObject[] FPS_Hands_ChildGameObjects; 
    public GameObject[] soldier_ChildGameObjects; 

    // Start is called before the first frame update
    void Start()
    {
        if(photonView.IsMine)
        {
            foreach(GameObject child in FPS_Hands_ChildGameObjects)
            {
                child.SetActive(true);
            }
            foreach(GameObject child in soldier_ChildGameObjects)
            {
                child.SetActive(false);
            }
        }
        else
        {
            foreach(GameObject child in FPS_Hands_ChildGameObjects)
            {
                child.SetActive(false);
            }
            foreach(GameObject child in soldier_ChildGameObjects)
            {
                child.SetActive(true);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
