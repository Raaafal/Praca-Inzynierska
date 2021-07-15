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
            (IPAddress[] adresy, int port) serwery = zaproszenie.DekodujZaproszenie();
            Polaczenie.Polacz(serwery.adresy, serwery.port);
        }
        SceneManager.LoadScene("Ustawienia");
    }
}
