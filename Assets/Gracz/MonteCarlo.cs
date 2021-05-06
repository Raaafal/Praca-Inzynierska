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
    List<Tuple<Tuple<int, int>, Statystyki>> ocenyRuchów;

    public override System.Tuple<int, int> WykonajRuch(LogikaPlanszy plansza)
    {
        if (czasOdZapytania < czasOdpowiedzi )
        {
            czasOdZapytania += Time.deltaTime;
            LogikaPlanszy symulacja;
            Debug.Log("ruch monte carlo");
            if (ocenyRuchów == null)
            {
                Debug.Log("ładuj ruchy");
                ocenyRuchów = new List<Tuple<Tuple<int, int>, Statystyki>>();
                for (int i = 0; i < plansza.Plansza.Length; ++i)
                    for (int j = 0; j < plansza.Plansza[i].Length; ++j)
                        if (LogikaPlanszy.CzyWolne(plansza.Plansza[i][j]))
                            ocenyRuchów.Add(new Tuple<Tuple<int, int>, Statystyki>(new Tuple<int, int>(i, j), new Statystyki(0, 0)));
                Debug.Log("brak ruchów");
            }
            foreach(var ruch in ocenyRuchów)
            {
                Debug.Log("symuluj");
                Debug.Log("testuj ruch" + ruch);
                Gracz ja= new LosowyRuch(), rywal=new LosowyRuch();
                symulacja = new LogikaPlanszy(plansza,ja,rywal);
                symulacja.ZarejestrujRuch(ruch.Item1.Item1, ruch.Item1.Item2, null);
                ruch.Item2.Rejestruj(symulacja.Symuluj()!=ja);
                Debug.Log("po rejestracji: " + ruch.Item2.wygrane + " " + ruch.Item2.przegrane);

            }
            return BrakRuchu;
        }
        else
        {
            czasOdZapytania = 0f;
            Tuple<int, int> najleprszyRuch=BrakRuchu;
            Statystyki najleszaOcena = new Statystyki(0, 1);
            foreach(var ruch in ocenyRuchów)
            {
                Debug.Log("odczytanie oceny: " + ruch.Item2.wygrane + " " + ruch.Item2.przegrane);
                try
                {
                    Debug.Log("ocena: " + ruch.Item2.Ocena + " => " + ruch.Item1);
                }
                catch (Exception e)
                {
                    Debug.Log("ocena: nieocenione => " + ruch.Item1);

                }
                if (najleszaOcena < ruch.Item2)
                {
                    najleszaOcena = ruch.Item2;
                    najleprszyRuch = ruch.Item1;
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
