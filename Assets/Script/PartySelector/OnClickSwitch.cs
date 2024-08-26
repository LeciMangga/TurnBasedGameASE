using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickSwitch : MonoBehaviour
{
    PartyManager partyManager;
    public string identifierPlayerClick;

    void Start(){
        partyManager = FindObjectOfType<PartyManager>();
    }

    void OnMouseDown(){
        Debug.Log("you switched to " + identifierPlayerClick);
        partyManager.SwitchParty(identifierPlayerClick);
    }
}
