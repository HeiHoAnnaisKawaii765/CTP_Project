using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MultipleQuestionScript : MonoBehaviour
{
    [SerializeField]
    string correctAnswer;
    [SerializeField]
    Image[] buttonBG;
    public void CheckResult(string answer)
    {
        switch(correctAnswer)
        {
            case "A":
                buttonBG[0].color = Color.blue;
                buttonBG[1].color = Color.red;
                buttonBG[2].color = Color.red;
                buttonBG[3].color = Color.red;
                break;
            case "B":
                buttonBG[1].color = Color.blue;
                buttonBG[0].color = Color.red;
                buttonBG[2].color = Color.red;
                buttonBG[3].color = Color.red;
                break;
            case "C":
                buttonBG[2].color = Color.blue;
                buttonBG[1].color = Color.red;
                buttonBG[0].color = Color.red;
                buttonBG[3].color = Color.red;
                break;
            case "D":
                buttonBG[3].color = Color.blue;
                buttonBG[1].color = Color.red;
                buttonBG[2].color = Color.red;
                buttonBG[0].color = Color.red;
                break;
        }
        if(answer==correctAnswer)
        {
            Debug.Log("Right");
        }
        else
        {
            Debug.Log("Wrong");
        }
    }
}
