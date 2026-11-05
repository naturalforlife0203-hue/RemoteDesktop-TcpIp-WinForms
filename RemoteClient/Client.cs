using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Net.Sockets;
using System.Windows.Forms;

namespace RemoteClient
{
    public partial class Client : Form
    {
        private class BlossomPetal
        {
            public float X { get; set; }
            public float Y { get; set; }
            public float SpeedY { get; set; }
            public float SpeedX { get; set; }
            public float Size { get; set; }
            public float Angle { get; set; }
            public float AngleSpeed { get; set; }
            public Color PetalColor { get; set; }
            public bool IsFullFlower { get; set; }
        }

        private List<BlossomPetal> petals = new List<BlossomPetal>();
        private Timer animTimer;
        private Timer countdownTimer;
        private Random rnd = new Random();
        private bool isTetTheme = false;
        private DateTime tetTargetDate = new DateTime(2027, 2, 6, 0, 0, 0);

        public Client()
        {
            InitializeComponent();
            ApplyStyling();
            InitPetalsAnimation();
            InitCountdownTimer();
            UpdateThemeVisuals();
        }

        private void ApplyStyling()
        {
            this.DoubleBuffered = true;

            panelCard.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (GraphicsPath path = GetRoundedPath(panelCard.ClientRectangle, 16))
                using (Pen pen = new Pen(isTetTheme ? Color.FromArgb(250, 218, 222) : Color.FromArgb(235, 237, 242), 1.5f))
                {
                    e.Graphics.DrawPath(pen, path);
                }
            };

            btnConnect.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (GraphicsPath path = GetRoundedPath(btnConnect.ClientRectangle, 12))
                {
                    btnConnect.Region = new Region(path);
                }
            };

