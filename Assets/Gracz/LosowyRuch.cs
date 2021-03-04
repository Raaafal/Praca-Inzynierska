using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LosowyRuch : Gracz
{
    float czasOdpowiedzi = 0.5f;

    float czasOdZapytania = 0f;
    public override System.Tuple<int, int> WykonajRuch(int[][] plansza)
    {
        if (czasOdZapytania > czasOdpowiedzi)
        {
            czasOdZapytania = 0f;

            int liczbaWolnych = 0;
            for(int i=0;i<plansza.Length;i++)
            {
                for (int j = 0; j < plansza[i].Length; j++)
                {
                    if (LogikaPlanszy.CzyWolne( plansza[i][j])) liczbaWolnych++;
                }
            }
            int los = Random.Range(0, liczbaWolnych - 1);
            for (int i = 0; i < plansza.Length; i++)
            {
                for (int j = 0; j < plansza[i].Length; j++)
                {
                    if (LogikaPlanszy.CzyWolne(plansza[i][j]))
                    {
                        if (los == 0)
                        {
                            return new System.Tuple<int, int>(i, j);
                        }
                        los--;
                    }
                    
                }
            }
        } 
        czasOdZapytania += Time.deltaTime;
        return BrakRuchu;
    }

    public LosowyRuch() : base()
    {
        nazwa = "Losowy Ruch";
        grajZ = "Losowym Ruchem";
    }
}
