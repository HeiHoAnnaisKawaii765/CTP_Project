using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RocketScript : MonoBehaviourPun
{
    Rigidbody rb;
    [SerializeField]
    float Speed,fuel,maxSpee,accRate;
    [SerializeField]
    Transform[] enginePos;
    public string headType;
    public int dam;
    [SerializeField]
    Renderer mat;
        // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        switch(headType)
        {
            case "AP":
                mat.material.color = Color.blue;
                break;
            case "HE":
                mat.material.color = Color.red;
                break;
        }
        
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
            for(int i = 0;i<enginePos.Length;i++)
            {
                rb.AddForceAtPosition(transform.up*Speed,enginePos[i].position);
            }

            
            fuel -= Time.deltaTime;
        }
        rb.AddForce(rb.velocity.y * -Vector3.up);
        rb.AddForce(rb.velocity.z * -Vector3.forward);
        rb.AddForce(rb.velocity.x * -Vector3.right);
        


    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag==Constrain.TAG_Water)
        {
            Destroy(gameObject, 2f);
        }
        else if(other.tag=="Target")
        {
            Destroy(gameObject);
        }
    }

}
