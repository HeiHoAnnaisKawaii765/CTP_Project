using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;
public class PlayerController : MonoBehaviourPun
{
    [SerializeField]
    Joystick moveJoystick, dirJoystick;
    [SerializeField]
    float moveSpeed, fireRate, mouseSensitivity = 100f, raycastDistance = 0.5f;
    [SerializeField]
    GameObject[] ui;
    [SerializeField]
    Transform camPos, camSet;
    [SerializeField]
    GameObject fireExtingsher;
    LevelManager levelManager;
    Rigidbody rb;
    float xRotation = 0f;
    bool touchVehicle, usingVeh, useFireExtingsher,placeRocket;
    public
    string team;
    ShipController ship;
    [SerializeField]
    Animator anim;
    Transform rayHutPos;

    private void Awake()
    {
        if (photonView.IsMine)
        {



        }
        else
        {
            for (int i = 0; i < ui.Length; i++)
            {
                Destroy(ui[i]);
            }

            // playerName.text = photonView.Owner.NickName;//show the name belongs to that player
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine)
        {
            //playerName.text = PhotonNetwork.NickName;

            Transform cameraTransform = Camera.main.gameObject.transform;  //Find main camera which is part of the scene instead of the prefab
            cameraTransform.parent = camPos.transform;  //Make the camera a child of the mount point
            cameraTransform.position = camPos.transform.position;  //Set position/rotation same as the mount point
            cameraTransform.rotation = camPos.transform.rotation;
            rb = this.GetComponent<Rigidbody>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine && PhotonNetwork.IsConnected)
        {
            return;
        }
        levelManager = FindObjectOfType<LevelManager>();
        ShootRay();

        transform.position += transform.forward * moveSpeed * moveJoystick.Vertical * Time.deltaTime;
        transform.position += transform.right * moveSpeed * moveJoystick.Horizontal * Time.deltaTime;


        float mouseX = dirJoystick.Horizontal * mouseSensitivity * Time.deltaTime;
        float mouseY = dirJoystick.Vertical * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        camSet.transform.localRotation = Quaternion.Euler(-xRotation, 0f, 0f);
        transform.Rotate(-Vector3.up * mouseX);
    }
    void ShootRay()
    {

        RaycastHit hit;
        if (Physics.Raycast(camPos.transform.position, transform.forward, out hit, 100))
        {

            if (hit.transform.tag == "Target")
            {
                if(placeRocket
                   )
                {

                }
                else
                {

                }
                rayHutPos.position = hit.transform.position;

            }



        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "AirShip")
        {
            ship = other.GetComponent<ShipController>();

            if (!usingVeh)
            {
                transform.SetParent(other.gameObject.transform);
            }

        }
        if (other.tag == "Vehicle")
        {

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "AirShip")
        {
            if (!usingVeh)
            {
                transform.SetParent(null);
            }


        }
        if (other.tag == "Vehicle")
        {
            touchVehicle = false;


        }
    }
    [PunRPC]
    public void TeamSelect(string teamName)
    {
        team = teamName;
    }
    [PunRPC]
    public void HitEffect(string shipTeamName)
    {
        if (team == shipTeamName)
        {
            Handheld.Vibrate();
        }

    }
    [PunRPC]
    void UseExting()
    {
        if(useFireExtingsher)
        {
            useFireExtingsher = false;
            placeRocket = true;

        }
        else
        {
            useFireExtingsher = true;
        }
        

    }
    [PunRPC]
    void PlaceRocket()
    {
        if (useFireExtingsher)
        {
            useFireExtingsher = false;
        }
        else
        {
            useFireExtingsher = true;
            placeRocket = false;
        }


    }
    public void SwitchModeButton(string buttonFunction)
    {
        switch(buttonFunction)
        {
            case "FX":
                UseExting();
                break;
        }

    }
}
