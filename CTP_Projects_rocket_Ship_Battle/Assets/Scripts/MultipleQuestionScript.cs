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
    private void Update()
    {
        lm = FindObjectOfType<LevelManager>();
        int index = Random.Range(1, 4);
        answerBlocks[0].transform.localPosition = answerPos[index].localPosition;
        answerBlocks[index].transform.localPosition = answerPos[0].localPosition;
        for(int i = 0;i< 4;i++)
        {
            if(!(i == 0||i==index))
            {
                answerBlocks[i].transform.position = answerPos[i].localPosition;
            }
        }
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
        photonView.RPC("SelfDestroy", RpcTarget.All);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag =="Vehicle")
        {
            team = other.GetComponent<ShipController>().team;
        }
    }
    [PunRPC]
    void SelfDestroy()
    {
        Destroy(gameObject, 5);
    }
    
}
