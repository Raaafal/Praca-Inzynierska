﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Znajomy : Gracz
{
    public Znajomy() {
        nazwa = "Znajomy";
        grajZ = "Znajomym";
        bot = false;
    }
    bool wyslanyRuch = false;
    bool wyslaneUstawienia = false;
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
            wyslaneUstawienia=Polaczenie.WyślijUstawienia();
        }
    }
    override
    public (int x, int y) WykonajRuch(LogikaPlanszy plansza)
    {
        if (!wyslaneUstawienia)
        {
            wyslaneUstawienia = Polaczenie.WyślijUstawienia();
            if (!wyslaneUstawienia)
            {
                return Gracz.BrakRuchu;
            }
        }
        Debug.Log("Znajomy.WykonajRuch");
        if (!wyslanyRuch)
        {
            Debug.Log("Znajomy.WykonajRuch - wyślij");
            Polaczenie.WyslijRuch(plansza.OstatniRuch);
            wyslanyRuch = true;
        }
        var ruch = Polaczenie.OdbierzRuch();
        Debug.Log("Znajomy.WykonajRuch - odbierz: "+ruch.x+','+ruch.y);
        if (ruch.x!=Gracz.BrakRuchu.x && ruch.y != Gracz.BrakRuchu.y)
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
