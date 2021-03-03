using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class PlanszaWUstawieniach : MonoBehaviour
{
    [SerializeField]
    GameObject pole;

    System.Action<int> klik;

    int wielkosc = 8;

    private void OnMouseUpAsButton()
    {
        klik?.Invoke(wielkosc);
    }
    private void Start()
    {
        if (transform.parent != null) return;
        Init(8, 4, null);
    }

    void Init(int wielkosc,float wielkoscNaEkranie,System.Action<int> klik)
    {
        this.klik = klik;
        this.wielkosc = wielkosc;

        var cols = GetComponents<BoxCollider2D>();
        foreach(var col in cols)
        {
            col.size = Vector2.one * wielkoscNaEkranie;
        }
        /*
        GameObject[][] Pola = new GameObject[wielkosc][];
        for (int i = 0; i < wielkosc; i++)
        {
            Pola[i] = new GameObject[wielkosc];
        }*/

        float wielkoscPola = wielkoscNaEkranie/wielkosc;
        for (int i = 0; i < wielkosc; i++)
        {
            for (int j = 0; j < wielkosc; j++)
            {
                GameObject p = Instantiate(pole, transform);
                //p.transform.SetParent(transform);
                //var tr = p.GetComponent<RectTransform>();
                var tr = p.transform;

                var pos = tr.localPosition;
                pos.x = -wielkoscPola * wielkosc / 2f + wielkoscPola / 2f + i * wielkoscPola;
                pos.y = -wielkoscPola * wielkosc / 2f + wielkoscPola / 2f + j * wielkoscPola;
                tr.localPosition = pos;

                var scale=tr.localScale;
                scale.x *= wielkoscPola;
                scale.y *= wielkoscPola;
                tr.localScale = scale;

                p.GetComponent<SpriteShapeRenderer>().color = (i + j) % 2 == 0 ? Color.white : Color.black;

                /*if ((i + j) % 2 == 0) {
                    var ri = p.GetComponent<RawImage>();
                    ri.color *= PARZYSTY_MNOZNIK_KOLORU;
                }*/


                //var script = p.AddComponent<Pole>();
                //script.Init(Klik, i, j);

                //p.GetComponent<>
                //Pola[i][j] = p;

                //KolorujPole(i, j, false);
            }
        }

        /*
        int szerPlanszy = wielkosc * wielkoscPola;
        //int szerWyswietlacza = (int)transform.parent.GetComponent<RectTransform>().rect.width;// Screen.width;
        var rtransform = GetComponent<RectTransform>();
        var skala = rtransform.localScale;
        //var cam = Camera.main;

        float unityNaPiksele = transform.parent.GetComponent<RectTransform>().rect.height / Camera.main.orthographicSize / 2f;

        skala.x = wielkoscNaEkranie*unityNaPiksele / szerPlanszy;
        skala.y = wielkoscNaEkranie*unityNaPiksele / szerPlanszy;
        rtransform.localScale = skala;*/
    }
}
