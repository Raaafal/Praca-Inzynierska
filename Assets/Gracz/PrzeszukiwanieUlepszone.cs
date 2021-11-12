using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrzeszukiwanieUlepszone : Przeszukiwanie
{
    public PrzeszukiwanieUlepszone() : base()
    {
        nazwa = "Minimax+";
        grajZ = "Minimaxem+";
    }
    protected override void Przeszukaj(int[][] plansza, out ((int x, int y), float jakosc) najlepszyRuchWGalezi, ref long czasSumaryczny, out long czas, long limitCzasu, bool ruchAlgorytmu, int wysokoscDrzewa, ref bool zabrakloCzasu)
    {
        najlepszyRuchWGalezi = ruchAlgorytmu ? (BrakRuchu, 0f) : (BrakRuchu, 1f);
        if (czasSumaryczny >= limitCzasu || wysokoscDrzewa <= 0)
        {
            if (czasSumaryczny >= limitCzasu)
            {
                zabrakloCzasu = true;
            }
            najlepszyRuchWGalezi = (BrakRuchu, 0.5f);
            czas = 0;
            return;
        }
        long czasStart = System.DateTime.Now.Ticks;
        long sumaCzasowDzieci = 0;
        int[][] nowaPlansza = new int[plansza.Length][];
        for (int i = 0; i < nowaPlansza.Length; i++)
        {
            nowaPlansza[i] = new int[plansza[i].Length];
        }

        float sredniaOcena = 0f;
        int liczbaOcen = 0;

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
                    long czasR;
                    Przeszukaj(nowaPlansza, out najlepszyRuchR, ref czasSumaryczny, out czasR, limitCzasu, !ruchAlgorytmu, wysokoscDrzewa - 1, ref zabrakloCzasu);
                    if (ruchAlgorytmu && najlepszyRuchWGalezi.jakosc <= najlepszyRuchR.jakosc)
                    {
                        najlepszyRuchWGalezi = ((i, j), najlepszyRuchR.jakosc);//TODO probabilistyczna średnia ruchów, zakładamy, że przeciwnik porusza się losowo, a my optymalnie
                    }
                    else if (!ruchAlgorytmu && najlepszyRuchWGalezi.jakosc >= najlepszyRuchR.jakosc)
                    {
                        najlepszyRuchWGalezi = ((i, j), najlepszyRuchR.jakosc);//minimax
                    }
                    sredniaOcena = (sredniaOcena * liczbaOcen + najlepszyRuchR.jakosc) / (liczbaOcen + 1);
                    liczbaOcen++;

                    sumaCzasowDzieci += czasR;

                }
            }
        }
        if (!istniejeMozliwoscRuchu)
        {
            if (ruchAlgorytmu) najlepszyRuchWGalezi = (BrakRuchu, 0f);
            else najlepszyRuchWGalezi = (BrakRuchu, 1f);
        } else if(!ruchAlgorytmu)
        {
            const float PrawdopodobienstwoLosowegoRuchuPrzeciwnika = 0.1f;
            najlepszyRuchWGalezi.jakosc = najlepszyRuchWGalezi.jakosc * (1f-PrawdopodobienstwoLosowegoRuchuPrzeciwnika) + sredniaOcena * PrawdopodobienstwoLosowegoRuchuPrzeciwnika;
        }

        czas = System.DateTime.Now.Ticks - czasStart;
        czasSumaryczny += czas - sumaCzasowDzieci;
    }
}
