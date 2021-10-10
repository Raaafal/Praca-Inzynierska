using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ja : Gracz
{
    class Klik
    {
        public int x, y;
        public Klik(int x,int y)
        {
            this.x = x;
            this.y = y;
        }
    }
    Klik klikniecie;
    bool czyMojRuch = false;//zapobiega rejestrowaniu kliknięcia, gdy "Ja" nie ma ruchu
    public override void NasluchujKlikniec(int x,int y)
    {
        if(czyMojRuch)
        klikniecie = new Klik(x, y);
    }
    public override (int x, int y) PlanujRuch(LogikaPlanszy plansza)
    {
        czyMojRuch = klikniecie == null;
        var ruch =  Gracz.BrakRuchu;
        if (!czyMojRuch)
        {
            ruch = (klikniecie.x, klikniecie.y);
            zatwierdzRuch();
        }
        klikniecie = null;
        return ruch;
    }
    public Ja():base()
    {
        //var pg = new PreferencjeGracza();
        nazwa = "Ja";
        grajZ = "Sobą";
        czyNasluchujeKlikniec = true;
        bot = false;
        Ograniczenia.MaksCzasNaRuch = Single.PositiveInfinity;
    }
    override
    public void Zakoncz(LogikaPlanszy plansza)
    {

        Gracz wygrany = plansza.Ruch ? plansza.Gracz2 : plansza.Gracz1;
        Statystyki.ZapiszGre(plansza.Gracz2.GetType(), wygrany.GetType().Equals(plansza.Gracz1.GetType()));
    }
}
