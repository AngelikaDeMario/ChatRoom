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
        private Queue<Message> messages;
        private Dictionary<int, Client> people;
        int UserIDNumber;
        private object messageLock = new object();
        InterfaceLog logger;
        private string userName;
        private object name;

        public Server(InterfaceLog logger)
        {
            server = new TcpListener(IPAddress.Any, 9999);
            messages = new Queue<Message>();
            people = new Dictionary<int, Client>();
            UserIDNumber = 0;
            this.logger = logger;
            server.Start();
        }
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

        private object BroadCast()
        {
            throw new NotImplementedException();
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

        private void AddClientToDictionary(Client client)
        {
            throw new NotImplementedException();
        }

        private void GetInformationForNotification(Client client)
        {
            Task<string> username = Task.Run(() => client.GetUserName());
            username.Wait();
            string.Name = username.Result.Trim('\0');
            NotifyClientOfNewClient(name, client);
        }

        private void NotifyClientOfNewClient(object name, Client client)
        {
            throw new NotImplementedException();
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

        private void RemoveClientFromDictionary(Client removedPerson)
        {
            throw new NotImplementedException();
        }

        private void Respond(string body, Client client)
        {
            Client.send(body);
        }

        private void AddToQueue(string message, Client client)
        {
            Message currentMessage = new Message(client, message);
            messages.Enqueue(currentMessage);
            logger.LogMessage(message);
        }
        private Message RemoveFromQueue()
        {
            return messages.Dequeue();
        }

        private void AddClientToDictionary()
        {
            people.Add(UserIDNumber, client);
            client.UserId = UserIDNumber;
            UserIDNumber++;
        }
        private void RemoveClientFromDictionary()
        {
            if (!(client == null))
            {
                people.Remove(client.UserId);
                CheckForPeople(client);
            }
        }

        private void CheckForPeople(Client client)
        {
            if (people.Count <= 0)
            {
                logger.ServerClosed();
                Environment.Exit(0);
            }
        }
        private void NotifyClientOfNewClient(string username, Client client)
        {
            string words = userName + "add to chatroom";
            client.userName = userName;
            AddToQueue(words, client);
            logger.LogPerson(userName);
        }

    }
}