﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LosowyRuch : Gracz
{
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
            int los = Random.Range(0, liczbaWolnych - 1);
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
    }
}
