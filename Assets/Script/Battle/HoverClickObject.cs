using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverClickObject : MonoBehaviour
{
    public int identifierPlayerInt;
    public int identifierEnemyInt;
    public int PlayerUnitAttackIdentifier;
    public string AttackType;
    public bool isPlayer;
    public bool AllowHoverClickPlayer;
    public bool AllowHoverClickEnemy;
    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void OnMouseDown(){
        if (AllowHoverClickPlayer && isPlayer){
            Debug.Log("Player Identifier number =" + identifierPlayerInt);
            gameManager.SpawnPanelMoveset(identifierPlayerInt);
        } else if (AllowHoverClickEnemy && (!isPlayer)){
            Debug.Log("Enemy Identifier number =" + identifierEnemyInt);
            gameManager.DamageCalc(identifierEnemyInt, PlayerUnitAttackIdentifier, AttackType, "Player");
        }
    }

}
