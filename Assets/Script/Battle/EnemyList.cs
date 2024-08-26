using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyList : MonoBehaviour
{
    public GameObject Sendok;
    public GameObject Garpu;
    public GameObject Pisau;
    public GameObject Peeler;
    public GameObject Fryer;

    public Unit SendokUnit { get; private set; }
    public Unit GarpuUnit { get; private set; }
    public Unit PisauUnit { get; private set; }
    public Unit PeelerUnit { get; private set; }
    public Unit FryerUnit { get; private set; }

    private int LevelNow;

    void Awake()
    {
        SendokUnit = Sendok.GetComponent<Unit>();
        GarpuUnit = Garpu.GetComponent<Unit>();
        PisauUnit = Pisau.GetComponent<Unit>();
        PeelerUnit = Peeler.GetComponent<Unit>();
        FryerUnit = Fryer.GetComponent<Unit>();
    }

    void Start(){
        LevelNow = PlayerPrefs.GetInt("onLevel");
        Debug.Log("Now is Level " + LevelNow);
    }

    public GameObject List1(){
        switch (LevelNow)
        {
            case 1:
                return Sendok;
            case 2:
                return Sendok;
            case 3:
                return Sendok;
            case 4:
                return Sendok;
            case 5:
                return Pisau;
            default:
                return null;
        }
    }

    public GameObject List2(){
        switch (LevelNow)
        {
            case 1:
                return Sendok;
            case 2:
                return Garpu;
            case 3:
                return Garpu;
            case 4:
                return Garpu;
            case 5:
                return Peeler;
            default:
                return null;
        }
    }

    public GameObject List3(){
        switch (LevelNow)
        {
            case 1:
                return Sendok;
            case 2:
                return Garpu;
            case 3:
                return Pisau;
            case 4:
                return Pisau;
            case 5:
                return Fryer;
            default:
                return null;
        }
    }
}
