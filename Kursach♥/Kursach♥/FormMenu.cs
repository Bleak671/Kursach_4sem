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
using System.Threading;

namespace Kursach_
{
    public partial class FormMenu : Form
    {
        public FormMenu(string str)
        {
            InitializeComponent();
            userName = str;
        }

        const int bufferSize = 65536;
        const int typeSize = 1;
        const int typeShift = 0;
        const int lengthSize = 2;
        const int lengthShift = 1;
        const int hdrSize = 3;
        const int dataShift = 3;
        const int TCP_PORT = 9875;
        const int UDP_PORT = 9876;

        bool isGameStarted = false;
        bool isHostConnected = false;
        bool isConnected = false;
        bool isYouHost = false;
        String userName;
        String userIp = GetLocalIPAddress();
        String fullUserName;
        Client host;
        Task TcpRec;
        Task UdpRec;
        public List<Client> clients = new List<Client>();

        public class Client
        {
            public NetworkStream stream;
            public IPEndPoint iep;
            public String name;
            public Client(IPEndPoint iepc, string str, NetworkStream streamlocal)
            {
                stream = streamlocal;
                iep = iepc;
                name = str;
            }

        }

        class Packet
        {
            public byte type;
            public UInt16 length;
            public byte[] data;

            public Packet(byte typelocal)
            {
                type = typelocal;
                length = hdrSize;
                data = null;
            }

            public Packet(byte typelocal, string datalocal)
            {
                type = typelocal;
                data = Encoding.Unicode.GetBytes(datalocal);
                length = (UInt16)(hdrSize + data.Length);
            }

            public Packet(byte[] datalocal)
            {
                type = datalocal[0];
                length = BitConverter.ToUInt16(datalocal, typeSize);
                data = new byte[length - hdrSize];
                Buffer.BlockCopy(datalocal, hdrSize, data, 0, length - hdrSize);
            }
            public byte[] getBytes()
            {
                byte[] datalocal = new byte[length];
                Buffer.BlockCopy(BitConverter.GetBytes(type), 0, datalocal, typeShift, typeSize);
                Buffer.BlockCopy(BitConverter.GetBytes(length), 0, datalocal, lengthShift, lengthSize);
                if (data != null)
                    Buffer.BlockCopy(data, 0, datalocal, dataShift, length - hdrSize);
                return datalocal;
            }
        }

        /*public static string GetIPAddress()
        {
            string externalip = new WebClient().DownloadString("http://icanhazip.com");
            externalip = externalip.Trim();
            return externalip;
        }*/

