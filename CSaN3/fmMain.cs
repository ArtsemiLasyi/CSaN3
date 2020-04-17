using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
        private const byte NUM_OF_UDP_PACKET = 10;
        private const string broadcast = "192.168.0.255";
        private bool alive = false;
        private string username;
        private TcpListener tcpListener;
        private List<ChatParticipant> Chatters = new List<ChatParticipant>();
        private IPAddress localIPAddress;


        // Отправка широковещательного пакета с именем пользователя
        private void SendBroadcastMessage()
        {
            // Отправить широковещательный пакет с именем
            UdpClient udpClient = new UdpClient(broadcast, udp_port);
            udpClient.EnableBroadcast = true;
            var data = Encoding.Unicode.GetBytes(username);
            Task.Factory.StartNew(ListeningForConnections);
            for (int i = 0; i < NUM_OF_UDP_PACKET; i++)
            {
                udpClient.Send(data, data.Length);
            }
            udpClient.Dispose();
        }

        // Ожидание Broadcast пакетов новых пользователей
        private void ListenBroadcastUDP()
        {
            var udpListener = new UdpClient(udp_port);
            udpListener.EnableBroadcast = true;
            while (true)
            {
                IPEndPoint remoteHost = null;
                var receivedData = udpListener.Receive(ref remoteHost);
                if (alive)
                {
                    if (remoteHost.Address.ToString() != localIPAddress.ToString())
                    {
                        if (!AlreadyConnected(remoteHost.Address))
                        {
                            var chatter = new ChatParticipant();
                            chatter.IPEndPoint = remoteHost;
                            chatter.username = Encoding.Unicode.GetString(receivedData);
                            chatter.Connect();
                            chatter.SendMessage(" подключился!", 1);
                            Chatters.Add(chatter);
                            Task.Factory.StartNew(() => ListenTCP(Chatters[Chatters.IndexOf(chatter)]));
                        }
                    }
                }
            }
        }

        // Ожидание подключений
        private void ListeningForConnections()
        {
            tcpListener = new TcpListener(localIPAddress, tcp_port);
            tcpListener.Start();
            while (alive)
            {
                if (tcpListener.Pending())
                {
                    var chatter = new ChatParticipant();
                    chatter.tcpClient = tcpListener.AcceptTcpClient();
                    chatter.IPEndPoint = ((IPEndPoint)chatter.tcpClient.Client.RemoteEndPoint);
                    chatter.stream = chatter.tcpClient.GetStream();
                    chatter.SendMessage(" подключился!", 1);
                    Chatters.Add(chatter);
                    Task.Factory.StartNew(() => ListenTCP(Chatters[Chatters.IndexOf(chatter)]));
                }
            }
            tcpListener.Stop();
        }

        // Обработка сообщений от узлов
        private void ListenTCP(ChatParticipant chatter)
        {
            var flag = chatter.Listen;
            while (flag)
            {
                if (chatter.stream.DataAvailable)
                {
                    string data = chatter.ReceiveMessage();
                    string message = chatter.getMessage(data);
                    if (chatter.getCode(data) == 3)
                    {
                        chatter.Listen = false;
                        Chatters.Remove(chatter);
                        chatter.Dispose();
                    }
                    DisplayAMessage(message);
                }
            }
        }

        // Отправить сообщение всем пользователям
        private void SendMessageByTCP(string message)
        {
            foreach (var chatter in Chatters)
            {
                chatter.SendMessage(message, 2);
            }
        }

        // Отключение
        private void Disconnect()
        {
            foreach (var user in Chatters)
            {
                user.Listen = false;
                user.SendMessage(" отключился!", 3);
                user.Dispose();
            }
            Chatters.Clear();
        }

        // Подключен ли уже пользователь с данным ip
        private bool AlreadyConnected(IPAddress ip)
        {
            foreach (var chatter in Chatters)
            {
                if (chatter.IPv4Address.ToString()==ip.ToString())
                {
                    return true;
                }
            }
            return false;
        }

        // Отобразить сообщение message пользователя username
        private void DisplayAMessage(string message)
        {
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
            SendMessageByTCP(message);
            string time = DateTime.Now.ToString();
            message = time + " " + message;
            tbChat.Text = username + message + "\r\n" + tbChat.Text;
            tbSendText.Text = "";
        }

        private IPAddress LocalIPAddress()
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip;
                }
            }
            return null;
        }

        public fmMain()
        {
            InitializeComponent();
            localIPAddress = LocalIPAddress();
            Task.Factory.StartNew(ListenBroadcastUDP);
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
            username = tbName.Text;
            tbName.ReadOnly = true;
            bbConnect.Enabled = false;
            bbDisconnect.Enabled = true;
            bbSendText.Enabled = true;
            SendBroadcastMessage();
        }

        private void bbDisconnect_Click(object sender, EventArgs e)
        {
            alive = false;
            bbConnect.Enabled = true;
            bbDisconnect.Enabled = false;
            bbSendText.Enabled = false;
        }
    }
}