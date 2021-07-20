using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogikaPlanszy
{
    public LogikaPlanszy()
    {
        gracz1 = new Ja();
        gracz1.Inicjalizuj();
        gracz2 = (Gracz)Activator.CreateInstance(Ustawienia.Przeciwnik);
        gracz2.Inicjalizuj();
        Ruch = Ustawienia.PierwszyRuch == Ustawienia.Ruch.Pierwszy;

        wielkosc = Ustawienia.WielkoscPlanszy;
        plansza = new int[wielkosc][];
        for (int i = 0; i < wielkosc; i++)
        {
            plansza[i] = new int[wielkosc];
        }
    }
    public LogikaPlanszy(LogikaPlanszy plansza, Gracz g1, Gracz g2)
    {
        wielkosc = plansza.wielkosc;
        this.plansza = new int[wielkosc][];
        for (int i = 0; i < plansza.plansza.Length; ++i)
        {
            this.plansza[i] = (int[])plansza.plansza[i].Clone();
        }
        gracz1 = g1;
        gracz2 = g2;
    }
    public LogikaPlanszy(int wielkosc, Gracz g1, Gracz g2,bool pierwszy=true)
    {
        this.wielkosc = wielkosc;
        plansza = new int[wielkosc][];
        for (int i = 0; i < wielkosc; i++)
        {
            plansza[i] = new int[wielkosc];
        }
        gracz1 = g1;
        gracz1.Inicjalizuj();
        gracz2 = g2;
        gracz2.Inicjalizuj();
        ruch = pierwszy;
    }
    [SerializeField]
    protected int wielkosc = 8;
    public int Wielkosc => wielkosc;

    public const int PoleZajete = -1;

    protected int [][]plansza;
    public virtual int[][] Plansza
    {
        get { return plansza; }
    }

    protected bool gra = true;

    protected (int x, int y) ostatniRuch =Gracz.BrakRuchu;
    public virtual (int x, int y) OstatniRuch
    {
        get { return ostatniRuch; }
        set { ostatniRuch = value; }
    }
    
    protected Gracz gracz1;//śledzimy statystyki dla gracza1
    protected Gracz gracz2;
    public Gracz Gracz1
    {
        get { return gracz1; }
    }
    public Gracz Gracz2
    {
        get { return gracz2; }
    }
    protected bool ruch=true;
    public virtual bool Ruch
    {
        get { return ruch; }
        set { ruch = value; }
    }

    public delegate void DelegatWykonajRuch(int x, int y, Gracz gracz);
    event DelegatWykonajRuch EventWykonanyRuch;
    public delegate void DelegatKoniecGry();
    event DelegatKoniecGry EventKoniecGry;
    public void DodajObserwatoraWykonanychRuchow(DelegatWykonajRuch obserwator)
    {
        EventWykonanyRuch += obserwator;
    }
    public void DodajObserwatoraKońcaGry(DelegatKoniecGry obserwator)
    {
        EventKoniecGry += obserwator;
    }

    void OdpytajGraczaORuch()
    {
        Gracz gracz = Ruch ? gracz1 : gracz2;
        (int x, int y) wykonanyRuch = gracz.WykonajRuch(this);
        if (wykonanyRuch != Gracz.BrakRuchu&&!MozliweRuchy()[wykonanyRuch.x][wykonanyRuch.y])
        {
            //PostawKrolowa(poleRuchu.Item1, poleRuchu.Item2, gracz.kolorKrolowej);
            ZarejestrujRuch(wykonanyRuch.x, wykonanyRuch.y,gracz);
            EventWykonanyRuch?.Invoke(wykonanyRuch.x,wykonanyRuch.y,gracz);
            Ruch = !Ruch;
            ostatniRuch = wykonanyRuch;
            //OdswierzKolory();
        }
    }
    public virtual Gracz KoniecGry()
    {
        gra = false;
        gracz1.Zakoncz(this);
        gracz2.Zakoncz(this);
        EventKoniecGry?.Invoke();
        Gracz wygrany = Ruch ? gracz2 : gracz1;

        //zapisujemy statystyki dla gracza1
        //Statystyki.ZapiszGre(gracz2.GetType(), wygrany.GetType().Equals(gracz1.GetType()));
        return wygrany;
    }
    public Gracz Tura()
    {
        if (gra)
        {
            OdpytajGraczaORuch();

            if (SprawdzCzyKoniecGry())
            {
                return KoniecGry();
            }
        }
        return null;
    }
    public Gracz Tury(int tury)
    {
        for (int tura = 0; tura < tury; ++tura)
        {
            Gracz zwyciezca=Tura();
            if (zwyciezca != null)
                return zwyciezca;
        }
        return null;
    }
    public Gracz Symuluj()
    {
        Gracz zwyciezca = null;
        while ((zwyciezca = Tura()) == null) ;
        return zwyciezca;
    }
    /*
    protected override void ClickCallback(int x, int y, GameObject obj)
    {
        if (plansza[x][y] != PoleZajete&& plansza[x][y]%2==0)
        {
            PostawKrolowa(x, y, Color.white);

            MojaKolej = !MojaKolej;
            Ruch(x, y);
            OdswierzKolory();
        }
    }*/

    public virtual void ZarejestrujRuch(int x,int y,Gracz gracz)
    {
        
        for(int i = 0; i < wielkosc; i++)
        {
            for(int j = 0; j < wielkosc; j++)
            {
                if (plansza[i][j] != PoleZajete)
                {
                    int localX = x - i;
                    int localY = y - j;
                    if (Mathf.Abs( localX * localX * localY) == Mathf.Abs(localX * localY * localY))
                    {
                        plansza[i][j] += 1;
                    }

                }
            }
        }
        plansza[x][y] = PoleZajete;
    }
    /*
    void OdswierzKolory()
    {
        for(int i = 0; i < wielkosc; i++)
        {
            for (int j = 0; j < wielkosc; j++)
            {
                KolorujPole(i,j,plansza[i][j] == PoleZajete || plansza[i][j] % 2 == 1);
            }

        }
    }*/
    public bool[][] MozliweRuchy()
    {
        bool[][] zajete = new bool[wielkosc][];
        for (int i = 0; i < wielkosc; i++)
        {
            zajete[i] = new bool[wielkosc];
            for (int j = 0; j < wielkosc; j++)
            {
                zajete[i][j] = !CzyWolne(plansza[i][j]);// plansza[i][j] == PoleZajete || plansza[i][j] % 2 == 1;
            }
        }
        return zajete;
    }
    bool SprawdzCzyKoniecGry()
    {
        bool[][] zajete = MozliweRuchy();
        foreach(var tab in zajete)
        {
            foreach(var wartosc in tab)
            {
                if (!wartosc) return false;
            }
        }
        return true;
    }
    public static bool CzyWolne(int p)
    {
        return p != PoleZajete && p % 2 == 0;
    }
}