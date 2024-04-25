using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ONTI2019
{
    public partial class LogareBiblioteca : Form
    {
        public LogareBiblioteca()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (CheckMail(textBox1.Text))
            {
                if (textBox2.Text != string.Empty)
                {
                    UserModel model = new UserModel
                    {
                        Email = textBox1.Text,
                        Password = PasswordEncryption.EncryptPassword(textBox2.Text)
                        
                    };
                    if (DatabaseHelper.CheckUtilizator(model.Email,model.Password))
                    {
                        this.Hide();
                        var bbl = new BibliotecarBiblioteca();
                        bbl.ShowDialog();
                        this.Close();
                    }
                }
            }
        }
        private bool CheckMail(string mail)
        {
            try 
            { 
            MailAddress adress = new MailAddress(mail);
                return true;
            }
            catch
            {
                MessageBox.Show("Adresa Invalida");
                return false;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
