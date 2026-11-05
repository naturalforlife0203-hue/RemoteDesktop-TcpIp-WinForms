namespace RemoteServer
{
    partial class Server
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
            this.btnStart = new System.Windows.Forms.Button();
            this.txtPin = new System.Windows.Forms.TextBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblPort = new System.Windows.Forms.Label();
            this.lblPin = new System.Windows.Forms.Label();
            this.txtChatLog = new System.Windows.Forms.TextBox();
            this.txtServerReply = new System.Windows.Forms.TextBox();
            this.btnSendReply = new System.Windows.Forms.Button();
            this.lblChatTitle = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.FromArgb(30, 144, 255);
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStart.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnStart.ForeColor = System.Drawing.Color.White;
            this.btnStart.Location = new System.Drawing.Point(25, 80);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(370, 36);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start Server";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // txtPort
            // 
            this.txtPort.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtPort.Location = new System.Drawing.Point(25, 38);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(170, 29);
            this.txtPort.TabIndex = 1;
            this.txtPort.Text = "9999";
            // 
            // txtPin
            // 
            this.txtPin.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtPin.Location = new System.Drawing.Point(225, 38);
            this.txtPin.Name = "txtPin";
            this.txtPin.Size = new System.Drawing.Size(170, 29);
            this.txtPin.TabIndex = 2;
            this.txtPin.Text = "1234";
            // 
            // lblStatus
            // 
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            this.lblStatus.ForeColor = System.Drawing.Color.Gray;
            this.lblStatus.Location = new System.Drawing.Point(25, 122);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(370, 20);
            this.lblStatus.TabIndex = 3;
            this.lblStatus.Text = "Status: Stopped";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblPort.Location = new System.Drawing.Point(25, 15);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(38, 20);
            this.lblPort.TabIndex = 4;
            this.lblPort.Text = "Port:";
            // 
            // lblPin
            // 
            this.lblPin.AutoSize = true;
            this.lblPin.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblPin.Location = new System.Drawing.Point(225, 15);
            this.lblPin.Name = "lblPin";
            this.lblPin.Size = new System.Drawing.Size(35, 20);
            this.lblPin.TabIndex = 5;
            this.lblPin.Text = "PIN:";
            // 
            // lblChatTitle
            // 
            this.lblChatTitle.AutoSize = true;
            this.lblChatTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.lblChatTitle.ForeColor = System.Drawing.Color.FromArgb(70, 75, 85);
            this.lblChatTitle.Location = new System.Drawing.Point(25, 155);
            this.lblChatTitle.Name = "lblChatTitle";
            this.lblChatTitle.Size = new System.Drawing.Size(182, 20);
            this.lblChatTitle.TabIndex = 6;
            this.lblChatTitle.Text = "💬 Hộp thư trao đổi (Chat)";
            // 
            // txtChatLog
            // 
            this.txtChatLog.BackColor = System.Drawing.Color.White;
            this.txtChatLog.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtChatLog.Location = new System.Drawing.Point(25, 180);
            this.txtChatLog.Multiline = true;
            this.txtChatLog.Name = "txtChatLog";
            this.txtChatLog.ReadOnly = true;
            this.txtChatLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtChatLog.Size = new System.Drawing.Size(370, 110);
            this.txtChatLog.TabIndex = 7;
            // 
            // txtServerReply
            // 
            this.txtServerReply.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtServerReply.Location = new System.Drawing.Point(25, 300);
            this.txtServerReply.Name = "txtServerReply";
            this.txtServerReply.Size = new System.Drawing.Size(280, 27);
            this.txtServerReply.TabIndex = 8;
            // 
            // btnSendReply
            // 
            this.btnSendReply.BackColor = System.Drawing.Color.FromArgb(40, 167, 69);
            this.btnSendReply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSendReply.Font = new System.Drawing.Font("Segoe UI Semibold", 8.5F, System.Drawing.FontStyle.Bold);
            this.btnSendReply.ForeColor = System.Drawing.Color.White;
            this.btnSendReply.Location = new System.Drawing.Point(315, 298);
            this.btnSendReply.Name = "btnSendReply";
            this.btnSendReply.Size = new System.Drawing.Size(80, 30);
            this.btnSendReply.TabIndex = 9;
            this.btnSendReply.Text = "Gửi";
            this.btnSendReply.UseVisualStyleBackColor = false;
            this.btnSendReply.Click += new System.EventHandler(this.btnSendReply_Click);
            // 
            // Server
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(246, 248, 250);
            this.ClientSize = new System.Drawing.Size(420, 345);
            this.Controls.Add(this.btnSendReply);
            this.Controls.Add(this.txtServerReply);
            this.Controls.Add(this.txtChatLog);
            this.Controls.Add(this.lblChatTitle);
            this.Controls.Add(this.lblPin);
            this.Controls.Add(this.lblPort);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.txtPin);
            this.Controls.Add(this.btnStart);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Server";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Remote Server Controller";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Server_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.TextBox txtPin;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.Label lblPin;
        private System.Windows.Forms.Label lblChatTitle;
        private System.Windows.Forms.TextBox txtChatLog;
        private System.Windows.Forms.TextBox txtServerReply;
        private System.Windows.Forms.Button btnSendReply;
    }
}