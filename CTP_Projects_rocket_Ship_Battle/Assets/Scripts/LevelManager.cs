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
    public
    Transform[] shipPos,orgShipPos;
    [SerializeField]
    float timeLength,restartGameLength=120,startCountDown = 150;
    [SerializeField]
    Slider timeSlider;
    [SerializeField]
    string[] winTxtContent;
    [SerializeField]
    TMP_Text timeText,winingTxt,restartCountText;
    [SerializeField]
    GameObject winUI,timerUI,quitUI;
    [SerializeField]
    ShipController[] ship;
    [SerializeField]
    Slider[] shipHpSlider;
    SoundManager sm;
    // Start is called before the first frame update
    void Start()
    {
        orgShipPos = new Transform[ship.Length];
        sm = FindObjectOfType<SoundManager>();
        teamARocketNum = new int[4];
        teamBRocketNum = new int[4];
        for (int i = 0;i<shipHpSlider.Length;i++)
        {
            shipHpSlider[i].maxValue = ship[i].hp;
            shipHpSlider[i].minValue = 0;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(FindObjectOfType<RocketScript>()!=null)
        {
            sm.ChangeSoundTrack(1);
        }
        else
        {
            sm.ChangeSoundTrack(0);
        }
        if(!gameStart)
        {
            quitUI.SetActive(true);
        
        }
        else
        {
            quitUI.SetActive(false);
        }
        photonView.RPC("UIGameMechan", RpcTarget.All);
        
    }
    [PunRPC]
    public void GameOver(string shipInc)
    {
        gameStart = false;
        gameOver = true;
        winUI.SetActive(true);
        switch (shipInc)
        {
            case "A":
                //B wins
                winingTxt.text = winTxtContent[1];
                break;
            case "B":
                winingTxt.text = winTxtContent[0];
                //A wins
                break;
            default:
                winingTxt.text = "The game is a draw";
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
        winUI.SetActive(false);
    }
    [PunRPC]
    private void ResetGame()
    {
        timeLength = 280;
        gameOver = false;
        teamAPlayerNum = 0;
        teamBPlayerNum = 0;
        for(int i =0;i<teamARocketNum.Length;i++)
        {
            teamARocketNum[i] = 0;
            teamBRocketNum[i] = 0;
        }
        for(int i =0;i<ship.Length;i++)
        {
            ship[i].gameObject.transform.position = orgShipPos[i].position;
            ship[i].gameObject.transform.rotation = orgShipPos[i].rotation;
            ship[i].hp = 1600;
            ship[i].speedSlider.value = 0;
            ship[i].steerSlider.value = 0;

        }
        PlayerController[] pScript = FindObjectsOfType<PlayerController>();
        foreach (PlayerController p in pScript)
        {
            p.team = null;
        }
    }

    [PunRPC]
    private void UpdatePlayerNum()
    {
        teamAPlayerNum = 0;
        teamBPlayerNum = 0;
        PlayerController[] pScript = FindObjectsOfType<PlayerController>();
        foreach (PlayerController p in pScript)
        {
             switch(p.team)
            {
                case "A":
                    teamAPlayerNum += 1;

                    break;
                case "B":
                    teamBPlayerNum += 1;

                    break;
            }
        }
    }

    [PunRPC]
    private void UpdatingHPValue()
    {
        
        for (int i = 0; i < ship.Length; i++)
        {
            shipHpSlider[i].value = ship[i].hp;
            

        }
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
    public void StayOrLeave(int index)
    {
        switch (index)
        {
            case 0:
                PhotonNetwork.LeaveRoom();
                PhotonNetwork.LoadLevel(0);
                break;
            case 1:
                if(photonView.IsMine)
                {
                    winUI.SetActive(false);
                    photonView.GetComponent<PlayerController>().TeamSelect(0);
                }
                break;
            case 2:
                if (photonView.IsMine)
                {
                    winUI.SetActive(false);
                    photonView.GetComponent<PlayerController>().TeamSelect(1);
                }
                break;
        }
    }
    public void LeaveRoom()
    {
        string Lteam="";
        if (!gameStart)
        {
            PlayerController[] pScript = FindObjectsOfType<PlayerController>();
            foreach (PlayerController p in pScript)
            {
                if (photonView.IsMine)
                {
                    Lteam = p.team;

                }
            }
            switch (Lteam)
            {
                case "A":

                    PhotonNetwork.LeaveRoom();
                    PhotonNetwork.LoadLevel(0);
                    teamAPlayerNum -= 1;
                    photonView.RPC("PNumChange", RpcTarget.All, 0);
                    break;
                case "B":
                    PhotonNetwork.LeaveRoom();
                    PhotonNetwork.LoadLevel(0);
                    photonView.RPC("PNumChange", RpcTarget.All,1);

                    break;
                default:
                    PhotonNetwork.LeaveRoom();
                    PhotonNetwork.LoadLevel(0);
                        break ;
            }
        }
    }
    [PunRPC]
    void PNumChange(int tea)
    {
        switch(tea)
        {
            case 0:
                teamAPlayerNum -= 1;
                break;
            case 1:
                teamBPlayerNum -= 1;
                break;
        }    
    }
    [PunRPC]
    void UIGameMechan()
    {
        if (!gameOver)
        {
            if (!gameStart)
            {
                
                if (PhotonNetwork.CountOfPlayers > 1)
                {
                    photonView.RPC("UpdatePlayerNum", RpcTarget.All);

                    if (teamAPlayerNum > 0 && teamBPlayerNum > 0)
                    {
                        startCountDown -= Time.deltaTime;
                        timeText.text = "Game will start at:" + Mathf.Floor((startCountDown / 60)).ToString("00") + ":" + (startCountDown % 60).ToString("00");
                        if (startCountDown <= 0)
                        {
                            photonView.RPC("StartGame", RpcTarget.All);
                            PlayerController[] pScript = FindObjectsOfType<PlayerController>();
                            foreach (PlayerController p in pScript)
                            {
                                if (p.team == null)
                                {
                                    p.photonView.RPC("TeamSelect", RpcTarget.All, Random.Range(0, 1));
                                }
                            }
                        }
                    }
                }

            }
            else
            {
                
                photonView.RPC("UpdatingHPValue", RpcTarget.All);
                restartGameLength = 120;
                timeLength -= Time.deltaTime;
                timeText.text = Mathf.Floor((timeLength / 60)).ToString("00") + ":" + (timeLength % 60).ToString("00");
                photonView.RPC("LowestRocketValue", RpcTarget.All);
                if (timeLength <= 0)
                {

                    if (ship[0].hp > ship[1].hp)
                    {
                        photonView.RPC("GameOver", RpcTarget.All, "B");
                    }
                    else if (ship[0].hp < ship[1].hp)
                    {
                        photonView.RPC("GameOver", RpcTarget.All, "A");
                    }
                    else
                    {

                        photonView.RPC("GameOver", RpcTarget.All, "");
                    }
                }

            }
        }
        else
        {
            restartGameLength -= Time.deltaTime;
            restartCountText.text = "Game will reset at:" + Mathf.Floor((restartGameLength / 60)).ToString("00") + ":" + (restartGameLength % 60).ToString("00");
            if (restartGameLength <= 0 && PhotonNetwork.CountOfPlayers > 1)
            {
                photonView.RPC("ResetGame", RpcTarget.All);
            }
            else
            {
                restartGameLength = 30f;
            }

        }
    }
}
