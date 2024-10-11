using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public enum BattleStateOn{Start, PlayerTurn, EnemyTurn, Won, Lose};
public class GameManager : MonoBehaviour
{
    public BattleStateOn battleState;
    public GameObject[] PosPlayer;
    public GameObject[] PosEnemy;
    
    public GameObject PosPanelMoveset;
    public GameObject PanelMoveset;
    
    public TextMeshProUGUI DialogText;

    public PartyList partylist;
    public EnemyList enemylist;

    GameObject[] PlayerPrefab;
    GameObject[] PlayerHUD;
    Slider[] PlayerHpSlider;
    GameObject[] PlayerObj;
    Unit[] PlayerUnit;
    HoverClickObject[] PlayerHoverScript;

    GameObject[] EnemyPrefab;
    GameObject[] EnemyHUD;
    Slider[] EnemyHpSlider;
    GameObject[] EnemyObj;
    Unit[] EnemyUnit;
    HoverClickObject[] EnemyHoverScript;

    int[] PlayerBuffNerfCount;
    int[] EnemyBuffNerfCount;

    GameObject PanelMovesetObj;


    public GameObject HudObj;

    public TextMeshProUGUI XpUnitText;

    public GameObject PanelResult;

    public AudioSource VictorySound;
    public AudioSource DefeatSound;
    public AudioSource LevelUpSound;
    public AudioSource StrikeSound;
    public AudioSource SlashSound;
    public AudioSource PierceSound;
    public AudioSource ButtonSFX;
    

    int turnCount;

    void Start(){
        partylist.playerListDict();
        Debug.Log(partylist.List1());
        Debug.Log(partylist.List2());
        Debug.Log(partylist.List3());
        PlayerPrefab = new GameObject[] {partylist.List1(), partylist.List2(), partylist.List3()};
        EnemyPrefab = new GameObject[] {enemylist.List1(), enemylist.List2(), enemylist.List3()};
        PlayerUnit = new Unit[3];
        EnemyUnit = new Unit[3];
        PlayerHoverScript = new HoverClickObject[3];
        EnemyHoverScript = new HoverClickObject[3];
        PlayerObj = new GameObject[3];
        EnemyObj = new GameObject[3];
        PlayerHUD = new GameObject[3];
        EnemyHUD = new GameObject[3];
        PlayerHpSlider = new Slider[3];
        EnemyHpSlider = new Slider[3];
        PlayerBuffNerfCount = new int[3];
        EnemyBuffNerfCount = new int[3];
        battleState = BattleStateOn.Start;
        turnCount = 0;
        for (int i=0; i<3; i++){
            PlayerBuffNerfCount[i] = 0;
            EnemyBuffNerfCount[i] = 0;
        }
        StartCoroutine(PlayerEnemySpawn());
    }

    IEnumerator PlayerEnemySpawn(){
        Debug.Log("Spawning");
        yield return StartCoroutine(SpawnPlayerEnemy(PlayerPrefab, true));
        yield return StartCoroutine(SpawnPlayerEnemy(EnemyPrefab, false));
        DialogText.text = "";
        yield return StartCoroutine(PlayerTurnBattle());
    }

    IEnumerator SpawnPlayerEnemy(GameObject[] Prefab, bool PlayerOrEnemy){
        DialogText.text = PlayerOrEnemy ? "Player Spawn" : "Enemy Spawn";

        for (int i = 0; i<3; i++){
            if (PlayerOrEnemy){
                if (Prefab[i] != null){
                    PlayerObj[i] = Instantiate(Prefab[i], PosPlayer[i].transform);
                    PlayerObj[i].transform.localScale = new Vector3(50f,50f,0f);
                    PlayerHUD[i] = Instantiate(HudObj, PosPlayer[i].transform);
                    RectTransform PlayerHudRectTransform = PlayerHUD[i].GetComponent<RectTransform>();
                    PlayerHudRectTransform.anchoredPosition = new Vector2(25f,170f);
                    TextMeshProUGUI PlayerNama = PlayerHUD[i].GetComponent<TextMeshProUGUI>();
                    PlayerHpSlider[i] = PlayerHUD[i].transform.Find("SliderHp")?.GetComponent<Slider>();
                    PlayerUnit[i] = PlayerObj[i].GetComponent<Unit>();
                    PlayerNama.text = PlayerUnit[i].Nama;

                    SetPlayerLevelAndXP(i);

                    BoxCollider2D PlayerCollider = PlayerObj[i].AddComponent<BoxCollider2D>();
                    PlayerCollider.size = new Vector2(2f,2f);
                    PlayerHoverScript[i] = PlayerObj[i].AddComponent<HoverClickObject>();
                    PlayerHoverScript[i].identifierPlayerInt = i;
                    PlayerHoverScript[i].AllowHoverClickPlayer = false;
                    PlayerHoverScript[i].isPlayer = true;

                    yield return new WaitForSeconds(0.5f);
                }
            } else {
                if (Prefab[i] != null){
                    EnemyObj[i] = Instantiate(Prefab[i], PosEnemy[i].transform);
                    EnemyObj[i].transform.localScale = new Vector3(50f,50f,0f);
                    EnemyHUD[i] = Instantiate(HudObj, PosEnemy[i].transform);
                    RectTransform EnemyHudRectTransform = EnemyHUD[i].GetComponent<RectTransform>();
                    EnemyHudRectTransform.anchoredPosition = new Vector2(25f,240f);
                    TextMeshProUGUI EnemyNama = EnemyHUD[i].GetComponent<TextMeshProUGUI>();
                    EnemyHpSlider[i] = EnemyHUD[i].transform.Find("SliderHp").GetComponent<Slider>();
                    EnemyUnit[i] = EnemyObj[i].GetComponent<Unit>();
                    EnemyNama.text = EnemyUnit[i].Nama;

                    
                    BoxCollider2D EnemyCollider = EnemyObj[i].AddComponent<BoxCollider2D>();
                    EnemyCollider.size = new Vector2(2f,2f);
                    EnemyHoverScript[i] = EnemyObj[i].AddComponent<HoverClickObject>();
                    EnemyHoverScript[i].identifierEnemyInt = i;
                    EnemyHoverScript[i].AllowHoverClickEnemy = false;
                    EnemyHoverScript[i].isPlayer = false;


                    yield return new WaitForSeconds(0.5f);
                }
            }
        }

    }

