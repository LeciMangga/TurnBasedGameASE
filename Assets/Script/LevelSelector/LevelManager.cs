using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public GameObject PanelLevel;
    public GameObject potatoPrefab;
    public GameObject[] PanelInfo;

    public GameObject LockLevel;

    private GameObject potato;
    private Vector3 potatoPos;
    private int potatoAtLevel;
    private Transform potatoTr;

    private int levelsUnlocked;
    private int onLevel;

    private GameObject lastPanelInfo;
    private GameObject spawnPanelInfo;

    
    public AudioSource ButtonClickSFX;


    Vector3[] LevelPos;
    Button[] HidButton;

    void Start()
    {
        LevelPos = new Vector3[6];
        HidButton = new Button[6];
        //spawning levels
        GameObject Levels = Instantiate(PanelLevel);
        //find start
        Transform Start = Levels.transform.Find("start");
        HidButton[0] = Start.transform.Find("buttonHidStart").GetComponent<Button>();
        LevelPos[0] = Start.position;
        //find level1
        Transform Level1 = Levels.transform.Find("level1");
        HidButton[1] = Level1.transform.Find("buttonHidLv1").GetComponent<Button>();
        LevelPos[1] = Level1.position;
        //find level2
        Transform Level2 = Levels.transform.Find("level2");
        HidButton[2] = Level2.transform.Find("buttonHidLv2").GetComponent<Button>();
        LevelPos[2] = Level2.position;
        //find level3
        Transform Level3 = Levels.transform.Find("level3");
        HidButton[3] = Level3.transform.Find("buttonHidLv3").GetComponent<Button>();
        LevelPos[3] = Level3.position;
        //find level4
        Transform Level4 = Levels.transform.Find("level4");
        HidButton[4] = Level4.transform.Find("buttonHidLv4").GetComponent<Button>();
        LevelPos[4] = Level4.position;
        //find level5
        Transform Level5 = Levels.transform.Find("level5");
        HidButton[5] = Level5.transform.Find("buttonHidLv5").GetComponent<Button>();
        LevelPos[5] = Level5.position;

        EnableAllButton();

        for (int i = 0; i<=5; i++){
            int index = i; 
            HidButton[i].onClick.AddListener(() => ButtonOnClickLevel(index));
        }

        void ButtonOnClickLevel(int i){
            ButtonClickSFX.Play();
            GetPotatoPos();
            StartCoroutine(movePotato(potatoPos, LevelPos[i], i));
        }

        //spawning the potato
        Vector3 spawnPos;
        spawnPos.x = LevelPos[0].x;
        spawnPos.y = LevelPos[0].y;
        spawnPos.z = LevelPos[0].z - 100;
        potato = Instantiate(potatoPrefab, spawnPos, Quaternion.identity);
        potatoTr = potato.GetComponent<Transform>();
        GetPotatoPos();

        //enabling button
        EnableAllButton();
        
        void GetPotatoPos(){
            potatoPos = potatoTr.position;
            Debug.Log("Pos Potato now = " + potatoPos );
            for (int i = 0; i <=5 ; i++){
                if (potatoPos == LevelPos[i]){
                    potatoAtLevel = i;
                }
            }
        }

        IEnumerator movePotato(Vector3 nowPos, Vector3 shouldPos,int Lvl){
            StartCoroutine(DisableAllButton());
            Destroy(lastPanelInfo);
            if (nowPos != shouldPos){
                if (potatoAtLevel > Lvl){
                    while (potatoAtLevel > Lvl){
                        Debug.Log("go to " + (potatoAtLevel - 1));
                        StartCoroutine(MovePotatoCoroutine(LevelPos[potatoAtLevel], LevelPos[potatoAtLevel - 1]));
                        potatoAtLevel -= 1;
                    }
                } else if (potatoAtLevel < Lvl){
                    while (potatoAtLevel < Lvl){
                        StartCoroutine(MovePotatoCoroutine(LevelPos[potatoAtLevel], LevelPos[potatoAtLevel + 1]));
                        potatoAtLevel += 1;
                    }
                }
            }
            StartCoroutine(spawnPanelInfoCoroutine(Lvl));
            yield return null;
        }

        IEnumerator MovePotatoCoroutine(Vector3 nowPos, Vector3 shouldPos){
            float time = 0;
            float duration = 1f;
            while (time < duration){
                potato.transform.position = Vector3.Lerp(nowPos, shouldPos, time/duration);
                time = time + Time.deltaTime;
                yield return null;
            }
            shouldPos.z -= 100;
            potato.transform.position = shouldPos;
        }

        IEnumerator spawnPanelInfoCoroutine(int lvl){
            Debug.Log("spawning panel = " + lvl);
            Destroy(lastPanelInfo);
            yield return new WaitForSeconds(2);
            lastPanelInfo = Instantiate(PanelInfo[lvl]);
            Transform PanelInfoLvl = lastPanelInfo.transform.Find("Level"+lvl+"Panel");
            if (lvl != 0){
                Button ButtonStartPanelInfo = PanelInfoLvl.transform.Find("ButtonLv"+lvl).GetComponent<Button>();
                onLevel = lvl;
                ButtonStartPanelInfo.onClick.AddListener(OnButtonLevel);
                
            }
            Button BackButtonPanelInfo = PanelInfoLvl.transform.Find("Back").GetComponent<Button>();
            BackButtonPanelInfo.onClick.AddListener(() => OnBackButtonPanelInfo(lvl));
            void OnBackButtonPanelInfo(int i){
                ButtonClickSFX.Play();
                Destroy(lastPanelInfo);
            }
            StartCoroutine(EnableAllButton());
        }
        //default levels
        levelsUnlocked = PlayerPrefs.GetInt("levelsUnlocked",1);
        PlayerPrefs.Save();

        for (int i = 2 ; i<=5; i++){
            HidButton[i].interactable = false;
        }

        IEnumerator DisableAllButton(){
            yield return new WaitForSeconds(0.1f);
            for (int i = 0 ; i<=5; i++){
                HidButton[i].interactable = false;
            }
            
            Debug.Log("disabling buttons");
        }

        
        IEnumerator EnableAllButton(){
            yield return null;
            HidButton[0].interactable = true;
            for (int i = 1; 5 >= i; i++){
                if (levelsUnlocked >= i){
                    HidButton[i].interactable = true;
                } else {
                    //Instantiate(LockLevel, HidButton[i].transform);
                }
            }
            Debug.Log("enabling buttons");
        }
        void OnButtonLevel(){
            ButtonClickSFX.Play();
            PlayerPrefs.SetInt("onLevel",onLevel);
            SceneManager.LoadScene("PartySelector", LoadSceneMode.Single);
        }
        
    }
    
    public void OnBackButtonClick(){
        ButtonClickSFX.Play();
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);  
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.Escape)){
            OnBackButtonClick();
        }
    }
}
