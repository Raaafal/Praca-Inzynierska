using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class LosowyRuch : Gracz
{
    private RNGCryptoServiceProvider generator = new RNGCryptoServiceProvider();
    private System.Random random = new System.Random();
    public override (int x, int y) PlanujRuch(LogikaPlanszy plansza)
    {
        int[][] uklad = plansza.Plansza;
        if (CzyOstatniaIteracja())
        {

            int liczbaWolnych = 0;
            for(int i=0;i<uklad.Length;i++)
            {
                for (int j = 0; j < uklad[i].Length; j++)
                {
                    if (LogikaPlanszy.CzyWolne( uklad[i][j])) liczbaWolnych++;
                }
            }
            var losoweBajty = new byte[sizeof(int)];
            generator.GetNonZeroBytes(losoweBajty);
            int los = (Math.Abs(BitConverter.ToInt32(losoweBajty,0)))%liczbaWolnych;
            for (int i = 0; i < uklad.Length; i++)
            {
                for (int j = 0; j < uklad[i].Length; j++)
                {
                    if (LogikaPlanszy.CzyWolne(uklad[i][j]))
                    {
                        if (los == 0)
                        {
                            return (x: i, y: j);
                        }
                        los--;
                    }
                    
                }
            }
        } 
        return BrakRuchu;
    }

    public LosowyRuch() : base()
    {
        nazwa = "Losowy Ruch";
        grajZ = "Losowym Ruchem";
        Debug.Log(nazwa);
    }
}
