using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogikaPlanszy : MonoBehaviour
{
    [SerializeField]
    protected const int Wielkosc = 8;

    protected const int POLE_ZAJETE = -1;

    protected int [][]plansza=new int[Wielkosc][];

    protected Gracz gracz1;
    protected Gracz gracz2;
    protected bool ruch=true;
    protected virtual bool Ruch
    {
        get { return ruch; }
        set { ruch = value; }
    }
    protected virtual void Start()
    {
        Ja ja = new Ja();
        Ja ja2 = new Ja();

        gracz1 = ja;
        gracz2 = ja2;

        if (gracz1.preferencjeGracza != null)
        {
            Ruch = gracz1.preferencjeGracza.czyPreferujePierwszyRuch;
        }
        if (gracz2.preferencjeGracza != null)
        {
            Ruch = !gracz2.preferencjeGracza.czyPreferujePierwszyRuch;
        }


        //base.Start();
        for(int i = 0; i < Wielkosc; i++)
        {
            plansza[i] = new int[Wielkosc];
            /*
            for (int j = 0; j < Wielkosc; j++)
            {
                plansza[i][j]=
            }*/
        }
    }
    void OdpytajGraczaORuch()
    {
        Gracz gracz = Ruch ? gracz1 : gracz2;
        var poleRuchu = gracz.WykonajRuch(plansza);
        if (poleRuchu != Gracz.BrakRuchu)
        {
            //PostawKrolowa(poleRuchu.Item1, poleRuchu.Item2, gracz.kolorKrolowej);
            ZarejestrujRuch(poleRuchu.Item1, poleRuchu.Item2,gracz);
            Ruch = !Ruch;
            //OdswierzKolory();
        }
    }
    private void Update()
    {
        OdpytajGraczaORuch();
    }
    /*
    protected override void ClickCallback(int x, int y, GameObject obj)
    {
        if (plansza[x][y] != POLE_ZAJETE&& plansza[x][y]%2==0)
        {
            PostawKrolowa(x, y, Color.white);

            MojaKolej = !MojaKolej;
            Ruch(x, y);
            OdswierzKolory();
        }
    }*/

    public virtual void ZarejestrujRuch(int x,int y,Gracz gracz)
    {
        
        for(int i = 0; i < Wielkosc; i++)
        {
            for(int j = 0; j < Wielkosc; j++)
            {
                if (plansza[i][j] != POLE_ZAJETE)
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
        plansza[x][y] = POLE_ZAJETE;
    }
    /*
    void OdswierzKolory()
    {
        for(int i = 0; i < Wielkosc; i++)
        {
            for (int j = 0; j < Wielkosc; j++)
            {
                KolorujPole(i,j,plansza[i][j] == POLE_ZAJETE || plansza[i][j] % 2 == 1);
            }

        }
    }*/
    protected bool[][] MozliweRuchy()
    {
        bool[][] zajete = new bool[Wielkosc][];
        for (int i = 0; i < Wielkosc; i++)
        {
            zajete[i] = new bool[Wielkosc];
            for (int j = 0; j < Wielkosc; j++)
            {
                zajete[i][j] = plansza[i][j] == POLE_ZAJETE || plansza[i][j] % 2 == 1;
            }
        }
        return zajete;
    }
}
