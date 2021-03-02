using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WyborUstawien : MonoBehaviour
{
    [SerializeField]
    GameObject biala;
    [SerializeField]
    GameObject czarna;
    [SerializeField]
    GameObject zaznaczenie;
    Ustawienia.Ruch ruch = Ustawienia.Ruch.Losowy;
    private void Start()
    {
        Ustawienia.PierwszyRuch = ruch;
    }
    public void KlikniecieNaBiala()
    {
        if (ruch != Ustawienia.Ruch.Pierwszy)
        {
            ruch = Ustawienia.Ruch.Pierwszy;
            zaznaczenie.SetActive(true);
            zaznaczenie.GetComponent<RectTransform>().position = biala.GetComponent<RectTransform>().position;
        } else
        {
            zaznaczenie.SetActive(false);
            ruch = Ustawienia.Ruch.Losowy;
        }
        
        Ustawienia.PierwszyRuch = ruch;
    }
    public void KlikniecieNaCzarna()
    {
        if (ruch != Ustawienia.Ruch.Drugi)
        {
            ruch = Ustawienia.Ruch.Drugi;
            zaznaczenie.SetActive(true);
            zaznaczenie.GetComponent<RectTransform>().position = czarna.GetComponent<RectTransform>().position;
        }
        else
        {
            zaznaczenie.SetActive(false);
            ruch = Ustawienia.Ruch.Losowy;
        }

        Ustawienia.PierwszyRuch = ruch;
    }
}
