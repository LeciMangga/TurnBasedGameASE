using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuSettings : MonoBehaviour
{
    void Start(){
        if (!(PlayerPrefs.HasKey("GetKentang"))){
            PlayerPrefs.SetInt("GetKentang",1);
        }
    }
    public void OnPlayButton(){
        SceneManager.LoadScene("LevelSelector", LoadSceneMode.Single);
    }

    public void OnPartyButton(){
        SceneManager.LoadScene("PartySelector", LoadSceneMode.Single);
    }

    public void OnPlayerListButton(){
        SceneManager.LoadScene("PlayerList", LoadSceneMode.Single);
    }

    public void OnExitButton(){
        Application.Quit();
    }
}
