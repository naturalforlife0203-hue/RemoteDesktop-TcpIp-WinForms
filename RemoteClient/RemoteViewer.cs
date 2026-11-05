using System;
using System.Drawing;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace RemoteClient
{
    public class RemoteViewer : Form
    {
        private TcpClient client;
        private NetworkStream networkStream;
        private BinaryWriter writer;
        private BinaryReader reader;

        private PictureBox pictureBoxScreen;
        private CheckBox chkControl;
        private Button btnHideCursor;
        private Button btnToggleChat;
        private Button btnSendFile;
        private Button btnFullscreen;
        private Button btnDisconnect;

        // Khung Chat tích hợp bên phải
        private Panel panelChat;
        private TextBox txtChatLog;
        private TextBox txtClientMsg;
        private Button btnSendChat;

        private Thread receiveThread;
        private bool isRunning = true;
        private bool isCursorHidden = false;
        private bool isFullscreen = false;

        public RemoteViewer(TcpClient client, NetworkStream stream, BinaryWriter writer, BinaryReader reader)
        {
            this.client = client;
            this.networkStream = stream;
            this.writer = writer;
            this.reader = reader;

            InitViewerUI();

            receiveThread = new Thread(ReceiveStreamAndMessages) { IsBackground = true };
            receiveThread.Start();
        }

        private void InitViewerUI()
        {
            this.Text = "Remote Desktop Session - Điều khiển máy tính từ xa";
            this.Size = new Size(1250, 750);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.Black;
            this.KeyPreview = true;

            // 1. Toolbar trên cùng
            Panel toolbar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 44,
                BackColor = Color.FromArgb(28, 30, 38),
                Padding = new Padding(10, 0, 10, 0)
            };

            chkControl = new CheckBox
            {
                Text = "🎮 Quyền chuột/phím",
                ForeColor = Color.FromArgb(255, 215, 100),
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(16, 12),
                Checked = false
            };

            btnHideCursor = CreateToolButton("👁️ Ẩn chuột", 205, (s, e) =>
            {
                isCursorHidden = !isCursorHidden;
                btnHideCursor.Text = isCursorHidden ? "👁️ Hiện chuột" : "👁️ Ẩn chuột";
                if (isCursorHidden) Cursor.Hide(); else Cursor.Show();
            });

            btnToggleChat = CreateToolButton("💬 Hộp thư Chat", 330, (s, e) =>
            {
                panelChat.Visible = !panelChat.Visible;
                btnToggleChat.BackColor = panelChat.Visible ? Color.FromArgb(70, 75, 95) : Color.FromArgb(45, 48, 60);
            });

            btnSendFile = CreateToolButton("📁 Gửi File", 460, (s, e) =>
            {
                using (OpenFileDialog ofd = new OpenFileDialog { Title = "Chọn tập tin gửi tới Server" })
                {
                    if (ofd.ShowDialog() == DialogResult.OK && writer != null)
                    {
                        try
                        {
                            byte[] fileBytes = File.ReadAllBytes(ofd.FileName);
                            string fileName = Path.GetFileName(ofd.FileName);

                            lock (writer)
                            {
                                writer.Write("FILE");
                                writer.Write(fileName);
                                writer.Write(fileBytes.Length);
                                writer.Write(fileBytes);
                                writer.Flush();
                            }
                            AppendChatLog("[Hệ thống]: Đã gửi file " + fileName);
                            MessageBox.Show("Đã gửi tệp: " + fileName, "Gửi File", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Lỗi gửi file: " + ex.Message);
                        }
                    }
                }
            });

            btnFullscreen = CreateToolButton("⛶ Toàn màn hình", 585, (s, e) =>
            {
                isFullscreen = !isFullscreen;
                if (isFullscreen)
                {
                    this.FormBorderStyle = FormBorderStyle.None;
                    this.WindowState = FormWindowState.Maximized;
                    btnFullscreen.Text = "⛶ Thu nhỏ";
                }
                else
                {
                    this.FormBorderStyle = FormBorderStyle.Sizable;
                    this.WindowState = FormWindowState.Normal;
                    btnFullscreen.Text = "⛶ Toàn màn hình";
                }
            });

            btnDisconnect = new Button
            {
                Text = "✕ Ngắt kết nối",
                ForeColor = Color.White,
                BackColor = Color.FromArgb(220, 53, 69),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold),
                Size = new Size(115, 30),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(toolbar.Width - 135, 7)
            };
            btnDisconnect.FlatAppearance.BorderSize = 0;
            btnDisconnect.Click += (s, e) => this.Close();

            toolbar.Controls.Add(chkControl);
            toolbar.Controls.Add(btnHideCursor);
            toolbar.Controls.Add(btnToggleChat);
            toolbar.Controls.Add(btnSendFile);
            toolbar.Controls.Add(btnFullscreen);
            toolbar.Controls.Add(btnDisconnect);

            // 2. Khung Chat tích hợp (Nằm dock bên phải)
            panelChat = new Panel
            {
                Dock = DockStyle.Right,
                Width = 280,
                BackColor = Color.FromArgb(246, 248, 250),
                Visible = true // Mặc định mở sẵn
            };

            Label lblChatHeader = new Label
            {
                Text = "💬 Trao đổi với Host",
                Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(50, 55, 65),
                Dock = DockStyle.Top,
                Height = 35,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(10, 0, 0, 0)
            };

            txtChatLog = new TextBox
            {
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                BackColor = Color.White,
                Font = new Font("Segoe UI", 9F),
                Dock = DockStyle.Fill
            };

            Panel pnlBottomChat = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 42,
                BackColor = Color.FromArgb(240, 242, 245),
                Padding = new Padding(5)
            };

            txtClientMsg = new TextBox
            {
                Font = new Font("Segoe UI", 9.5F),
                Dock = DockStyle.Fill
            };

            btnSendChat = new Button
            {
                Text = "Gửi",
                Dock = DockStyle.Right,
                Width = 60,
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold)
            };
            btnSendChat.FlatAppearance.BorderSize = 0;
            btnSendChat.Click += (s, e) => SendChatMessage();
            txtClientMsg.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { SendChatMessage(); e.SuppressKeyPress = true; } };

            pnlBottomChat.Controls.Add(txtClientMsg);
            pnlBottomChat.Controls.Add(btnSendChat);

            panelChat.Controls.Add(txtChatLog);
            panelChat.Controls.Add(pnlBottomChat);
            panelChat.Controls.Add(lblChatHeader);

            // 3. Màn hình remote
            pictureBoxScreen = new PictureBox
            {
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Black
            };

            pictureBoxScreen.MouseMove += PictureBoxScreen_MouseMove;
            pictureBoxScreen.MouseDown += PictureBoxScreen_MouseDown;
            pictureBoxScreen.MouseUp += PictureBoxScreen_MouseUp;
            this.KeyDown += RemoteViewer_KeyDown;
            this.KeyUp += RemoteViewer_KeyUp;
            this.FormClosing += RemoteViewer_FormClosing;

            this.Controls.Add(pictureBoxScreen);
            this.Controls.Add(panelChat);
            this.Controls.Add(toolbar);
        }

        private Button CreateToolButton(string text, int x, EventHandler onClick)
        {
            Button btn = new Button
            {
                Text = text,
                ForeColor = Color.FromArgb(235, 240, 248),
                BackColor = Color.FromArgb(45, 48, 60),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8.5F),
                Size = new Size(118, 30),
                Location = new Point(x, 7)
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.Click += onClick;
            return btn;
        }

        private void SendChatMessage()
        {
            string msg = txtClientMsg.Text.Trim();
            if (!string.IsNullOrEmpty(msg) && writer != null)
            {
                try
                {
                    lock (writer)
                    {
                        writer.Write("CHAT");
                        writer.Write(msg);
                        writer.Flush();
                    }
                    AppendChatLog("[Tôi]: " + msg);
                    txtClientMsg.Clear();
                }
                catch { }
            }
        }

        private void AppendChatLog(string msg)
        {
            if (txtChatLog.InvokeRequired)
            {
                txtChatLog.Invoke((Action)(() => AppendChatLog(msg)));
                return;
            }
            txtChatLog.AppendText(msg + Environment.NewLine);
        }

        private void ReceiveStreamAndMessages()
        {
            while (isRunning && client != null && client.Connected)
            {
                try
                {
                    string packetType = reader.ReadString();

                    if (packetType == "SCREEN")
                    {
                        int length = reader.ReadInt32();
                        byte[] buffer = reader.ReadBytes(length);

                        using (MemoryStream ms = new MemoryStream(buffer))
                        {
                            Image img = Image.FromStream(ms);
                            Image old = pictureBoxScreen.Image;
                            pictureBoxScreen.Image = (Image)img.Clone();
                            if (old != null) old.Dispose();
                        }
                    }
                    else if (packetType == "SERVER_CHAT")
                    {
                        string serverMsg = reader.ReadString();
                        AppendChatLog("[Server]: " + serverMsg);
                    }
                }
                catch { break; }
            }
        }

        private void PictureBoxScreen_MouseMove(object sender, MouseEventArgs e)
        {
            if (isRunning && chkControl.Checked && pictureBoxScreen.Image != null)
            {
                try
                {
                    float percentX = (float)e.X / pictureBoxScreen.Width;
                    float percentY = (float)e.Y / pictureBoxScreen.Height;
                    lock (writer)
                    {
                        writer.Write("MOVE");
                        writer.Write(percentX);
                        writer.Write(percentY);
                    }
                }
                catch { }
            }
        }

        private void PictureBoxScreen_MouseDown(object sender, MouseEventArgs e)
        {
            if (isRunning && chkControl.Checked)
            {
                try
                {
                    lock (writer)
                    {
                        if (e.Button == MouseButtons.Left) writer.Write("L_DOWN");
                        else if (e.Button == MouseButtons.Right) writer.Write("R_DOWN");
                    }
                }
                catch { }
            }
        }

        private void PictureBoxScreen_MouseUp(object sender, MouseEventArgs e)
        {
            if (isRunning && chkControl.Checked)
            {
                try
                {
                    lock (writer)
                    {
                        if (e.Button == MouseButtons.Left) writer.Write("L_UP");
                        else if (e.Button == MouseButtons.Right) writer.Write("R_UP");
                    }
                }
                catch { }
            }
        }

        private void RemoteViewer_KeyDown(object sender, KeyEventArgs e)
        {
            if (isRunning && chkControl.Checked && !txtClientMsg.Focused)
            {
                try
                {
                    lock (writer)
                    {
                        writer.Write("KEY_DOWN");
                        writer.Write((int)e.KeyCode);
                    }
                }
                catch { }
            }
        }

        private void RemoteViewer_KeyUp(object sender, KeyEventArgs e)
        {
            if (isRunning && chkControl.Checked && !txtClientMsg.Focused)
            {
                try
                {
                    lock (writer)
                    {
                        writer.Write("KEY_UP");
                        writer.Write((int)e.KeyCode);
                    }
                }
                catch { }
            }
        }

        private void RemoteViewer_FormClosing(object sender, FormClosingEventArgs e)
        {
            isRunning = false;
            if (isCursorHidden) Cursor.Show();
            try { if (client != null) client.Close(); } catch { }
        }
    }
}