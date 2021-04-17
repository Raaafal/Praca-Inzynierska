using System.Collections;
using System.IO;
using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Client : Gracz
{


    private bool socketReady;
    private TcpClient socket;
    private NetworkStream stream;
    private StreamWriter writer;
    private StreamReader reader;

    private Tuple<int, int> ruchGospodarza = BrakRuchu, ruchGoscia = BrakRuchu;

    private bool sended = false;

    private int[][] poprzedniUklad;

    public Client()
    {

        nazwa = "client";
        grajZ = "client";
    }
    override
    public void Inicjalizuj()
    {
        Debug.Log("Client: conecting to server");
        ConnectToServer();
    }
    override
    public Tuple<int, int> WykonajRuch(LogikaPlanszy plansza)
    {
        if (socketReady)
        {
            if (!sended)
            {/*
                if (poprzedniUklad != null)
                {
                    ruchGoscia= BrakRuchu;
                    for (int i = 0; i < poprzedniUklad.Length; ++i)
                        for (int j = 0; j < poprzedniUklad[i].Length; ++j)
                        {
                            if (poprzedniUklad[i][j] != plansza[i][j])
                            {
                                ruchGoscia = new Tuple<int, int>(i, j);
                                break;
                            }
                        }
                }
                else
                {
                    ruchGoscia = BrakRuchu;
                    for (int i = 0; i < plansza.Length; ++i)
                        for (int j = 0; j < plansza[i].Length; ++j)
                        {
                            if (plansza[i][j]==LogikaPlanszy.PoleZajete)
                            {
                                ruchGoscia = new Tuple<int, int>(i, j);
                                break;
                            }
                        }
                }*/
                ruchGoscia = plansza.OstatniRuch;
                ruchGospodarza = BrakRuchu;
                if (ruchGoscia.Item1 != -1 && ruchGospodarza.Item2 != -1)
                    Send(new string(new char[] { (char)ruchGoscia.Item1, (char)ruchGoscia.Item2 }));
                sended = true;
                Debug.Log("Client: sended move: "+ ruchGoscia.ToString());
            }
        }
        Update();
        Debug.Log("Client: return move: "+ruchGospodarza.ToString());
        return ruchGospodarza;
    }

    // Update is called once per frame
    void Update()
    {
        if (socketReady)
        {
            if (stream.DataAvailable)
            {
                string data = reader.ReadLine();
                if (data != null)
                    OnIncomingData(data);
            }
        }
    }

    public void ConnectToServer()
    {
        if (socketReady)
        {
            return;
        }

        string host = "127.0.0.1";
        //    h = GameObject.Find("HostInput").GetComponent<InputField>().text;
        //if (h != "")
        //    host = h;

        int port = 1111;

        try
        {
            socket = new TcpClient(host, port);
            stream = socket.GetStream();
            writer = new StreamWriter(stream);
            reader = new StreamReader(stream);
            socketReady = true;
        }catch(Exception e)
        {
            Debug.Log("Socket error: " + e.Message);
        }
    }

    private void OnIncomingData(string data)
    {
        Debug.Log("Recived: " + data);

        if (data[0] > 200 || data[1] > 200)
        {
            ruchGospodarza = BrakRuchu;
        }
        else
        {
            ruchGospodarza = new Tuple<int, int>(data[0], data[1]);
            sended = false;
        }

        Debug.Log("Recived: " + ruchGospodarza.ToString());
        //Ustawienia.Przeciwnik = typeof(Server);
        //Ustawienia.WielkoscPlanszy = data[1];
        //Ustawienia.PierwszyRuch = data[0]=='b'? Ustawienia.Ruch.Pierwszy:Ustawienia.Ruch.Drugi;
        //SceneManager.LoadScene("Gra");
    }

   public void Send(string data)
    {
        if (!socketReady)
            return;

        writer.WriteLine(data);
        writer.Flush();
        sended = false;
    }
    private void CloseSocket()
    {
        if (!socketReady)
            return;

        writer.Close();
        reader.Close();
        socket.Close();
        socketReady = false;
    }
    private void OnAplicationQuit()
    {
        CloseSocket();
    }
    private void OnDisable()
    {
        CloseSocket();
    }
}