            // Switch gọn gàng với icon hoa đào
            pnlSwitch.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                Rectangle rect = new Rectangle(0, 0, pnlSwitch.Width - 1, pnlSwitch.Height - 1);
                using (GraphicsPath path = GetRoundedPath(rect, rect.Height / 2))
                {
                    Color trackColor = isTetTheme ? Color.FromArgb(235, 64, 52) : Color.FromArgb(215, 220, 228);
                    using (SolidBrush brush = new SolidBrush(trackColor))
                    {
                        e.Graphics.FillPath(brush, path);
                    }

                    int knobSize = rect.Height - 6;
                    int knobX = isTetTheme ? (rect.Width - knobSize - 4) : 4;
                    int knobY = 3;
                    using (SolidBrush knobBrush = new SolidBrush(Color.White))
                    {
                        e.Graphics.FillEllipse(knobBrush, knobX, knobY, knobSize, knobSize);
                    }
                }
            };
        }

        private GraphicsPath GetRoundedPath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            float d = radius * 2F;
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        private void InitPetalsAnimation()
        {
            Color[] colors = {
                Color.FromArgb(255, 182, 193),
                Color.FromArgb(240, 128, 128),
                Color.FromArgb(255, 215, 0)
            };

            for (int i = 0; i < 28; i++)
            {
                petals.Add(new BlossomPetal
                {
                    X = rnd.Next(0, this.Width),
                    Y = rnd.Next(0, this.Height),
                    SpeedY = (float)(rnd.NextDouble() * 1.5 + 1.0),
                    SpeedX = (float)(rnd.NextDouble() * 1.0 - 0.5),
                    Size = rnd.Next(10, 15),
                    Angle = rnd.Next(0, 360),
                    AngleSpeed = (float)(rnd.NextDouble() * 3.0 - 1.5),
                    PetalColor = colors[rnd.Next(colors.Length)],
                    IsFullFlower = (i % 4 == 0)
                });
            }

            panelHeader.Paint += ChildPanel_Paint;
            panelCard.Paint += ChildPanel_Paint;

            animTimer = new Timer { Interval = 30 };
            animTimer.Tick += (s, e) =>
            {
                if (!isTetTheme) return;

                foreach (var p in petals)
                {
                    p.Y += p.SpeedY;
                    p.X += p.SpeedX + (float)Math.Sin(p.Y * 0.035) * 0.45f;
                    p.Angle += p.AngleSpeed;

                    if (p.Y > this.Height)
                    {
                        p.Y = -20;
                        p.X = rnd.Next(0, this.Width);
                    }
                }
                this.Invalidate();
                panelHeader.Invalidate();
                panelCard.Invalidate();
            };
            animTimer.Start();
        }

        private void InitCountdownTimer()
        {
            countdownTimer = new Timer { Interval = 1000 };
            countdownTimer.Tick += (s, e) => UpdateCountdownDisplay();
            countdownTimer.Start();
        }

        private void UpdateCountdownDisplay()
        {
            if (!isTetTheme) return;
            TimeSpan diff = tetTargetDate - DateTime.Now;
            if (diff.TotalSeconds > 0)
            {
                lblCountdown.Text = string.Format("Còn {0} ngày {1} giờ {2} phút {3} giây\nlà sẽ đến một Tết Đinh Mùi 2027 bình an ╰(*°▽°*)╯",
                    diff.Days, diff.Hours, diff.Minutes, diff.Seconds);
            }
            else
            {
                lblCountdown.Text = "Tết Đinh Mùi 2027 đã đến!\nChúc bạn và gia đình vạn sự như ý ╰(*°▽°*)╯";
            }
        }

        private void DrawPetals(Graphics g, int offsetX, int offsetY)
        {
            if (!isTetTheme) return;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            foreach (var p in petals)
            {
                float localX = p.X - offsetX;
                float localY = p.Y - offsetY;

                GraphicsState state = g.Save();
                g.TranslateTransform(localX, localY);
                g.RotateTransform(p.Angle);

                if (p.IsFullFlower)
                {
                    using (SolidBrush petalBrush = new SolidBrush(p.PetalColor))
                    using (SolidBrush centerBrush = new SolidBrush(Color.FromArgb(200, 70, 0)))
                    {
                        for (int k = 0; k < 5; k++)
                        {
                            g.RotateTransform(72);
                            g.FillEllipse(petalBrush, 0, -p.Size * 0.7f, p.Size * 0.55f, p.Size * 0.85f);
                        }
                        g.FillEllipse(centerBrush, -p.Size * 0.2f, -p.Size * 0.2f, p.Size * 0.4f, p.Size * 0.4f);
                    }
                }
                else
                {
                    using (GraphicsPath path = new GraphicsPath())
                    {
                        path.AddBezier(0, -p.Size, p.Size * 0.65f, -p.Size * 0.3f, p.Size * 0.45f, p.Size * 0.75f, 0, p.Size);
                        path.AddBezier(0, p.Size, -p.Size * 0.45f, p.Size * 0.75f, -p.Size * 0.65f, -p.Size * 0.3f, 0, -p.Size);

                        using (SolidBrush brush = new SolidBrush(p.PetalColor))
                        {
                            g.FillPath(brush, path);
                        }
                    }
                }

                g.Restore(state);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawPetals(e.Graphics, 0, 0);
        }

        private void ChildPanel_Paint(object sender, PaintEventArgs e)
        {
            Control c = (Control)sender;
            DrawPetals(e.Graphics, c.Left, c.Top);
        }

        private void pnlSwitch_Click(object sender, EventArgs e)
        {
            isTetTheme = !isTetTheme;
            pnlSwitch.Invalidate();
            UpdateThemeVisuals();
        }

        private void UpdateThemeVisuals()
        {
            if (isTetTheme)
            {
                lblSwitchText.Text = "🌸";
                lblSwitchText.ForeColor = Color.FromArgb(220, 50, 60);

                panelHeader.BackColor = Color.FromArgb(254, 244, 245);
                lblTitle.Text = "XUÂN KẾT NỐI";
                lblTitle.ForeColor = Color.FromArgb(195, 35, 45);
                lblSub.Text = "Chúc Mừng Năm Mới 2027";
                lblSub.ForeColor = Color.FromArgb(170, 110, 115);

                btnConnect.BackColor = Color.FromArgb(220, 50, 60);

                // Bỏ dòng chữ thừa phía trên để nhường trọn khung cho Countdown
                lblStatus.Text = "";
                lblStatus.Visible = false;

                lblCountdown.Visible = true;
                lblCountdown.Location = new Point(10, 225);
                lblCountdown.Size = new Size(360, 60);
                lblCountdown.TextAlign = ContentAlignment.MiddleCenter;
                lblCountdown.Font = new Font("Segoe UI", 8.5F, FontStyle.Italic);
                UpdateCountdownDisplay();
            }
            else
            {
                lblSwitchText.Text = "🌸";
                lblSwitchText.ForeColor = Color.FromArgb(140, 145, 155);

                panelHeader.BackColor = Color.FromArgb(248, 249, 251);
                lblTitle.Text = "REMOTE DESKTOP";
                lblTitle.ForeColor = Color.FromArgb(30, 35, 45);
                lblSub.Text = "Remote Desktop Client - TCP/IP";
                lblSub.ForeColor = Color.FromArgb(130, 135, 145);

                btnConnect.BackColor = Color.FromArgb(235, 64, 52);

                lblStatus.Visible = true;
                lblStatus.Text = "SẴN SÀNG KẾT NỐI";
                lblStatus.ForeColor = Color.FromArgb(150, 155, 165);
                lblStatus.Location = new Point(25, 230);

                lblCountdown.Visible = false;
            }

            this.Invalidate();
            panelHeader.Invalidate();
            panelCard.Invalidate();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            string ip = txtIP.Text.Trim();
            string pin = txtPin.Text.Trim();

            if (!int.TryParse(txtPort.Text.Trim(), out int port))
            {
                MessageBox.Show("Cổng (Port) không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            lblStatus.Text = "ĐANG KẾT NỐI...";
            lblStatus.ForeColor = Color.FromArgb(230, 130, 0);

            try
            {
                TcpClient client = new TcpClient();
                client.Connect(ip, port);
                NetworkStream stream = client.GetStream();
                BinaryWriter writer = new BinaryWriter(stream);
                BinaryReader reader = new BinaryReader(stream);

                writer.Write(pin);
                bool isAuthenticated = reader.ReadBoolean();

                if (!isAuthenticated)
                {
                    MessageBox.Show("Mã PIN không đúng! Kết nối bị từ chối.", "Lỗi PIN", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    lblStatus.Text = "SAI MÃ PIN";
                    lblStatus.ForeColor = Color.FromArgb(220, 53, 69);
                    client.Close();
                    return;
                }

                lblStatus.Text = "KẾT NỐI THÀNH CÔNG";
                lblStatus.ForeColor = Color.FromArgb(40, 167, 69);

                this.Hide();
                RemoteViewer viewer = new RemoteViewer(client, stream, writer, reader);
                viewer.ShowDialog();

                lblStatus.Text = isTetTheme ? "CHÚC MỪNG NĂM MỚI 2027" : "SẴN SÀNG KẾT NỐI";
                lblStatus.ForeColor = isTetTheme ? Color.FromArgb(195, 35, 45) : Color.FromArgb(150, 155, 165);
                this.Show();
            }
            catch (Exception ex)
            {
                lblStatus.Text = "KẾT NỐI THẤT BẠI";
                lblStatus.ForeColor = Color.FromArgb(220, 53, 69);
                MessageBox.Show("Không thể kết nối đến máy chủ: " + ex.Message, "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}