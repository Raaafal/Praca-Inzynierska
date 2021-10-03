using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MonteCarlo : Gracz
{
    float czasOdpowiedzi = 0.5f;

    float czasOdZapytania = 0f;

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

    public override (int x, int y) WykonajRuch(LogikaPlanszy plansza)
    {
        if (czasOdZapytania < czasOdpowiedzi )
        {
            czasOdZapytania += Time.deltaTime;
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
            foreach(var ruch in ocenyRuchów)
            {
                Debug.Log("symuluj");
                Debug.Log("testuj ruch" + ruch);
                Gracz ja= new LosowyRuch(0), rywal=new LosowyRuch(0);
                symulacja = new LogikaPlanszy(plansza,ja,rywal);
                symulacja.ZarejestrujRuch(ruch.ruch.x, ruch.ruch.y, null);
                ruch.Item2.Rejestruj(symulacja.Symuluj()!=ja);
                Debug.Log("po rejestracji: " + ruch.statystyki.wygrane + " " + ruch.statystyki.przegrane);

            }
            return BrakRuchu;
        }
        else
        {
            czasOdZapytania = 0f;
            (int x, int y) najleprszyRuch = ocenyRuchów[0].ruch;
            Statystyki najleszaOcena = ocenyRuchów[0].statystyki;
            foreach(var ruch in ocenyRuchów)
            {
                Debug.Log("odczytanie oceny: " + ruch.Item2.wygrane + " " + ruch.statystyki.przegrane);
                try
                {
                    Debug.Log("ocena: " + ruch.statystyki.Ocena + " => " + ruch.ruch);
                }
                catch (Exception e)
                {
                    Debug.Log("ocena: nieocenione => " + ruch.ruch);

                }
                if (najleszaOcena < ruch.statystyki)
                {
                    najleszaOcena = ruch.statystyki;
                    najleprszyRuch = ruch.ruch;
                }
            }
                    Debug.Log("najlepsza ocena: " + najleszaOcena + " => " + najleprszyRuch);
            ocenyRuchów = null;
            return najleprszyRuch;
        }
    }

    public MonteCarlo() : base()
    {
        nazwa = "Monte Carlo";
        grajZ = "Monte Carlo";
    }
}
