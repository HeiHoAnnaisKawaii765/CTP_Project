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
    GameObject ship;
    AudioSource sound;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        sound = this.GetComponent<AudioSource>();
        sound.volume = FindObjectOfType<SoundManager>().sfxVolume;
        switch (headType)
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
        ship = other.gameObject;
        Transform myTransform = this.transform;
        if(other.tag==Constrain.TAG_Water)
        {
            PhotonView.Destroy(gameObject);
        }
        if(other.tag=="Vehicle")
        {
            switch(headType)
            {
                case "AP":
                    ship.GetComponent<ShipController>().photonView.RPC("GetHit", RpcTarget.All, dam, false, myTransform.position);
                    break;
                case "HE":
                    ship.GetComponent<ShipController>().photonView.RPC("GetHit", RpcTarget.All, dam, true, myTransform.position);
                    break;
            }
            PhotonView.Destroy(gameObject,0.1f);
        }
        
    }
    [PunRPC]
    void Destroy(float time)
    {
        Destroy(gameObject, time);
    }
}
