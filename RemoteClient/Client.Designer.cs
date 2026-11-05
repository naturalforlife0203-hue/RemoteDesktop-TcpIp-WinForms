namespace RemoteClient
{
    partial class Client
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblSwitchText = new System.Windows.Forms.Label();
            this.pnlSwitch = new System.Windows.Forms.Panel();
            this.lblSub = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelCard = new System.Windows.Forms.Panel();
            this.lblCountdown = new System.Windows.Forms.Label();
            this.lblIP = new System.Windows.Forms.Label();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.lblPort = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.lblPin = new System.Windows.Forms.Label();
            this.txtPin = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.panelHeader.SuspendLayout();
            this.panelCard.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(248, 249, 251);
            this.panelHeader.Controls.Add(this.lblSwitchText);
            this.panelHeader.Controls.Add(this.pnlSwitch);
            this.panelHeader.Controls.Add(this.lblSub);
            this.panelHeader.Controls.Add(this.lblTitle);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(430, 85);
            this.panelHeader.TabIndex = 0;
            // 
            // lblSwitchText
            // 
            this.lblSwitchText.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblSwitchText.ForeColor = System.Drawing.Color.FromArgb(140, 145, 155);
            this.lblSwitchText.Location = new System.Drawing.Point(340, 12);
            this.lblSwitchText.Name = "lblSwitchText";
            this.lblSwitchText.Size = new System.Drawing.Size(56, 22);
            this.lblSwitchText.TabIndex = 3;
            this.lblSwitchText.Text = "🌸";
            this.lblSwitchText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlSwitch
            // 
            this.pnlSwitch.BackColor = System.Drawing.Color.Transparent;
            this.pnlSwitch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pnlSwitch.Location = new System.Drawing.Point(340, 38);
            this.pnlSwitch.Name = "pnlSwitch";
            this.pnlSwitch.Size = new System.Drawing.Size(56, 28);
            this.pnlSwitch.TabIndex = 2;
            this.pnlSwitch.Click += new System.EventHandler(this.pnlSwitch_Click);
            // 
            // lblSub
            // 
            this.lblSub.AutoSize = true;
            this.lblSub.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblSub.ForeColor = System.Drawing.Color.FromArgb(130, 135, 145);
            this.lblSub.Location = new System.Drawing.Point(24, 48);
            this.lblSub.Name = "lblSub";
            this.lblSub.Size = new System.Drawing.Size(206, 20);
            this.lblSub.TabIndex = 1;
            this.lblSub.Text = "Remote Desktop Client - TCP/IP";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 13.5F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(30, 35, 45);
            this.lblTitle.Location = new System.Drawing.Point(22, 14);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(225, 31);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "REMOTE DESKTOP";
            // 
            // panelCard
            // 
            this.panelCard.BackColor = System.Drawing.Color.White;
            this.panelCard.Controls.Add(this.lblCountdown);
            this.panelCard.Controls.Add(this.lblIP);
            this.panelCard.Controls.Add(this.txtIP);
            this.panelCard.Controls.Add(this.lblPort);
            this.panelCard.Controls.Add(this.txtPort);
            this.panelCard.Controls.Add(this.lblPin);
            this.panelCard.Controls.Add(this.txtPin);
            this.panelCard.Controls.Add(this.btnConnect);
            this.panelCard.Controls.Add(this.lblStatus);
            this.panelCard.Location = new System.Drawing.Point(25, 102);
            this.panelCard.Name = "panelCard";
            this.panelCard.Size = new System.Drawing.Size(380, 310);
            this.panelCard.TabIndex = 1;
            // 
            // lblCountdown
            // 
            this.lblCountdown.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Italic);
            this.lblCountdown.ForeColor = System.Drawing.Color.FromArgb(200, 45, 55);
            this.lblCountdown.Location = new System.Drawing.Point(10, 225);
            this.lblCountdown.Name = "lblCountdown";
            this.lblCountdown.Size = new System.Drawing.Size(360, 65);
            this.lblCountdown.TabIndex = 8;
            this.lblCountdown.Text = "";
            this.lblCountdown.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblCountdown.Visible = false;
            // 
            // lblIP
            // 
            this.lblIP.AutoSize = true;
            this.lblIP.Font = new System.Drawing.Font("Segoe UI Semibold", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblIP.ForeColor = System.Drawing.Color.FromArgb(115, 120, 130);
            this.lblIP.Location = new System.Drawing.Point(25, 15);
            this.lblIP.Name = "lblIP";
            this.lblIP.Size = new System.Drawing.Size(139, 20);
            this.lblIP.TabIndex = 0;
            this.lblIP.Text = "ĐỊA CHỈ IP ĐỐI TÁC";
            // 
            // txtIP
            // 
            this.txtIP.BackColor = System.Drawing.Color.FromArgb(248, 249, 251);
            this.txtIP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtIP.Font = new System.Drawing.Font("Consolas", 10.5F);
            this.txtIP.ForeColor = System.Drawing.Color.FromArgb(40, 45, 55);
            this.txtIP.Location = new System.Drawing.Point(25, 38);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(330, 28);
            this.txtIP.TabIndex = 1;
            this.txtIP.Text = "127.0.0.1";
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Font = new System.Drawing.Font("Segoe UI Semibold", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblPort.ForeColor = System.Drawing.Color.FromArgb(115, 120, 130);
            this.lblPort.Location = new System.Drawing.Point(25, 78);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(46, 20);
            this.lblPort.TabIndex = 2;
            this.lblPort.Text = "PORT";
            // 
            // txtPort
            // 
            this.txtPort.BackColor = System.Drawing.Color.FromArgb(248, 249, 251);
            this.txtPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPort.Font = new System.Drawing.Font("Consolas", 10.5F);
            this.txtPort.ForeColor = System.Drawing.Color.FromArgb(40, 45, 55);
            this.txtPort.Location = new System.Drawing.Point(25, 102);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(150, 28);
            this.txtPort.TabIndex = 3;
            this.txtPort.Text = "9999";
            // 
            // lblPin
            // 
            this.lblPin.AutoSize = true;
            this.lblPin.Font = new System.Drawing.Font("Segoe UI Semibold", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblPin.ForeColor = System.Drawing.Color.FromArgb(115, 120, 130);
            this.lblPin.Location = new System.Drawing.Point(200, 78);
            this.lblPin.Name = "lblPin";
            this.lblPin.Size = new System.Drawing.Size(60, 20);
            this.lblPin.TabIndex = 4;
            this.lblPin.Text = "MÃ PIN";
            // 
            // txtPin
            // 
            this.txtPin.BackColor = System.Drawing.Color.FromArgb(248, 249, 251);
            this.txtPin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPin.Font = new System.Drawing.Font("Consolas", 10.5F);
            this.txtPin.ForeColor = System.Drawing.Color.FromArgb(40, 45, 55);
            this.txtPin.Location = new System.Drawing.Point(205, 102);
            this.txtPin.Name = "txtPin";
            this.txtPin.Size = new System.Drawing.Size(150, 28);
            this.txtPin.TabIndex = 5;
            this.txtPin.Text = "1234";
            // 
            // btnConnect
            // 
            this.btnConnect.BackColor = System.Drawing.Color.FromArgb(235, 64, 52);
            this.btnConnect.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnConnect.FlatAppearance.BorderSize = 0;
            this.btnConnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConnect.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.btnConnect.ForeColor = System.Drawing.Color.White;
            this.btnConnect.Location = new System.Drawing.Point(25, 155);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(330, 46);
            this.btnConnect.TabIndex = 6;
            this.btnConnect.Text = "KẾT NỐI MÁY";
            this.btnConnect.UseVisualStyleBackColor = false;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI Semibold", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(150, 155, 165);
            this.lblStatus.Location = new System.Drawing.Point(25, 235);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(330, 25);
            this.lblStatus.TabIndex = 7;
            this.lblStatus.Text = "SẴN SÀNG KẾT NỐI";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Client
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(244, 245, 248);
            this.ClientSize = new System.Drawing.Size(430, 440);
            this.Controls.Add(this.panelCard);
            this.Controls.Add(this.panelHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Client";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Remote Desktop Client";
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelCard.ResumeLayout(false);
            this.panelCard.PerformLayout();
            this.ResumeLayout(false);
        }
        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSub;
        private System.Windows.Forms.Panel pnlSwitch;
        private System.Windows.Forms.Label lblSwitchText;
        private System.Windows.Forms.Panel panelCard;
        private System.Windows.Forms.Label lblIP;
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label lblPin;
        private System.Windows.Forms.TextBox txtPin;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblCountdown;
    }
}