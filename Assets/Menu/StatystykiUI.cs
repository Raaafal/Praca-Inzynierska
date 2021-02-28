using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatystykiUI : MonoBehaviour
{
    [SerializeField]
    Text tekst;

    [SerializeField]
    WyborPrzeciwnika wybor;

    Gracz aktualnyPrzeciwnik;

    // Start is called before the first frame update
    void Start()
    {
        //yield return new WaitForEndOfFrame();
        WyswietlStatystyki();
    }
    public void WyswietlStatystyki()
    {
        Type przeciwnik = wybor.KtoryPrzeciwnik();
        aktualnyPrzeciwnik = (Gracz)Activator.CreateInstance(wybor.KtoryPrzeciwnik());
        var statystyki = Statystyki.Wczytaj(aktualnyPrzeciwnik);
        if(przeciwnik.Equals(typeof(Ja)))
            tekst.text = statystyki.Item2 + " ▄▀";
        else tekst.text = statystyki.Item1 + "♔/" + statystyki.Item2 + " ▄▀";
    }
    public void Resetuj()
    {
        Statystyki.Resetuj(aktualnyPrzeciwnik);
        WyswietlStatystyki();
    }
}
