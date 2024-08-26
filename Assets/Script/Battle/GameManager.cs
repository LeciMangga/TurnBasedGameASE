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
    public MoveSetList moveSetList;

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


    GameObject PanelMovesetObj;


    public GameObject HudObj;



    void Start(){
        partylist.playerListDict();
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

        battleState = BattleStateOn.Start;
        StartCoroutine(PlayerEnemySpawn());
    }

    IEnumerator PlayerEnemySpawn(){
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
                    PlayerHUD[i] = Instantiate(HudObj, PosPlayer[i].transform);
                    RectTransform PlayerHudRectTransform = PlayerHUD[i].GetComponent<RectTransform>();
                    PlayerHudRectTransform.anchoredPosition = new Vector2(25f,110f);
                    TextMeshProUGUI PlayerNama = PlayerHUD[i].GetComponent<TextMeshProUGUI>();
                    PlayerHpSlider[i] = PlayerHUD[i].transform.Find("SliderHp")?.GetComponent<Slider>();
                    PlayerUnit[i] = PlayerObj[i].GetComponent<Unit>();
                    PlayerNama.text = PlayerUnit[i].Nama;

                    BoxCollider2D PlayerCollider = PlayerObj[i].AddComponent<BoxCollider2D>();
                    PlayerCollider.size = new Vector2(1f,1f);
                    PlayerHoverScript[i] = PlayerObj[i].AddComponent<HoverClickObject>();
                    PlayerHoverScript[i].identifierPlayerInt = i;
                    PlayerHoverScript[i].AllowHoverClickPlayer = false;
                    PlayerHoverScript[i].isPlayer = true;

                    yield return new WaitForSeconds(0.5f);
                }
            } else {
                if (Prefab[i] != null){
                    EnemyObj[i] = Instantiate(Prefab[i], PosEnemy[i].transform);
                    EnemyHUD[i] = Instantiate(HudObj, PosEnemy[i].transform);
                    RectTransform EnemyHudRectTransform = EnemyHUD[i].GetComponent<RectTransform>();
                    EnemyHudRectTransform.anchoredPosition = new Vector2(25f,110f);
                    TextMeshProUGUI EnemyNama = EnemyHUD[i].GetComponent<TextMeshProUGUI>();
                    EnemyHpSlider[i] = EnemyHUD[i].transform.Find("SliderHp").GetComponent<Slider>();
                    EnemyUnit[i] = EnemyObj[i].GetComponent<Unit>();
                    EnemyNama.text = EnemyUnit[i].Nama;

                    
                    BoxCollider2D EnemyCollider = EnemyObj[i].AddComponent<BoxCollider2D>();
                    EnemyCollider.size = new Vector2(1f,1f);
                    EnemyHoverScript[i] = EnemyObj[i].AddComponent<HoverClickObject>();
                    EnemyHoverScript[i].identifierEnemyInt = i;
                    EnemyHoverScript[i].AllowHoverClickEnemy = false;
                    EnemyHoverScript[i].isPlayer = false;


                    yield return new WaitForSeconds(0.5f);
                }
            }
        }

    }

    IEnumerator PlayerTurnBattle(){
        yield return new WaitForSeconds(1);
        battleState = BattleStateOn.PlayerTurn;
        if (battleState == BattleStateOn.PlayerTurn){
            DialogText.text = "Player Turn";
            allowPlayerHover();
            disableEnemyHover();
        }
        
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
        PanelMovesetSpecialText.text = PlayerUnit[identifierInt].moveName;

        void BackButton(){
            Destroy(PanelMovesetObj);
        }

        void NormalMoveButton(){
            Destroy(PanelMovesetObj);
            ChooseEnemy("Normal");
        }

        void SpecialMoveButton(){
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

            float Def = (DamagedUnit.Attack * (DamagedUnit.Defense/100f));
            float DamageOutput = (AttackerUnit.Attack - Def) * WeaknessModifier;
            Debug.Log("Damage Output =" + DamageOutput);
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
                PlayerUnit[AttackerUnitIdentifier].Defense += 10;
                yield return new WaitForSeconds(0.5f);
                DialogText.text = PlayerUnit[AttackerUnitIdentifier].Nama + " Defense up";
            } else if (PlayerUnit[AttackerUnitIdentifier].moveName == "Splash"){
                EnemyUnit[DamagedUnitIdentifier].isStunned = true;
                yield return new WaitForSeconds(0.5f);
                DialogText.text = PlayerUnit[AttackerUnitIdentifier].Nama + " Stun " + EnemyUnit[DamagedUnitIdentifier].Nama;
            } else if (PlayerUnit[AttackerUnitIdentifier].moveName == "Stink"){
                for (int i = 0; i<3; i++){
                    if (EnemyObj[i] != null){
                        EnemyUnit[i].Defense = EnemyUnit[i].Defense/2;
                    }
                    yield return new WaitForSeconds(0.5f);
                    DialogText.text = "All Enemy Defense down";
                }
            } else if (PlayerUnit[AttackerUnitIdentifier].moveName == "Burning Hot"){
                for (int i = 0; i<3; i++){
                    if (PlayerObj[i] != null){
                        PlayerUnit[i].Attack = PlayerUnit[i].Attack * 2;
                    }
                    yield return new WaitForSeconds(0.5f);
                    DialogText.text = "All Player Attack up";
                }
            } else if (PlayerUnit[AttackerUnitIdentifier].moveName == "Cabbage Shield"){
                int x = Random.Range(0,2);
                PlayerUnit[x].isShield = true;
                yield return new WaitForSeconds(0.5f);
                DialogText.text = PlayerUnit[AttackerUnitIdentifier].Nama + " is Shielded";
            }
            StartCoroutine(EnemyTurn());
        } else {

            DialogText.text = "Enemy Use " + EnemyUnit[AttackerUnitIdentifier].moveName;
            yield return new WaitForSeconds(1.5f);
            //Enemy moveset
            if (EnemyUnit[AttackerUnitIdentifier].moveName == "Slam"){
                for (int i = 0; i<3; i++){
                    if (PlayerObj[i] != null){
                        PlayerUnit[i].Attack = PlayerUnit[i].Attack - 3;
                    }
                    DialogText.text = "All Player Attack down";
                    yield return new WaitForSeconds(0.5f);
                }
            } else if (EnemyUnit[AttackerUnitIdentifier].moveName == "Stance"){
                for (int i = 0; i<3; i++){
                    if (EnemyObj[i] != null){
                        EnemyUnit[i].isShield = true;
                    }
                    yield return new WaitForSeconds(0.5f);
                    DialogText.text = "All Enemy is Shielded";
                }
            } else if(EnemyUnit[AttackerUnitIdentifier].moveName == "slice"){
                float WeaknessModifier = CheckTypePower(PlayerUnit[DamagedUnitIdentifier].type, EnemyUnit[AttackerUnitIdentifier].type);
                float Def = (PlayerUnit[DamagedUnitIdentifier].Attack * (PlayerUnit[DamagedUnitIdentifier].Defense/100f));
                float DamageOutput = (EnemyUnit[AttackerUnitIdentifier].Attack - Def) * WeaknessModifier;
                StartCoroutine(ReduceHp(DamageOutput, DamagedUnitIdentifier, AttackerUnitIdentifier, "Enemy"));
            } else if(EnemyUnit[AttackerUnitIdentifier].moveName == "Peel"){
                int x = Random.Range(0,2);
                PlayerUnit[x].Defense -= 10;
                yield return new WaitForSeconds(0.5f);
                DialogText.text = PlayerUnit[AttackerUnitIdentifier].Nama + " Defense down";
            } else if(EnemyUnit[AttackerUnitIdentifier].moveName == "Dip Fry"){
                for (int i = 0; i<3; i++){
                    if (PlayerObj[i] != null){
                        PlayerUnit[i].isStunned = true;
                    }
                    yield return new WaitForSeconds(0.5f);
                    DialogText.text = "All Player is Stunned";
                }
            }
            
            StartCoroutine(PlayerTurnBattle());
        }
        yield return new WaitForSeconds(1);
        
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
        yield return new WaitForSeconds(1.5f);
        if (WhoAttack == "Player"){
            DialogText.text = "You Deal " + (int)DamageOutput + " to " + EnemyUnit[DamagedUnitIdentifier].Nama;
            for (int ShouldHp = EnemyUnit[DamagedUnitIdentifier].CurrentHp - (int)DamageOutput; EnemyUnit[DamagedUnitIdentifier].CurrentHp > ShouldHp; EnemyUnit[DamagedUnitIdentifier].CurrentHp--){
                SetHpBar(EnemyHpSlider[DamagedUnitIdentifier], EnemyUnit[DamagedUnitIdentifier].MaxHp, EnemyUnit[DamagedUnitIdentifier].CurrentHp);
                Debug.Log("current hp" + EnemyUnit[DamagedUnitIdentifier].CurrentHp);
                yield return new WaitForSeconds(0.1f);
            }
            StartCoroutine(UnitDie(DamagedUnitIdentifier, "Enemy"));
            StartCoroutine(EnemyTurn());
        } else if(WhoAttack == "Enemy"){
            DialogText.text = "Enemy Deal " + (int)DamageOutput + " to " + PlayerUnit[DamagedUnitIdentifier].Nama;
            for (int ShouldHp = PlayerUnit[DamagedUnitIdentifier].MaxHp - (int)DamageOutput; PlayerUnit[DamagedUnitIdentifier].CurrentHp > ShouldHp; PlayerUnit[DamagedUnitIdentifier].CurrentHp--){
                SetHpBar(PlayerHpSlider[DamagedUnitIdentifier], PlayerUnit[DamagedUnitIdentifier].MaxHp, PlayerUnit[DamagedUnitIdentifier].CurrentHp);
                yield return new WaitForSeconds(0.1f);
            }
            StartCoroutine(UnitDie(DamagedUnitIdentifier, "Player"));
            StartCoroutine(PlayerTurnBattle());
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
        yield return new WaitForSeconds(1);
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
            Destroy(PlayerObj[dieUnitIdentifier]);
        } else if (WhoDie == "Enemy"){
            Destroy(EnemyObj[dieUnitIdentifier]);
        }
    }
}