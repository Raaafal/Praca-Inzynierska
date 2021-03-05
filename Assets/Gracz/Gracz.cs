using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gracz
{
    public PreferencjeGracza preferencjeGracza;
    public virtual int[] ObslugiwaneRozmiaryPlansz { get; }= new int[] { 3, 4, 5, 6,8 ,10 };
    public Color KolorKrolowej { get; set; } = new Color(1, 1, 1, 1);
    public static System.Tuple<int, int> BrakRuchu { get; } = new System.Tuple<int, int>(-1, -1);
    public virtual System.Tuple<int,int> WykonajRuch(int [][]plansza)
    {
        return BrakRuchu;
    }
    public bool czyNasluchujeKlikniec = false;
    public virtual void NasluchujKlikniec(int x, int y)
    {
    }
    public string nazwa="Przeciwnik";
    public string grajZ = "Przeciwnikiem";
}
