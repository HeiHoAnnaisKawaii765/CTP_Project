using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class ShipController : MonoBehaviourPun
{
    float SpeedRate;
    int hp = 1600;
    [SerializeField]
    float maxSpeed,maxturnRate;
    Rigidbody rb;
    float currentTurnRate,radius = 4f;
    public bool isControlling;
    [SerializeField]
    Slider speedSlider, steerSlider;
    bool isOnFire;
    public string team;
    LevelManager lm;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        speedSlider.maxValue = maxSpeed;
        speedSlider.minValue = -1;
        steerSlider.maxValue = maxturnRate;
        steerSlider.minValue = -maxturnRate;
    }

    // Update is called once per frame
    void Update()
    {
        lm = FindObjectOfType<LevelManager>();
        if(isOnFire)
        {
            hp -= (int)(60 * Time.deltaTime);
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
        if(other.tag==Constrain.TAG_Water)
        {
            rb.AddForce(Vector3.up * 10f);
            rb.useGravity = false;
            
            
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
    
        
}
