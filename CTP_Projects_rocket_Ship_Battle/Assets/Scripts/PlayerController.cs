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
    public
    GameObject fireExtingsher,teamSelectButtons,changeTeamButton, rocketPanel,binoUI,controlUI,localCanvas;
    LevelManager levelManager;
    Rigidbody rb;
    float xRotation = 0f;
    public 
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
    [SerializeField]
    TMP_Text[] rocketValues;
    
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
            localCanvas.transform.SetParent(null);
            photonView.RPC("ChangeColor", RpcTarget.All);
            controlUI.SetActive(false);
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
        photonView.RPC("UpdateRocketNum", RpcTarget.All);
        photonView.RPC("AnimationUpdate", RpcTarget.All);
        photonView.RPC("UpdateTeam", RpcTarget.All);
        ShootRay();
        
        if(usingVeh)
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
            transform.SetParent(other.gameObject.transform);
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
                levelManager.photonView.RPC("TeamMemberNumChange", RpcTarget.All, 1);
                transform.position = levelManager.shipPos[0].position;
                break;
            case 1:
                team = "B";
                levelManager.photonView.RPC("TeamMemberNumChange", RpcTarget.All, 1);
                transform.position = levelManager.shipPos[1].position;
                break;

        }
        controlUI.SetActive(true);
        teamSelectButtons.SetActive(false);
    }

    public void SelectTeam(int mt)
    {
        photonView.RPC("TeamSelect", RpcTarget.All, mt);
    }

    public void PickTeam(int select)
    {
        photonView.RPC("TeamSelect", RpcTarget.All, select);
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
    [PunRPC]
    void PlaceRocket()
    {
        if(levelManager.gameStart)
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
       


    }
    [PunRPC]
    void UpdateRocketNum()
    {
        for (int i = 0; i < rocketValues.Length; i++)
        {
            if (team == "A")
            {
                rocketValues[i].text = "x" +levelManager.teamARocketNum[i].ToString("0");
            }
            if (team == "B")
            {
                rocketValues[i].text ="x" + levelManager.teamBRocketNum[i].ToString("0");
            }
        }


    }
    public void SwitchModeButton(string buttonFunction)
    {
        switch(buttonFunction)
        {
            case "FX":
                photonView.RPC("UseExting",RpcTarget.All);
                break;
            case "PR":
                photonView.RPC("PlaceRocket", RpcTarget.All);
                break;

        }

    }
    public void RocketSelect(int value)
    {
        switch(team)
        {
            case "A":
                if(levelManager.teamARocketNum[value]>0)
                {
                    photonView.RPC("LaunchRocket", RpcTarget.All, value);
                }

                break;
            case "B":
                if (levelManager.teamBRocketNum[value] > 0)
                {
                    photonView.RPC("LaunchRocket", RpcTarget.All, value);
                }

                break;
        }
        

    }
    [PunRPC]
    void LaunchRocket(int index)
    {
        GameObject rocketlch = Instantiate(rocket[index], new Vector3(transform.position.x, transform.position.y + 10, transform.position.z),Quaternion.identity);
        levelManager.photonView.RPC("AddDedectRocket", RpcTarget.All, -1, index, team);
    }
    
    public void FireRocket()
    {
        FindObjectOfType<SoundManager>().PlayEffectSoundButton(0);
        photonView.RPC("LaunchRocket", RpcTarget.All);
    }
    [PunRPC]
    void ChangeColor()
    {
        myMat.color = Random.ColorHSV();
    }
    [PunRPC]
    public void UseVeh(bool result)
    {
        usingVeh = result;
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
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 12, 4.5f * Time.deltaTime);
        }
    }
    [PunRPC]
    void UpdateTeam()
    {
        if(team!=null)
        {
            switch(team)
            {
                case "A":
                    team = "A";
                    break;
                case "B":
                    team = "B";
                    break;
            }
        }
    }


}