    void SetPlayerLevelAndXP(int identifier){
        PlayerUnit[identifier].Level = PlayerPrefs.GetInt("Level"+PlayerUnit[identifier].Nama , 1);
        PlayerUnit[identifier].exp = PlayerPrefs.GetInt("XP"+PlayerUnit[identifier].Nama , 1);
    }

    IEnumerator PlayerTurnBattle(){
        yield return new WaitForSeconds(1);
        battleState = BattleStateOn.PlayerTurn;
        if (battleState == BattleStateOn.PlayerTurn){
            DialogText.text = "Player Turn";
            allowPlayerHover();
            disableEnemyHover();
        }
        turnCount += 1;
    }

    void allowPlayerHover(){
        for (int i = 0; i<3; i++){
            if (PlayerObj[i] != null){
                PlayerHoverScript[i].AllowHoverClickPlayer = true;
            }
        }
    }

    void disablePlayerHover(){
        for (int i = 0; i<3; i++){
            if (PlayerObj[i] != null){
                PlayerHoverScript[i].AllowHoverClickPlayer = false;
            }
        }
    }

    void allowEnemyHover(){
        for (int i = 0; i<3; i++){
            if (EnemyObj[i] != null){
                EnemyHoverScript[i].AllowHoverClickEnemy = true;
            }
        }
    }

    void disableEnemyHover(){
        for (int i = 0; i<3; i++){
            if (EnemyObj[i] != null){
                EnemyHoverScript[i].AllowHoverClickEnemy = false;
            }
        }
    }


    public void SpawnPanelMoveset(int identifierInt){
        ButtonSFX.Play();
        disablePlayerHover();
        PanelMovesetObj = Instantiate(PanelMoveset, PosPanelMoveset.transform);
        Button PanelMovesetNormalButton = PanelMovesetObj.transform.Find("NormalMove").GetComponent<Button>();
        PanelMovesetNormalButton.onClick.AddListener(NormalMoveButton);
        TextMeshProUGUI PanelMovesetNormalText = PanelMovesetNormalButton.transform.Find("NormalMoveText").GetComponent<TextMeshProUGUI>();

        Button PanelMovesetSpecialButton = PanelMovesetObj.transform.Find("SpecialMove").GetComponent<Button>();
        PanelMovesetSpecialButton.onClick.AddListener(SpecialMoveButton);
        TextMeshProUGUI PanelMovesetSpecialText = PanelMovesetSpecialButton.transform.Find("SpecialMoveText").GetComponent<TextMeshProUGUI>();

        Button PanelMovesetBackButton = PanelMovesetObj.transform.Find("Back").GetComponent<Button>();
        PanelMovesetBackButton.onClick.AddListener(BackButton);
        
        
        PanelMovesetNormalText.text = PlayerUnit[identifierInt].type;
        if (PlayerUnit[identifierInt].moveName != ""){
            PanelMovesetSpecialText.text = PlayerUnit[identifierInt].moveName;
        } else {
            Destroy(PanelMovesetSpecialButton.gameObject);
        }

        void BackButton(){
            ButtonSFX.Play();
            Destroy(PanelMovesetObj);
            allowPlayerHover();
        }

        void NormalMoveButton(){
            ButtonSFX.Play();
            Destroy(PanelMovesetObj);
            ChooseEnemy("Normal");
        }

        void SpecialMoveButton(){
            ButtonSFX.Play();
            Destroy(PanelMovesetObj);
            if (PlayerUnit[identifierInt].Nama == "Tomat"){
                ChooseEnemy("Special");
            } else {
                StartCoroutine(IdentifySpecialMove(identifierInt, -1, "Player"));
            }
        }
        
        void ChooseEnemy(string AttType){
            disablePlayerHover();
            allowEnemyHover();
            DialogText.text = "Choose Enemy";
            for (int i = 0; i<3; i++){
                EnemyHoverScript[i].PlayerUnitAttackIdentifier = identifierInt;
                EnemyHoverScript[i].AttackType = AttType;
            }
        }
    }


