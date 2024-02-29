using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class LevelManager : MonoBehaviourPun
{
    public int teamARocketNum, teamBRocketNum,teamAPlayerNum,teamBPlayerNum;
    public bool gameOver,gameStart;
    public int[] teamAIngredient, teamBIngredient;

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
        }
    }
    public void GameOver()
    {
        gameOver = true;
    }
    [PunRPC]
    private void LowestRocketValue()
    {
        
    }
}
