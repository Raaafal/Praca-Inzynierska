using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PasekAktywnegoGracza {
    RawImage pasek;
    public PasekAktywnegoGracza(string nazwaGracza1,string nazwaGracza2, RawImage pasek,Text tekstGracza1,Text tekstGracza2)
    {
        this.pasek = pasek;

        tekstGracza1.text = nazwaGracza1;
        tekstGracza2.text = nazwaGracza2;
    }
    public void SygnalizujCzyjRuch(bool ruch)
    {
        var rect = pasek.uvRect;
        if (ruch)
        {

            rect.x = 0;
            rect.width = 1;
        }
        else
        {
            rect.x = 1;
            rect.width = -1;
        }
        pasek.uvRect = rect;
    }
}
