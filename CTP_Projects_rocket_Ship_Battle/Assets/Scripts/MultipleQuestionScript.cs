using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class MultipleQuestionScript : MonoBehaviourPun
{
    [SerializeField]
    string correctAnswer;
    [SerializeField]
    Image[] buttonBG;
    LevelManager lm;
    string team;
    [SerializeField]
    int reward;
    [SerializeField]
    Transform[] answerPos;
    [SerializeField]
    GameObject[] answerBlocks;
    ShipController ship;
    private void Start()
    {
       
    }
    private void Update()
    {
        lm = FindObjectOfType<LevelManager>();
       
    }
    public void CheckResult(string answer)
    {
        switch(correctAnswer)
        {
            case "A":
                buttonBG[0].color = Color.blue;
                buttonBG[1].color = Color.red;
                buttonBG[2].color = Color.red;
                buttonBG[3].color = Color.red;
                break;
            case "B":
                buttonBG[1].color = Color.blue;
                buttonBG[0].color = Color.red;
                buttonBG[2].color = Color.red;
                buttonBG[3].color = Color.red;
                break;
            case "C":
                buttonBG[2].color = Color.blue;
                buttonBG[1].color = Color.red;
                buttonBG[0].color = Color.red;
                buttonBG[3].color = Color.red;
                break;
            case "D":
                buttonBG[3].color = Color.blue;
                buttonBG[1].color = Color.red;
                buttonBG[2].color = Color.red;
                buttonBG[0].color = Color.red;
                break;
        }
        if(answer==correctAnswer)
        {
            Debug.Log("Right");
            lm.photonView.RPC("AddDedectRocket", RpcTarget.All,1, reward,team);
        }
        else
        {
            Debug.Log("Wrong");
        }
        ship.photonView.RPC("GenNewQs", RpcTarget.All);
        photonView.RPC("SelfDestroy", RpcTarget.All);
        
        

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Vehicle")
        {
            //photonView.RPC("SelfTeam", RpcTarget.All, other);
            team = other.GetComponent<ShipController>().team;
            ship = other.GetComponent<ShipController>();
        }
    }
    [PunRPC]
    void SelfDestroy()
    {
        Destroy(this.gameObject, 5);
    }
    [PunRPC]
    void SelfTeam(Collider col)
    {
        
    }
    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(3);
        photonView.RPC("SelfDestroy", RpcTarget.All);
    }
    
    
}
