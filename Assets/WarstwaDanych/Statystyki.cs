using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Statystyki
{
    public static void ZapiszGre(Gracz przeciwnik,bool wygrana)
    {
        PlayerPrefs.SetInt(Klucz(przeciwnik, true), 1+PlayerPrefs.GetInt(Klucz(przeciwnik, true), 0));
        if(wygrana)
            PlayerPrefs.SetInt(Klucz(przeciwnik, false), 1 + PlayerPrefs.GetInt(Klucz(przeciwnik, false), 0));

    }
    public static Tuple<int,int> Wczytaj(Gracz przeciwnik)
    {
        return new Tuple<int, int>(PlayerPrefs.GetInt(Klucz(przeciwnik, false), 0), PlayerPrefs.GetInt(Klucz(przeciwnik, true), 0));
    }
    static string Klucz(Gracz przeciwnik,bool wszystkieCzyWygrane)
    {
        return przeciwnik.GetType().Name + (wszystkieCzyWygrane ? "wszystkie_" : "wygrane_");
    }
    public static void Resetuj(Gracz przeciwnik)
    {
        PlayerPrefs.DeleteKey(Klucz(przeciwnik, false));
        PlayerPrefs.DeleteKey(Klucz(przeciwnik, true));

    }
}
