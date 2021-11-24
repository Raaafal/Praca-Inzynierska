using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

static
public class Polaczenie
{

    public const int domyslnyPort=7878;
    static private TcpClient klient;
    static private TcpListener serwer;
    static bool serwerPostawiony=false;
    public static bool czySerwer
    {
        get
        {
            return serwerPostawiony;
        }
    }

    static
    public int PostawSerwer()
    {
        if (serwer != null)
            serwer.Stop();
        try
        {
            serwer = new TcpListener(IPAddress.Any, domyslnyPort);
            serwer.Start();
            serwerPostawiony = true;
            Debug.Log("Postawiono serwer - domyślny port: "+domyslnyPort);
        }
        catch
        { 
            serwer= new TcpListener(IPAddress.Any, 0);
            serwer.Start();
            serwerPostawiony = true;
            Debug.Log("Postawiono serwer - port: "+ ((IPEndPoint)serwer.LocalEndpoint).Port);
        };
        serwer.BeginAcceptTcpClient(CzekajNaKlienta, serwer);
        return ((IPEndPoint)serwer.LocalEndpoint).Port;
    }

    static
    void CzekajNaKlienta(IAsyncResult async)
    {
        Debug.Log("Klient się pojawił");
        TcpListener listener = (TcpListener)async.AsyncState;
        klient = listener.EndAcceptTcpClient(async);
        Debug.Log("server: new client");
    }

    static
    public void ZatrzymajSerwer()
    {
        serwer.Stop();
        serwerPostawiony = false;
        Debug.Log("zarzymano serwer");
    }
    static
    public bool PostawKlienta(IPAddress[] serwery,int port)
    {
        //testuj adresy
        foreach(var ip in serwery)
        {
            try
            {
                klient = new TcpClient(ip.ToString(), port);
                Debug.Log("postawiono klienta "+port);
                return true;
            }
            catch
            {
            };
        }
        Debug.Log("niepostawiono klienta");
        return false;
    }
    static
    public void ZatrzymajKlienta()
    {
        if (klient != null)
            klient.Close();
    }
    static 
    public bool Polacz(IPAddress[]serwery,int port)
    {
        if (serwer != null)
        {
            IPAddress[] ips = Dns.GetHostAddresses(Dns.GetHostName());
            int portSerwera = ((IPEndPoint)serwer.LocalEndpoint).Port;
            if (port == portSerwera)
            {
                foreach (var ipSerwera in serwery)
                    foreach (var ipHosta in ips)
                    {
                        if (ipHosta.Equals(ipSerwera))
                        {
                            if (serwerPostawiony)
                            {
                                Debug.Log("połącz - już postawiono serwer");
                                return true;
                            }
                            else
                            {
                                serwer.Start();
                                serwerPostawiony = true;
                                Debug.Log("połącz - postawiono serwer");
                                return true;
                            }
                        }
                    }
            }
        }
        if (serwerPostawiony)
        {
            Debug.Log("połącz - wyłączono serwer");
            serwer.Stop();
        }
        Debug.Log("połącz - postaw klienta");
        return PostawKlienta(serwery, port);

    }

    public static PreferencjeGracza OdbierzUstawienia()
    {
        NetworkStream stream = klient.GetStream();
        if (stream.DataAvailable)
        {
            Byte[] data = new byte[2];
            int len = stream.Read(data, 0, 2);
            if (len < 2)
            {
                Debug.Log("OdbierzUstawienia pusto");
                return null;
            }
            else
            {
                PreferencjeGracza preferencje = new PreferencjeGracza();
                preferencje.preferowanyRozmiarPlanszy = data[0];
                preferencje.czyPreferujePierwszyRuch = data[1] == 1;
                Debug.Log("OdbierzUstawienia:"+preferencje.preferowanyRozmiarPlanszy+","+preferencje.czyPreferujePierwszyRuch);
                return preferencje;
            }
        }
        Debug.Log("OdbierzUstawienia - brak danych");
        return null;
    }
    public static bool WyślijUstawienia()
    {
        if(null == klient)
        {
            return false;
        }
        NetworkStream stream = klient.GetStream();
        Byte[] data = new byte[2] { (byte)Ustawienia.WielkoscPlanszy, Ustawienia.PierwszyRuch==Ustawienia.Ruch.Pierwszy? (byte)0 : (byte)1 };//ustawienia z punktu patrzenia rywala dla niego samego
        stream.Write(data, 0, 2);
        Debug.Log("WyślijUstawoienia:" + data);
        return true;
    }
    static
    public void WyslijRuch(int x, int y)
    {
        NetworkStream stream = klient.GetStream();
        Byte[] data = new byte[2] { (byte)x, (byte)y };
        stream.Write(data, 0, 2);
        Debug.Log("WyślijRuch:" + x + ',' + y);

    }
    static
    public bool WyslijRuch((int x, int y) ruch)
    {
        if(null == klient)
        {
            return false;
        }
        NetworkStream stream = klient.GetStream();
        Byte[] data = new byte[2] { (byte)ruch.x, (byte)ruch.y };
        stream.Write(data, 0, 2);
        Debug.Log("WyślijRuch:" + (byte)ruch.x + ',' + (byte)ruch.y);
        return true;
    }
    public static (int x, int y) OdbierzRuch()
    {
        if(null == klient)
        {
            return Gracz.BrakRuchu;
        }
        NetworkStream stream = klient.GetStream();
        if (stream.DataAvailable)
        {
            Byte[] data = new byte[2];
            int len = stream.Read(data, 0, 2);
            if (len < 2)
            {
                Debug.Log("OdbierzUstawienia pusto");
                return Gracz.BrakRuchu;
            }
            else
            {
                return (x:data[0],y:data[1]);
            }
        }
        Debug.Log("OdbierzUstawienia - brak danych");
        return Gracz.BrakRuchu;
    }
}
