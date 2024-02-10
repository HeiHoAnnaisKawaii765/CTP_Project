using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class LauncherScript : MonoBehaviourPunCallbacks
{
    [SerializeField]
    Transform[] spawnPoint;
    // Start is called before the first frame update
    void Start()
    {
        //PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.Instantiate("Player", spawnPoint[Random.Range(0,1)].position, Quaternion.identity);
    }

   
}
