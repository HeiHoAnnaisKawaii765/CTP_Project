using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class LevelManager : MonoBehaviourPun
{
    public int teamAPlayerNum,teamBPlayerNum;
    public bool gameOver,gameStart;
    public int[] teamARocketNum, teamBRocketNum;
    [SerializeField]
    Transform[] shipPos;
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
                photonView.RPC("LowestRocketValue", RpcTarget.All);
             
            }
        }
    }
    public void GameOver()
    {
        gameOver = true;
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
