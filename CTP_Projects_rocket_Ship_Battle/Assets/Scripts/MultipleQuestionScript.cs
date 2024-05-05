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
    [SerializeField] Sprite im;
    private void Start()
    {
        Image image = transform.Find("BG").GetComponent<Image>();
        image.sprite = im;
        reward = Random.Range(0, 3);
    }
    private void Update()
    {
        lm = FindObjectOfType<LevelManager>();
       
    }
    public void CheckResult(string answer)
    {
        if(lm.gameStart)
        {
            switch (correctAnswer)
            {
                case "A":
                    buttonBG[0].color = Color.blue;

                    break;
                case "B":
                    buttonBG[1].color = Color.blue;

                    break;
                case "C":
                    buttonBG[2].color = Color.blue;

                    break;
                case "D":
                    buttonBG[3].color = Color.blue;

                    break;
            }
            if (answer == correctAnswer)
            {
                Debug.Log("Right");
                lm.photonView.RPC("AddDedectRocket", RpcTarget.All, 1, reward, team);
            }
            else
            {
                Debug.Log("Wrong");
            }
            StartCoroutine(Destroy());
            Destroy(gameObject,3f);
        }
       
        //photonView.RPC("SelfDestroy", RpcTarget.All);
        
        

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
        Destroy(this.gameObject);
    }
    [PunRPC]
    void SelfTeam(Collider col)
    {
        team = col.GetComponent<ShipController>().team;
        ship = col.GetComponent<ShipController>();
    }
    [PunRPC]
    void RandomNum()
    {
        
    }
    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(2);
        ship.photonView.RPC("GenNewQs", RpcTarget.All);
    }
    
    
}
