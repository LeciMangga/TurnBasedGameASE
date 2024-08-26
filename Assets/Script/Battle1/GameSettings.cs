using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum BattleState {Start, PlayerTurn, EnemyTurn, Won, Lose}

public class GameSettings : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Transform playerPos;
    public Transform enemyPos;
    public HUDBattle playerHUD;
    public HUDBattle enemyHUD;
    public TextMeshProUGUI textDialog;
    public GameObject MoveSetPanel;
    public GameObject PartyListPanel;
    public PartyList partylist;
    
    Unit playerUnit;
    Unit enemyUnit;
    public BattleState nowState;

    bool isAttackButtonPressed = false;
    bool isMoveSetPressed = false;
    bool isSwitchPressed = false;
    bool isPartyListPressed = false;

    GameObject playerGo;
    GameObject enemyGo;

    private int LevelNow;

    // Start is called before the first frame update
    void Start()
    {
        LevelNow = PlayerPrefs.GetInt("onLevel");
        partylist.playerListDict();
        nowState = BattleState.Start;
        StartCoroutine(SpawnPlayerEnemy());
    }

    IEnumerator SpawnPlayerEnemy(){

        isAttackButtonPressed = true;
        isMoveSetPressed = true;
        isSwitchPressed = true;
        isPartyListPressed = true;

        yield return new WaitForSeconds(0.5f);

        playerGo = Instantiate(partylist.List1());
        Debug.Log("Player spawned :" + playerGo);      
        if (playerGo != null){
            playerUnit = playerGo.GetComponent<Unit>();
            playerHUD.setHUD(playerUnit);
        }
        textDialog.text = "Player Spawned";


        yield return new WaitForSeconds(0.5f);


        enemyGo = Instantiate(enemyPrefab);
        Debug.Log("Enemy spawned :"+ enemyGo);
        if (enemyGo != null) {
            enemyUnit = enemyGo.GetComponent<Unit>();
            enemyHUD.setHUD(enemyUnit);
        }
        textDialog.text = "Enemy Spawned";


        yield return new WaitForSeconds(0.5f);


        textDialog.text = "";

        nowState = BattleState.PlayerTurn;
        StartCoroutine(PlayerMove());
    }


    IEnumerator PlayerMove(){
        yield return new WaitForSeconds(0.5f);
        textDialog.text = "Player Turn";
        yield return new WaitForSeconds(0.5f);
        isAttackButtonPressed = false;
        isMoveSetPressed = false;
        isSwitchPressed = false;
        isPartyListPressed = false;
    }

    public void OnAttackButton(){
        StartCoroutine(AttackButtonGO());
    }

    IEnumerator AttackButtonGO(){
        if (!isAttackButtonPressed) {
            isAttackButtonPressed = true;
            isSwitchPressed = true;
            Debug.Log("attack button");
            yield return new WaitForSeconds(0.5f);
            GameObject ParentCanvasMoveSetPanel = Instantiate(MoveSetPanel);
            Transform panelMoveSet = ParentCanvasMoveSetPanel.transform.Find("MoveSetPanel");


            //find move1 button
            Button move1Button = panelMoveSet.transform.Find("Move1").GetComponent<Button>();
            TextMeshProUGUI Move1Text = move1Button.transform.Find("Move1Text").GetComponent<TextMeshProUGUI>();
            move1Button.onClick.AddListener(MoveSet1);
            Move1Text.text = playerUnit.moveName;


            //find back button           
            Button backButton = panelMoveSet.transform.Find("Back").GetComponent<Button>();
            backButton.onClick.AddListener(BackRemoveMoveset);

            //moveset1
            void MoveSet1(){
                if (!isMoveSetPressed){
                    isMoveSetPressed = true;
                    Debug.Log("move1 button");
                    Destroy(ParentCanvasMoveSetPanel, 0.1f);
                    textDialog.text = "You Use " + playerUnit.moveName;
                    StartCoroutine(MoveDmgAndEndTurn(1));
                    }
            }

            // back
            void BackRemoveMoveset(){
                isMoveSetPressed = true;
                Destroy(ParentCanvasMoveSetPanel, 0.1f);
                isAttackButtonPressed = false;
                isMoveSetPressed = false;
                isSwitchPressed = false;
            }
        }
    }

    //
    IEnumerator MoveDmgAndEndTurn(int i){
        if (i==1){
            yield return StartCoroutine(MoveDmg());    
        } else if (i==2){
            yield return StartCoroutine(MoveDmg());
        }
        PlayerEndTurn();
    }

    void PlayerEndTurn(){
            if (enemyUnit.CurrentHp != 0){
                nowState = BattleState.EnemyTurn;
                EnemyMove();
            } else {
                nowState = BattleState.Won;
                EndGame();
            }
    }

    IEnumerator MoveDmg(){
        yield return new WaitForSeconds(1);
        int moveDamageplayer = damageCalc(playerUnit, enemyUnit);
        yield return new WaitForSeconds(0.5f);
        int shouldHp = enemyUnit.CurrentHp - moveDamageplayer;
        if (shouldHp < 0){
            shouldHp = 0;
        }
        Debug.Log("shouldhp = " + shouldHp + "current hp =" + enemyUnit.CurrentHp);
        textDialog.text = "You Deal " + moveDamageplayer + " damage";
        while (enemyUnit.CurrentHp > shouldHp){
            Debug.Log("shouldhp = " + shouldHp + "current hp =" + enemyUnit.CurrentHp);
            yield return new WaitForSeconds(0.05f);
            enemyUnit.CurrentHp -= 1;
            enemyHUD.setHp(enemyUnit.CurrentHp);
        }
        yield return new WaitForSeconds(1);
    }
    
    public void OnSwitchButton(){
        StartCoroutine(SwitchButtonGO());
    }

    IEnumerator SwitchButtonGO(){
        if (!isSwitchPressed){
            isAttackButtonPressed = true;
            isSwitchPressed = true;
            Debug.Log("Switch Button");
            partylist.playerListDict();

            yield return new WaitForSeconds(0.5f);
            GameObject ParentCanvasPartyListPanel = Instantiate(PartyListPanel);
            Transform panelpartylist = ParentCanvasPartyListPanel.transform.Find("PartyPanel");

            //find pos1
            Button pos1Button = panelpartylist.transform.Find("pos1").GetComponent<Button>();
            TextMeshProUGUI pos1Text = pos1Button.transform.Find("pos1Text").GetComponent<TextMeshProUGUI>();
            GameObject pos1Object = partylist.List1();
            Unit pos1Unit = pos1Object.GetComponent<Unit>();
            pos1Text.text = pos1Unit.Nama;
            pos1Button.onClick.AddListener(Switch1);

            void Switch1(){
                textDialog.text = "You are already using this character";
            }

            //find pos2
            Button pos2Button = panelpartylist.transform.Find("pos2").GetComponent<Button>();
            TextMeshProUGUI pos2Text = pos2Button.transform.Find("pos2Text").GetComponent<TextMeshProUGUI>();
            GameObject pos2Object = partylist.List2();
            Unit pos2Unit = pos2Object.GetComponent<Unit>();
            pos2Text.text = pos2Unit.Nama;
            pos2Button.onClick.AddListener(Switch2);

            void Switch2(){
                if (!isPartyListPressed){
                    isPartyListPressed = true;     
                    Debug.Log("Switch 2 button");
                    Destroy(ParentCanvasPartyListPanel, 0.1f);
                    textDialog.text = "You are Using " + pos2Text.text;
                    StartCoroutine(SwitchAndEndTurn(2, pos2Object, pos2Unit));
                    pos1Unit.partyPosition = 2;
                    pos2Unit.partyPosition = 1;
                }
            }


            //find pos3
            Button pos3Button = panelpartylist.transform.Find("pos3").GetComponent<Button>();
            TextMeshProUGUI pos3Text = pos3Button.transform.Find("pos3Text").GetComponent<TextMeshProUGUI>();
            GameObject pos3Object = partylist.List3();
            Unit pos3Unit = pos3Object.GetComponent<Unit>();
            pos3Text.text = pos3Unit.Nama;
            pos3Button.onClick.AddListener(Switch3);

            void Switch3(){
                if (!isPartyListPressed){
                    isPartyListPressed = true;
                    Debug.Log("Switch 3 button");
                    Destroy(ParentCanvasPartyListPanel, 0.1f);
                    textDialog.text = "You are Using " + pos3Text.text;
                    StartCoroutine(SwitchAndEndTurn(3, pos3Object, pos3Unit));
                    pos1Unit.partyPosition = 3;
                    pos3Unit.partyPosition = 1;
                }
            }

            //find back
            Button backButtonSwitch = panelpartylist.transform.Find("Back").GetComponent<Button>();
            backButtonSwitch.onClick.AddListener(BackRemovePartyList);

            void BackRemovePartyList(){
                isPartyListPressed = true;
                Destroy(ParentCanvasPartyListPanel, 0.1f);
                isAttackButtonPressed = false;
                isPartyListPressed = false;
                isSwitchPressed = false;
            }

        }
    }
    
    IEnumerator SwitchAndEndTurn(int pos, GameObject posObject, Unit posUnit){
        yield return new WaitForSeconds(1);
        playerGo = Instantiate(posObject);
        playerUnit = posUnit;
        Debug.Log("Player spawned :" + playerGo);      
        playerHUD.setHUD(playerUnit);
        textDialog.text = "Player Spawned";
        yield return new WaitForSeconds(0.5f);
        PlayerEndTurn();
    }
    void EnemyMove(){
        textDialog.text = "Enemy Turn";
        int randomMove = Random.Range(1,4);
        Debug.Log(randomMove);
        StartCoroutine(EnemyMoveAndEndTurn(randomMove));
    }

    IEnumerator EnemyMoveAndEndTurn(int randomMove){
        yield return StartCoroutine(EnemyMove(randomMove));
        EnemyEndTurn();
    }

    IEnumerator EnemyMove(int randomMove){
        textDialog.text = "Enemy Use " + enemyUnit.moveName;
        yield return new WaitForSeconds(1);
        int moveDamageEnemy = damageCalc(enemyUnit, playerUnit);
        int shouldHp = playerUnit.CurrentHp - moveDamageEnemy;
        if (shouldHp < 0){
            shouldHp = 0;
        }
        textDialog.text = "Enemy Deal " + moveDamageEnemy + " damage";
        while (playerUnit.CurrentHp > shouldHp){
            yield return new WaitForSeconds(0.05f);
            playerUnit.CurrentHp -= 1;
            playerHUD.setHp(playerUnit.CurrentHp);
        }
        yield return new WaitForSeconds(1);
    }

    void EnemyEndTurn(){
        Debug.Log("enemy end turn");
        if (playerUnit.CurrentHp != 0){
                nowState = BattleState.PlayerTurn;
                StartCoroutine(PlayerMove());
            } else {
                nowState = BattleState.Lose;
                EndGame();
            }
    }

    int damageCalc(Unit attacker, Unit defender){
        bool dmgDouble = false;
        int outputDmg;
        
        if ((attacker.type == "strike" && defender.type == "pierce") || (attacker.type =="pierce" && defender.type == "slash") || (attacker.type == "slash" && defender.type == "strike")){
            dmgDouble = true;
            StartCoroutine(Effective());
        }

        outputDmg = attacker.Attack + attacker.movedmg - defender.Defense;
        Debug.Log("attack = " + (attacker.Attack + attacker.movedmg) + "def = " + defender.Defense);

        if (dmgDouble){
            outputDmg = outputDmg *2;
        }
        if (outputDmg < 0){
            outputDmg = 0;
        }
        Debug.Log("Output = " + outputDmg + "DoubleDmg = " + dmgDouble);
        return outputDmg;
    }

    IEnumerator Effective(){
        textDialog.text = "Super Effective";
        yield return new WaitForSeconds(0.2f);
    }


    void EndGame(){
        Debug.Log("End Game");

        if (nowState == BattleState.Won) {
            textDialog.text = "You Won";
            Debug.Log("Won");
        } else if (nowState == BattleState.Lose) {
            Debug.Log("Lose");
            textDialog.text = "You Lose";
        } else {
            Debug.Log("error");
        }
    }
}
