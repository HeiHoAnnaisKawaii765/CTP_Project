using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
public class NetworkLoginin : MonoBehaviourPunCallbacks
{
    [SerializeField]
    GameObject loginInObj, nameObj, roomListObj;
    [SerializeField]
    InputField roomName, playerName;
    [SerializeField]
    RoomListManager listManager;
    int roomSL;
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        nameObj.SetActive(true);
        PhotonNetwork.JoinLobby();
    }

    public void PlayButton()
    {
        nameObj.SetActive(false);
        PhotonNetwork.NickName = playerName.text;
        loginInObj.SetActive(true);
        if (PhotonNetwork.InLobby)
        {
            roomListObj.SetActive(true);
        }
    }
    public void JoinOrCreateButton(int selection)
    {
        if (roomName.text.Length < 2)
        {
            return;
        }
        roomSL = selection;
        loginInObj.SetActive(false);
        RoomOptions options = new RoomOptions { MaxPlayers = 10 };

        PhotonNetwork.JoinOrCreateRoom(roomName.text, options, default);
    }

    public override void OnJoinedRoom()
    {
        Cursor.lockState = CursorLockMode.Locked;

        PhotonNetwork.LoadLevel(1);
    }
    public void CopyName(string inputName)
    {
        GameObject field = GameObject.Find(inputName);
        InputField inField = field.GetComponent<InputField>();
        Debug.Log(EventSystem.current.currentSelectedGameObject.GetComponentInChildren<TMP_Text>().text);
        inField.text = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<TMP_Text>().text;
    }
    public void Quit()
    {
        Application.Quit();
    }

}