using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickObject : MonoBehaviour
{   
    PartyManager partyManager;
    public int SwitchedParty;

    void Start(){
        partyManager = FindObjectOfType<PartyManager>();
    }

    void OnMouseDown(){
        if (!partyManager.isCanvasSwitchOn) {
            partyManager.isCanvasSwitchOn = true;
            partyManager.SpawnSwitchPanelParty(SwitchedParty);
            Debug.Log("you click this collider = list" + SwitchedParty);
        }
    }
}