    public void DamageCalc(int DamagedUnitIdentifier, int AttackerUnitIdentifier, string AttackType, string WhoAttack){
        ButtonSFX.Play();
        Unit DamagedUnit = null;
        Unit AttackerUnit = null;
        if (WhoAttack == "Player"){
            DamagedUnit = EnemyUnit[DamagedUnitIdentifier];
            AttackerUnit = PlayerUnit[AttackerUnitIdentifier];
        } else if (WhoAttack == "Enemy"){
            DamagedUnit = PlayerUnit[DamagedUnitIdentifier];
            AttackerUnit = EnemyUnit[AttackerUnitIdentifier];
        }
        if (AttackType == "Normal"){
            //check weakness
            float WeaknessModifier;            
            WeaknessModifier = CheckTypePower(DamagedUnit.type, AttackerUnit.type);
            DialogText.text = AttackerUnit.Nama + " use " + AttackerUnit.type;
            if (AttackerUnit.type == "Strike"){
                StrikeSound.Play();
            } else if (AttackerUnit.type == "Slash"){
                SlashSound.Play();
            } else if (AttackerUnit.type == "Pierce"){
                PierceSound.Play();
            }

            float Def = (AttackerUnit.Total.Attack * (DamagedUnit.Total.Defense/100f));
            float DamageOutput = (AttackerUnit.Total.Attack - Def) * WeaknessModifier;
            Debug.Log("Damage Output "+ DamageOutput);
            StartCoroutine(ReduceHp(DamageOutput, AttackerUnitIdentifier, DamagedUnitIdentifier, WhoAttack));
        } else if (AttackType == "Special"){
            StartCoroutine(IdentifySpecialMove(AttackerUnitIdentifier, DamagedUnitIdentifier, WhoAttack));
        }
    }

