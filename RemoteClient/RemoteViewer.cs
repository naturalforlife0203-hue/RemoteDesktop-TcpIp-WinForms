using System;
using System.Drawing;
using System.Drawing.Drawing2D;
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
            this.Size = new Size(1280, 760);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(18, 20, 26);
            this.KeyPreview = true;

            // 1. Toolbar phẳng
            Panel toolbar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 52,
                BackColor = Color.FromArgb(28, 31, 42),
                Padding = new Padding(12, 8, 12, 8)
            };

            // Nút Ẩn / Hiện chuột
            btnHideCursor = CreateToolButton("👁️ Ẩn chuột", 14, (s, e) =>
            {
                isCursorHidden = !isCursorHidden;
                btnHideCursor.Text = isCursorHidden ? "👁️ Hiện chuột" : "👁️ Ẩn chuột";
                if (isCursorHidden) Cursor.Hide(); else Cursor.Show();
                pictureBoxScreen.Focus();
            });

            // Nút Ẩn / Hiện khung Chat
            btnToggleChat = CreateToolButton("💬 Hộp thư", 140, (s, e) =>
            {
                panelChat.Visible = !panelChat.Visible;
                btnToggleChat.BackColor = panelChat.Visible ? Color.FromArgb(65, 75, 105) : Color.FromArgb(42, 46, 62);
                pictureBoxScreen.Focus();
            });

            // Nút Gửi File sang Host
            btnSendFile = CreateToolButton("📁 Gửi File", 266, (s, e) =>
            {
                using (OpenFileDialog ofd = new OpenFileDialog { Title = "Chọn tập tin gửi tới Host" })
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
                            AppendChatLog("[Hệ thống]: Đã gửi thành công tập tin " + fileName);
                            MessageBox.Show("Đã gửi tệp: " + fileName, "Gửi File", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Lỗi gửi file: " + ex.Message);
                        }
                    }
                }
                pictureBoxScreen.Focus();
            });

            // Nút Chuyển chế độ Toàn màn hình
            btnFullscreen = CreateToolButton("⛶ Toàn màn hình", 392, (s, e) =>
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
                pictureBoxScreen.Focus();
            });

            // Nút Ngắt kết nối
            btnDisconnect = new Button
            {
                Text = "✕ Ngắt kết nối",
                ForeColor = Color.White,
                BackColor = Color.FromArgb(220, 53, 69),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold),
                Size = new Size(125, 36),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(toolbar.Width - 142, 8),
                Cursor = Cursors.Hand,
                TabStop = false
            };
            btnDisconnect.FlatAppearance.BorderSize = 0;
            ApplyRoundedRegion(btnDisconnect, 10);
            btnDisconnect.Click += (s, e) => this.Close();

            toolbar.Controls.Add(btnHideCursor);
            toolbar.Controls.Add(btnToggleChat);
            toolbar.Controls.Add(btnSendFile);
            toolbar.Controls.Add(btnFullscreen);
            toolbar.Controls.Add(btnDisconnect);

            // 2. Khung Chat tích hợp bên phải
            panelChat = new Panel
            {
                Dock = DockStyle.Right,
                Width = 280,
                BackColor = Color.FromArgb(246, 248, 252),
                Visible = true
            };

            Label lblChatHeader = new Label
            {
                Text = "💬 Hộp thư trao đổi",
                Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(45, 50, 65),
                Dock = DockStyle.Top,
                Height = 38,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(12, 0, 0, 0)
            };

            txtChatLog = new TextBox
            {
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                BackColor = Color.White,
                Font = new Font("Segoe UI", 9F),
                BorderStyle = BorderStyle.None,
                Dock = DockStyle.Fill
            };

            Panel pnlBottomChat = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 44,
                BackColor = Color.FromArgb(238, 240, 246),
                Padding = new Padding(6)
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
                Width = 65,
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                TabStop = false
            };
            btnSendChat.FlatAppearance.BorderSize = 0;
            btnSendChat.Click += (s, e) => SendChatMessage();
            txtClientMsg.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { SendChatMessage(); e.SuppressKeyPress = true; } };

            pnlBottomChat.Controls.Add(txtClientMsg);
            pnlBottomChat.Controls.Add(btnSendChat);

            panelChat.Controls.Add(txtChatLog);
            panelChat.Controls.Add(pnlBottomChat);
            panelChat.Controls.Add(lblChatHeader);

            // 3. Màn hình điều khiển Remote
            pictureBoxScreen = new PictureBox
            {
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Black
            };

            pictureBoxScreen.MouseMove += PictureBoxScreen_MouseMove;
            pictureBoxScreen.MouseDown += PictureBoxScreen_MouseDown;
            pictureBoxScreen.MouseUp += PictureBoxScreen_MouseUp;
            this.KeyPress += RemoteViewer_KeyPress;
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
                ForeColor = Color.FromArgb(230, 235, 245),
                BackColor = Color.FromArgb(42, 46, 62),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F),
                Size = new Size(116, 36),
                Location = new Point(x, 8),
                Cursor = Cursors.Hand,
                TabStop = false
            };
            btn.FlatAppearance.BorderSize = 0;
            ApplyRoundedRegion(btn, 8);
            btn.Click += onClick;
            return btn;
        }

        private void ApplyRoundedRegion(Control control, int radius)
        {
            control.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                Rectangle rect = control.ClientRectangle;
                using (GraphicsPath path = new GraphicsPath())
                {
                    float d = radius * 2F;
                    path.AddArc(rect.X, rect.Y, d, d, 180, 90);
                    path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
                    path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
                    path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
                    path.CloseFigure();
                    control.Region = new Region(path);
                }
            };
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
            pictureBoxScreen.Focus();
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

        // Hàm hỗ trợ đọc đúng và đủ chính xác số byte từ luồng TCP
        private byte[] ReadExactBytes(BinaryReader reader, int totalBytes)
        {
            byte[] buffer = new byte[totalBytes];
            int bytesRead = 0;
            while (bytesRead < totalBytes)
            {
                int read = reader.Read(buffer, bytesRead, totalBytes - bytesRead);
                if (read <= 0) throw new EndOfStreamException("Mất kết nối mạng khi đang nhận ảnh.");
                bytesRead += read;
            }
            return buffer;
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
                        // Đọc chính xác toàn bộ mảng byte của khung hình
                        byte[] buffer = ReadExactBytes(reader, length);

                        using (MemoryStream ms = new MemoryStream(buffer))
                        {
                            Image newImg = Image.FromStream(ms);

                            // Cập nhật giao diện an toàn trên luồng UI
                            pictureBoxScreen.BeginInvoke((Action)(() =>
                            {
                                Image oldImg = pictureBoxScreen.Image;
                                pictureBoxScreen.Image = (Image)newImg.Clone();
                                newImg.Dispose();
                                if (oldImg != null) oldImg.Dispose();
                            }));
                        }
                    }
                    else if (packetType == "SERVER_CHAT")
                    {
                        string serverMsg = reader.ReadString();
                        AppendChatLog("[Server]: " + serverMsg);
                    }
                }
                catch
                {
                    // Nếu mất kết nối hoặc luồng bị ngắt thì thoát vòng lặp an toàn
                    break;
                }
            }
        }

        // Tự động điều khiển chuột liên tục
        private void PictureBoxScreen_MouseMove(object sender, MouseEventArgs e)
        {
            if (isRunning && pictureBoxScreen.Image != null)
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
            pictureBoxScreen.Focus(); // Giữ chắc focus khi click chuột vào máy ảo
            if (isRunning)
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
            if (isRunning)
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

        // Gõ phím trực tiếp bằng Unicode
        private void RemoteViewer_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (isRunning && !txtClientMsg.Focused)
            {
                try
                {
                    lock (writer)
                    {
                        writer.Write("UNICODE_KEY");
                        writer.Write(e.KeyChar);
                        writer.Flush();
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