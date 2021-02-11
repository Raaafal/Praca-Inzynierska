using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Plansza : MonoBehaviour
{
    [SerializeField]
    protected GameObject queen;

    [SerializeField]
    protected GameObject pole;

    protected float colorMul = 0.5f;

    protected GameObject[][] Pola = new GameObject[WIELKOSC][];

    [SerializeField]
    RawImage pasek;

    [SerializeField]
    protected const int WIELKOSC = 8;

    bool mojaKolej = true;
    protected bool MojaKolej
    {
        get { return mojaKolej; }
        set
        {
            var rect = pasek.uvRect;
            if (value)
            {
                
                rect.x = 0;
                rect.width = 1;
            }else
            {
                rect.x = 1;
                rect.width =-1;
            }
            pasek.uvRect = rect;
            mojaKolej = value;
        }
    }

    // Start is called before the first frame update
    protected void Start()
    {
        for (int i = 0; i < WIELKOSC; i++)
        {
            Pola[i] = new GameObject[WIELKOSC];
        }

        int wielkoscPola = 100;
        for(int i = 0; i < WIELKOSC; i++)
        {
            for (int j = 0; j < WIELKOSC; j++)
            {
                GameObject p = Instantiate(pole,transform);
                //p.transform.SetParent(transform);
                var tr=p.GetComponent<RectTransform>();
                var pos=tr.localPosition;
                pos.x = -wielkoscPola * WIELKOSC / 2f+wielkoscPola/2f+i*wielkoscPola;
                pos.y= -wielkoscPola * WIELKOSC / 2f + wielkoscPola / 2f + j *wielkoscPola;
                tr.localPosition = pos;
                if ((i + j) % 2 == 0) {
                    var ri = p.GetComponent<RawImage>();
                    ri.color *= colorMul;
                }

                var script=p.AddComponent<Pole>();
                script.Init(CallbackHandler,i,j);

                Pola[i][j] = p;
            }
        }
        int szerPlanszy = WIELKOSC * wielkoscPola;
        int szerWyswietlacza = Screen.width;
        var rtransform = GetComponent<RectTransform>();
        var skala = rtransform.localScale;
        skala.x *= (float)szerWyswietlacza / szerPlanszy;
        skala.y *= (float)szerWyswietlacza / szerPlanszy;
        rtransform.localScale = skala;

        MojaKolej = mojaKolej;
    }
    protected void CallbackHandler(int x, int y, GameObject obj)
    {
        ClickCallback(x, y, obj);
    }
    protected virtual void ClickCallback(int x,int y,GameObject obj)
    {

    }
}
