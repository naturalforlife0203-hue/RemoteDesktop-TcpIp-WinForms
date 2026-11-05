using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace RemoteServer
{
    public partial class Server : Form
    {
        private TcpListener serverListener;
        private TcpClient connectedClient;
        private NetworkStream networkStream;
        private BinaryWriter writer;
        private BinaryReader reader;
        private Thread listenThread;
        private Thread screenThread;
        private Thread commandThread;
        private bool isRunning = false;

        [DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);
        private const uint KEYEVENTF_KEYUP = 0x0002;

        public Server()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (!isRunning)
            {
                try
                {
                    int port = int.Parse(txtPort.Text.Trim());
                    serverListener = new TcpListener(IPAddress.Any, port);
                    serverListener.Start();
                    isRunning = true;

                    lblStatus.Text = "Status: Đang lắng nghe kết nối...";
                    lblStatus.ForeColor = Color.DarkOrange;
                    btnStart.Enabled = false;
                    btnStart.BackColor = Color.Gray;

                    listenThread = new Thread(ListenForClient) { IsBackground = true };
                    listenThread.Start();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi mở cổng: " + ex.Message);
                }
            }
        }

        private void ListenForClient()
        {
            try
            {
                connectedClient = serverListener.AcceptTcpClient();
                networkStream = connectedClient.GetStream();
                reader = new BinaryReader(networkStream);
                writer = new BinaryWriter(networkStream);

                string clientPin = reader.ReadString();
                if (clientPin != txtPin.Text.Trim())
                {
                    writer.Write(false);
                    connectedClient.Close();
                    lblStatus.Text = "Status: Từ chối (Sai mã PIN)";
                    lblStatus.ForeColor = Color.Red;
                    btnStart.Enabled = true;
                    btnStart.BackColor = Color.FromArgb(30, 144, 255);
                    isRunning = false;
                    serverListener.Stop();
                    return;
                }

                writer.Write(true);
                lblStatus.Text = "Status: Client đã kết nối!";
                lblStatus.ForeColor = Color.Green;
                AppendChatLog("System: Client đã kết nối thành công.");

                screenThread = new Thread(SendScreenStream) { IsBackground = true };
                screenThread.Start();

                commandThread = new Thread(ReceiveCommands) { IsBackground = true };
                commandThread.Start();
            }
            catch { }
        }

        private void SendScreenStream()
        {
            Rectangle bounds = Screen.PrimaryScreen.Bounds;

            while (isRunning && connectedClient != null && connectedClient.Connected)
            {
                try
                {
                    using (Bitmap bmp = new Bitmap(bounds.Width, bounds.Height))
                    {
                        using (Graphics g = Graphics.FromImage(bmp))
                        {
                            g.CopyFromScreen(0, 0, 0, 0, bounds.Size);
                        }

                        using (MemoryStream ms = new MemoryStream())
                        {
                            bmp.Save(ms, ImageFormat.Jpeg);
                            byte[] buffer = ms.ToArray();

                            lock (writer)
                            {
                                writer.Write("SCREEN");
                                writer.Write(buffer.Length);
                                writer.Write(buffer);
                                writer.Flush();
                            }
                        }
                    }
                    Thread.Sleep(33); // ~30 FPS
                }
                catch { break; }
            }
        }

        private void ReceiveCommands()
        {
            Rectangle bounds = Screen.PrimaryScreen.Bounds;

            while (isRunning && connectedClient != null && connectedClient.Connected)
            {
                try
                {
                    string command = reader.ReadString();

                    if (command == "MOVE")
                    {
                        float percentX = reader.ReadSingle();
                        float percentY = reader.ReadSingle();
                        int realX = (int)(percentX * bounds.Width);
                        int realY = (int)(percentY * bounds.Height);
                        Cursor.Position = new Point(realX, realY);
                    }
                    else if (command == "L_DOWN")
                    {
                        mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                    }
                    else if (command == "L_UP")
                    {
                        mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                    }
                    else if (command == "R_DOWN")
                    {
                        mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0);
                    }
                    else if (command == "R_UP")
                    {
                        mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
                    }
                    else if (command == "KEY_DOWN")
                    {
                        byte keyCode = (byte)reader.ReadInt32();
                        keybd_event(keyCode, 0, 0, 0);
                    }
                    else if (command == "KEY_UP")
                    {
                        byte keyCode = (byte)reader.ReadInt32();
                        keybd_event(keyCode, 0, KEYEVENTF_KEYUP, 0);
                    }
                    else if (command == "CHAT")
                    {
                        string msg = reader.ReadString();
                        AppendChatLog("[Client]: " + msg);
                    }
                    else if (command == "FILE")
                    {
                        string fileName = reader.ReadString();
                        int fileSize = reader.ReadInt32();
                        byte[] fileBytes = reader.ReadBytes(fileSize);

                        string savePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName);
                        File.WriteAllBytes(savePath, fileBytes);
                        AppendChatLog("[File]: Đã nhận " + fileName + " (Lưu tại Desktop)");
                    }
                }
                catch { break; }
            }
        }

        // Server gửi tin nhắn phản hồi về Client
        private void btnSendReply_Click(object sender, EventArgs e)
        {
            string reply = txtServerReply.Text.Trim();
            if (!string.IsNullOrEmpty(reply) && connectedClient != null && connectedClient.Connected && writer != null)
            {
                try
                {
                    lock (writer)
                    {
                        writer.Write("SERVER_CHAT");
                        writer.Write(reply);
                        writer.Flush();
                    }
                    AppendChatLog("[Tôi]: " + reply);
                    txtServerReply.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi gửi tin: " + ex.Message);
                }
            }
        }

        private void AppendChatLog(string text)
        {
            if (txtChatLog.InvokeRequired)
            {
                txtChatLog.Invoke((Action)(() => AppendChatLog(text)));
                return;
            }
            txtChatLog.AppendText(text + Environment.NewLine);
        }

        private void Server_FormClosing(object sender, FormClosingEventArgs e)
        {
            isRunning = false;
            if (connectedClient != null) connectedClient.Close();
            if (serverListener != null) serverListener.Stop();
        }
    }
}