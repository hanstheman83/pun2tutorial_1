using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Shooting : MonoBehaviour
{
    [SerializeField] Camera fpsCamera;

    public float fireRate = 0.1f;
    float fireTimer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(fireTimer <= fireRate) // Coroutine better..
        {
            fireTimer += Time.deltaTime;
        }

        if(Input.GetButton("Fire1") && fireTimer > fireRate)
        {
            fireTimer = 0.0f;
            RaycastHit _hit;
            Ray ray = fpsCamera.ViewportPointToRay(new Vector3(0.5f,0.5f)); // X, Y, center of screne..

            if(Physics.Raycast(ray, out _hit, 100)) // 100 is max distance..
            {
                Debug.Log("Hit : " + _hit.collider.gameObject.name);
                if(_hit.collider.gameObject.CompareTag("Player") && !_hit.collider.gameObject.GetComponent<PhotonView>().IsMine)
                {
                    _hit.collider.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBuffered, 10f); // specific remote player based on photonView ID.
                }
            }
        }
    }
}
