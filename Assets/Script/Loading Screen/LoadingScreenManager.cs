using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class LoadingScreenManager : MonoBehaviour
{
        
    public TextMeshProUGUI LoadingText;
    public TextMeshProUGUI asyncProcessText;

    string LoadStrIdentifier;
    

    void Start()
    {
        LoadStrIdentifier = PlayerPrefs.GetString("LoadStrIdentifier");        
        StartCoroutine(LoadAsyncScene(LoadStrIdentifier));
    }

    IEnumerator LoadAsyncScene(string LoadStrIdentifier){
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(LoadStrIdentifier);
        string dot = ".";
        int i = 1;
        while (!asyncLoad.isDone){
            if (i<=3){
                LoadingText.text = "Loading" + dot;
                dot = dot + dot;
            } else {     
                LoadingText.text = "Loading" + dot;
                dot = ".";
            }
            asyncProcessText.text = "L0ading progress: " + asyncLoad.progress;
            yield return null;
        }
    }
}
