    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;
    using UnityEngine.SceneManagement;

    public class PartyManager : MonoBehaviour
    {
        public GameObject pos1;
        public GameObject pos2;
        public GameObject pos3;

        public TextMeshProUGUI nama1;
        public TextMeshProUGUI nama2;
        public TextMeshProUGUI nama3;

        public PartyList partylist;
        public PlayerList playerlist;

        public GameObject canvasPanelSwitch;
        public GameObject canvasEmptySwitch1;
        public GameObject canvasEmptySwitch2;
        public GameObject canvasEmptySwitch3;

        public GameObject EmptyWarning;
        public GameObject MidPos;

        public bool isCanvasSwitchOn;

        int WhatPartyPosSwitched;

        GameObject CanvasPanelSwitchParty;

        //party obj variables
        GameObject PartyList1Empty;
        GameObject PartyList1prefab;
        GameObject PartyList2Empty;
        GameObject PartyList2prefab;
        GameObject PartyList3Empty;
        GameObject PartyList3prefab;

        Unit PartyList1Unit;
        Unit PartyList2Unit;
        Unit PartyList3Unit;

        string Pos1Now;
        string Pos2Now;
        string Pos3Now;


        void Start(){
            SpawningParty();
        }

        void SpawningParty(){
            //dict
            partylist.playerListDict();

            //destroying existing
            if (PartyList1Empty != null) Destroy(PartyList1Empty);
            if (PartyList1prefab != null) Destroy(PartyList1prefab);
            if (PartyList2Empty != null) Destroy(PartyList2Empty);
            if (PartyList2prefab != null) Destroy(PartyList2prefab);
            if (PartyList3Empty != null) Destroy(PartyList3Empty);
            if (PartyList3prefab != null) Destroy(PartyList3prefab);


            //spawning 1
            if (partylist.List1() == null) {
                PartyList1Empty = Instantiate(canvasEmptySwitch1);
                Transform PanelEmpty1transform = PartyList1Empty.transform.Find("PanelEmpty1");
                GameObject PanelEmpty1Obj = PanelEmpty1transform.gameObject;
                BoxCollider2D boxColliderEmpty1 = PanelEmpty1Obj.AddComponent<BoxCollider2D>();
                boxColliderEmpty1.size = new Vector2(200f,200f);
                OnClickObject ScriptEmpty1 = PanelEmpty1Obj.AddComponent<OnClickObject>();
                ScriptEmpty1.SwitchedParty = 1;

                nama1.text = "-";
                Pos1Now = null;
            } else {
                PartyList1prefab = Instantiate(partylist.List1(), pos1.transform);
                if (PartyList1prefab != null){ 
                    PartyList1Unit = PartyList1prefab.GetComponent<Unit>();
                    Debug.Log(PartyList1Unit.Nama);
                    nama1.text = PartyList1Unit.Nama;
                    Pos1Now = PartyList1Unit.Nama;
                }
                BoxCollider2D boxColliderList1 = PartyList1prefab.AddComponent<BoxCollider2D>();
                boxColliderList1.size = new Vector2(1f,1f);
                OnClickObject ScriptObj1 = PartyList1prefab.AddComponent<OnClickObject>();
                ScriptObj1.SwitchedParty = PartyList1Unit.partyPosition;
            }


            //spawning 2
            if (partylist.List2() == null) {
                PartyList2Empty = Instantiate(canvasEmptySwitch2);
                Transform PanelEmpty2transform = PartyList2Empty.transform.Find("PanelEmpty2");
                GameObject PanelEmpty2Obj = PanelEmpty2transform.gameObject;
                BoxCollider2D boxColliderEmpty2 = PanelEmpty2Obj.AddComponent<BoxCollider2D>();
                boxColliderEmpty2.size = new Vector2(200f,200f);
                OnClickObject ScriptEmpty2 = PanelEmpty2Obj.AddComponent<OnClickObject>();
                ScriptEmpty2.SwitchedParty = 2;

                nama2.text = "-";
                Pos2Now = null;
            } else {
                PartyList2prefab = Instantiate(partylist.List2(), pos2.transform);
                if (PartyList2prefab != null){ 
                    PartyList2Unit = PartyList2prefab.GetComponent<Unit>();
                    Debug.Log(PartyList2Unit.Nama);
                    nama2.text = PartyList2Unit.Nama;
                    Pos2Now = PartyList2Unit.Nama;
                }
                BoxCollider2D boxColliderList2 = PartyList2prefab.AddComponent<BoxCollider2D>();
                boxColliderList2.size = new Vector2(1f,1f);
                OnClickObject ScriptObj2 = PartyList2prefab.AddComponent<OnClickObject>();
                ScriptObj2.SwitchedParty = PartyList2Unit.partyPosition;
            }


            //spawning 3
            if (partylist.List3() == null){
                PartyList3Empty = Instantiate(canvasEmptySwitch3);
                Transform PanelEmpty3transform = PartyList3Empty.transform.Find("PanelEmpty3");
                GameObject PanelEmpty3Obj = PanelEmpty3transform.gameObject;
                BoxCollider2D boxColliderEmpty3 = PanelEmpty3Obj.AddComponent<BoxCollider2D>();
                boxColliderEmpty3.size = new Vector2(200f,200f);
                OnClickObject ScriptEmpty3 = PanelEmpty3Obj.AddComponent<OnClickObject>();
                ScriptEmpty3.SwitchedParty = 3;

                nama3.text = "-";
                Pos3Now = null;
            } else {
                PartyList3prefab = Instantiate(partylist.List3(), pos3.transform);
                if (PartyList3prefab != null){ 
                    PartyList3Unit = PartyList3prefab.GetComponent<Unit>();
                    Debug.Log(PartyList3Unit.Nama);
                    nama3.text = PartyList3Unit.Nama;
                    Pos3Now = PartyList3Unit.Nama;
                }
                BoxCollider2D boxColliderList3 = PartyList3prefab.AddComponent<BoxCollider2D>();
                boxColliderList3.size = new Vector2(1f,1f);
                OnClickObject ScriptObj3 = PartyList3prefab.AddComponent<OnClickObject>();
                ScriptObj3.SwitchedParty = PartyList3Unit.partyPosition;
            }
        }

        public void SpawnSwitchPanelParty(int passIntPosParty){
            isCanvasSwitchOn = true;
            WhatPartyPosSwitched = passIntPosParty;

            CanvasPanelSwitchParty = Instantiate(canvasPanelSwitch);
            Transform SwitchPanel = CanvasPanelSwitchParty.transform.Find("SwitchPanel");

            //search kentang
            Transform PosKentang = SwitchPanel.transform.Find("posKentang");
            TextMeshProUGUI KentangText = PosKentang.transform.Find("KentangText").GetComponent<TextMeshProUGUI>();
            if (PlayerPrefs.HasKey("GetKentang")){  
                GameObject KentangGo = Instantiate(playerlist.Kentang, PosKentang);
                KentangGo.transform.localScale = new Vector3(50f,50f,0f);
                KentangText.text = playerlist.KentangUnit.Nama;
                BoxCollider2D KentangCollider = KentangGo.AddComponent<BoxCollider2D>();
                KentangCollider.size = new Vector2(2f,2f);
                OnClickSwitch KentangScript = KentangGo.AddComponent<OnClickSwitch>();
                KentangScript.identifierPlayerClick = "Kentang";
            } else {
                Destroy(PosKentang);
                Destroy(KentangText);
            }
            

            //search tomat
            Transform PosTomat = SwitchPanel.transform.Find("posTomat");
            TextMeshProUGUI TomatText = PosTomat.transform.Find("TomatText").GetComponent<TextMeshProUGUI>();
            if (PlayerPrefs.HasKey("GetTomat")){
                GameObject TomatGo = Instantiate(playerlist.Tomat, PosTomat);
                TomatGo.transform.localScale = new Vector3(50f,50f,0f);
                TomatText.text = playerlist.TomatUnit.Nama;
                BoxCollider2D TomatCollider = TomatGo.AddComponent<BoxCollider2D>();
                TomatCollider.size = new Vector2(2f,2f);
                OnClickSwitch TomatScript = TomatGo.AddComponent<OnClickSwitch>();
                TomatScript.identifierPlayerClick = "Tomat";
            } else {
                TomatText.text = "";
            }
            

            //search garlic
            Transform PosGarlic = SwitchPanel.transform.Find("posGarlic");
            TextMeshProUGUI GarlicText = PosGarlic.transform.Find("GarlicText").GetComponent<TextMeshProUGUI>();
            if (PlayerPrefs.HasKey("GetGarlic")){
                GameObject GarlicGo = Instantiate(playerlist.Garlic, PosGarlic);
                GarlicGo.transform.localScale = new Vector3(50f,50f,0f);
                GarlicText.text = playerlist.GarlicUnit.Nama;
                BoxCollider2D GarlicCollider = GarlicGo.AddComponent<BoxCollider2D>();
                GarlicCollider.size = new Vector2(2f,2f);
                OnClickSwitch GarlicScript = GarlicGo.AddComponent<OnClickSwitch>();
                GarlicScript.identifierPlayerClick = "Garlic";
            } else {
                GarlicText.text = "";
            }            

            //search Cabai
            Transform PosCabai = SwitchPanel.transform.Find("posCabai");
            TextMeshProUGUI CabaiText = PosCabai.transform.Find("CabaiText").GetComponent<TextMeshProUGUI>();
            if (PlayerPrefs.HasKey("GetCabai")){
                GameObject CabaiGo = Instantiate(playerlist.Cabai, PosCabai);
                CabaiGo.transform.localScale = new Vector3(50f,50f,0f);
                CabaiText.text = playerlist.CabaiUnit.Nama;
                BoxCollider2D CabaiCollider = CabaiGo.AddComponent<BoxCollider2D>();
                CabaiCollider.size = new Vector2(2f,2f);
                OnClickSwitch CabaiScript = CabaiGo.AddComponent<OnClickSwitch>();
                CabaiScript.identifierPlayerClick = "Cabai";    
            } else {
                CabaiText.text = "";
            }
            

            //search KembangKol
            Transform PosKembangKol = SwitchPanel.transform.Find("posKembangKol");
            TextMeshProUGUI KembangKolText = PosKembangKol.transform.Find("KembangKolText").GetComponent<TextMeshProUGUI>();
            if (PlayerPrefs.HasKey("GetKembangKol")){
                GameObject KembangKolGo = Instantiate(playerlist.KembangKol, PosKembangKol);
                KembangKolGo.transform.localScale = new Vector3(50f,50f,0f);
                KembangKolText.text = playerlist.KembangKolUnit.Nama;
                BoxCollider2D KembangKolCollider = KembangKolGo.AddComponent<BoxCollider2D>();
                KembangKolCollider.size = new Vector2(2f,2f);
                OnClickSwitch KembangKolScript = KembangKolGo.AddComponent<OnClickSwitch>();
                KembangKolScript.identifierPlayerClick = "KembangKol";    
            } else {
                KembangKolText.text = "";
            }
            

            
        }

        public void SwitchParty(string SwitchedObj){
            Destroy(CanvasPanelSwitchParty);

            if (playerlist.KentangUnit.partyPosition == WhatPartyPosSwitched){
                playerlist.KentangUnit.partyPosition = 0;
            } else if (playerlist.TomatUnit.partyPosition == WhatPartyPosSwitched){
                playerlist.TomatUnit.partyPosition = 0;
            } else if (playerlist.GarlicUnit.partyPosition == WhatPartyPosSwitched){
                playerlist.GarlicUnit.partyPosition = 0;
            } else if (playerlist.CabaiUnit.partyPosition == WhatPartyPosSwitched){
                playerlist.CabaiUnit.partyPosition = 0;
            } else if (playerlist.KembangKolUnit.partyPosition == WhatPartyPosSwitched){
                playerlist.KembangKolUnit.partyPosition = 0;
            }



            if (SwitchedObj == "Kentang"){
                playerlist.KentangUnit.partyPosition = WhatPartyPosSwitched;
            } else if (SwitchedObj == "Tomat"){
                playerlist.TomatUnit.partyPosition = WhatPartyPosSwitched;
            } else if (SwitchedObj == "Garlic"){
                playerlist.GarlicUnit.partyPosition = WhatPartyPosSwitched;
            } else if (SwitchedObj == "Cabai"){
                playerlist.CabaiUnit.partyPosition = WhatPartyPosSwitched;
            } else if (SwitchedObj == "KembangKol"){
                playerlist.KembangKolUnit.partyPosition = WhatPartyPosSwitched;
            } else {
                Debug.LogWarning("no changed");
            }
            SpawningParty();
            isCanvasSwitchOn = false;
        }

        void Update(){
            if (Input.GetKeyDown(KeyCode.Escape)){
                SetPosGlobal();
                PlayerPrefs.SetString("LoadStrIdentifier", "LevelSelector");
                SceneManager.LoadScene("Loading Screen", LoadSceneMode.Single);
            }
        }

        void SetPosGlobal(){
            if (Pos1Now != null){
                PlayerPrefs.SetString("Pos1", Pos1Now);
            }
            if (Pos2Now != null){
                PlayerPrefs.SetString("Pos2", Pos2Now);
            }
            if (Pos3Now != null){
                PlayerPrefs.SetString("Pos3", Pos3Now);
            }
        }

        public void onConfirmButton(){
            partylist.playerListDict();
            if (partylist.List1() == null && partylist.List2() == null && partylist.List3() == null){
                StartCoroutine(EmptyWarningSpawn());
            } else {
                SetPosGlobal();
                PlayerPrefs.SetString("LoadStrIdentifier", "Battle");
                SceneManager.LoadScene("Loading Screen", LoadSceneMode.Single);
            }
        }

        IEnumerator EmptyWarningSpawn(){
            GameObject EmptyWarnObj = Instantiate(EmptyWarning, MidPos.transform);
            Image EmptyWarnImg = EmptyWarnObj.GetComponent<Image>();
            Color EmptyWarnColor = EmptyWarnImg.color;
            while (EmptyWarnColor.a > 0){
                EmptyWarnColor.a -= 0.01f;
                EmptyWarnImg.color = EmptyWarnColor;
                yield return new WaitForSeconds(0.05f);
            }
        }
    }
