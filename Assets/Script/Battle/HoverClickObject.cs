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
            gameManager.SpawnPanelMoveset(identifierPlayerInt);
        } else if (AllowHoverClickEnemy && (!isPlayer)){
            gameManager.DamageCalc(identifierEnemyInt, PlayerUnitAttackIdentifier, AttackType, "Player");
        }
    }

}
