namespace CSaN3
{
    partial class fmMain
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
            this.bbName = new System.Windows.Forms.Button();
            this.bbSendText = new System.Windows.Forms.Button();
            this.tbName = new System.Windows.Forms.TextBox();
            this.tbSendText = new System.Windows.Forms.TextBox();
            this.tbChat = new System.Windows.Forms.TextBox();
            this.bbConnect = new System.Windows.Forms.Button();
            this.bbDisconnect = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // bbName
            // 
            this.bbName.Location = new System.Drawing.Point(-2, 59);
            this.bbName.Name = "bbName";
            this.bbName.Size = new System.Drawing.Size(130, 30);
            this.bbName.TabIndex = 0;
            this.bbName.Text = "Изменить имя";
            this.bbName.UseVisualStyleBackColor = true;
            this.bbName.Click += new System.EventHandler(this.bbName_Click);
            // 
            // bbSendText
            // 
            this.bbSendText.Enabled = false;
            this.bbSendText.Location = new System.Drawing.Point(261, 548);
            this.bbSendText.Name = "bbSendText";
            this.bbSendText.Size = new System.Drawing.Size(161, 37);
            this.bbSendText.TabIndex = 3;
            this.bbSendText.Text = "Отправить";
            this.bbSendText.UseVisualStyleBackColor = true;
            this.bbSendText.Click += new System.EventHandler(this.bbSendText_Click);
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(-2, 31);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(257, 22);
            this.tbName.TabIndex = 4;
            // 
            // tbSendText
            // 
            this.tbSendText.Location = new System.Drawing.Point(419, 548);
            this.tbSendText.Multiline = true;
            this.tbSendText.Name = "tbSendText";
            this.tbSendText.Size = new System.Drawing.Size(714, 37);
            this.tbSendText.TabIndex = 5;
            // 
            // tbChat
            // 
            this.tbChat.Location = new System.Drawing.Point(261, -2);
            this.tbChat.Multiline = true;
            this.tbChat.Name = "tbChat";
            this.tbChat.ReadOnly = true;
            this.tbChat.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbChat.Size = new System.Drawing.Size(881, 544);
            this.tbChat.TabIndex = 6;
            // 
            // bbConnect
            // 
            this.bbConnect.Location = new System.Drawing.Point(-2, 126);
            this.bbConnect.Name = "bbConnect";
            this.bbConnect.Size = new System.Drawing.Size(130, 30);
            this.bbConnect.TabIndex = 7;
            this.bbConnect.Text = "Подключиться";
            this.bbConnect.UseVisualStyleBackColor = true;
            this.bbConnect.Click += new System.EventHandler(this.bbConnect_Click);
            // 
            // bbDisconnect
            // 
            this.bbDisconnect.Enabled = false;
            this.bbDisconnect.Location = new System.Drawing.Point(125, 126);
            this.bbDisconnect.Name = "bbDisconnect";
            this.bbDisconnect.Size = new System.Drawing.Size(130, 30);
            this.bbDisconnect.TabIndex = 8;
            this.bbDisconnect.Text = "Отключиться";
            this.bbDisconnect.UseVisualStyleBackColor = true;
            this.bbDisconnect.Click += new System.EventHandler(this.bbDisconnect_Click);
            // 
            // fmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(1135, 588);
            this.Controls.Add(this.bbDisconnect);
            this.Controls.Add(this.bbConnect);
            this.Controls.Add(this.tbChat);
            this.Controls.Add(this.tbSendText);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.bbSendText);
            this.Controls.Add(this.bbName);
            this.Name = "fmMain";
            this.Text = "Локальный чат";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bbName;
        private System.Windows.Forms.Button bbSendText;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.TextBox tbSendText;
        private System.Windows.Forms.TextBox tbChat;
        private System.Windows.Forms.Button bbConnect;
        private System.Windows.Forms.Button bbDisconnect;
    }
}

