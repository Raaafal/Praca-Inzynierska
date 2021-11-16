using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Statystyki
{
    public static void ZapiszGre(Type przeciwnik,bool wygrana)
    {
        PlayerPrefs.SetInt(Klucz(przeciwnik, true), 1+PlayerPrefs.GetInt(Klucz(przeciwnik, true), 0));
        if(wygrana)
            PlayerPrefs.SetInt(Klucz(przeciwnik, false), 1 + PlayerPrefs.GetInt(Klucz(przeciwnik, false), 0));

    }
    public static (int wygrane,int rozegrane) Wczytaj(Type przeciwnik)
    {
        return (wygrane: PlayerPrefs.GetInt(Klucz(przeciwnik, false), 0), rozegrane: PlayerPrefs.GetInt(Klucz(przeciwnik, true), 0));
    }
    static string Klucz(Type przeciwnik,bool wszystkieCzyWygrane)
    {
        return przeciwnik.Name + (wszystkieCzyWygrane ? "wszystkie_" : "wygrane_");
    }
    public static void Resetuj(Type przeciwnik)
    {
        PlayerPrefs.DeleteKey(Klucz(przeciwnik, false));
        PlayerPrefs.DeleteKey(Klucz(przeciwnik, true));

    }
}
