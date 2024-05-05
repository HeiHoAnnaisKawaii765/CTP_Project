using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TutShipScript : MonoBehaviour
{
    float bounancy = 50;
    float SpeedRate;
    public
    int hp = 1600;
    [SerializeField]
    float maxSpeed, maxturnRate;
    Rigidbody rb;
    float currentTurnRate, radius = 4f;
    public bool isControlling;
    public
    Slider speedSlider, steerSlider;
    bool isOnFire;
    public string team;
    LevelManager lm;
    TutorialPlayerScript p;
    [SerializeField]
    GameObject takeControlButton, rudder, shipcontrolSlider, shipUI, fameObj;
    [SerializeField]
    GameObject[] quizItem;
    public Transform conPos;
    [SerializeField]
    Transform mcPos;
    [SerializeField]
    TutSCript tut;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();

        speedSlider.maxValue = maxSpeed;
        speedSlider.minValue = -1;
        steerSlider.maxValue = maxturnRate;
        steerSlider.minValue = -maxturnRate;
        p = FindObjectOfType<TutorialPlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        SpeedRate = speedSlider.value;
        currentTurnRate = steerSlider.value;
        transform.Rotate(0, currentTurnRate / 1000 * SpeedRate * Time.deltaTime, 0);
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
            rb.velocity -= new Vector3(rb.velocity.x / 10, 0, 0);
        }
        if (rb.velocity.z > 0)
        {
            rb.velocity -= new Vector3(0, 0, rb.velocity.z / 10);
        }
        else if (rb.velocity.z < 0)
        {
            rb.velocity -= new Vector3(0, 0, rb.velocity.z / 10);
        }
    }
    public void TakeControl(int choice)
    {
        tut.AddPos(choice);
        if (isControlling)
        {
            isControlling = false;
            p.Veh();
            shipUI.SetActive(false);
            

        }
        else
        {

            isControlling = true;
            shipUI.SetActive(true);
            p.transform.position = conPos.transform.position;
            p.transform.rotation = conPos.transform.rotation;
            p.Veh();
            p.controlUI.SetActive(false);
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
        if (rb.velocity.y > 0)
        {
            return new Vector3(0, rb.velocity.y, 0);
        }
        else if (rb.velocity.y < 0)
        {
            return new Vector3(0, rb.velocity.y, 0);
        }
        else
        {
            return Vector3.zero;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == Constrain.TAG_Fire)
        {
            isOnFire = true;
        }
        if (other.tag == Constrain.TAG_Water)
        {
            if (hp <= 0)
            {


                bounancy -= Time.deltaTime;
                rb.AddForce(Vector3.up * bounancy);
            }
            if (!(bounancy <= 0))
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
        if (other.tag == "GameController")
        {
            tut.AddPos(6);
           
        }
    }

}
