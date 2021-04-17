using System.Collections;
using System.Collections.Generic;
using System.Net;
using System;
//using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Server : Gracz
{
    public int port = 1111;

    private ServerClient client;
    private TcpListener server;
    private bool serverStarted=false;

    private Tuple<int, int> ruchGospodarza = BrakRuchu, ruchGoscia = BrakRuchu;

    private bool sended = false;

    private int[][] poprzedniUklad;

    public Server()
    {
        nazwa = "server";
        grajZ = "server";
    }
    override
    public void Inicjalizuj()
    {
        StartServer();
    }
    override
    public Tuple<int, int> WykonajRuch(LogikaPlanszy plansza)
    {
        if (serverStarted &&client!=null && IsConected(client.tcp))
        {
            if (!sended)
            {
                ruchGospodarza = plansza.OstatniRuch;
                ruchGoscia = BrakRuchu;
                if (ruchGospodarza.Item1 != -1 && ruchGospodarza.Item2 != -1)
                    Send(new string(new char[] { (char)ruchGospodarza.Item1, (char)ruchGospodarza.Item2 }));
                Debug.Log("Server: sended move: " + ruchGospodarza.ToString());
                sended = true;
            }
        }
        Update();
        Debug.Log("Server: return move: " + ruchGoscia.ToString());
        return ruchGoscia;
    }

    public void StartServer()
    {
        if (serverStarted)
            return;
        //client = new List<ServerClient>();
        //disconnected = new List<ServerClient>();

        try
        {
            server = new TcpListener(IPAddress.Any, port);
            server.Start();

            StartListening();
            serverStarted = true;
            Debug.Log("server started");
        }catch(Exception e)
        {
            Debug.Log("Socket error: " + e.StackTrace);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!serverStarted)
            return;

        if (client != null)
        {
            if (!IsConected(client.tcp))
            {
                client.tcp.Close();
                Debug.Log("lose connection");
                //disconnected.Add(c);
            }
            else
            {
                NetworkStream s = client.tcp.GetStream();
                if (s.DataAvailable)
                {
                    StreamReader reader = new StreamReader(s, true);
                    string data = reader.ReadLine();
                    if (data != null)
                    {
                        OnIncomingData(client, data);
                    }
                }
            }
        }
    
    }

    private bool IsConected(TcpClient c)
    {
        try
        {
            if(c!=null && c.Client!=null && c.Client.Connected)
            {
                if (c.Client.Poll(0, SelectMode.SelectRead))
                {
                    return !(c.Client.Receive(new byte[1], SocketFlags.Peek) == 0);
                }
                return true;
            }
            return false;
        }
        catch
        {
            return false;
        }
    }
    protected void OnIncomingData(ServerClient c, string data)
    {
        if (data[0] > 200 || data[1] > 200) {
            ruchGoscia = BrakRuchu;
        }
        else
        {
            ruchGoscia = new Tuple<int, int>((int)data[0],(int)data[1] );
            sended = false;
        }
        Debug.Log(c.clientName + "write: " + ruchGoscia);

    }
    private void StartListening()
    {
        server.BeginAcceptTcpClient(AcceptTcpClient, server);
    }
    private void AcceptTcpClient(IAsyncResult ar)
    {
        TcpListener listener = (TcpListener)ar.AsyncState;
        client=new ServerClient(listener.EndAcceptTcpClient(ar));
        Debug.Log("server: new client");
        StartListening();

        //Multicast("helow " + client.clientName,new List<ServerClient> { client});
        //Send("b\0x8");

        //Ustawienia.Przeciwnik = typeof(Client);
        //Ustawienia.WielkoscPlanszy = 8;
        //Ustawienia.PierwszyRuch=Ustawienia.Ruch.Drugi;
        //SceneManager.LoadScene("Gra");

    }
    private void Send(string data)
    {
        StreamWriter writer = new StreamWriter(client.tcp.GetStream());
        writer.WriteLine(data);
        writer.Flush();
    }
    private void Multicast(string data, List<ServerClient> cs)
    {
        foreach (ServerClient c in cs)
        {
            try
            {
                StreamWriter writer = new StreamWriter(c.tcp.GetStream());
                writer.WriteLine(data);
                writer.Flush();
            }
            catch (Exception e)
            {
                Debug.Log("Writer error:" + e.Message + c.clientName);
            }
        }
    }
}

public class ServerClient
{
    public TcpClient tcp;
    public string clientName;

    public ServerClient(TcpClient clientSocket)
    {
        clientName = "Guest";
        tcp = clientSocket;
    }
}