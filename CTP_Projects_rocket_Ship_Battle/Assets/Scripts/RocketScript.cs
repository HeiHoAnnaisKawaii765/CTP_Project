using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RocketScript : MonoBehaviourPun
{
    Rigidbody rb;
    [SerializeField]
    float Speed,fuel,maxSpee,accRate;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!(fuel<=0))
        {
            if(Speed<maxSpee)
            {
                Speed += accRate *Time.deltaTime;
            }
            rb.AddForce(Speed * transform.forward);
            fuel -= Time.deltaTime;
        }
        rb.AddForce(rb.velocity.y * -Vector3.up);
        rb.AddForce(rb.velocity.z * -Vector3.forward);
        rb.AddForce(rb.velocity.x * -Vector3.right);
        


    }

}
