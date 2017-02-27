using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MailTester
{
    public partial class Form1 : Form
    {
        private string _BeilagePath = null;

        public Form1()
        {
            InitializeComponent();

            //var enUs = new CultureInfo("en-us");
            //var timestamp = DateTime.Now.ToString(enUs.DateTimeFormat);
            //var date = DateTime.Parse(timestamp, enUs.NumberFormat);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                MailHelper.SendMail(smtpHostTextBox.Text, smtpPortTextBox.Text, 
                    userTextBox.Text, passTextBox.Text, smtpSslCheckBox.Checked,
                    _BeilagePath, htmlCheckBox.Checked, smtpToTextBox.Text,
                    smtpSubjectTextBox.Text, smtpBodyTextBox.Text, smtpFromTextBox.Text);
                MessageBox.Show("Erfolg. Mailbox kontrollieren!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler: " + ex.Message + (ex.InnerException == null ? "" : "\n" + ex.InnerException.Message));
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var dlg = new OpenFileDialog();
            dlg.Filter = "Excel Dateien (*.xls;*.xlsx)|*.xls;*.xlsx|Alle Dateien (*.*)|*.*";
            dlg.InitialDirectory = Path.GetDirectoryName(typeof (Form1).Assembly.Location);
            DialogResult dr = dlg.ShowDialog();
            if (dr == DialogResult.OK)
            {
                _BeilagePath = dlg.FileName;
                attachmentLabel.Text = Path.GetFileName(_BeilagePath);
            }
        }
    }
}
