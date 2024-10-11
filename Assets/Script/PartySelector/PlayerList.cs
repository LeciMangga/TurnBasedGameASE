using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerList : MonoBehaviour
{
    public GameObject Kentang;
    public GameObject Tomat;
    public GameObject Garlic;
    public GameObject Cabai;
    public GameObject KembangKol;

    public Unit KentangUnit { get; private set; }
    public Unit TomatUnit { get; private set; }
    public Unit GarlicUnit { get; private set; }
    public Unit CabaiUnit { get; private set; }
    public Unit KembangKolUnit { get; private set; }

    void Awake()
    {
        KentangUnit = Kentang.GetComponent<Unit>();
        TomatUnit = Tomat.GetComponent<Unit>();
        GarlicUnit = Garlic.GetComponent<Unit>();
        CabaiUnit = Cabai.GetComponent<Unit>();
        KembangKolUnit = KembangKol.GetComponent<Unit>();
    }

    public void SetPlayerPos(){
        string pos1 = PlayerPrefs.GetString("Pos1", null);
        string pos2 = PlayerPrefs.GetString("Pos2", null);
        string pos3 = PlayerPrefs.GetString("Pos3", null);
        if (pos1 == KentangUnit.Nama){
            KentangUnit.partyPosition = 1;
        } else if (pos1 == TomatUnit.Nama){
            TomatUnit.partyPosition = 1;
        } else if (pos1 == GarlicUnit.Nama){
            GarlicUnit.partyPosition = 1;
        } else if (pos1 == CabaiUnit.Nama){
            CabaiUnit.partyPosition = 1;
        } else if (pos1 == KembangKolUnit.Nama){
            KembangKolUnit.partyPosition = 1;
        }

        if (pos2 == KentangUnit.Nama){
            KentangUnit.partyPosition = 2;
        } else if (pos2 == TomatUnit.Nama){
            TomatUnit.partyPosition = 2;
        } else if (pos2 == GarlicUnit.Nama){
            GarlicUnit.partyPosition = 2;
        } else if (pos2 == CabaiUnit.Nama){
            CabaiUnit.partyPosition = 2;
        } else if (pos2 == KembangKolUnit.Nama){
            KembangKolUnit.partyPosition = 2;
        }

        if (pos3 == KentangUnit.Nama){
            KentangUnit.partyPosition = 3;
        } else if (pos3 == TomatUnit.Nama){
            TomatUnit.partyPosition = 3;
        } else if (pos3 == GarlicUnit.Nama){
            GarlicUnit.partyPosition = 3;
        } else if (pos3 == CabaiUnit.Nama){
            CabaiUnit.partyPosition = 3;
        } else if (pos3 == KembangKolUnit.Nama){
            KembangKolUnit.partyPosition = 3;
        }
    }
}
