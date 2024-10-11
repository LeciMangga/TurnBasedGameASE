using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuSettings : MonoBehaviour
{
    public AudioSource MainMenuAudio;
    public AudioSource ButtonClickSFX;

    public GameObject MiddlePos;
    public GameObject BotMidPos;

    bool ToggleSpeakerButton = true;

    public GameObject PanelCredit;
    

    void Start(){
        if (!(PlayerPrefs.HasKey("GetKentang"))){
            PlayerPrefs.SetInt("GetKentang",1);
        }
    }
    public void OnPlayButton(){
        ButtonClickSFX.Play();
        PlayerPrefs.SetString("LoadStrIdentifier","LevelSelector");
        SceneManager.LoadScene("Loading Screen", LoadSceneMode.Single);
    }
    public void OnCreditButton(){
        ButtonClickSFX.Play();
        GameObject PanelCreditObj = Instantiate(PanelCredit, BotMidPos.transform);
        Button PanelCreditCloseButton = PanelCreditObj.transform.Find("Close").GetComponent<Button>();
        PanelCreditCloseButton.onClick.AddListener(CloseCredit);
        StartCoroutine(movePanelBotToMid(PanelCreditObj));
        void CloseCredit(){
            Destroy(PanelCreditObj);
        }
    }
    public void OnExitButton(){
        ButtonClickSFX.Play();
        Application.Quit();
    }
    public void OnSpeakerButton(){
        ButtonClickSFX.Play();
        if (ToggleSpeakerButton){    
            MainMenuAudio.Pause();
            ToggleSpeakerButton = false;
        } else {
            MainMenuAudio.UnPause();
            ToggleSpeakerButton = true;
        }
    }
    

    IEnumerator movePanelBotToMid(GameObject Panel){
        float time = 0;
        float duration = 0.2f;
        while (time < duration){
            Panel.transform.position = Vector3.Lerp(BotMidPos.transform.position, MiddlePos.transform.position, time/duration);
            time = time + Time.deltaTime;
            yield return null;
        }    
        
    }
}
