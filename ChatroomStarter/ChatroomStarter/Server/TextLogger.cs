using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class TextLogger : InterfaceLog
    {
        private Object logLock = new object();
        string path = "log.txt";
        public void LogMessage(string message)
        {
            lock (logLock)
            {
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(userName + "is being received.");

                }
            }
        }

        public void LogPerson(string userName)
        {
            lock (logLock)
            {
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(userName + "joined the Force server.");

                }
            }
        }

        public void LogError (Exception e)
        {
            lock (logLock)
            {
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(e + " was shot down.");
                }
            }
        }
        public void LogPersonLeft(Client client)
        {
            lock (logLock)
            {
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(client.userName + " has left Resistance chatroom");
                }
            }
        }

        public void ServerClosed()
        {
            lock (logLock)
            {
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine("The server has closed. You are on the Dark Side.");
                }
            }
        }
    
        

    }
}
