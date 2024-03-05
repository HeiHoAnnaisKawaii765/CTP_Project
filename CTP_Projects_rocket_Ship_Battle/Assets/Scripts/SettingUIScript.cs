using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingUIScript : MonoBehaviour
{
    [SerializeField]
    GameObject settingUI;
    bool isOn;
    // Start is called before the first frame update
    public void OpenCloseSettingPanel()
    {
        if(isOn)
        {
            settingUI.SetActive(false);
            isOn = false;
        }
        else
        {
            settingUI.SetActive(true);
            isOn = true;
        }
    }
}
