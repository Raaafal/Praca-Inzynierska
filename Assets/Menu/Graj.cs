using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Graj : MonoBehaviour
{
    public Zaproszenie zaproszenie;
    public void Klik()
    {
        if (zaproszenie.isActiveAndEnabled)
        {
            Tuple<IPAddress[], int> serwery = zaproszenie.DekodujZaproszenie();
            Polaczenie.Polacz(serwery.Item1, serwery.Item2);
        }
        SceneManager.LoadScene("Ustawienia");
    }
}
