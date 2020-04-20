using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace CSaN3
{
    public class ChatParticipant : IDisposable
    {
        private const int tcp_port = 7171;
        private string ipStart = "[";
        private string ipEnd = "]";
        private string nameEnd = " ";
        private string codeEnd = "|";

        public TcpClient tcpClient;
        public NetworkStream stream;
        public bool alive = true;

        public string username;
        public IPAddress IPv4Address;

        public void Connect()
        {
            tcpClient = new TcpClient();
            tcpClient.Connect(IPv4Address, tcp_port);
            stream = tcpClient.GetStream();
        }

        public void SendMessage(string message, string username, int code)
        {
            message = MakeMessage(message, username,  code);
            byte[] data = Encoding.Unicode.GetBytes(message);
            try
            {
                stream.Write(data, 0, data.Length);
            }
            catch { }
        }

        public string ReceiveMessage()
        {
            byte[] data = new byte[256];
            StringBuilder response = new StringBuilder();
            do
            {
                int bytes = stream.Read(data, 0, data.Length);
                response.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }
            while (stream.DataAvailable);
            return response.ToString();
        }
        
        public void Dispose()
        {
            stream.Dispose();
            tcpClient.Dispose();
        }

        public string MakeMessage(string message)
        {
            return codeEnd + username + nameEnd + ipStart + IPv4Address + ipEnd + message;
        }

        public string MakeMessage(string message, string username, int code)
        {
            return code.ToString() + codeEnd + username + nameEnd + ipStart + IPv4Address + ipEnd + message;
        }

        public string getChatterName(string message)
        {
            int index1 = message.IndexOf(codeEnd) + 1;
            int index2 = message.IndexOf(nameEnd) - 1;
            string name = message.Substring(index1, index2 - index1 + 1);
            if ((name.Equals("")) || (name.Equals(" ")))
                return "NONAME";
            return name;
        }

        public string getChatterIP(string message)
        {
            int index1 = message.IndexOf(ipStart) + 1;
            int index2 = message.IndexOf(ipEnd) - 1;
            return message.Substring(index1, index2 - index1 + 1);
        }

        public int getCode(string message)
        {
            int index = message.IndexOf(codeEnd);
            if (index == 1)
            {
                string code = message.Substring(0, 1);
                return Int32.Parse(code);
            }
            return 0;
        }

        public string getMessage(string message)
        {
            int index = message.IndexOf(ipEnd) + 1;
            return message.Remove(0, index);
        }
    }
}
