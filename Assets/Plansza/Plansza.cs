using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Plansza : MonoBehaviour
{
    protected LogikaPlanszy logikaPlanszy;

   [SerializeField]
    protected GameObject queen;

    [SerializeField]
    protected GameObject pole;

    protected const float ParzystyMnoznikKoloru = 0.9f;
    protected Color kolorPolaZablokowanego = new Color(1f,0.3f,0.3f);

    protected GameObject[][] Pola;

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

    [SerializeField]
    GameObject komunikat;
    [SerializeField]
    Text tekstKomunikatu;

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

    public bool Ruch
    {
        get { return logikaPlanszy.Ruch; }
        set {
            logikaPlanszy.Ruch = value;
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
    protected int wielkosc => logikaPlanszy.Wielkosc;
    protected Gracz gracz1 => logikaPlanszy.Gracz1;
    protected Gracz gracz2 => logikaPlanszy.Gracz2;

    protected void Update()
    {
        logikaPlanszy.Tura();
    }

    // Start is called before the first frame update
    protected void Start()
    {
        logikaPlanszy=new LogikaPlanszy(
            Ustawienia.WielkoscPlanszy,
            new Ja(),
            (Gracz)Activator.CreateInstance(Ustawienia.Przeciwnik),
            Ustawienia.PierwszyRuch == Ustawienia.Ruch.Pierwszy
        );
        logikaPlanszy.DodajObserwatoraWykonanychRuchow( ZarejestrujRuch);
        logikaPlanszy.DodajObserwatoraKońcaGry( ()=> KoniecGry());

        Pola = new GameObject[wielkosc][];
        for (int i = 0; i < wielkosc; i++)
        {
            Pola[i] = new GameObject[wielkosc];
        }

        int wielkoscPola = 100;
        for(int i = 0; i < wielkosc; i++)
        {
            for (int j = 0; j < wielkosc; j++)
            {
                GameObject p = Instantiate(pole,transform);
                //p.transform.SetParent(transform);
                var tr=p.GetComponent<RectTransform>();
                var pos=tr.localPosition;
                pos.x = -wielkoscPola * wielkosc / 2f+wielkoscPola/2f+i*wielkoscPola;
                pos.y= -wielkoscPola * wielkosc / 2f + wielkoscPola / 2f + j *wielkoscPola;
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
        int szerPlanszy = wielkosc * wielkoscPola;
        int szerWyswietlacza = (int)transform.parent.GetComponent<RectTransform>().rect.width;// Screen.width;
        var rtransform = GetComponent<RectTransform>();
        var skala = rtransform.localScale;
        skala.x = (float)szerWyswietlacza / szerPlanszy;
        skala.y = (float)szerWyswietlacza / szerPlanszy;
        rtransform.localScale = skala;


        //Ruch = ruch;//synchronizacja

        

        if (gracz1.czyNasluchujeKlikniec) DodajNasluchiwaczaKlikniec(gracz1.NasluchujKlikniec);
        if (gracz2.czyNasluchujeKlikniec) DodajNasluchiwaczaKlikniec(gracz2.NasluchujKlikniec);

        if (Ruch)
        {
            gracz1.KolorKrolowej = kolorKrolowejPierwszyRuch;
            gracz2.KolorKrolowej = kolorKrolowejDrugiRuch;

            
        }else
        {
            gracz2.KolorKrolowej = kolorKrolowejPierwszyRuch;
            gracz1.KolorKrolowej = kolorKrolowejDrugiRuch;
        }
        pasekAktywnegoGracza = new PasekAktywnegoGracza(gracz1.nazwa, gracz2.nazwa, pasek,tekstGracza1,tekstGracza2);
        pasekAktywnegoGracza.SygnalizujCzyjRuch(Ruch);
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
        else Pola[x][y].GetComponent<RawImage>().color = new Color(0.7f,0.7f,0.7f);//pole.GetComponent<RawImage>().color;

        if ((x + y) % 2 == 0) Pola[x][y].GetComponent<RawImage>().color *= ParzystyMnoznikKoloru;
    }

    protected void PostawKrolowa(int x,int y,Color color)
    {
        var newQueen = Instantiate(queen, transform);
        newQueen.GetComponent<RectTransform>().position = Pola[x][y].GetComponent<RectTransform>().position;
        var ri=newQueen.GetComponent<RawImage>();
        ri.color = color;
    }
    public void ZarejestrujRuch(int x, int y, Gracz gracz)
    {
        PostawKrolowa(x, y, gracz.KolorKrolowej);
        OdswierzKolory();
        pasekAktywnegoGracza.SygnalizujCzyjRuch(!logikaPlanszy.Ruch);
    }
    void OdswierzKolory()
    {
        bool[][] zajete = logikaPlanszy.MozliweRuchy();
        for (int i = 0; i < wielkosc; i++)
        {
            for (int j = 0; j < wielkosc; j++)
            {
                KolorujPole(i, j,zajete[i][j]);
            }

        }
    }
    void DodajNasluchiwaczaKlikniec(Action<int,int> klikniecie)
    {
        EventKlikniecia += (x,y)=> klikniecie(x,y);
    }
    protected Gracz KoniecGry()
    {
        Gracz wygrany = Ruch ? gracz2 : gracz1;

        tekstKomunikatu.text = "Zwycięzca:\n" + wygrany.nazwa;

        StartCoroutine(NaKoniec());
        return wygrany;
    }
    IEnumerator NaKoniec()
    {
        yield return new WaitForSeconds(0.5f);
        komunikat.SetActive(true);
        yield return new WaitForSeconds(3f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }
}
