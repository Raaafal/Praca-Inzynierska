using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogikaAreny : MonoBehaviour
{
    public class Statystyka
    {
        public (int wygrane, int iteracje) Pierwszy, Drugi;
        public int ruchy;
        /* wygrane prezentują łączną liczbę wygranych
         * iteracje prezentują sumę wszystkich iteracji w trakci wszystkich rozgrywek
         * ruchy prezentują sumę wszystkich ruchów dla obu graczy w trakcie wszystkich rozgrywek
         */
        public Statystyka()
        {
            Pierwszy = (0, 0);
            Drugi = (0, 0);
            ruchy = 0;
        }
        public int Wygrane
        {
            get
            {
                return Pierwszy.wygrane;
            }
            set
            {
                Pierwszy.wygrane = value;
            }
        }
        public int Przegrane
        {
            get
            {
                return Drugi.wygrane;
            }
            set
            {
                Drugi.wygrane = value;
            }
        }
        public int Rozegrane
        {
            get
            {
                return Pierwszy.wygrane+Drugi.wygrane;
            }
        }
        public int SrednioRuchow
        {
            get
            {
                if (Rozegrane == 0)
                {
                    return 0;
                }
                return ruchy / Rozegrane;
            }
        }
        public int SrodnioIteracjiPierwszego
        {
            get
            {
                if (Rozegrane == 0)
                {
                    return 0;
                }
                return Pierwszy.iteracje / Rozegrane;
            }
        }
        public int SrodnioIteracjiDrugiego
        {
            get
            {
                if (Rozegrane == 0)
                {
                    return 0;
                }
                return Drugi.iteracje / Rozegrane;
            }
        }
        public float WygraneProcent
        {
            get
            {
                return Wygrane / (float)Rozegrane;
            }
        }
        public float PrzegraneProcent
        {
            get
            {
                return Przegrane / (float)Rozegrane;
            }
        }
        public void RejestrujWynik(bool pierwszy)
        {
            this.Pierwszy.wygrane += Convert.ToInt32(pierwszy);
            Drugi.wygrane += Convert.ToInt32(!pierwszy);
        }
        public void RejestrujIteracje(bool pierwszy)
        {
            this.Pierwszy.iteracje += Convert.ToInt32(pierwszy);
            Drugi.iteracje += Convert.ToInt32(!pierwszy);
        }
    }
    protected int wielkoscPlanszy = 4;
    protected List<(int pierwszy,int drugi)> rozgrywki=new List<(int pierwszy, int drugi)>();
    protected int obecniGracze = 0;
    public (int pierwszy, int drugi) ObecniGracze {
        get
        {
            return rozgrywki[obecniGracze];
        }
    }
    protected Type[] uczestnicy = new Type[] {//lista wszytkich botów
        typeof(LosowyRuch),
        typeof(MonteCarlo),
        typeof(Przeszukiwanie)
    };
    public Statystyka[,] statystyki;
    protected LogikaPlanszy plansza;
    public LogikaAreny()
    {
        statystyki = new Statystyka[uczestnicy.Length,uczestnicy.Length];
        for (int i = 0; i < uczestnicy.Length; ++i)
        {
            for (int j = 0; j < uczestnicy.Length; ++j)
            {
                statystyki[i,j] = new Statystyka();
            }
        }
    }
    public void WszyscyDoGry() {
        rozgrywki.Clear();
        for (int uczestnik = 0; uczestnik < uczestnicy.Length; ++uczestnik)
        {
            for (int rywal = 0; rywal < uczestnicy.Length; ++rywal)
            {
                rozgrywki.Add((uczestnik, rywal));
            }
        }
    }
    public void NastepnaRozgrywka()
    {
        plansza = new LogikaPlanszy(wielkoscPlanszy,
            (Gracz)Activator.CreateInstance(uczestnicy[ObecniGracze.pierwszy]),
            (Gracz)Activator.CreateInstance(uczestnicy[ObecniGracze.drugi])
        );
        obecniGracze=(obecniGracze+1)%rozgrywki.Count;
    }
    private (int x, int y) ostatniRuch = (-1, -1);
    public bool KrokSymulacji()//jednej symulacji
    {

        var zwyciezca = plansza.Tura();
        var statystyka = statystyki[ObecniGracze.pierwszy, ObecniGracze.drugi];
        statystyka.RejestrujIteracje(plansza.Ruch);
        if (zwyciezca != null)
        {
            statystyka.RejestrujWynik(zwyciezca == plansza.Gracz1);
            ostatniRuch = (-1, -1);
            return false;
        }
        statystyka.ruchy += Convert.ToInt32(ostatniRuch != (x: plansza.OstatniRuch.Item1, y: plansza.OstatniRuch.Item2));
        ostatniRuch = (plansza.OstatniRuch.Item1, plansza.OstatniRuch.Item2);
        return true;
    }
    public bool KrokSymulacjiRozgrywek()//wszystkich symulacji
    {

        if (rozgrywki != null && rozgrywki.Count > 0)
        {
            try
            {
                if (!KrokSymulacji())//jeżeli symulacja zakończyła się
                {
                    NastepnaRozgrywka();
                    return true;
                }
            }catch(Exception e)
            {
                NastepnaRozgrywka();
            }
        }
        return false;
    }
    public void DoGry(int pierwszy, int drugi)
    {
        if (rozgrywki.IndexOf((pierwszy, drugi))<0)
        {
            rozgrywki.Add((pierwszy, drugi));
        }
    }
    public void ZGry(int pierwszy, int drugi)
    {
        int pozycja = rozgrywki.IndexOf((pierwszy, drugi));
        if (pozycja>=0)
        {
            rozgrywki[pozycja]=rozgrywki[rozgrywki.Count-1];
            rozgrywki.RemoveAt(rozgrywki.Count-1);
        } ;
    }
}
