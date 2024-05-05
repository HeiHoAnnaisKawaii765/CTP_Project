using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TutorialPlayerScript : MonoBehaviour
{
    public
    GameObject fireExtingsher,controlUI, rocketPanel, binoUI;
    [SerializeField]
    Joystick moveJoystick, dirJoystick;
    [SerializeField]
    float moveSpeed, fireRate, mouseSensitivity = 100f;
    [SerializeField]
    GameObject[] ui, rocketModel, rocket;
    [SerializeField]
    Transform camPos, camSet;
    ShipController ship;
    int[] rValue= new int[4];
    [SerializeField]
    TMP_Text[] rocketValues;
    Rigidbody rb;
    float xRotation = 0f;
    public
    bool touchVehicle, usingVeh, useFireExtingsher, placeRocket, binocular;
    [SerializeField]
    TutSCript tut;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        for(int i = 0;i<rValue.Length; i++)
        {
            rValue[i] = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < rValue.Length; i++)
        {
            rocketValues[i].text= rValue[i].ToString();
        }
        if (usingVeh)
        {
            camSet.transform.localRotation = Quaternion.Euler(-xRotation, 0f, 0f);
            fireExtingsher.SetActive(false);
            controlUI.SetActive(false);
        }
        else
        {
            controlUI.SetActive(true);
            transform.position += transform.forward * moveSpeed * moveJoystick.Vertical * Time.deltaTime;
            transform.position += transform.right * moveSpeed * moveJoystick.Horizontal * Time.deltaTime;


            float mouseX = dirJoystick.Horizontal * mouseSensitivity * Time.deltaTime;
            float mouseY = dirJoystick.Vertical * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            camSet.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
        }
    }
    private void OnTriggerStay(Collider other)
    {
       
        if (other.tag == "Vehicle")
        {
            transform.SetParent(other.gameObject.transform);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag== "Respawn")
        {
            tut.AddPos(0);
            
        }
    }

    public void UseExting()
    {
        if (useFireExtingsher)
        {
            useFireExtingsher = false;
            fireExtingsher.SetActive(false);
            if (placeRocket)
            {
                placeRocket = false;
                rocketPanel.SetActive(false);
            }


        }
        else
        {
            useFireExtingsher = true;
            fireExtingsher.SetActive(true);
        }


    }
    public void LaunchRocket(int index)
    {
        if(rValue[index]>0)
        {
            FindObjectOfType<SoundManager>().PlayEffectSoundButton(0);
            GameObject rocketlch = Instantiate(rocket[index], new Vector3(transform.position.x, transform.position.y + 10, transform.position.z), Quaternion.identity);
            tut.AddPos(4);
            rValue[index] -= 1;
        }
        
    }
    public void SwitchModeButton(string buttonFunction)
    {
        switch (buttonFunction)
        {
            case "FX":
                UseExting();
                break;
            case "PR":
                PlaceRocket();
                break;

        }

    }
    void PlaceRocket()
    {
        if (!placeRocket)
        {
            if (fireExtingsher)
            {
                useFireExtingsher = false;
            }

            placeRocket = true;
            rocketPanel.SetActive(true);
        }
        else
        {

            placeRocket = false;
            rocketPanel.SetActive(false);
        }



    }
    public void Binocular()
    {
        if (binocular)
        {
            binocular = false;
            binoUI.SetActive(false);
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 60, 4.5f * Time.deltaTime);
        }
        else
        {
            binocular = true;
            binoUI.SetActive(true);
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 12, 4.5f * Time.deltaTime);
        }
    }
    public void Veh()
    {
        if(usingVeh)
        {
            usingVeh = false;
            
        }
        else {
            usingVeh = true; 
        }
    }
    public void AdRo()
    {
        rValue[Random.Range(0, rocket.Length)] += 1;
        tut.AddPos(2);
    }
}
