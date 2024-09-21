using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;

public class Unit : MonoBehaviour
{
    public int MaxHp;
    public int CurrentHp;
    public int Attack;
    public int Defense;
    public string Nama;
    public int Level;
    public int exp;
    public string moveName;
    public int movedmg;
    public int partyPosition;
    public string type;

    public struct StatusEffect
    {
        public bool isStunned;
        public int StunnedTurn;
        public bool isShield;
    }
    public StatusEffect Effect;
    public struct Stats
    {
        public int Attack;
        public int Defense;
        public int MaxHp;
        public int TurnEndBuffNerf;
    }
    public Stats[] Modified = new Stats[50];
    public Stats Total;
}