using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
public class RoomButtonScript : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {RectTransform rect =
        GetComponent<RectTransform>();
        rect.localPosition = Vector3.zero;
    }
    public void CopyName(string inputName)
    {
        GameObject field = GameObject.Find(inputName);
        

        TMP_InputField inField = field.GetComponent<TMP_InputField>();
        Debug.Log(EventSystem.current.currentSelectedGameObject.GetComponentInChildren<TMP_Text>().text);
        inField.text = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<TMP_Text>().text;
    }
}
