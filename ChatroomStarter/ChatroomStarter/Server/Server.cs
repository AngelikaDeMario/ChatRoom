﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class Server
    {
        public static Client client;
        TcpListener server;
        public Server()
        {
            server = new TcpListener(IPAddress.Parse("127.0.0.1"), 9999);
            server.Start();
        }
        public void Run()
        {
            Task.Run(() = AcceptClient());
            Task.Run(() = BroadCast());
        }
        private void AcceptClient()
        {
            while (true)
            {
                try
                {
                    TcpClient clientSocket = default(TcpClient);
                    clientSocket = server.AcceptTcpClient();
                    Console.WriteLine("Connected");
                    NetworkStream stream = clientSocket.GetStream();
                    client = new Client(stream, clientSocket);
                    AddClientToDictionary(client);
                    Task username = Task.Run(() => GetInformationForNotification(client));
                    username.Wait();
                    Task.Run(() => CreateNewClientChat(clientSocket, client));
                }
                catch (Exception e)
                {
                    logger.ServerClosed();
                }
            }
        }

        private void GetInformationForNotification(Client client)
        {
            Task<string> username = Task.Run(() => client.GetUserName());
            username.Wait();
            string.Name = username.Result.Trim('\0');
            NotifyClientOfNewClient(name, client);
        }

        private void CreateNewClientChat(TcpClient clientSocket, Client client)
        {
            while (true)
            {
                try
                {

                }
            }
        }
        private void Respond(string body)
        {
             client.Send(body);
        }
    }
}