    IEnumerator IdentifySpecialMove(int AttackerUnitIdentifier, int DamagedUnitIdentifier, string WhoAttack){
        yield return new WaitForSeconds(1);
        if (WhoAttack == "Player"){

            DialogText.text = "You Use " + PlayerUnit[AttackerUnitIdentifier].moveName;
            yield return new WaitForSeconds(1.5f);
            //Player moveset
            if (PlayerUnit[AttackerUnitIdentifier].moveName == "Harden"){
                PlayerBuffNerfCount[AttackerUnitIdentifier] += 1;
                Debug.Log(AttackerUnitIdentifier + "|||||" + PlayerBuffNerfCount[AttackerUnitIdentifier]);
                PlayerUnit[AttackerUnitIdentifier].Modified[PlayerBuffNerfCount[AttackerUnitIdentifier]].Defense = 10;
                PlayerUnit[AttackerUnitIdentifier].Modified[PlayerBuffNerfCount[AttackerUnitIdentifier]].TurnEndBuffNerf = turnCount + 2;
                yield return new WaitForSeconds(0.5f);
                DialogText.text = PlayerUnit[AttackerUnitIdentifier].Nama + " Defense up";
            } else if (PlayerUnit[AttackerUnitIdentifier].moveName == "Splash"){
                EnemyUnit[DamagedUnitIdentifier].Effect.isStunned = true;
                EnemyUnit[DamagedUnitIdentifier].Effect.StunnedTurn += 2;
                yield return new WaitForSeconds(0.5f);
                DialogText.text = PlayerUnit[AttackerUnitIdentifier].Nama + " Stun " + EnemyUnit[DamagedUnitIdentifier].Nama;
            } else if (PlayerUnit[AttackerUnitIdentifier].moveName == "Stink"){
                for (int i = 0; i<3; i++){
                    if (EnemyObj[i] != null){
                        EnemyBuffNerfCount[i] += 1;
                        EnemyUnit[i].Modified[EnemyBuffNerfCount[i]].Defense = (EnemyUnit[i].Total.Defense/2)*(-1);
                        EnemyUnit[i].Modified[EnemyBuffNerfCount[i]].TurnEndBuffNerf = turnCount + 2;
                    }
                    yield return new WaitForSeconds(0.5f);
                    DialogText.text = "All Enemy Defense down";
                }
            } else if (PlayerUnit[AttackerUnitIdentifier].moveName == "Burning Hot"){
                for (int i = 0; i<3; i++){
                    if (PlayerObj[i] != null){
                        PlayerBuffNerfCount[i] += 1;
                        PlayerUnit[i].Modified[PlayerBuffNerfCount[i]].Attack = PlayerUnit[i].Attack;
                        PlayerUnit[i].Modified[PlayerBuffNerfCount[i]].TurnEndBuffNerf = turnCount + 2;
                    }
                    yield return new WaitForSeconds(0.5f);
                    DialogText.text = "All Player Attack up";
                }
            } else if (PlayerUnit[AttackerUnitIdentifier].moveName == "Cabbage Shield"){
                int x = Random.Range(0,2);
                PlayerUnit[x].Effect.isShield = true;
                yield return new WaitForSeconds(0.5f);
                DialogText.text = PlayerUnit[AttackerUnitIdentifier].Nama + " is Shielded";
            }
            yield return new WaitForSeconds(1);
            StartCoroutine(EnemyTurn());
        } else {

            DialogText.text = "Enemy Use " + EnemyUnit[AttackerUnitIdentifier].moveName;
            yield return new WaitForSeconds(1.5f);

            //Enemy moveset
            if (EnemyUnit[AttackerUnitIdentifier].moveName == "Slam"){
                for (int i = 0; i<3; i++){
                    if (PlayerObj[i] != null){
                        PlayerBuffNerfCount[i] += 1;
                        PlayerUnit[i].Modified[PlayerBuffNerfCount[i]].Attack = -3;
                        PlayerUnit[i].Modified[PlayerBuffNerfCount[i]].TurnEndBuffNerf = turnCount + 2;
                        if (PlayerUnit[i].Total.Attack < 0){
                            PlayerUnit[i].Total.Attack = 0;
                        }
                    }
                    DialogText.text = "All Player Attack down";
                    yield return new WaitForSeconds(0.5f);
                }
            } else if (EnemyUnit[AttackerUnitIdentifier].moveName == "Stance"){
                for (int i = 0; i<3; i++){
                    if (EnemyObj[i] != null){
                        EnemyUnit[i].Effect.isShield = true;
                    }
                    yield return new WaitForSeconds(0.5f);
                    DialogText.text = "All Enemy is Shielded";
                }
            } else if(EnemyUnit[AttackerUnitIdentifier].moveName == "slice"){
                float WeaknessModifier = CheckTypePower(PlayerUnit[DamagedUnitIdentifier].type, EnemyUnit[AttackerUnitIdentifier].type);
                float Def = (PlayerUnit[DamagedUnitIdentifier].Total.Attack * (PlayerUnit[DamagedUnitIdentifier].Total.Defense/100f));
                float DamageOutput = (EnemyUnit[AttackerUnitIdentifier].Total.Attack - Def) * WeaknessModifier;
                StartCoroutine(ReduceHp(DamageOutput, DamagedUnitIdentifier, AttackerUnitIdentifier, "Enemy"));
            } else if(EnemyUnit[AttackerUnitIdentifier].moveName == "Peel"){
                int x = Random.Range(0,2);
                while (PlayerObj[x] == null){
                    x = Random.Range(0,2);
                }
                PlayerBuffNerfCount[x] += 1;
                PlayerUnit[x].Modified[PlayerBuffNerfCount[x]].Defense = -10;
                yield return new WaitForSeconds(0.5f);
                DialogText.text = PlayerUnit[AttackerUnitIdentifier].Nama + " Defense down";
            } else if(EnemyUnit[AttackerUnitIdentifier].moveName == "Dip Fry"){
                for (int i = 0; i<3; i++){
                    if (PlayerObj[i] != null){
                        PlayerUnit[i].Effect.isStunned = true;
                        PlayerUnit[i].Effect.StunnedTurn += 2;
                    }
                    yield return new WaitForSeconds(0.5f);
                    DialogText.text = "All Player is Stunned";
                }
            }
            yield return new WaitForSeconds(1);
            StartCoroutine(PlayerTurnBattle());
        }
        
    }

    float CheckTypePower(string Damaged, string Attacker){
        if (Attacker == Damaged){
            return 1;
        } else if ((Attacker == "Strike" && Damaged == "Pierce") || (Attacker == "Pierce" && Damaged == "Slash") || (Attacker == "Slash" && Damaged == "Strike")){
            DialogText.text = "It's Super Effective";
            return 2;
        } else if ((Attacker == "Strike" && Damaged == "Slash") || (Attacker == "Pierce" && Damaged == "Strike") || (Attacker == "Slash" && Damaged == "Pierce")){
            DialogText.text = "It's Not Effective";
            return 0.5f;
        } else {
            return 1;
        }
    }

