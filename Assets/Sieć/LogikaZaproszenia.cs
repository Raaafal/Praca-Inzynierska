using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

public class LogikaZaproszenia
{
    private int domyslnyPort;
    public LogikaZaproszenia(int domyslnyPort)
    {
        this.domyslnyPort = domyslnyPort;
    }

    public string WygenerujZaproszenie(IPAddress[] adresy, int port)
    {
        var adresyDoZaproszenia = from ip in adresy
                                  where ip.ToString().Contains('.')
                                  select ip.ToString();
        string zaproszenie = String.Join(",", adresyDoZaproszenia);
        if (port != domyslnyPort)
        {
            zaproszenie += "," + port.ToString();
        }
        return zaproszenie;
    }
    public (IPAddress[] adresy, int port) DekodujZaproszenie(string zaproszenie)
    {
        string[] elementy = zaproszenie.Split(',');
        int port = domyslnyPort;
        if (!elementy[elementy.Length - 1].Contains('.'))//wykryj port w zaprowszeniu
        {
            port = Int32.Parse(elementy[elementy.Length - 1]);
            Array.Resize(ref elementy, elementy.Length - 1);//wsuń port z elementów
        }
        var adresy = from ip in elementy
                     select IPAddress.Parse(ip);
        return (adresy.ToArray(), port);
    }
}