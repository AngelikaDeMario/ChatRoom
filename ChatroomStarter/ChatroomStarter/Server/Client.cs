﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Client
    {
        NetworkStream stream;
        TcpClient client;
        public string UserId;
        public int UserId;
        public string UserName;
        private byte[] recievedMessage;
        internal object userName;

        public Client(NetworkStream Stream, TcpClient Client)
        {
            stream = Stream;
            client = Client;
            UserId = "495933b6-1762-47a1-b655-483510072e73";
          
        }

        public bool isConnected
        {
            get { return client.Connected; }
        }

        public bool IsConnected { get; internal set; }

        public void Send(string Message)
        {
            byte[] message = Encoding.ASCII.GetBytes(Message);
            stream.Write(message, 0, message.Count());
        }
        public string Recieve()
        {
            try
            {
                while (true)
                byte[] recievedMessage = new byte[256];
                stream.Read(recievedMessage, 0, recievedMessage.Length);
                string recievedMessageString = Encoding.ASCII.GetString(recievedMessage);
                Console.WriteLine(recievedMessageString);
                return recievedMessageString;
            }

            catch (Exception e)
            {
                string message = "No more, the server is" + e;
                return message;
            }
        }

        public string GetUserName()
        {
            Send("What is your name, young Padawan?");
                userName = Receive();
                return userName;
        }

        private object Receive()
        {
            throw new NotImplementedException();
        }

        internal static void send(string body)
        {
            throw new NotImplementedException();
        }
    }
}