    IEnumerator ReduceHp(float DamageOutput, int AttackerUnitIdentifier, int DamagedUnitIdentifier, string WhoAttack){
        int DamagedUnitIdentifier2 = 0;
        yield return new WaitForSeconds(1.5f);
        if (WhoAttack == "Player"){
            DialogText.text = "You Deal " + (int)DamageOutput + " to " + EnemyUnit[DamagedUnitIdentifier].Nama;
            if (PlayerUnit[AttackerUnitIdentifier].Nama != "Kentang"){
                yield return StartCoroutine(CheckShieldAndReduceHp());
            } else {
                //plot armor sang mc si Kentang
                yield return StartCoroutine(CheckShieldAndReduceHp());
                int randomUnitAttacked;
                if (DamagedUnitIdentifier == 1){
                    randomUnitAttacked = 2;
                } else if (DamagedUnitIdentifier == 2) {
                    randomUnitAttacked = 3;
                } else {
                    randomUnitAttacked = 1;
                }
                DamagedUnitIdentifier2 = DamagedUnitIdentifier;
                DamagedUnitIdentifier = randomUnitAttacked;
                yield return StartCoroutine(CheckShieldAndReduceHp());
            }

            IEnumerator CheckShieldAndReduceHp(){
                if (!EnemyUnit[DamagedUnitIdentifier].Effect.isShield){
                    for (int ShouldHp = EnemyUnit[DamagedUnitIdentifier].CurrentHp - (int)DamageOutput; EnemyUnit[DamagedUnitIdentifier].CurrentHp > ShouldHp; EnemyUnit[DamagedUnitIdentifier].CurrentHp--){
                            SetHpBar(EnemyHpSlider[DamagedUnitIdentifier], EnemyUnit[DamagedUnitIdentifier].MaxHp, EnemyUnit[DamagedUnitIdentifier].CurrentHp);
                            yield return new WaitForSeconds(0.01f);
                        }
                } else {
                    EnemyUnit[DamagedUnitIdentifier].Effect.isShield = false;
                    DialogText.text = "Enemy Shielded";
                    yield return new WaitForSeconds(1);
                    DialogText.text = "";
                    yield return new WaitForSeconds(0.5f);
                    DialogText.text = "Enemy shield broke";
                }
            }
            
            
            yield return new WaitForSeconds(1);
            if (EnemyUnit[DamagedUnitIdentifier].CurrentHp <= 0 && EnemyObj[DamagedUnitIdentifier] != null){
                StartCoroutine(UnitDie(DamagedUnitIdentifier, "Enemy"));
            }
            if (PlayerUnit[AttackerUnitIdentifier].Nama == "Kentang"){
                if (EnemyUnit[DamagedUnitIdentifier2].CurrentHp <= 0 && EnemyObj[DamagedUnitIdentifier2] != null){
                    StartCoroutine(UnitDie(DamagedUnitIdentifier2, "Enemy"));
                }
            }
            if (CheckAlive(false)){
                StartCoroutine(EnemyTurn());
            } else {
                StartCoroutine(EndGame(true));
            }
        } else if(WhoAttack == "Enemy"){
            DialogText.text = "Enemy Deal " + (int)DamageOutput + " to " + PlayerUnit[DamagedUnitIdentifier].Nama;
            if (!PlayerUnit[DamagedUnitIdentifier].Effect.isShield){
                for (int ShouldHp = PlayerUnit[DamagedUnitIdentifier].CurrentHp - (int)DamageOutput; PlayerUnit[DamagedUnitIdentifier].CurrentHp > ShouldHp; PlayerUnit[DamagedUnitIdentifier].CurrentHp--){
                    SetHpBar(PlayerHpSlider[DamagedUnitIdentifier], PlayerUnit[DamagedUnitIdentifier].MaxHp, PlayerUnit[DamagedUnitIdentifier].CurrentHp);
                    yield return new WaitForSeconds(0.01f);
                }
            } else {
                PlayerUnit[DamagedUnitIdentifier].Effect.isShield = false;
                DialogText.text = "Player Shielded";
                yield return new WaitForSeconds(1);
                DialogText.text = "";
                yield return new WaitForSeconds(0.5f);
                DialogText.text = "Player shield broke";
            }
            
            yield return new WaitForSeconds(1);
            if (PlayerUnit[DamagedUnitIdentifier].CurrentHp <= 0){
                StartCoroutine(UnitDie(DamagedUnitIdentifier, "Player"));
            }
            if (CheckAlive(true)){
                StartCoroutine(PlayerTurnBattle());
            } else {
                StartCoroutine(EndGame(false));
            }
        }
    }

    void SetHpBar(Slider SliderHp, int MaxHp, int CurrentHp){
        SliderHp.maxValue = MaxHp;
        SliderHp.value = CurrentHp;
    }

    IEnumerator EnemyTurn(){
        disableEnemyHover();
        disablePlayerHover();
        Debug.Log("Enemy Turn");
        DialogText.text = "Enemy Turn";
        yield return new WaitForSeconds(2);
        battleState = BattleStateOn.EnemyTurn;
        int EnemyAttackerIdentifier = Random.Range(0,3);
        while (EnemyObj[EnemyAttackerIdentifier] == null){
            EnemyAttackerIdentifier = Random.Range(0,3);
        }
        int PlayerDamagedIdentifier = Random.Range(0,3);
        while (PlayerObj[PlayerDamagedIdentifier] == null){
            PlayerDamagedIdentifier = Random.Range(0,3);
        }
        int AttackTypeRandomizer = Random.Range(0,2);
        string AttackType;
        if (AttackTypeRandomizer == 0){
            AttackType = "Normal";
        } else {
            AttackType = "Special";
        }
        DamageCalc(PlayerDamagedIdentifier, EnemyAttackerIdentifier, AttackType, "Enemy");
    }

