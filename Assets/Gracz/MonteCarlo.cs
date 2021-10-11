using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

public class MonteCarlo : Gracz
{

    class Statystyki
    {
        public int wygrane, przegrane;
        public float Ocena
        {
            get { return (float)wygrane / (wygrane + przegrane); }
        }
        public Statystyki(int wygrane,int przegrane)
        {
            this.wygrane = wygrane;
            this.przegrane = przegrane;
        }
        public void Rejestruj(bool wynik)
        {
            wygrane += wynik ? 1 : 0;
            przegrane += wynik ? 0 : 1;
            Debug.Log("w:" + wygrane + "\np:" + przegrane); Debug.Log("testuj ruch " + Ocena);
        }

        static
        public bool operator <(Statystyki a,Statystyki b)
        {
            return a.przegrane * b.wygrane > b.przegrane * a.wygrane;

        }

        static
        public bool operator >(Statystyki a,Statystyki b)
        {

            return a.przegrane * b.przegrane  < b.wygrane * a.wygrane;
        }
    }
    List<((int x, int y) ruch, Statystyki statystyki)> ocenyRuchów;

    public override (int x, int y) PlanujRuch(LogikaPlanszy plansza)
    {
        LogikaPlanszy symulacja;
        Debug.Log("ruch monte carlo");
        if (ocenyRuchów == null)
        {
            Debug.Log("ładuj ruchy");
            ocenyRuchów = new List<((int x, int y) ruch, Statystyki statystyki)>();
            for (int i = 0; i < plansza.Plansza.Length; ++i)
                for (int j = 0; j < plansza.Plansza[i].Length; ++j)
                    if (LogikaPlanszy.CzyWolne(plansza.Plansza[i][j]))
                        ocenyRuchów.Add((ruch: (x: i, y: j), statystyki: new Statystyki(0, 0)));
            Debug.Log("brak ruchów");
        }
        var najlebszyRuch = ocenyRuchów[0];
        foreach (var ruch in ocenyRuchów)
        {
            Debug.Log("symuluj");
            Debug.Log("testuj ruch" + ruch);
            Gracz ja = new LosowyRuch(), rywal = new LosowyRuch();
            ja.Ograniczenia.MaksCzasNaRuch = 0;
            rywal.Ograniczenia.MaksCzasNaRuch = 0;
            symulacja = new LogikaPlanszy(plansza, ja, rywal);
            symulacja.ZarejestrujRuch(ruch.ruch.x, ruch.ruch.y);
            ruch.statystyki.Rejestruj(symulacja.Symuluj() != ja);
            Debug.Log("po rejestracji: " + ruch.statystyki.wygrane + " " + ruch.statystyki.przegrane);
            if (najlebszyRuch.statystyki.Ocena < ruch.statystyki.Ocena)
            {
                najlebszyRuch = ruch;
            }

        }
        if (CzyOstatniaIteracja())
        {
            ocenyRuchów = null;
        }
        return najlebszyRuch.ruch;
    }

    public MonteCarlo() : base()
    {
        nazwa = "Monte Carlo";
        grajZ = "Monte Carlo";
    }
}
