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
    public override Tuple<int, int> WykonajRuch(int[][] plansza)
    {
        czyMojRuch = klikniecie == null;
        var ret= klikniecie==null?Gracz.BrakRuchu:new Tuple<int, int>(klikniecie.x,klikniecie.y);
        klikniecie = null;
        return ret;
    }
    public Ja():base()
    {
        //var pg = new PreferencjeGracza();
        nazwa = "Ja";
        grajZ = "Sobą";
        czyNasluchujeKlikniec = true;

    }
}
