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
    private string[] adresyIP= { };
    public void Start()
    {
        //DontDestroyOnLoad(this);
    }

    // Update is called once per frame
   

    public void WyenerujZaproszenie()
    {
        int port = Polaczenie.PostawSerwer();
        IPAddress[] adresy = Dns.GetHostAddresses(Dns.GetHostName());
        string[] ips = new string[adresy.Length + 1];
        int i = 0;
        foreach(IPAddress ip in adresy)
        {
            if(ip.ToString().Contains('.'))
            ips[i++] = IPnaZaproszenie(ip);
        }
        if (port != Polaczenie.domyslnyPort)
        {
            ips[i++] = port.ToString();
        }
        PoleZaproszenia.text= string.Join(",",ips,0,i) ;
        Debug.Log("adresy: "+ string.Join(",", ips));
    }
    public Tuple<IPAddress[],int> DekodujZaproszenie()
    {
        string zaproszenie = PoleZaproszenia.text;
        string[] elementy=zaproszenie.Split(',');
        int port;
        IPAddress[] adresy;
        if (elementy[elementy.Length - 1].Contains<Char>('.')) {
            port = Polaczenie.domyslnyPort;
            adresy = new IPAddress[elementy.Length];
            int i = 0;
            foreach(string ip in elementy)
            {
                adresy[i++] = IPAddress.Parse(ip);
            }
        }
        else
        {
            port = int.Parse(elementy[elementy.Length - 1]);
            adresy = new IPAddress[elementy.Length-1];
            for(int i=0; i < elementy.Length - 1; ++i)
            {
                adresy[i] = IPAddress.Parse(elementy[i]);
            }
        }
        return new Tuple<IPAddress[],int>(adresy,port);
    }
    public string IPnaZaproszenie(IPAddress ip)
    /*
        w zaproszen nie mogą występować podobne znaki
            - 0 O
            - 1 l I
            - VV W
        adresy IP o skróconej formie
            - adresy lokalne w klasach A,B,C
     */
    {
        string znaki = "ABCDEFGHJKLMNPRSTUVXYZabcdefghijkmnopqrstuvxyz0123456789";
        string ret = "";
        int wartosc = BitConverter.ToInt32( ip.GetAddressBytes(),0);
        int limit = 0;
        {
            ret += znaki[wartosc % znaki.Length];
            wartosc /= znaki.Length;
            limit++;
            Debug.Log("ret=" + ret+"\nreszta cyfr=" + wartosc + "\niter=" + limit);
        }while (limit<10) ;
        return ret;

    }
    
}
