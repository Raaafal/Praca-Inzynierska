using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogikaPlanszy : Plansza
{
    protected const int POLE_ZAJETE = -1;

    protected int [][]plansza=new int[WIELKOSC][];
    protected new void Start()
    {
        base.Start();
        for(int i = 0; i < WIELKOSC; i++)
        {
            plansza[i] = new int[WIELKOSC];
            /*
            for (int j = 0; j < WIELKOSC; j++)
            {
                plansza[i][j]=
            }*/
        }
    }

    protected override void ClickCallback(int x, int y, GameObject obj)
    {
        if (plansza[x][y] != POLE_ZAJETE&& plansza[x][y]%2==0)
        {
            var newQueen = Instantiate(queen, transform);
            newQueen.GetComponent<RectTransform>().position = obj.GetComponent<RectTransform>().position;

            MojaKolej = !MojaKolej;
            Ruch(x, y);
            OdswierzKolory();
        }
    }
    public void Ruch(int x,int y)
    {
        
        for(int i = 0; i < WIELKOSC; i++)
        {
            for(int j = 0; j < WIELKOSC; j++)
            {
                if (plansza[i][j] != POLE_ZAJETE)
                {
                    int localX = x - i;
                    int localY = y - j;
                    if (Mathf.Abs( localX * localX * localY) == Mathf.Abs(localX * localY * localY))
                    {
                        plansza[i][j] += 1;
                    }

                }
            }
        }
        plansza[x][y] = POLE_ZAJETE;
    }
    void OdswierzKolory()
    {
        for(int i = 0; i < WIELKOSC; i++)
        {
            for (int j = 0; j < WIELKOSC; j++)
            {
                KolorujPole(i,j,plansza[i][j] == POLE_ZAJETE || plansza[i][j] % 2 == 1);
                /*
                if(plansza[i][j] != POLE_ZAJETE&& plansza[i][j] % 2 == 0)
                {
                    Pola[i][j].GetComponent<RawImage>().color = pole.GetComponent<RawImage>().color;
                } else
                {
                    Pola[i][j].GetComponent<RawImage>().color = new Color(1f,0f,0f);
                }

                if ((i + j) % 2 == 0)
                    Pola[i][j].GetComponent<RawImage>().color *= colorMul;*/
            }

        }
    }
}
