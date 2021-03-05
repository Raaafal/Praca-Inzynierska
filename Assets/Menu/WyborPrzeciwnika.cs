using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using System.Linq;

public class WyborPrzeciwnika : MonoBehaviour
{
    [SerializeField]
    Dropdown wybor;

    Type[] przeciwnicy = new Type[] { typeof(Ja),typeof(LosowyRuch) };

    private void Start()
    {
        var opcje = new List<Dropdown.OptionData>();
        for(int i=0;i<przeciwnicy.Length;i++)
        {
            var Typ = przeciwnicy[i];
            opcje.Add(new Dropdown.OptionData(((Gracz)Activator.CreateInstance(Typ)).grajZ));

        }
        wybor.options = opcje;

        int index = przeciwnicy.ToList().IndexOf(Ustawienia.Przeciwnik);
        if (index == -1) Ustawienia.Przeciwnik = KtoryPrzeciwnik();
        else wybor.value=index;
        //Ustawienia.Przeciwnik = KtoryPrzeciwnik();
    }
    public void Wybor()
    {
        //Ustawienia.Przeciwnik = (Gracz)Activator.CreateInstance(KtoryPrzeciwnik());
        Ustawienia.Przeciwnik = KtoryPrzeciwnik();

    }
    public Type KtoryPrzeciwnik()
    {
        return przeciwnicy[wybor.value];
    }
}
