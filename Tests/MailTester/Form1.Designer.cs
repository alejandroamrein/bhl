namespace MailTester
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.smtpHostTextBox = new System.Windows.Forms.TextBox();
            this.smtpPortTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.smtpFromTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.smtpToTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.smtpSubjectTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.smtpBodyTextBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.smtpSslCheckBox = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.attachmentLabel = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.htmlCheckBox = new System.Windows.Forms.CheckBox();
            this.userTextBox = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.passTextBox = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "SMP Server";
            // 
            // smtpHostTextBox
            // 
            this.smtpHostTextBox.Location = new System.Drawing.Point(82, 12);
            this.smtpHostTextBox.Name = "smtpHostTextBox";
            this.smtpHostTextBox.Size = new System.Drawing.Size(140, 20);
            this.smtpHostTextBox.TabIndex = 1;
            this.smtpHostTextBox.Text = "ganath.ch";
            // 
            // smtpPortTextBox
            // 
            this.smtpPortTextBox.Location = new System.Drawing.Point(82, 38);
            this.smtpPortTextBox.Name = "smtpPortTextBox";
            this.smtpPortTextBox.Size = new System.Drawing.Size(48, 20);
            this.smtpPortTextBox.TabIndex = 3;
            this.smtpPortTextBox.Text = "25";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Port";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "SSL:";
            // 
            // smtpFromTextBox
            // 
            this.smtpFromTextBox.Location = new System.Drawing.Point(82, 90);
            this.smtpFromTextBox.Name = "smtpFromTextBox";
            this.smtpFromTextBox.Size = new System.Drawing.Size(257, 20);
            this.smtpFromTextBox.TabIndex = 7;
            this.smtpFromTextBox.Text = "fritz@ganath.ch";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Vom:";
            // 
            // smtpToTextBox
            // 
            this.smtpToTextBox.Location = new System.Drawing.Point(82, 116);
            this.smtpToTextBox.Name = "smtpToTextBox";
            this.smtpToTextBox.Size = new System.Drawing.Size(257, 20);
            this.smtpToTextBox.TabIndex = 9;
            this.smtpToTextBox.Text = "fritz@ganath.ch";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 119);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(23, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "An:";
            // 
            // smtpSubjectTextBox
            // 
            this.smtpSubjectTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.smtpSubjectTextBox.Location = new System.Drawing.Point(82, 142);
            this.smtpSubjectTextBox.Name = "smtpSubjectTextBox";
            this.smtpSubjectTextBox.Size = new System.Drawing.Size(650, 20);
            this.smtpSubjectTextBox.TabIndex = 11;
            this.smtpSubjectTextBox.Text = "Test";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 145);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Betreff:";
            // 
            // smtpBodyTextBox
            // 
            this.smtpBodyTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.smtpBodyTextBox.Location = new System.Drawing.Point(82, 168);
            this.smtpBodyTextBox.Multiline = true;
            this.smtpBodyTextBox.Name = "smtpBodyTextBox";
            this.smtpBodyTextBox.Size = new System.Drawing.Size(650, 184);
            this.smtpBodyTextBox.TabIndex = 13;
            this.smtpBodyTextBox.Text = "Some text";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 171);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(36, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Inhalt:";
            // 
            // smtpSslCheckBox
            // 
            this.smtpSslCheckBox.AutoSize = true;
            this.smtpSslCheckBox.Location = new System.Drawing.Point(82, 67);
            this.smtpSslCheckBox.Name = "smtpSslCheckBox";
            this.smtpSslCheckBox.Size = new System.Drawing.Size(15, 14);
            this.smtpSslCheckBox.TabIndex = 14;
            this.smtpSslCheckBox.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(657, 358);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 15;
            this.button1.Text = "Abbrechen";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(576, 358);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 16;
            this.button2.Text = "Senden";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(613, 114);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(119, 23);
            this.button3.TabIndex = 19;
            this.button3.Text = "Beilage hizufügen...";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // attachmentLabel
            // 
            this.attachmentLabel.Location = new System.Drawing.Point(345, 93);
            this.attachmentLabel.Name = "attachmentLabel";
            this.attachmentLabel.Size = new System.Drawing.Size(387, 23);
            this.attachmentLabel.TabIndex = 20;
            this.attachmentLabel.Text = "Keine Beilage";
            this.attachmentLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(250, 67);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(69, 13);
            this.label8.TabIndex = 21;
            this.label8.Text = "HTML-Inhalt:";
            // 
            // htmlCheckBox
            // 
            this.htmlCheckBox.AutoSize = true;
            this.htmlCheckBox.Location = new System.Drawing.Point(325, 67);
            this.htmlCheckBox.Name = "htmlCheckBox";
            this.htmlCheckBox.Size = new System.Drawing.Size(15, 14);
            this.htmlCheckBox.TabIndex = 22;
            this.htmlCheckBox.UseVisualStyleBackColor = true;
            // 
            // userTextBox
            // 
            this.userTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.userTextBox.Location = new System.Drawing.Point(376, 12);
            this.userTextBox.Name = "userTextBox";
            this.userTextBox.Size = new System.Drawing.Size(140, 20);
            this.userTextBox.TabIndex = 24;
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(318, 15);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(52, 13);
            this.label9.TabIndex = 23;
            this.label9.Text = "Benutzer:";
            // 
            // passTextBox
            // 
            this.passTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.passTextBox.Location = new System.Drawing.Point(592, 12);
            this.passTextBox.Name = "passTextBox";
            this.passTextBox.PasswordChar = '*';
            this.passTextBox.Size = new System.Drawing.Size(140, 20);
            this.passTextBox.TabIndex = 26;
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(533, 15);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 13);
            this.label10.TabIndex = 25;
            this.label10.Text = "Passwort:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(744, 393);
            this.Controls.Add(this.passTextBox);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.userTextBox);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.htmlCheckBox);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.attachmentLabel);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.smtpSslCheckBox);
            this.Controls.Add(this.smtpBodyTextBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.smtpSubjectTextBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.smtpToTextBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.smtpFromTextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.smtpPortTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.smtpHostTextBox);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox smtpHostTextBox;
        private System.Windows.Forms.TextBox smtpPortTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox smtpFromTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox smtpToTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox smtpSubjectTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox smtpBodyTextBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox smtpSslCheckBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label attachmentLabel;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox htmlCheckBox;
        private System.Windows.Forms.TextBox userTextBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox passTextBox;
        private System.Windows.Forms.Label label10;
    }
}

