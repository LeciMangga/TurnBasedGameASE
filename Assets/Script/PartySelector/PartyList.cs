using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyList : MonoBehaviour
{
    public PlayerList playerlist;
    private Dictionary<int, GameObject> partyPosLookup;


    public void playerListDict(){
        partyPosLookup = new Dictionary<int, GameObject>();
        AddToDictionary(playerlist.KentangUnit.partyPosition, playerlist.Kentang);
        AddToDictionary(playerlist.TomatUnit.partyPosition, playerlist.Tomat);
        AddToDictionary(playerlist.GarlicUnit.partyPosition, playerlist.Garlic);
        AddToDictionary(playerlist.CabaiUnit.partyPosition, playerlist.Cabai);
        AddToDictionary(playerlist.KembangKolUnit.partyPosition, playerlist.KembangKol);
    }

    private void AddToDictionary(int key, GameObject value)
    {
        if (!partyPosLookup.ContainsKey(key))
        {
            partyPosLookup.Add(key, value);
        }
    }


    public GameObject List1(){
        if (partyPosLookup.TryGetValue(1, out GameObject result)){
            return result;
        } else {
            return null;
        }
    }

    public GameObject List2(){
        if (partyPosLookup.TryGetValue(2, out GameObject result)){
            return result;
        } else {
            return null;
        }
    }

    public GameObject List3(){
        if (partyPosLookup.TryGetValue(3, out GameObject result)){
            return result;
        } else {
            return null;
        }
    }
}
