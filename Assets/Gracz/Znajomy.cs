using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Znajomy : Gracz
{
    public string nazwa = "Znajomy";
    public string grajZ = "Znajomym";
    bool wyslanyRuch = false;
    override
    public int[] ObslugiwaneRozmiaryPlansz
    {
        get
        {
            if (Polaczenie.czySerwer)
            {
                Debug.Log("znajomy.serwer.obsługiwane plansze");
                return new Gracz().ObslugiwaneRozmiaryPlansz;
            }
            else if (preferencjeGracza == null)
            {
                Debug.Log("znajomy.klient.obsługiwane plansze - zapamiętaj");
                preferencjeGracza = Polaczenie.OdbierzUstawienia();
                if (preferencjeGracza == null)
                {
                    Debug.Log("znajomy.klient.obsługiwane plansze - zapamiętaj");
                    return null;
                }
                else
                {
                Debug.Log("znajomy.klient.obsługiwane plansze - zapamiętaj");
                    Ustawienia.PierwszyRuch = preferencjeGracza.czyPreferujePierwszyRuch ? Ustawienia.Ruch.Pierwszy : Ustawienia.Ruch.Drugi;
                }
            }
            Debug.Log("znajomy.klient.obsługiwane plansze");   
            return new int[] { preferencjeGracza.preferowanyRozmiarPlanszy };

        } 
    }
    override
    public void Inicjalizuj()
    {
        if (Polaczenie.czySerwer)
        {
            Debug.Log("znajomy.server.inicjuj");
            Polaczenie.WyślijUstawienia();
        }
    }
    override
    public Tuple<int, int> WykonajRuch(LogikaPlanszy plansza)
    {
        Debug.Log("Znajomy.WykonajRuch");
        if (!wyslanyRuch)
        {
            Debug.Log("Znajomy.WykonajRuch - wyślij");
            Polaczenie.WyslijRuch(plansza.OstatniRuch);
            wyslanyRuch = true;
        }
        var ruch = Polaczenie.OdbierzRuch();
        Debug.Log("Znajomy.WykonajRuch - odbierz: "+ruch.Item1+','+ruch.Item2);
        if (ruch.Item1!=Gracz.BrakRuchu.Item1 && ruch.Item2 != Gracz.BrakRuchu.Item2)
        {
            wyslanyRuch = false;
        }
        return ruch;
    }
    override
    public void Zakoncz(LogikaPlanszy plansza)
    {
        Debug.Log("Znajomy.WykonajRuch");
        if (!wyslanyRuch)
        {
            Debug.Log("Znajomy.WykonajRuch - wyślij");
            Polaczenie.WyslijRuch(plansza.OstatniRuch);
            wyslanyRuch = true;
        }
    }
}
