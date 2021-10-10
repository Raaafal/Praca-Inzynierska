using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

public class Zaproszenie:MonoBehaviour
{
    public InputField PoleZaproszenia;
    public Button PrzyciskZaproszenia;
    protected const int domyslnyPort=7878;
    protected LogikaZaproszenia logikaZaproszenia= new LogikaZaproszenia(domyslnyPort);
    public int DomyslnyPort => domyslnyPort;
    public void WygenerujZaproszenie()
    {
        int port = Polaczenie.PostawSerwer();
        IPAddress[] adresy = Dns.GetHostAddresses(Dns.GetHostName());
        PoleZaproszenia.text= logikaZaproszenia.WygenerujZaproszenie(adresy,port);
    }
    public (IPAddress[] adresy,int port) DekodujZaproszenie()
    {
        string zaproszenie = PoleZaproszenia.text;
        return logikaZaproszenia.DekodujZaproszenie(zaproszenie);
    }
    
}
