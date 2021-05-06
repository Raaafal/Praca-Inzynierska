using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogikaPlanszy : MonoBehaviour
{
    public LogikaPlanszy() { }
    public LogikaPlanszy(LogikaPlanszy plansza,Gracz g1,Gracz g2)
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
    [SerializeField]
    protected int wielkosc = 8;

    public const int PoleZajete = -1;

    protected int [][]plansza;
    public virtual int[][] Plansza
    {
        get { return plansza; }
    }

    protected bool gra = true;

    protected System.Tuple<int, int> ostatniRuch=Gracz.BrakRuchu;
    public virtual System.Tuple<int, int> OstatniRuch
    {
        get { return ostatniRuch; }
        set { ostatniRuch = value; }
    }
    
    protected Gracz gracz1;//śledzimy statystyki dla gracza1
    protected Gracz gracz2;
    protected bool ruch=true;
    protected virtual bool Ruch
    {
        get { return ruch; }
        set { ruch = value; }
    }

    protected virtual void Start()
    {
        //Ja ja = new Ja();
        //Ja ja2 = new Ja();
        if (gracz1 == null)
            gracz1 = new Ja();
        gracz1.Inicjalizuj();
        //gracz1.preferencjeGracza = new PreferencjeGracza();
        //gracz1.preferencjeGracza.czyPreferujePierwszyRuch = Ustawienia.PierwszyRuch == Ustawienia.Ruch.Pierwszy;
        //gracz1.preferencjeGracza.preferowanyRozmiarPlanszy = Ustawienia.WielkoscPlanszy;
        if (gracz2 == null)
            gracz2 = (Gracz)Activator.CreateInstance(Ustawienia.Przeciwnik);
        gracz2.Inicjalizuj();
        
        /*
        if (gracz1.preferencjeGracza != null)
        {
            Ruch = gracz1.preferencjeGracza.czyPreferujePierwszyRuch;
        }
        if (gracz2.preferencjeGracza != null)
        {
            Ruch = !gracz2.preferencjeGracza.czyPreferujePierwszyRuch;
        }*/
        Ruch= Ustawienia.PierwszyRuch == Ustawienia.Ruch.Pierwszy;
        wielkosc = Ustawienia.WielkoscPlanszy;


        plansza = new int[wielkosc][];
        //base.Start();
        for (int i = 0; i < wielkosc; i++)
        {
            plansza[i] = new int[wielkosc];
            /*
            for (int j = 0; j < wielkosc; j++)
            {
                plansza[i][j]=
            }*/
        }
    }
    void OdpytajGraczaORuch()
    {
        Gracz gracz = Ruch ? gracz1 : gracz2;
        Tuple<int,int> wykonanyRuch = gracz.WykonajRuch(this);
        if (wykonanyRuch != Gracz.BrakRuchu&&!MozliweRuchy()[wykonanyRuch.Item1][wykonanyRuch.Item2])
        {
            //PostawKrolowa(poleRuchu.Item1, poleRuchu.Item2, gracz.kolorKrolowej);
            ZarejestrujRuch(wykonanyRuch.Item1, wykonanyRuch.Item2,gracz);
            Ruch = !Ruch;
            ostatniRuch = wykonanyRuch;
            //OdswierzKolory();
        }
    }
    private void Update()
    {
        if (gra)
        {
            OdpytajGraczaORuch();

            if (SprawdzCzyKoniecGry())
            {
                KoniecGry();
            }
        }
    }
    protected virtual Gracz KoniecGry()
    {
        gra = false;
        gracz1.Zakoncz(this);
        gracz2.Zakoncz(this);
        Gracz wygrany = Ruch ? gracz2 : gracz1;

        //zapisujemy statystyki dla gracza1
        Statystyki.ZapiszGre(gracz2.GetType(), wygrany.GetType().Equals(gracz1.GetType()));
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
    protected bool[][] MozliweRuchy()
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