    IEnumerator UnitDie(int dieUnitIdentifier, string WhoDie){
        yield return null;
        if (WhoDie == "Player"){
            SpriteRenderer PlayerObjSpriteRenderer = PlayerObj[dieUnitIdentifier].GetComponent<SpriteRenderer>();
            Color PlayerObjColor = PlayerObjSpriteRenderer.color;
            while (PlayerObjColor.a > 0){
                PlayerObjColor.a -= 0.01f;
                PlayerObjSpriteRenderer.color = PlayerObjColor;
                yield return new WaitForSeconds(0.01f);
            }
            Destroy(PlayerObj[dieUnitIdentifier]);
            Destroy(PlayerHUD[dieUnitIdentifier]);
        } else if (WhoDie == "Enemy"){
            //TextMeshProUGUI XpUnitTextObj = Instantiate(XpUnitText,PosEnemy[dieUnitIdentifier].transform);
            SpriteRenderer EnemyObjSpriteRenderer = EnemyObj[dieUnitIdentifier].GetComponent<SpriteRenderer>();
            Color EnemyObjColor = EnemyObjSpriteRenderer.color;
            while (EnemyObjColor.a > 0){
                EnemyObjColor.a -= 0.01f;
                EnemyObjSpriteRenderer.color = EnemyObjColor;
                yield return new WaitForSeconds(0.01f);
            }
            Destroy(EnemyObj[dieUnitIdentifier]);
            Destroy(EnemyHUD[dieUnitIdentifier]);
        }
    }

    bool CheckAlive(bool isPlayer){
        bool returnValue = false;
        if (isPlayer){
            for (int i=0 ; i<3; i++){
                if (PlayerObj[i] != null){
                    returnValue = returnValue || (PlayerUnit[i].CurrentHp > 0);
                }
            }
        } else {
            for (int i=0 ; i<3; i++){
                if (EnemyObj[i] != null){
                    returnValue = returnValue || (EnemyUnit[i].CurrentHp > 0);
                }
            }
        }
        return returnValue;
    }

    IEnumerator EndGame(bool isWin){
        yield return new WaitForSeconds(2);
        DialogText.text = isWin ? "You Won" : "You Lose";
        yield return (DestroyObjects());
        if (isWin){
            battleState = BattleStateOn.Won;
            Debug.Log("Won");
            VictorySound.Play();
            yield return StartCoroutine(resultPanel(true));
        } else {
            battleState = BattleStateOn.Lose;
            Debug.Log("Lose");
            DefeatSound.Play();
            yield return StartCoroutine(resultPanel(false));
        }
    }

