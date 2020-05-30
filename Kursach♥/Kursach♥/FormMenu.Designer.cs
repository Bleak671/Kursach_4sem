namespace Kursach_
{
    partial class FormMenu
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblFullName = new System.Windows.Forms.Label();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.listLobby = new System.Windows.Forms.ListBox();
            this.listChat = new System.Windows.Forms.ListBox();
            this.btnLeaveLobby = new System.Windows.Forms.Button();
            this.btnCreateLobby = new System.Windows.Forms.Button();
            this.btnConnectLobby = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.textBoxChat = new System.Windows.Forms.TextBox();
            this.btnSendMessage = new System.Windows.Forms.Button();
            this.btnStartGame = new System.Windows.Forms.Button();
            this.textBoxHostIP = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lblFullName
            // 
            this.lblFullName.AutoSize = true;
            this.lblFullName.Location = new System.Drawing.Point(639, 23);
            this.lblFullName.Name = "lblFullName";
            this.lblFullName.Size = new System.Drawing.Size(68, 13);
            this.lblFullName.TabIndex = 0;
            this.lblFullName.Text = "Полное имя";
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(623, 39);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.ReadOnly = true;
            this.textBoxName.Size = new System.Drawing.Size(151, 20);
            this.textBoxName.TabIndex = 1;
            // 
            // listLobby
            // 
            this.listLobby.FormattingEnabled = true;
            this.listLobby.Location = new System.Drawing.Point(31, 23);
            this.listLobby.Name = "listLobby";
            this.listLobby.Size = new System.Drawing.Size(184, 238);
            this.listLobby.TabIndex = 2;
            // 
            // listChat
            // 
            this.listChat.FormattingEnabled = true;
            this.listChat.Location = new System.Drawing.Point(282, 39);
            this.listChat.Name = "listChat";
            this.listChat.Size = new System.Drawing.Size(248, 342);
            this.listChat.TabIndex = 3;
            // 
            // btnLeaveLobby
            // 
            this.btnLeaveLobby.Location = new System.Drawing.Point(70, 277);
            this.btnLeaveLobby.Name = "btnLeaveLobby";
            this.btnLeaveLobby.Size = new System.Drawing.Size(108, 23);
            this.btnLeaveLobby.TabIndex = 4;
            this.btnLeaveLobby.Text = "Покинуть лобби";
            this.btnLeaveLobby.UseVisualStyleBackColor = true;
            this.btnLeaveLobby.Click += new System.EventHandler(this.btnLeaveLobby_Click);
            // 
            // btnCreateLobby
            // 
            this.btnCreateLobby.Location = new System.Drawing.Point(642, 358);
            this.btnCreateLobby.Name = "btnCreateLobby";
            this.btnCreateLobby.Size = new System.Drawing.Size(132, 23);
            this.btnCreateLobby.TabIndex = 5;
            this.btnCreateLobby.Text = "Создать лобби";
            this.btnCreateLobby.UseVisualStyleBackColor = true;
            this.btnCreateLobby.Click += new System.EventHandler(this.btnCreateLobby_Click);
            // 
            // btnConnectLobby
            // 
            this.btnConnectLobby.Location = new System.Drawing.Point(642, 303);
            this.btnConnectLobby.Name = "btnConnectLobby";
            this.btnConnectLobby.Size = new System.Drawing.Size(132, 23);
            this.btnConnectLobby.TabIndex = 6;
            this.btnConnectLobby.Text = "Подключиться к лобби";
            this.btnConnectLobby.UseVisualStyleBackColor = true;
            this.btnConnectLobby.Click += new System.EventHandler(this.btnConnectLobby_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(713, 415);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 7;
            this.btnExit.Text = "Выйти";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // textBoxChat
            // 
            this.textBoxChat.Location = new System.Drawing.Point(282, 396);
            this.textBoxChat.Name = "textBoxChat";
            this.textBoxChat.Size = new System.Drawing.Size(248, 20);
            this.textBoxChat.TabIndex = 8;
            // 
            // btnSendMessage
            // 
            this.btnSendMessage.Location = new System.Drawing.Point(536, 396);
            this.btnSendMessage.Name = "btnSendMessage";
            this.btnSendMessage.Size = new System.Drawing.Size(23, 23);
            this.btnSendMessage.TabIndex = 9;
            this.btnSendMessage.Text = ">";
            this.btnSendMessage.UseVisualStyleBackColor = true;
            // 
            // btnStartGame
            // 
            this.btnStartGame.Location = new System.Drawing.Point(53, 337);
            this.btnStartGame.Name = "btnStartGame";
            this.btnStartGame.Size = new System.Drawing.Size(142, 44);
            this.btnStartGame.TabIndex = 10;
            this.btnStartGame.Text = "Играть!";
            this.btnStartGame.UseVisualStyleBackColor = true;
            // 
            // textBoxHostIP
            // 
            this.textBoxHostIP.Location = new System.Drawing.Point(623, 277);
            this.textBoxHostIP.Name = "textBoxHostIP";
            this.textBoxHostIP.Size = new System.Drawing.Size(151, 20);
            this.textBoxHostIP.TabIndex = 11;
            this.textBoxHostIP.Text = "IP хоста лобби";
            // 
            // FormMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.textBoxHostIP);
            this.Controls.Add(this.btnStartGame);
            this.Controls.Add(this.btnSendMessage);
            this.Controls.Add(this.textBoxChat);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnConnectLobby);
            this.Controls.Add(this.btnCreateLobby);
            this.Controls.Add(this.btnLeaveLobby);
            this.Controls.Add(this.listChat);
            this.Controls.Add(this.listLobby);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.lblFullName);
            this.Name = "FormMenu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblFullName;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.ListBox listLobby;
        private System.Windows.Forms.ListBox listChat;
        private System.Windows.Forms.Button btnLeaveLobby;
        private System.Windows.Forms.Button btnCreateLobby;
        private System.Windows.Forms.Button btnConnectLobby;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.TextBox textBoxChat;
        private System.Windows.Forms.Button btnSendMessage;
        private System.Windows.Forms.Button btnStartGame;
        private System.Windows.Forms.TextBox textBoxHostIP;
    }
}

