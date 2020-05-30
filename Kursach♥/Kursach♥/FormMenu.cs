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
        const int APP_PORT = 9875;

        bool isGameStarted = false;
        bool isHostConnected = false;
        bool isConnected = false;
        bool isYouHost = false;
        String userName;
        String userIp = GetLocalIPAddress();
        String fullUserName;
        Client host;
        Task TcpRec;
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

        private void TcpReceive()
        {
            while (isConnected)
            {
                TcpListener listener = new TcpListener(IPAddress.Parse(GetLocalIPAddress()), APP_PORT);
                listener.Start();
                TcpClient client = listener.AcceptTcpClient();
                if (String.Compare(((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString(), GetLocalIPAddress()) == 0)
                    continue;
                NetworkStream stream = client.GetStream();
                byte[] data = new byte[bufferSize];
                stream.Read(data, 0, data.Length);
                Packet MessageTcp = new Packet(data);
                if (MessageTcp.length > 3)
                    MessageBox.Show(Encoding.Unicode.GetString(MessageTcp.data));
                IPEndPoint iep = new IPEndPoint(IPAddress.Parse(((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString()), APP_PORT);
                switch (MessageTcp.type)
                {
                    case 0: //Запрос на подключение к лобби
                        if (!isGameStarted && isYouHost)
                        {
                            client.Close();
                            client = new TcpClient(new IPEndPoint(iep.Address, APP_PORT));
                            stream.Close();
                            stream = client.GetStream();
                            Client ClientCheck = new Client(iep, Encoding.Unicode.GetString(MessageTcp.data), stream);
                            if (!clients.Contains(ClientCheck))
                            {
                                clients.Add(ClientCheck);
                                this.Invoke(new MethodInvoker(() =>
                                {
                                    listLobby.Items.Add(ClientCheck.name);
                                    listChat.Items.Add(DateTime.Now.Hour + "." + DateTime.Now.Minute + " " + ClientCheck.name + ": присоединился");
                                }));

                                MessageTcp = new Packet(1, userName);
                                MessageBox.Show(Encoding.Unicode.GetString(MessageTcp.data));
                                stream.Write(MessageTcp.getBytes(), 0, MessageTcp.getBytes().Length);

                                foreach (Client cl in clients)
                                {
                                    if (cl.iep.Address != iep.Address)
                                    {
                                        MessageTcp = new Packet(2, ClientCheck.name);
                                        cl.stream.Write(MessageTcp.getBytes(), 0, MessageTcp.getBytes().Length);
                                        MessageTcp = new Packet(1, cl.name);
                                        stream.Write(MessageTcp.getBytes(), 0, MessageTcp.getBytes().Length);
                                    }
                                }
                            }
                        }
                        break;
                    case 1: //Ответ подключаемому пользователю
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
                }

                TcpClient clientTcp = new TcpClient(textBoxHostIP.Text, APP_PORT);
                NetworkStream stream = clientTcp.GetStream();
                host = new Client(new IPEndPoint(IPAddress.Parse(textBoxHostIP.Text), APP_PORT), null, stream);
                Packet pck = new Packet(0, userName);
                stream.Write(pck.getBytes(), 0, pck.getBytes().Length);
                MessageBox.Show(Encoding.Unicode.GetString(pck.data));
                stream.Close();
                clientTcp.Close();
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
            }
            isYouHost = true;
            MessageBox.Show("Лобби создано!\nПодключение через ваш IP");
            String wtf = GetLocalIPAddress();
            host = new Client(new IPEndPoint(IPAddress.Parse(wtf), APP_PORT), null, null);
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
