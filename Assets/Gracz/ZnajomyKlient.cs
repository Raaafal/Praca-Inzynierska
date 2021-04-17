using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class ZnajomyKlient : Gracz
{
    TcpListener soketZnajomego;
    TcpClient mojSocket;
    public override System.Tuple<int, int> WykonajRuch(int[][] plansza)
    {
		
		
		
		//plansza.Length;
		//LogikaPlanszy.CzyWolne( plansza[i][j]);
		//return new System.Tuple<int, int>(i, j);
        return BrakRuchu;
    }

    public ZnajomyKlient() : base()
    {
        nazwa = "znajomy";
        grajZ = "znajomym";

        soketZnajomego = new TcpListener(IPAddress.Any, 1111);
        soketZnajomego.Start(); 



    }
}