        public static string GetLocalIPAddress()
        {
            string localIP;
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0);
            try
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint Point = socket.LocalEndPoint as IPEndPoint;
                localIP = Point.Address.ToString();
            }
            catch
            {
                IPEndPoint Point = socket.LocalEndPoint as IPEndPoint;
                localIP = Point.Address.ToString();
            }
            return localIP;
        }

        private void UdpReceive()
        {
            while (isConnected)
            {
                UdpClient client = new UdpClient(UDP_PORT);
                IPEndPoint iep = null;
                byte[] data = client.Receive(ref iep);
                Packet message = new Packet(data);
                if (String.Compare(iep.Address.ToString(), GetLocalIPAddress()) == 0)
                    continue;
                client.Close();
                switch (message.type)
                {
                    case 0: //Запрос на подключение к хосту
                        TcpClient clientTcp = new TcpClient(iep.Address.ToString(), TCP_PORT);
                        NetworkStream stream = clientTcp.GetStream();
                        Client cl = new Client(new IPEndPoint(iep.Address, TCP_PORT), Encoding.Unicode.GetString(message.data), stream);
                        if (!clients.Contains(cl))
                        {
                            message = new Packet(0, textBoxName.Text);
                            stream.Write(message.getBytes(), 0, message.getBytes().Length);
                            clients.Add(cl);
                            this.Invoke(new MethodInvoker(() =>
                            {
                                listLobby.Items.Add(cl.name);
                            }));
                        }
                        break;
                    case 1: 

                        break;
                }
            }
        }

        private void TcpReceive()
        {
            while (isConnected)
            {
                TcpListener listener = new TcpListener(IPAddress.Parse(GetLocalIPAddress()), TCP_PORT);
                listener.Start();
                TcpClient client = listener.AcceptTcpClient();
                if (String.Compare(((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString(), GetLocalIPAddress()) == 0)
                    continue;
                NetworkStream stream = client.GetStream();
                byte[] data = new byte[bufferSize];
                stream.Read(data, 0, data.Length);
                Packet MessageTcp = new Packet(data);
                IPEndPoint iep = new IPEndPoint(IPAddress.Parse(((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString()), TCP_PORT);
                switch (MessageTcp.type)
                {
                    case 0: //Установка TCP потока от клиента
                        if (!isGameStarted && isYouHost)
                        {
                            host = new Client(iep, Encoding.Unicode.GetString(MessageTcp.data), stream);
                            this.Invoke(new MethodInvoker(() =>
                            {
                                listLobby.Items.Add(host.name);
                            }));
                        }
                        break;
                    case 1: 
                        MessageBox.Show(Encoding.Unicode.GetString(MessageTcp.data));
                        if (!isGameStarted)
                        {
                            Client ClientCheck = new Client(iep, Encoding.Unicode.GetString(MessageTcp.data), stream);
                            if (!isHostConnected)
                            {
                                host = ClientCheck;
                                isHostConnected = true;
                            }
                            this.Invoke(new MethodInvoker(() =>
                            {
                                listLobby.Items.Add(ClientCheck.name);
                            }));
                        }
                        break;
                    case 2: //Оповещение пользователей о новом
                        if (!isGameStarted)
                        {
                            Client ClientCheck = new Client(iep, Encoding.Unicode.GetString(MessageTcp.data), stream);
                            if (!clients.Contains(ClientCheck))
                            {
                                this.Invoke(new MethodInvoker(() =>
                                {
                                    listLobby.Items.Add(ClientCheck.name);
                                    listChat.Items.Add(DateTime.Now.Hour + "." + DateTime.Now.Minute + " " + ClientCheck.name + ": присоединился");
                                }));
                            }
                        }
                        break;
                    case 10: //Отправка сообщения в чат
                        foreach (Client ClientsCheck in clients)
                        {
                            if (ClientsCheck.iep.Equals(iep))
                            {
                                this.Invoke(new MethodInvoker(() =>
                                {
                                    listChat.Items.Add(DateTime.Now + " " + ClientsCheck.name + ": " + Encoding.Unicode.GetString(MessageTcp.data));
                                }));
                                break;
                            }
                        }
                        break;
                    default:
                        break;
                }
                client.Close();
                stream.Close();
                listener.Stop();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            fullUserName = userName + '(' + userIp + ')';
            textBoxName.Text = fullUserName;
        }

        private void btnConnectLobby_Click(object sender, EventArgs e)
        {
            try
            {
                if (!isConnected)
                {
                    isConnected = true;
                    TcpRec = new Task(TcpReceive);
                    TcpRec.Start();
                    UdpRec = new Task(UdpReceive);
                    UdpRec.Start();
                }

                UdpClient udpClient = new UdpClient();
                Packet pck = new Packet(0, userName);
                udpClient.Send(pck.getBytes(), pck.getBytes().Length, new IPEndPoint(IPAddress.Parse(textBoxHostIP.Text), UDP_PORT));
                udpClient.Close();
                MessageBox.Show("Запрос на подключение отправлен");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCreateLobby_Click(object sender, EventArgs e)
        {
            if (!isConnected)
            {
                isConnected = true;
                TcpRec = new Task(TcpReceive);
                TcpRec.Start();
                UdpRec = new Task(UdpReceive);
                UdpRec.Start();
            }
            isYouHost = true;
            MessageBox.Show("Лобби создано!\nПодключение через ваш IP");
            String wtf = GetLocalIPAddress();
            host = new Client(new IPEndPoint(IPAddress.Parse(wtf), TCP_PORT), null, null);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLeaveLobby_Click(object sender, EventArgs e)
        {
            btnConnectLobby.Enabled = true;
            btnCreateLobby.Enabled = true;
            btnSendMessage.Enabled = false;
            btnLeaveLobby.Enabled = false;
            btnStartGame.Enabled = false;
            clients = new List<Client>();
            isHostConnected = false;
        }
    }
}
