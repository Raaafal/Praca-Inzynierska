using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Plansza : LogikaPlanszy
{
    [SerializeField]
    protected GameObject queen;

    [SerializeField]
    protected GameObject pole;

    protected const float ParzystyMnoznikKoloru = 0.5f;
    protected Color kolorPolaZablokowanego = new Color(1,0,0);

    protected GameObject[][] Pola = new GameObject[Wielkosc][];

    [SerializeField]
    RawImage pasek;

    protected delegate void Klikniecie(int x, int y);
    protected event Klikniecie EventKlikniecia;

    [SerializeField]
    static Color kolorKrolowejPierwszyRuch = new Color(1,1,1);
    [SerializeField]
    static Color kolorKrolowejDrugiRuch = new Color(0, 0, 0);

    PasekAktywnegoGracza pasekAktywnegoGracza;

    [SerializeField]
    Text tekstGracza1;
    [SerializeField]
    Text tekstGracza2;

    /*
    bool mojaKolej = true;
    protected bool MojaKolej
    {
        get { return mojaKolej; }
        set
        {
            var rect = pasek.uvRect; 
            if (value)
            {
                
                rect.x = 0;
                rect.width = 1;
            }else
            {
                rect.x = 1;
                rect.width =-1;
            }
            pasek.uvRect = rect;
            mojaKolej = value;
        }
    }*/

    protected override bool Ruch
    {
        get { return ruch; }
        set {
            ruch = value;
            /*
            var rect = pasek.uvRect;
            if (value)
            {

                rect.x = 0;
                rect.width = 1;
            }
            else
            {
                rect.x = 1;
                rect.width = -1;
            }
            pasek.uvRect = rect;*/
            if(pasekAktywnegoGracza!=null)
            pasekAktywnegoGracza.SygnalizujCzyjRuch(value);
        }
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        for (int i = 0; i < Wielkosc; i++)
        {
            Pola[i] = new GameObject[Wielkosc];
        }

        int wielkoscPola = 100;
        for(int i = 0; i < Wielkosc; i++)
        {
            for (int j = 0; j < Wielkosc; j++)
            {
                GameObject p = Instantiate(pole,transform);
                //p.transform.SetParent(transform);
                var tr=p.GetComponent<RectTransform>();
                var pos=tr.localPosition;
                pos.x = -wielkoscPola * Wielkosc / 2f+wielkoscPola/2f+i*wielkoscPola;
                pos.y= -wielkoscPola * Wielkosc / 2f + wielkoscPola / 2f + j *wielkoscPola;
                tr.localPosition = pos;


                /*if ((i + j) % 2 == 0) {
                    var ri = p.GetComponent<RawImage>();
                    ri.color *= PARZYSTY_MNOZNIK_KOLORU;
                }*/
                

                var script=p.AddComponent<Pole>();
                script.Init(ClickCallback, i,j);

                Pola[i][j] = p;

                KolorujPole(i, j, false);
            }
        }
        int szerPlanszy = Wielkosc * wielkoscPola;
        int szerWyswietlacza = Screen.width;
        var rtransform = GetComponent<RectTransform>();
        var skala = rtransform.localScale;
        skala.x *= (float)szerWyswietlacza / szerPlanszy;
        skala.y *= (float)szerWyswietlacza / szerPlanszy;
        rtransform.localScale = skala;


        Ruch = ruch;//synchronizacja

        base.Start();

        if (gracz1.czyNasluchujeKlikniec) DodajNasluchiwaczaKlikniec(gracz1.NasluchujKlikniec);
        if (gracz2.czyNasluchujeKlikniec) DodajNasluchiwaczaKlikniec(gracz2.NasluchujKlikniec);

        if (ruch)
        {
            gracz1.kolorKrolowej = kolorKrolowejPierwszyRuch;
            gracz2.kolorKrolowej = kolorKrolowejDrugiRuch;

            
        }else
        {
            gracz2.kolorKrolowej = kolorKrolowejPierwszyRuch;
            gracz1.kolorKrolowej = kolorKrolowejDrugiRuch;
        }
        pasekAktywnegoGracza = new PasekAktywnegoGracza(gracz1.nazwa, gracz2.nazwa, pasek,tekstGracza1,tekstGracza2);
    }
    /*
    protected void CallbackHandler(int x, int y, GameObject obj)
    {
        ClickCallback(x, y, obj);
    }*/
    protected virtual void ClickCallback(int x,int y,GameObject obj)
    {
        EventKlikniecia?.Invoke(x, y);
    }
    protected void KolorujPole(int x,int y,bool zablokowane)
    {
        if (zablokowane)
            Pola[x][y].GetComponent<RawImage>().color = kolorPolaZablokowanego;
        else Pola[x][y].GetComponent<RawImage>().color = pole.GetComponent<RawImage>().color;

        if ((x + y) % 2 == 0) Pola[x][y].GetComponent<RawImage>().color *= ParzystyMnoznikKoloru;
    }

    protected void PostawKrolowa(int x,int y,Color color)
    {
        var newQueen = Instantiate(queen, transform);
        newQueen.GetComponent<RectTransform>().position = Pola[x][y].GetComponent<RectTransform>().position;
        var ri=newQueen.GetComponent<RawImage>();
        ri.color = color;
    }
    public override void ZarejestrujRuch(int x, int y, Gracz gracz)
    {
        PostawKrolowa(x, y, gracz.kolorKrolowej);
        base.ZarejestrujRuch(x, y, gracz);
        OdswierzKolory();
    }
    void OdswierzKolory()
    {
        bool[][] zajete = MozliweRuchy();
        for (int i = 0; i < Wielkosc; i++)
        {
            for (int j = 0; j < Wielkosc; j++)
            {
                KolorujPole(i, j,zajete[i][j]);
            }

        }
    }
    void DodajNasluchiwaczaKlikniec(Action<int,int> klikniecie)
    {
        EventKlikniecia += (x,y)=> klikniecie(x,y);
    }

}
