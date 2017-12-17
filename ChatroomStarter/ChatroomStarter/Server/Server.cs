using System;
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
                    Task<string> message = Task.Run(() => client.Recieve());
                    message.Wait();
                    Task<string>[] messages = new Task<string>[] { message };
                    string CurrentMessage = messages[0].Result;
                    string currentmessage = messages[0].Result;
                    AddToQueue(currentmessage, client);
                }
                catch (Exception e)
                {
                    string error = "You have evacuated your communications system";
                    Console.Write(error);
                    logger.LogInfo(e);
                }
            }
        }

        private void Broadcast()
        {
            while (true)
            {
                if (messages.Count() > 0)
                {
                    Message message = RemoveFromQueue();
                    lock (messageLock)
                    {
                        Client removedPerson = null;
                        foreach (KeyValuePair<int, Client> clients in people)
                        {
                            if (message.sender.IsConnected == true)
                            {
                                if (!(message.sender.userName == clients.Value.userName))
                                {
                                    clients.Value.Send(message.Body);
                                }
                            }
                            else
                            {
                                logger.LogPersonLeft(message.sender);
                                removedPerson = message.sender;
                            }
                        }
                        RemoveClientFromDictionary(removedPerson);
                    }
                }
            }
        }
            private void Respond(string body Client, client)
        {
            Client.send(body);
        }


    }
}