    IEnumerator DestroyObjects(){
        for (int i = 0; i<3 ; i++){
            if (PlayerObj[i] != null){
                Destroy(PlayerObj[i]);
                Destroy(PlayerHUD[i]);
            }
            if (EnemyObj[i] != null){
                Destroy(EnemyObj[i]);
                Destroy(EnemyHUD[i]);
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator resultPanel(bool isWin){
        int LevelNow = PlayerPrefs.GetInt("onLevel");
        int[] LevelXp = {800,1000,1500,2500,5000};

        GameObject PanelResultObj = Instantiate(PanelResult, PosPanelMoveset.transform);
        TextMeshProUGUI PanelResultText = PanelResultObj.transform.Find("ResultText").GetComponent<TextMeshProUGUI>();
        Transform[] PanelObjPos = new Transform[3];
        Slider[] PanelObjSliderXP = new Slider[3];
        TextMeshProUGUI[] PanelObjLevelText = new TextMeshProUGUI[3];
        TextMeshProUGUI[] PanelObjXpText = new TextMeshProUGUI[3];
        Button LevelSelectButton = PanelResultObj.transform.Find("LevelSelButton").GetComponent<Button>();
        LevelSelectButton.onClick.AddListener(LevelSelectOnButton);
        Button NextButton = PanelResultObj.transform.Find("NextButton").GetComponent<Button>();
        NextButton.onClick.AddListener(() => NextLevelOnButton(LevelNow));

        for (int i = 0; i<3; i++) {
            string FindPlayerPos = "PlayerPos" + (i+1);
            PanelObjPos[i] = PanelResultObj.transform.Find(FindPlayerPos);
            PanelObjSliderXP[i] = PanelObjPos[i].transform.Find("XPSlider").GetComponent<Slider>();
            PanelObjLevelText[i] = PanelObjPos[i].transform.Find("Level").GetComponent<TextMeshProUGUI>();
            PanelObjXpText[i] = PanelObjPos[i].transform.Find("Xp").GetComponent<TextMeshProUGUI>();
        }
        if (isWin){
            NextButton.interactable = false;
            LevelSelectButton.interactable = false;
            PanelResultText.text = "VICTORY";
            for (int j = 0; j<3; j++){
                if (PlayerPrefab[j] != null){
                    PlayerObj[j] = Instantiate(PlayerPrefab[j],PanelObjPos[j].transform);
                    PlayerObj[j].transform.localScale = new Vector2(20f,20f);
                    PlayerUnit[j] = PlayerObj[j].GetComponent<Unit>();
                    PanelObjLevelText[j].text = PlayerUnit[j].Level.ToString();
                    StartCoroutine(SetXPBar(j, LevelXp[LevelNow-1]));
                } else {
                    Destroy(PanelObjSliderXP[j].gameObject);
                    Destroy(PanelObjLevelText[j].gameObject);
                }
            }
            int levelsUnlocked = PlayerPrefs.GetInt("levelsUnlocked");
            if (levelsUnlocked <= LevelNow) {
                PlayerPrefs.SetInt("levelsUnlocked", (LevelNow + 1));
            }
            Reward(LevelNow);
            SaveProgress();
            yield return new WaitForSeconds(1);
        } else {
            PanelResultText.text = "DEFEAT";
            NextButton.interactable = false;
            for (int k = 0; k<3 ; k++){
                Destroy(PanelObjSliderXP[k]);
            }
        }
        yield return null;


        IEnumerator SetXPBar(int identifier, int expGain){
            int maxXp;
            maxXp = setMaxXp(PlayerUnit[identifier].Level);
            PanelObjXpText[identifier].text = PlayerUnit[identifier].exp + "/" + maxXp;
            PanelObjSliderXP[identifier].maxValue = maxXp;
            PanelObjSliderXP[identifier].value = PlayerUnit[identifier].exp;
            int shouldExp = PlayerUnit[identifier].exp + expGain;
            int overflowExp;
            if (shouldExp >= maxXp){
                overflowExp = shouldExp - maxXp;
                shouldExp = maxXp;
                yield return StartCoroutine(XpBar(shouldExp, PlayerUnit[identifier].exp));
                PlayerUnit[identifier].Level += 1;
                PanelObjLevelText[identifier].text = PlayerUnit[identifier].Level.ToString();
                PlayerUnit[identifier].exp = 0;
                maxXp = setMaxXp(PlayerUnit[identifier].Level);
                yield return StartCoroutine(PlayLevelUpSound());
                PanelObjSliderXP[identifier].maxValue = maxXp;
                PanelObjSliderXP[identifier].value = PlayerUnit[identifier].exp;
                yield return StartCoroutine(XpBar(overflowExp, PlayerUnit[identifier].exp));
                PlayerUnit[identifier].exp = overflowExp;
            } else {
                yield return StartCoroutine(XpBar(shouldExp, PlayerUnit[identifier].exp));
                PlayerUnit[identifier].exp = shouldExp;
            }
            SaveProgress();
            NextButton.interactable = true;
            LevelSelectButton.interactable = true;

            IEnumerator PlayLevelUpSound(){
                LevelUpSound.Play();
                yield return new WaitForSeconds(0.5f);
            }

            IEnumerator XpBar(int should, int now){
                for (int i = now; i<=should;i++){
                    PanelObjSliderXP[identifier].value = i;
                    PanelObjXpText[identifier].text = i + "/" + PanelObjSliderXP[identifier].maxValue;
                    yield return new WaitForSeconds(0.01f);
                }
            }

            int setMaxXp(int level){
                switch (PlayerUnit[identifier].Level)
                {
                    case 1:
                        return 300;
                    case 2:
                        return 900;
                    case 3:
                        return 2700;
                    case 4:
                        return 6500;
                    case 5:
                        return 9999;
                    default:
                        return 0;
                }
            }
        }
    }

    

    void LevelSelectOnButton(){
        SceneManager.LoadScene("LevelSelector",LoadSceneMode.Single);
    }

    void NextLevelOnButton(int LevelNow){
        PlayerPrefs.SetInt("onLevel", LevelNow+1);
        SceneManager.LoadScene("Battle", LoadSceneMode.Single);
    }

    void Reward(int LevelNow){
        switch (LevelNow)
        {
            case 1:
                PlayerPrefs.SetInt("GetTomat",1);
                break;
            case 2:
                break;
            case 3:
                PlayerPrefs.SetInt("GetGarlic",1);
                break;
            case 4:
                break;
            case 5:
                break;
        }
    }

    void SaveProgress(){
        for (int i = 0 ; i<3 ; i++){
            Debug.Log("im here in loop saveprogress");
            if (PlayerObj[i] != null){
                Debug.Log("im here in if save progress");
                PlayerPrefs.SetInt("Level"+PlayerUnit[i].Nama, PlayerUnit[i].Level);
                PlayerPrefs.SetInt("XP"+PlayerUnit[i].Nama, PlayerUnit[i].exp);
            }
        }
        PlayerPrefs.Save();
    }

    void Update(){
        //modify stats
        for (int i=0 ; i<3 ; i++){
            if (PlayerObj[i] != null){
                PlayerUnit[i].Total.Attack = PlayerUnit[i].Attack + totalAtt(i,true);
                PlayerUnit[i].Total.Defense = PlayerUnit[i].Defense + totalDef(i,true) + LevelStats(i, "Def");
                PlayerUnit[i].Total.MaxHp = PlayerUnit[i].MaxHp + totalMaxHp(i,true) + LevelStats(i,"MaxHp");
            }
            if (EnemyObj[i] != null){
                EnemyUnit[i].Total.Attack = EnemyUnit[i].Attack + totalAtt(i,false);
                EnemyUnit[i].Total.Defense = EnemyUnit[i].Defense + totalDef(i,false);
                EnemyUnit[i].Total.MaxHp = EnemyUnit[i].MaxHp + totalMaxHp(i,false);          
            }
        }

        int totalAtt(int identifier, bool isPlayer){
            int total = 0;
            if (isPlayer){
                for (int i=1; i<= PlayerBuffNerfCount[identifier]; i++){
                    if (turnCount < PlayerUnit[identifier].Modified[i].TurnEndBuffNerf){
                        total += PlayerUnit[identifier].Modified[i].Attack;
                    }
                }
            } else {
                for (int i=1; i<= EnemyBuffNerfCount[identifier]; i++){
                    if (turnCount < EnemyUnit[identifier].Modified[i].TurnEndBuffNerf){
                        total += EnemyUnit[identifier].Modified[i].Attack;
                    }
                }
            }
            return total;
        }

        int totalDef(int identifier, bool isPlayer){
            int total = 0;
            if (isPlayer){
                for (int i=1; i<= PlayerBuffNerfCount[identifier]; i++){
                    if (turnCount < PlayerUnit[identifier].Modified[i].TurnEndBuffNerf){
                        total += PlayerUnit[identifier].Modified[i].Defense;
                    }
                }
            } else {
                for (int i=1; i<= EnemyBuffNerfCount[identifier]; i++){
                    if (turnCount < EnemyUnit[identifier].Modified[i].TurnEndBuffNerf){
                        total += EnemyUnit[identifier].Modified[i].Defense;
                    }                    
                }
            }
            return total;
        }

        int totalMaxHp(int identifier, bool isPlayer){
            int total = 0;
            if (isPlayer){
                for (int i=1; i<= PlayerBuffNerfCount[identifier]; i++){
                    if (turnCount < PlayerUnit[identifier].Modified[i].TurnEndBuffNerf){
                        total += PlayerUnit[identifier].Modified[i].MaxHp;
                    }                    
                }
            } else {
                for (int i=1; i<= EnemyBuffNerfCount[identifier]; i++){
                    if (turnCount < EnemyUnit[identifier].Modified[i].TurnEndBuffNerf){
                        total += EnemyUnit[identifier].Modified[i].MaxHp;
                    }                    
                }
            }
            return total;
        }

        int LevelStats(int identifier, string StatsTag){
            if (PlayerUnit[identifier].Nama != "Kentang"){
                switch (PlayerUnit[identifier].Level)
                {
                    case 2:
                        if (StatsTag == "Def"){
                            return 5;
                        } else if (StatsTag == "MaxHp"){
                            return 0;
                        } else {
                            return 0;
                        }
                    case 3:
                        if (StatsTag == "Def"){
                            return 5;
                        } else if (StatsTag == "MaxHp"){
                            return 25;
                        } else {
                            return 0;
                        }
                    case 4:
                        if (StatsTag == "Def"){
                            return 10;
                        } else if (StatsTag == "MaxHp"){
                            return 25;
                        } else {
                            return 0;
                        }
                    case 5:
                        if (StatsTag == "Def"){
                            return 10;
                        } else if (StatsTag == "MaxHp"){
                            return 50;
                        } else {
                            return 0;
                        }
                    default:
                        return 0;

                }
            } else {
                switch (PlayerUnit[identifier].Level)
                {
                    case 2:
                        PlayerUnit[identifier].moveName = "Harden";
                        if (StatsTag == "Def"){
                            return 0;
                        } else if (StatsTag == "MaxHp"){
                            return 0;
                        } else {
                            return 0;
                        }
                    case 3:
                        if (StatsTag == "Def"){
                            return 0;
                        } else if (StatsTag == "MaxHp"){
                            return 50;
                        } else {
                            return 0;
                        }
                    case 4:
                        if (StatsTag == "Def"){
                            return 10;
                        } else if (StatsTag == "MaxHp"){
                            return 25;
                        } else {
                            return 0;
                        }
                    case 5:
                        if (StatsTag == "Def"){
                            return 10;
                        } else if (StatsTag == "MaxHp"){
                            return 50;
                        } else {
                            return 0;
                        }
                    default:
                        return 0;
                }
            }
        }
    }
}