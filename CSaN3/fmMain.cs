using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace CSaN3
{
    public partial class fmMain : Form
    {
        private const int udp_port = 11000;
        private const int tcp_port = 7171;
        private const byte packetsNumber = 10;
        private bool alive = false;
  

        private TcpListener tcpListener;
        private List<ChatParticipant> Chatters = new List<ChatParticipant>();
        private IPAddress localIP;
        public string username;
        private const int CONNECT = 1;
        private const int MESSAGE = 2;
        private const int DISCONNECT = 3;
        private object synclock = new object();

        // Отправка широковещательного пакета
        private void SendBroadcast()
        {
            UdpClient udpClient = new UdpClient(IPAddress.Broadcast.ToString(), udp_port);
            udpClient.EnableBroadcast = true;
            string message = username + "|" + localIP;
            var data = Encoding.Unicode.GetBytes(message);
            Task.Factory.StartNew(ListenForConnections);
            for (int i = 0; i < packetsNumber; i++)
            {
                udpClient.Send(data, data.Length);
                Thread.Sleep(20);
            }
            udpClient.Dispose();
        }

        // Ожидание Broadcast пакетов новых пользователей
        private void ListenUDP()
        {
            var udpListener = new UdpClient(udp_port);
            udpListener.EnableBroadcast = true;
            while (true)
            {
                IPEndPoint host = null;
                var receivedData = udpListener.Receive(ref host);
                if (alive)
                {
                    if (!host.Address.Equals(localIP))
                    {
                        if (!isConnected(host.Address))
                        {
                            var chatter = new ChatParticipant();
                            chatter.IPv4Address = host.Address;
                            string message = Encoding.Unicode.GetString(receivedData);
                            int index = message.LastIndexOf("|")+1;
                            chatter.username = message.Substring(0, index-1);
                            chatter.Connect();
                            chatter.SendMessage(chatter.username + " подключился!", CONNECT);
                            Chatters.Add(chatter);
                            Task.Factory.StartNew(() => ListenChatter(Chatters[Chatters.IndexOf(chatter)]));
                        }
                    }
                }
            }
        }

        // Ожидание подключений
        private void ListenForConnections()
        {
            tcpListener = new TcpListener(localIP, tcp_port);
            tcpListener.Start();
            while (alive)
            {
                if (tcpListener.Pending())
                {
                    var chatter = new ChatParticipant();
                    chatter.tcpClient = tcpListener.AcceptTcpClient();

                    chatter.IPv4Address = ((IPEndPoint)chatter.tcpClient.Client.RemoteEndPoint).Address;

                    chatter.stream = chatter.tcpClient.GetStream();
                    Chatters.Add(chatter);
                    Task.Factory.StartNew(() => ListenChatter(Chatters[Chatters.IndexOf(chatter)]));
                }
            }
            tcpListener.Stop();
        }

        // Обработка сообщений от пользователя
        private void ListenChatter(ChatParticipant chatter)
        {
            while (chatter.alive)
            {
                if (chatter.stream.DataAvailable)
                {
                    string data = chatter.ReceiveMessage();
                    string message = data;
                    if (chatter.getCode(data) == CONNECT)
                    {
                        chatter.username = chatter.getChatterName(message);
                    }
                    if (chatter.getCode(data) == DISCONNECT)
                    {
                        chatter.alive = false;
                        Chatters.Remove(chatter);
                        chatter.Dispose();
                    }
                    DisplayMessage(message);
                }
            }
        }

        // Отправить сообщение всем пользователям
        private void SendMessageTCP(string message)
        {
            foreach (var chatter in Chatters)
            {
                chatter.SendMessage(message, MESSAGE);
            }
        }

        // Отключение
        private void Disconnect()
        {
            lock (synclock)
            {
                foreach (var chatter in Chatters)
                {
                    chatter.alive = false;
                    chatter.SendMessage(" отключился!", DISCONNECT);
                    chatter.Dispose();
                }
                Chatters.Clear();
            }
        }

        // Подключен ли уже пользователь с данным ip
        private bool isConnected(IPAddress ip)
        {
            foreach (var chatter in Chatters)
            {
                string IP = chatter.IPv4Address.ToString();
                if (IP.Equals(ip.ToString()))
                    return true;
            }
            return false;
        }

        // Отобразить сообщение пользователя
        private void DisplayMessage(string message)
        {
            //Обращение к исходному потоку
            this.Invoke(new MethodInvoker(() =>
            {
                string time = DateTime.Now.ToString();
                tbChat.Text = time + " " + message + "\r\n" + tbChat.Text;
            }));
        }

        // Отправить сообщение
        private void SendMessage()
        {
            string message = tbSendText.Text;
            SendMessageTCP(message);
            string time = DateTime.Now.ToString();

            var temp = new ChatParticipant();
            temp.username = username;
            temp.IPv4Address = localIP;
            message = temp.MakeMessage(message);
            message = time + " " + message;

            tbChat.Text = message + "\r\n" + tbChat.Text;
            tbSendText.Text = "";
        }

        // Локальный IP-адрес
        private IPAddress LocalIPAddress()
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    return ip;
            }
            return null;
        }

        public fmMain()
        {
            InitializeComponent();
            localIP = LocalIPAddress();
            Task.Factory.StartNew(ListenUDP);
        }

        private void bbSendText_Click(object sender, EventArgs e)
        {
            SendMessage();
        }

        private void bbName_Click(object sender, EventArgs e)
        {
            tbName.ReadOnly = false;
        }

        private void bbConnect_Click(object sender, EventArgs e)
        {
            alive = true;
            tbChat.Clear();
            username = tbName.Text;
            tbName.ReadOnly = true;
            bbConnect.Enabled = false;
            bbDisconnect.Enabled = true;
            bbSendText.Enabled = true;
            SendBroadcast();
        }

        private void bbDisconnect_Click(object sender, EventArgs e)
        {
            alive = false;
            bbConnect.Enabled = true;
            bbDisconnect.Enabled = false;
            bbSendText.Enabled = false;
            Disconnect();
        }
    }
}