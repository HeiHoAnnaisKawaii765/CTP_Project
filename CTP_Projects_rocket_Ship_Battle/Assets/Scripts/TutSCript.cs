using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class TutSCript : MonoBehaviour
{
    [SerializeField]
    string[] missons;
    [SerializeField]
    TMP_Text missonText;
    int proceture;
    [SerializeField]
    RectTransform touchUI;
    [SerializeField]
    GameObject[] display,tutItem;
    [SerializeField]
    Vector3[] touchPos;
    // Start is called before the first frame update
    void Start()
    {
        proceture = 0;
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0;i<missons.Length;i++)
        {
            if(i!=proceture)
            {
                display[i].SetActive(false);
            }
            else
            {
                display[i].SetActive(true);
            }
        }
        switch(proceture)
        {
            case 0:
                tutItem[0].SetActive(true);
                tutItem[1].SetActive(false);
                break;
            case 6:
                tutItem[0].SetActive(false);
                tutItem[1].SetActive(true);
                break;
            default:
                tutItem[0].SetActive(false); tutItem[1].SetActive(false);
                break;

        }
        touchUI.localPosition = touchPos[proceture];
        missonText.text = missons[proceture];
        if(proceture==7)
        {
            PlayerPrefs.SetInt(Constrain.PLAYER_Tut, 1);
            StartCoroutine(LoadBackToMenu());
        }
    }
    IEnumerator LoadBackToMenu()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(0);
    }
    public void AddPos(int pro)
    {
        
        if(pro==proceture)
        {
           proceture += 1;
        }
        
    }
}
