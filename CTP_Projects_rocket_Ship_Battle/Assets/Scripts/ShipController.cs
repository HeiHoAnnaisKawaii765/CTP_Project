using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class ShipController : MonoBehaviourPun
{
    float bounancy = 10;
    float SpeedRate;
    public
    int hp = 1600;
    [SerializeField]
    float maxSpeed,maxturnRate;
    Rigidbody rb;
    float currentTurnRate,radius = 4f;
    public bool isControlling;
    [SerializeField]
    Slider speedSlider, steerSlider,healthSlider;
    bool isOnFire;
    public string team;
    LevelManager lm;
    PlayerController[] pCon;
    [SerializeField]
    GameObject takeControlButton,rudder,shipcontrolSlider,shipUI,fameObj;
    [SerializeField]
    GameObject[] quizItem;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        speedSlider.maxValue = maxSpeed;
        speedSlider.minValue = -1;
        steerSlider.maxValue = maxturnRate;
        steerSlider.minValue = -maxturnRate;
        healthSlider.maxValue = hp;
    }

    // Update is called once per frame
    void Update()
    {
        lm = FindObjectOfType<LevelManager>();
        healthSlider.value = hp;
        if (isOnFire)
        {
            hp -= (int)(60 * Time.deltaTime);
        }
        if(hp<0)
        {
            lm.photonView.RPC("GameOver", RpcTarget.All, team);
        }
        SpeedRate = speedSlider.value;
        currentTurnRate = steerSlider.value;
        transform.Rotate(0, currentTurnRate * SpeedRate*Time.deltaTime, 0);

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
        if (other.tag == Constrain.TAG_Border)
        {
            transform.position = new Vector3(0, -66.8f, 0);
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
            for(int i =0;i<pCon.Length;i++)
            {
                if(other.GetComponent<RocketScript>().headType=="HE")
                {
                    photonView.RPC("GetHit", RpcTarget.All, other.GetComponent<RocketScript>().dam, true, other.gameObject.transform);
                }
                else
                {
                    photonView.RPC("GetHit", RpcTarget.All, other.GetComponent<RocketScript>().dam, false, other.gameObject.transform);
                }
                
                pCon[i].photonView.RPC("HitEffect", RpcTarget.All,team);
            }
            
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.tag =="Player")
        {
            if(!isControlling)
            {
                takeControlButton.SetActive(true);
            }
            else
            {
                takeControlButton.SetActive(false);
            }
        }
        if (collision.gameObject.tag == "Vehicle")
        {
            photonView.RPC("GetHit", RpcTarget.All, 160, false, gameObject.transform);
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
    void GetHit(int damage,bool onFire,Transform hitPos)
    {
        hp -= damage;
        if(onFire)
        {
            GameObject flame = Instantiate(fameObj, hitPos);
        }
    }
    [PunRPC]
    public void GenNewwQs(int damage)
    {
        GameObject obj = Instantiate(quizItem[Random.Range(0, quizItem.Length)]);

    }
    [PunRPC]
    void RudderTurn()
    {
        rudder.transform.Rotate(currentTurnRate, 0, 0);
    }

}
