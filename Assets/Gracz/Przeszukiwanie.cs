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
        nazwa = "Minimax";
        grajZ = "Minimaxem";
    }

    protected float czasOdpowiedzi = 0.5f;//w sekundach

    protected float czasOdZapytania = 0f;

    protected ((int x, int y) ruch,float jakosc) aktualnieNajlepszyRuch = (BrakRuchu,0f);
    //int glebokoscAnalizyNajlepszegoRuchu = 0;
    protected int glebokoscPrzeszukiwania = 1;
    protected const long SecondsToTicks = 10000000;
    public override (int x, int y) PlanujRuch(LogikaPlanszy plansza)
    {

        if (aktualnieNajlepszyRuch.ruch==BrakRuchu)
        {
            for (int i = 0; i < plansza.Plansza.Length; i++)
            {
                for (int j = 0; j < plansza.Plansza[i].Length; j++)
                {
                    if (LogikaPlanszy.CzyWolne(plansza.Plansza[i][j]))
                    {
                        aktualnieNajlepszyRuch = ((i, j), 0f);
                        i = plansza.Plansza.Length;
                        break;
                    }
                }
            }
        }
        int liczbaPolBezPionka = 0;
        for (int i = 0; i < plansza.Plansza.Length; i++)
        {
            for (int j = 0; j < plansza.Plansza[i].Length; j++)
            {
                if (plansza.Plansza[i][j] != LogikaPlanszy.PoleZajete)
                {
                    liczbaPolBezPionka++;
                }
            }
        }

        if (czasOdZapytania < czasOdpowiedzi)
        {
            //float czas = 0f;
            bool zabrakloCzasu = false;
            long _czasOdZapytania = (long)(SecondsToTicks * czasOdZapytania);
            long _czasOdpowiedzi = (long)(SecondsToTicks * czasOdpowiedzi);
            ((int x, int y), float jakosc) najlepszyRuchWPrzeszukaniu= (BrakRuchu, 0f);
            Przeszukaj(plansza.Plansza,out najlepszyRuchWPrzeszukaniu,ref _czasOdZapytania,out _, _czasOdpowiedzi,true,glebokoscPrzeszukiwania,ref zabrakloCzasu);
            czasOdZapytania = (float)((double)_czasOdZapytania / (double)SecondsToTicks);
            //czasOdpowiedzi = _czasOdpowiedzi / SecondsToTicks;

            czasOdZapytania += Mathf.Epsilon;

            if (!zabrakloCzasu|| aktualnieNajlepszyRuch.jakosc <= najlepszyRuchWPrzeszukaniu.jakosc)
            {
                aktualnieNajlepszyRuch = najlepszyRuchWPrzeszukaniu;
                //glebokoscAnalizyNajlepszegoRuchu = glebokoscPrzeszukiwania;
            }
            
            /*if (czasOdZapytania >= czasOdpowiedzi)
            {
                Debug.Log(zabrakloCzasu.ToString());
            }*/
            Debug.Log("Przeszukiwanie: Wysokość drzewa=" + glebokoscPrzeszukiwania + "  czasOdZapytania=" + czasOdZapytania + "/" + czasOdpowiedzi + "   czy zabrakło czasu na obliczenia: " + zabrakloCzasu
                +"   szansa na wygraną (Dla optymalnej taktyki przeciwnika)="+(aktualnieNajlepszyRuch.jakosc*100f)+"%");
            glebokoscPrzeszukiwania++;
            
            //czasOdZapytania = czasOdpowiedzi;
        }
        if (czasOdZapytania >= czasOdpowiedzi||glebokoscPrzeszukiwania>liczbaPolBezPionka+1)
        {
            Debug.Log(liczbaPolBezPionka);
            var ret = aktualnieNajlepszyRuch.Item1;
            aktualnieNajlepszyRuch = (BrakRuchu, 0f);
            czasOdZapytania = 0f;
            glebokoscPrzeszukiwania = 1;
            //glebokoscAnalizyNajlepszegoRuchu = 0;
            return ret;
        }
        //czasOdZapytania += Time.deltaTime;
        return BrakRuchu;
    }
    protected virtual void Przeszukaj(int[][] plansza, out ((int x, int y), float jakosc) najlepszyRuchWGalezi,ref long czasSumaryczny,out long czas,long limitCzasu,bool ruchAlgorytmu,int wysokoscDrzewa,ref bool zabrakloCzasu)
    {
        najlepszyRuchWGalezi = ruchAlgorytmu?(BrakRuchu, 0f): (BrakRuchu, 1f);
        if (czasSumaryczny >= limitCzasu||wysokoscDrzewa<=0)
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
                    long czasR;
                    Przeszukaj(nowaPlansza, out najlepszyRuchR,ref czasSumaryczny, out czasR, limitCzasu, !ruchAlgorytmu,wysokoscDrzewa-1,ref zabrakloCzasu);
                    if (ruchAlgorytmu&&najlepszyRuchWGalezi.jakosc <= najlepszyRuchR.jakosc)
                    {
                        najlepszyRuchWGalezi = ((i,j),najlepszyRuchR.jakosc);//TODO probabilistyczna średnia ruchów, zakładamy, że przeciwnik porusza się losowo, a my optymalnie
                    } else if (!ruchAlgorytmu&&najlepszyRuchWGalezi.jakosc >= najlepszyRuchR.jakosc)
                    {
                        najlepszyRuchWGalezi = ((i, j),najlepszyRuchR.jakosc);//minimax
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

        czas = System.DateTime.Now.Ticks - czasStart;
        czasSumaryczny += czas - sumaCzasowDzieci;
    }

}
