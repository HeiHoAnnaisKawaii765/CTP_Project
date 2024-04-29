using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class FireScript : MonoBehaviourPun
{
    float fireRemain =150;
    private void Update()
    {
        transform.localScale = new Vector3(1, fireRemain / 150, 1);
        if(fireRemain<=0)
        {
            photonView.RPC("Off", RpcTarget.All);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.tag=="Water")
        {
            photonView.RPC("Pouring", RpcTarget.All);  
        }
    }
    [PunRPC]
    void Pouring()
    {
        fireRemain -= 20 * Time.deltaTime;
    }
    [PunRPC]
    void Off()
    {
        Destroy(gameObject);
    }
}
