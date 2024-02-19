using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class LevelManager : MonoBehaviourPun
{
    public int teamARocketNum, teamBRocketNum;
    public bool gameOver;
    public int[] teamAIngredient, teamBIngredient;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(gameOver)
        {

        }
    }
    public void GameOver()
    {
        gameOver = true;
    }
}
