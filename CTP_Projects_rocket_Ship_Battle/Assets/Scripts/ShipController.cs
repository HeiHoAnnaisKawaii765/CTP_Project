using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShipController : MonoBehaviour
{
    float SpeedRate;
    [SerializeField]
    float maxSpeed,maxturnRate;
    Rigidbody rb;
    float currentTurnRate,radius = 4f;
    public bool isControlling;
    [SerializeField]
    Slider speedSlider, steerSlider;

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
            rb.velocity += new Vector3(0, rb.velocity.x / 10, 0);
        }
        if (rb.velocity.z > 0)
        {
            rb.velocity -= new Vector3(0,0,rb.velocity.z / 10);
        }
        else if(rb.velocity.z < 0)
        {
            rb.velocity += new Vector3(0, 0, rb.velocity.z / 10);
        }
    }
    public void SpeedControl(float rate)
    {
        SpeedRate = rate;
    }
    public void TurnControl(float rate)
    {
        currentTurnRate = rate;
    }
    
        
}
