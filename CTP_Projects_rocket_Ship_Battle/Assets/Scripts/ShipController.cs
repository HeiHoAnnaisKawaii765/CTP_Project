using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class ShipController : MonoBehaviourPun
{
    float bounancy = 50;
    float SpeedRate;
    public
    int hp = 1600;
    [SerializeField]
    float maxSpeed,maxturnRate;
    Rigidbody rb;
    float currentTurnRate,radius = 4f;
    public bool isControlling;
    public 
    Slider speedSlider, steerSlider;
    bool isOnFire;
    public string team;
    LevelManager lm;
    PlayerController[] pCon;
    [SerializeField]
    GameObject takeControlButton,rudder,shipcontrolSlider,shipUI,fameObj;
    [SerializeField]
    GameObject[] quizItem;
    public Transform conPos;
    [SerializeField]
    Transform mcPos;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        
        speedSlider.maxValue = maxSpeed;
        speedSlider.minValue = -1;
        steerSlider.maxValue = maxturnRate;
        steerSlider.minValue = -maxturnRate;
        photonView.RPC("GenNewQs", RpcTarget.All);
        pCon = FindObjectsOfType<PlayerController>();
        
    }

    // Update is called once per frame
    void Update()
    {
        lm = FindObjectOfType<LevelManager>();
        if(lm.gameStart)
        {
            if (isOnFire)
            {
                photonView.RPC("GetHit", RpcTarget.All, 40*Time.deltaTime, false, Vector3.zero);
            }
            if (hp <= 0)
            {
                lm.photonView.RPC("GameOver", RpcTarget.All, team);
            }
            
            transform.Rotate(0, currentTurnRate/1000 * SpeedRate * Time.deltaTime, 0);
            photonView.RPC("ShowConBTN", RpcTarget.All);
            photonView.RPC("RudderTurn", RpcTarget.All);
        }
        
        

    }
    private void FixedUpdate()
    {

        rb.AddForce(transform.forward * SpeedRate);
        if (rb.velocity.x > 0)
        {
            rb.velocity -= new Vector3(rb.velocity.x / 10, 0, 0);
        }
        else if (rb.velocity.x < 0)
        {
            rb.velocity -= new Vector3(rb.velocity.x / 10,0, 0);
        }
        if (rb.velocity.z > 0)
        {
            rb.velocity -= new Vector3(0,0,rb.velocity.z / 10);
        }
        else if(rb.velocity.z < 0)
        {
            rb.velocity -= new Vector3(0, 0, rb.velocity.z / 10);
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        if(other.tag==Constrain.TAG_Fire)
        {
            isOnFire = true;
        }
        if(other.tag==Constrain.TAG_Water)
        {
            if(hp<=0)
            {
                
                
                bounancy -= Time.deltaTime;
                rb.AddForce(Vector3.up * bounancy);
            }
            if(!(bounancy<=0))
            {
                rb.useGravity = false;
            }
           
            rb.AddForce(Vector3.up * bounancy);
            


        }
        else
        {
            rb.useGravity = true;
        }
        if (other.tag == Constrain.TAG_Border)
        {
            transform.position = new Vector3(0, -66.8f, 0);
        }
        if (other.gameObject.tag == "Player")
        {
           
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == Constrain.TAG_Water)
        {
            rb.velocity -= VerticalSpeed();
            rb.useGravity = true;
            
        }
        
         
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == Constrain.TAG_Border)
        {
            transform.position = new Vector3(0, -66.8f, 0);
        }
        if(other.tag =="Bullet")
        {
            RocketScript r = other.GetComponent<RocketScript>();
            Transform rTran = other.transform;
            if (r.headType == "HE")
            {
                //photonView.RPC("GetHit", RpcTarget.All, r.dam, true, rTran);
            }
            else
            {
                
            }
            //PhotonView.Destroy(other.gameObject);
            for (int i =0;i<pCon.Length;i++)
            {
                
                
                pCon[i].photonView.RPC("HitEffect", RpcTarget.All,team);
            }
            
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        
        if (collision.gameObject.tag == Constrain.TAG_bump)
        {
            photonView.RPC("GetHit", RpcTarget.All, 40 * Time.deltaTime, false, Vector3.zero);
        }
    }
    float CurrentSpeed()
    {
        float x = rb.velocity.x, z = rb.velocity.z;
        float result = Mathf.Sqrt((x * x) + (z * z));
        return result;

    }
    Vector3 VerticalSpeed()
    {
        if(rb.velocity.y>0)
        {
            return new Vector3(0, rb.velocity.y, 0);
        }
        else if(rb.velocity.y<0)
        {
            return new Vector3(0, rb.velocity.y, 0);
        }
        else
        {
            return Vector3.zero;
        }
    }
    [PunRPC]
    void GetHit(int damage,bool onFire,Vector3 hitPos)
    {
        hp -= damage;
        if(onFire)
        {
            GameObject flame = Instantiate(fameObj, hitPos,Quaternion.identity);
        }
    }
    [PunRPC]
    public void GenNewQs()
    {
        string qZName = quizItem[Random.Range(0, quizItem.Length)].name;
        PhotonNetwork.Instantiate(qZName, mcPos.position, mcPos.rotation);
        //GameObject obj = Instantiate(quizItem[Random.Range(0, quizItem.Length)],mcPos.position,mcPos.rotation);

    }
    [PunRPC]
    void RudderTurn()
    {
        rudder.transform.rotation = Quaternion.Euler(currentTurnRate, 0, 0);
    }
    public void TakeControl(int choice)
    {
        
        if(isControlling)
        {
            isControlling = false;

            shipUI.SetActive(false);
            foreach (PlayerController p in pCon)
            {
                if (photonView.IsMine)
                {if(p.team==team)
                    {
                        p.photonView.RPC("UseVeh", RpcTarget.All, false);
                        p.controlUI.SetActive(true);
                    }
                    
                    
                }
            }
            
        }
        else
        {
            
            isControlling = true;
            shipUI.SetActive(true);
            foreach (PlayerController p in pCon)
            {
                if(photonView.IsMine)
                {
                    if(p.team==team)
                    {
                        p.transform.position = conPos.transform.position;
                        p.transform.rotation = conPos.transform.rotation;
                        p.photonView.RPC("UseVeh", RpcTarget.All, true);
                        p.controlUI.SetActive(false);
                    }
                    

                }
            }
        }
       
        
        
    }
    [PunRPC]
    void ShowConBTN()
    {
        if (!isControlling)
        {
            takeControlButton.SetActive(true);
        }
        else
        {
            foreach (PlayerController p in pCon)
            {

                if (p.team == team)
                {
                    if (!p.usingVeh)
                    {
                        takeControlButton.SetActive(false);
                    }
                }
            }

        }
    }
    [PunRPC]
    void UpdateMovementForward(float ver)
    {
        SpeedRate = ver;
        
    }
    public void SetSpeed(float speed)
    {
        photonView.RPC("UpdateMovementForward", RpcTarget.All, speed);
    }
    [PunRPC]
    void UpdateMovementTurn(float hor)
    {
       
        currentTurnRate = hor;
    }
    public void SetTurn(float speed)
    {
        photonView.RPC("UpdateMovementTurn", RpcTarget.All, speed);
    }
}
