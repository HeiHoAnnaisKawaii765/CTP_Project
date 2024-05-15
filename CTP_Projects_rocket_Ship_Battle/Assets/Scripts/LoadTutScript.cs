using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadTutScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int prompt = PlayerPrefs.GetInt(Constrain.PLAYER_Tut, 0);
        if(prompt==0)
        {
            LoadTut();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LoadTut()
    {
        SceneManager.LoadScene(2);
    }
}
