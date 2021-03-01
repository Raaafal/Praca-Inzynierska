using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ustawienia
{
    public enum Ruch:int
    {
        Losowy,
        Pierwszy,
        Drugi
    }

    private static string ruchKlucz = "ruch";
    private static string wielkoscKlucz = "wielkosc";
    private static string przeciwnikKlucz = "przeciwnik";

    //wartości, gdy przeczytanie danych z pamięci się nie powiedzie
    private static int DomyslnaWielkosc = 8;
    private static string DomyslnyPrzeciwnik = "Ja";

    private static Ruch pierwszyRuch;
    private static int wielkoscPlanszy;
    private static Type przeciwnik;
    static public Ruch PierwszyRuch { get { return pierwszyRuch; }
        set { pierwszyRuch=value;
            PlayerPrefs.SetInt(ruchKlucz, (int)value);
        }
    }
    static public int WielkoscPlanszy { get { return wielkoscPlanszy; }
        set { wielkoscPlanszy = value;
            PlayerPrefs.SetInt(wielkoscKlucz, wielkoscPlanszy);
        }
    }
    static public Type Przeciwnik
    {
        get { return przeciwnik; }
        set
        {
            przeciwnik = value;
            PlayerPrefs.SetString(przeciwnikKlucz, przeciwnik.Name);
        }
    }
    static Ustawienia()
    {
        pierwszyRuch = (Ruch)PlayerPrefs.GetInt(ruchKlucz, (int)Ruch.Losowy);
        if(pierwszyRuch==Ruch.Losowy)
        {
            pierwszyRuch = WylosujRuch();
        }
        wielkoscPlanszy = PlayerPrefs.GetInt(wielkoscKlucz,DomyslnaWielkosc);

        //przeciwnik = (Gracz)Activator.CreateInstance(Type.GetType(PlayerPrefs.GetString(przeciwnikKlucz, DomyslnyPrzeciwnik)));
        przeciwnik = Type.GetType(PlayerPrefs.GetString(przeciwnikKlucz, DomyslnyPrzeciwnik));


        //string typ = przeciwnik.GetType().Name;
    }
    static Ruch WylosujRuch()
    {
        return UnityEngine.Random.value < 0.5f ? Ruch.Pierwszy : Ruch.Drugi;
    }
}
