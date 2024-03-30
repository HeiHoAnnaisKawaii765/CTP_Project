using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
public class LevelManager : MonoBehaviourPun
{
    public int teamAPlayerNum,teamBPlayerNum;
    public bool gameOver,gameStart;
    public int[] teamARocketNum, teamBRocketNum;
    [SerializeField]
    Transform[] shipPos;
    [SerializeField]
    float timeLength;
    [SerializeField]
    Slider timeSlider;
    [SerializeField]
    string[] winTxtContent;
    [SerializeField]
    TMP_Text timeText,winingTxt;
    [SerializeField]
    GameObject winUI,timerUI;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(!gameOver)
        {
            if(!gameStart)
            {
                
            }
            else
            {
                timeLength -= Time.deltaTime;
                timeText.text = Mathf.Floor((timeLength / 60)).ToString("00") + ":" + (timeLength % 60).ToString("00");
                photonView.RPC("LowestRocketValue", RpcTarget.All);
                if(timeLength<=0)
                { 
                    gameOver = true;
                }
             
            }
        }
        else
        {

        }
    }
    [PunRPC]
    public void GameOver(string shipInc)
    {
        gameOver = true;
        switch(shipInc)
        {
            case "A":
                //B wins
                winingTxt.text = winTxtContent[1];
                break;
            case "B":
                winingTxt.text = winTxtContent[0];
                //A wins
                break;
        }
    }
    [PunRPC]
    private void LowestRocketValue()
    {
        for (int i = 0; i<teamARocketNum.Length;i++)
        {
            if(teamARocketNum[i]<0)
            {
                teamARocketNum[i] = 0;
            }
        }
        for (int i = 0; i < teamBRocketNum.Length; i++)
        {
            if (teamBRocketNum[i] < 0)
            {
                teamBRocketNum[i] = 0;
            }
        }
    }
    [PunRPC]
    public void TeamMemberNumChange(int select)
    {
        switch (select)
        {
            case 0:
                teamAPlayerNum += 1;
                break;
            case 1:
                teamBPlayerNum += 1;
                break;

        }
    }
    [PunRPC]
    private void StartGame()
    {
        gameStart = true;
    }
    [PunRPC]
    public void AddDedectRocket(int value,int type,string team)
    {
        switch(team)
        {
            case "A":
                teamARocketNum[type] += value;
                break;
            case "B":
                teamBRocketNum[type] += value;
                break;
        }
    }
}
