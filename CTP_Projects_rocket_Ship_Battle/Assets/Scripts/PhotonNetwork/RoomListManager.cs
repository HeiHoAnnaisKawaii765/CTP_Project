using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class RoomListManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    GameObject roomNamePrefab;
    [SerializeField]
    Transform gridLayout;

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        for(int i =0;i < gridLayout.childCount; i++)
        {
            if(gridLayout.GetChild(i).gameObject.GetComponentInChildren<TMP_Text>().text == roomList[i].Name)
            {
                Destroy(gridLayout.GetChild(i).gameObject);

                if(roomList[i].PlayerCount ==0)
                {
                    roomList.Remove(roomList[i]);
                    Destroy(gridLayout.GetChild(i).gameObject);
                }
            }
        }
        foreach(var room in roomList)
        {
            GameObject newRoom = Instantiate(roomNamePrefab, gridLayout.position, Quaternion.identity);

            newRoom.GetComponentInChildren<TMP_Text>().text = room.Name;

            newRoom.transform.SetParent(gridLayout);

            
        }
    }
   
}
