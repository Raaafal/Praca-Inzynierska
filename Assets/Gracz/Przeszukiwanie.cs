using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Przeszukiwanie : Gracz
{
    public Przeszukiwanie(float czas) : this()
    {
        czasOdpowiedzi = czas;
    }
    public Przeszukiwanie() : base()
    {
        nazwa = "Przeszukiwanie";
        grajZ = "Przeszukiwaniem";
    }

    float czasOdpowiedzi = 0.5f;

    float czasOdZapytania = 0f;

    float czasDzialaniaNaKlatke = 0.1f;

    ((int x, int y),float jakosc) aktualnieNajlepszyRuch = (BrakRuchu,0f);
    int glebokoscAnalizyNajlepszegoRuchu = 0;
    int glebokoscPrzeszukiwania = 1;
    public override (int x, int y) WykonajRuch(int[][] plansza)
    {
        if (aktualnieNajlepszyRuch.Item1==BrakRuchu)
        {
            for (int i = 0; i < plansza.Length; i++)
            {
                for (int j = 0; j < plansza[i].Length; j++)
                {
                    if (LogikaPlanszy.CzyWolne(plansza[i][j]))
                    {
                        aktualnieNajlepszyRuch = ((i, j), 0f);
                        i = plansza.Length;
                        break;
                    }
                }
            }
        }
        if (czasOdZapytania < czasOdpowiedzi)
        {
            
        }
        if (czasOdZapytania >= czasOdpowiedzi)
        {
            var ret = aktualnieNajlepszyRuch.Item1;
            aktualnieNajlepszyRuch = (BrakRuchu, 0f);
            czasOdZapytania = 0f;
            glebokoscPrzeszukiwania = 1;
            glebokoscAnalizyNajlepszegoRuchu = 0;
            return ret;
        }
        //czasOdZapytania += Time.deltaTime;
        return BrakRuchu;
    }
    void Przeszukaj(int[][] plansza, out ((int x, int y), float jakosc) najlepszyRuchWGalezi,ref float czasSumaryczny,out float czas,float limitCzasu,bool ruchAlgorytmu)
    {
        najlepszyRuchWGalezi = (BrakRuchu, 0f);
        if (czasSumaryczny >= limitCzasu)
        {
            najlepszyRuchWGalezi = (BrakRuchu, 0.5f);
            czas = 0f;
            return;
        }
        float czasStart = Time.timeSinceLevelLoad;
        float sumaCzasowDzieci = 0f;
        int[][] nowaPlansza = new int[plansza.Length][];
        for(int i = 0; i < nowaPlansza.Length; i++)
        {
            nowaPlansza[i] = new int[plansza[i].Length];
        }

        bool istniejeMozliwoscRuchu = false;
        for (int i = 0; i < plansza.Length; i++)
        {
            for (int j = 0; j < plansza[i].Length; j++)
            {
                if (LogikaPlanszy.CzyWolne(plansza[i][j]))
                {
                    istniejeMozliwoscRuchu = true;
                    for (int k = 0; k < plansza.Length; k++)
                    {
                        for (int m = 0; m < plansza[k].Length; m++)
                        {
                            nowaPlansza[k][m] = plansza[k][m];
                        }
                    }
                    LogikaPlanszy.ZarejestrujRuch(i, j, nowaPlansza);
                    ////////////////////rekurencja
                    ((int x, int y), float jakosc) najlepszyRuchR;
                    float czasR;
                    Przeszukaj(nowaPlansza, out najlepszyRuchR,ref czasSumaryczny, out czasR, limitCzasu, !ruchAlgorytmu);
                    if (ruchAlgorytmu&&najlepszyRuchWGalezi.jakosc < najlepszyRuchR.jakosc)
                    {
                        najlepszyRuchWGalezi = ((i,j),najlepszyRuchR.jakosc);//TODO probabilistyczna średnia ruchów, zakładamy, że przeciwnik porusza się losowo, a my optymalnie
                    } else if (!ruchAlgorytmu&&najlepszyRuchWGalezi.jakosc > najlepszyRuchR.jakosc)
                    {
                        najlepszyRuchWGalezi = ((i, j), najlepszyRuchR.jakosc);//minimax
                    }
                    sumaCzasowDzieci += czasR;

                }
            }
        }
        if(!istniejeMozliwoscRuchu)
        {
            if(ruchAlgorytmu) najlepszyRuchWGalezi = (BrakRuchu, 0f);
            else najlepszyRuchWGalezi = (BrakRuchu, 1f);
        } //else najlepszyRuchWGalezi= (BrakRuchu, 1f);

        czas = Time.timeSinceLevelLoad - czasStart;
        czasSumaryczny += czas - sumaCzasowDzieci;
    }

}
