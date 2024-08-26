using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public GameObject PanelLevel;
    public GameObject potatoPrefab;
    public GameObject PanelInfoStart;
    public GameObject PanelInfo1;
    public GameObject PanelInfo2;
    public GameObject PanelInfo3;
    public GameObject PanelInfo4;
    public GameObject PanelInfo5;

    private GameObject potato;
    private Vector3 potatoPos;
    private Transform potatoTr;

    private int levelsUnlocked;
    private int onLevel;

    private GameObject lastPanelInfo;
    private GameObject spawnPanelInfo;

    void Start()
    {
        //spawning levels
        GameObject Levels = Instantiate(PanelLevel);
        //find start
        Transform Start = Levels.transform.Find("start");
        Button StartHidButton = Start.transform.Find("buttonHidStart").GetComponent<Button>();
        Vector3 StartPos = Start.position;
        //find level1
        Transform Level1 = Levels.transform.Find("level1");
        Transform Level1Inside = Level1.transform.Find("level1i");
        Button Lv1HidButton = Level1.transform.Find("buttonHidLv1").GetComponent<Button>();
        Vector3 Level1Pos = Level1.position;
        //find level2
        Transform Level2 = Levels.transform.Find("level2");
        Transform Level2Inside = Level2.transform.Find("level2i");
        Button Lv2HidButton = Level2.transform.Find("buttonHidLv2").GetComponent<Button>();
        Vector3 Level2Pos = Level2.position;
        //find level3
        Transform Level3 = Levels.transform.Find("level3");
        Transform Level3Inside = Level3.transform.Find("level3i");
        Button Lv3HidButton = Level3.transform.Find("buttonHidLv3").GetComponent<Button>();
        Vector3 Level3Pos = Level3.position;
        //find level4
        Transform Level4 = Levels.transform.Find("level4");
        Transform Level4Inside = Level4.transform.Find("level4i");
        Button Lv4HidButton = Level4.transform.Find("buttonHidLv4").GetComponent<Button>();
        Vector3 Level4Pos = Level4.position;
        //find level5
        Transform Level5 = Levels.transform.Find("level5");
        Transform Level5Inside = Level5.transform.Find("level5i");
        Button Lv5HidButton = Level5.transform.Find("buttonHidLv5").GetComponent<Button>();
        Vector3 Level5Pos = Level5.position;

        //variabel titik2nya
        Vector3 ujungKiriAtasPos = Level4Pos;
        ujungKiriAtasPos.y = 1005;

        StartHidButton.onClick.AddListener(ButtonClickMovePotatoStart);
        Lv1HidButton.onClick.AddListener(ButtonClickMovePotatoLv1);
        Lv2HidButton.onClick.AddListener(ButtonClickMovePotatoLv2);
        Lv3HidButton.onClick.AddListener(ButtonClickMovePotatoLv3);
        Lv4HidButton.onClick.AddListener(ButtonClickMovePotatoLv4);
        Lv5HidButton.onClick.AddListener(ButtonClickMovePotatoLv5);

        //spawning the potato
        Vector3 spawnPos;
        spawnPos.x = StartPos.x;
        spawnPos.y = StartPos.y;
        spawnPos.z = StartPos.z - 100;
        potato = Instantiate(potatoPrefab, spawnPos, Quaternion.identity);
        potatoTr = potato.GetComponent<Transform>();
        GetPotatoPos();

        //enabling button
        EnableAllButton();
        
        void GetPotatoPos(){
            potatoPos = potatoTr.position;
            Debug.Log("Pos Potato now = " + potatoPos );
        }

        IEnumerator movePotato(Vector3 nowPos, Vector3 shouldPos,int Lvl){
            StartCoroutine(DisableAllButton());
            Destroy(lastPanelInfo);
            if ((nowPos.y == 1005) && (shouldPos.y == 863)){
                Debug.Log("ke ujung kiri atas");
                yield return StartCoroutine(MovePotatoCoroutine(nowPos,ujungKiriAtasPos));
                yield return StartCoroutine(MovePotatoCoroutine(ujungKiriAtasPos, Level4Pos));
                StartCoroutine(MovePotatoCoroutine(Level4Pos,shouldPos));
            } else if (nowPos.y == shouldPos.y){
                StartCoroutine(MovePotatoCoroutine(nowPos,shouldPos));
            } else if (nowPos.y == 863 && shouldPos.y == 1005 ){
                Debug.Log("harus ke lv4 dulu");
                yield return StartCoroutine(MovePotatoCoroutine(nowPos,Level4Pos));
                yield return StartCoroutine(MovePotatoCoroutine(Level4Pos,ujungKiriAtasPos));
                StartCoroutine(MovePotatoCoroutine(ujungKiriAtasPos,shouldPos));
            }
            StartCoroutine(spawnPanelInfoCoroutine(Lvl));
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
            Destroy(lastPanelInfo);
            yield return new WaitForSeconds(2);
            lastPanelInfo = Instantiate(spawnPanelInfo);
            Debug.Log("spawn = " + spawnPanelInfo);
            Debug.Log("last = " + lastPanelInfo);
            if (lvl != 0){
                Transform PanelInfoLvl = lastPanelInfo.transform.Find("Level"+lvl+"Panel");
                Button ButtonStartPanelInfo = PanelInfoLvl.transform.Find("ButtonLv"+lvl).GetComponent<Button>();
                onLevel = lvl;
                ButtonStartPanelInfo.onClick.AddListener(OnButtonLevel);
            }
            StartCoroutine(EnableAllButton());
        }

        void ButtonClickMovePotatoStart(){
            Debug.Log("Button Clicked");
            GetPotatoPos();
            StartCoroutine(movePotato(potatoPos, StartPos,0));
            spawnPanelInfo = PanelInfoStart;
        }
        void ButtonClickMovePotatoLv1(){
            Debug.Log("Button Clicked");
            GetPotatoPos();
            StartCoroutine(movePotato(potatoPos, Level1Pos,1));
            spawnPanelInfo = PanelInfo1;
        }
        void ButtonClickMovePotatoLv2(){
            Debug.Log("Button Clicked");
            GetPotatoPos();
            StartCoroutine(movePotato(potatoPos, Level2Pos,2));
            spawnPanelInfo = PanelInfo2;
        }
        void ButtonClickMovePotatoLv3(){
            Debug.Log("Button Clicked");
            GetPotatoPos();
            StartCoroutine(movePotato(potatoPos, Level3Pos,3));
            spawnPanelInfo = PanelInfo3;
        }
        void ButtonClickMovePotatoLv4(){
            GetPotatoPos();
            Debug.Log("Button Clicked");
            StartCoroutine(movePotato(potatoPos, Level4Pos,4));
            spawnPanelInfo = PanelInfo4;
        }
        void ButtonClickMovePotatoLv5(){
            GetPotatoPos();
            Debug.Log("Button Clicked");
            StartCoroutine(movePotato(potatoPos, Level5Pos,5));
            spawnPanelInfo = PanelInfo5;
        }

        //default levels
        levelsUnlocked = PlayerPrefs.GetInt("levelsUnlocked",1);
        PlayerPrefs.Save();

        Lv2HidButton.interactable = false;
        Lv3HidButton.interactable = false;
        Lv4HidButton.interactable = false;
        Lv5HidButton.interactable = false;

        IEnumerator DisableAllButton(){
            yield return new WaitForSeconds(0.1f);
            StartHidButton.interactable = false;
            Lv1HidButton.interactable = false;
            Lv2HidButton.interactable = false;
            Lv3HidButton.interactable = false;
            Lv4HidButton.interactable = false;
            Lv5HidButton.interactable = false;
            
            Debug.Log("disabling buttons");
        }

        
        IEnumerator EnableAllButton(){
            yield return new WaitForSeconds(0.1f);
            StartHidButton.interactable = true;
            Lv1HidButton.interactable = true;
            for (int i = 1; levelsUnlocked >= i; i++){
                if (i==2){
                    Lv2HidButton.interactable = true;
                } else if (i==3){
                    Lv3HidButton.interactable = true;
                } else if (i==4){
                    Lv4HidButton.interactable = true;
                } else if (i==5){
                    Lv5HidButton.interactable = true;
                }
            }
            Debug.Log("enabling buttons");
        }
        void OnButtonLevel(){
            PlayerPrefs.SetInt("onLevel",onLevel);
            SceneManager.LoadScene("Battle", LoadSceneMode.Single);
        }
    }
    
    public void OnBackButtonClick(){
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);  
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.Escape)){
            OnBackButtonClick();
        }
    }
}
