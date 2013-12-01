using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Security.Cryptography;

using JDLL.Data;

namespace JDLL.Windows
{
    public partial class LoginBox : Form
    {
        public String Username { get; private set; }

        public String Password { get; private set; }

        public LoginBox(String Sender, String Title = "Login")
        {
            InitializeComponent();

            this.label3.Text += "\r\n" + Sender;
            this.Text = Title;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Username = User.Text;
            Password = Variables.CalculateMD5(Pass.Text);

            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        private void User_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button2_Click(this, null);
            }
        }
    }
}
