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
    GameObject[] ui,rocketModel,rocket;
    [SerializeField]
    Transform camPos, camSet;
    [SerializeField]
    GameObject fireExtingsher,teamSelectButtons,changeTeamButton, rocketPanel,binoUI,controlUI;
    LevelManager levelManager;
    Rigidbody rb;
    float xRotation = 0f;
    bool touchVehicle, usingVeh, useFireExtingsher,placeRocket,binocular;
    public
    string team;
    ShipController ship;
    [SerializeField]
    Animator charAnim;
    Transform rayHutPos;
    GameObject
         localRocket;
    int RocketSelection;
    [SerializeField]
    Material myMat;
    
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
            photonView.RPC("ChangeColor", RpcTarget.All);
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
        
        if(usingVeh)
        {
            camSet.transform.localRotation = Quaternion.Euler(-xRotation, 0f, 0f);
            controlUI.SetActive(false);
        }
        else
        {
            controlUI.SetActive(true);
            transform.position += transform.forward * moveSpeed * moveJoystick.Vertical * Time.deltaTime;
            transform.position += transform.right * moveSpeed * moveJoystick.Horizontal * Time.deltaTime;

            if (fireExtingsher)
            {
                fireExtingsher.SetActive(true);
            }
            else
            {
                fireExtingsher.SetActive(false);
            }
            float mouseX = dirJoystick.Horizontal * mouseSensitivity * Time.deltaTime;
            float mouseY = dirJoystick.Vertical * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            camSet.transform.localRotation = Quaternion.Euler(-xRotation, 0f, 0f);
            transform.Rotate(-Vector3.up * mouseX);
        }
        
    }
    void ShootRay()
    {

        RaycastHit hit;
        if (Physics.Raycast(camPos.transform.position, transform.forward, out hit, 100))
        {

            if (hit.transform.tag == "Target")
            {
                if(placeRocket)
                {
                    if(localRocket!=null)
                    {
                        localRocket.transform.position = rayHutPos.transform.position;
                        localRocket.transform.eulerAngles = new Vector3(transform.eulerAngles.x - 45, 0, 0);
                    }
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
    public void TeamSelect(int tm)
    {
        switch(tm)
        {
            case 0:
                team = "A";
                levelManager.photonView.RPC("TeamMemberNumChange", RpcTarget.All, 0);
                transform.position = levelManager.shipPos[0].position;
                break;
            case 1:
                team = "B";
                levelManager.photonView.RPC("TeamMemberNumChange", RpcTarget.All, 1);
                transform.position = levelManager.shipPos[0].position;
                break;

        }
        teamSelectButtons.SetActive(false);
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
            rocketPanel.SetActive(false);

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
            placeRocket = true;
            rocketPanel.SetActive(true);
        }
        else
        {
            
            placeRocket = false;
        }


    }
    public void SwitchModeButton(string buttonFunction)
    {
        switch(buttonFunction)
        {
            case "FX":
                photonView.RPC("UseExit",RpcTarget.All);
                break;
            case "PR":
                photonView.RPC("PlaceRocket", RpcTarget.All);
                break;

        }

    }
    public void RocketSelect(int value)
    {
        if(localRocket!=null)
        {
            Destroy(localRocket);
            localRocket = null;
            localRocket = rocketModel[value];
        }
        else
        {
            localRocket = rocketModel[value];
        }
        
    }
    [PunRPC]
    void LaunchRocket()
    {
        GameObject rocketlch = Instantiate(rocket[RocketSelection], new Vector3(rayHutPos.position.x, rayHutPos.position.y + 5, rayHutPos.position.z), rayHutPos.rotation);
    }
    
    public void FireRocket()
    {
        photonView.RPC("LaunchRocket", RpcTarget.All);
    }
    [PunRPC]
    void ChangeColor()
    {
        myMat.color = Random.ColorHSV();
    }
    [PunRPC]
    void AnimationUpdate()
    {
        if(moveJoystick.Horizontal !=0)
        {
            charAnim.SetBool("WLR", true);
        }
        else
        {
            charAnim.SetBool("WLR", false);
        }
        if (moveJoystick.Vertical != 0)
        {
            charAnim.SetBool("W", true);
        }
        else
        {
            charAnim.SetBool("W", false);
        }
        if (useFireExtingsher)
        {
            charAnim.SetBool("F", true);
        }
        else
        {
            charAnim.SetBool("F", false);
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
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 25, 4.5f * Time.deltaTime);
        }
    }
    


}
