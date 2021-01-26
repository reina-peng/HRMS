using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HRMS
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            this.CenterToScreen();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtPassword.Text == "1111")
            {
                HomePage hp = new HomePage();
                hp.lblAccount.Text = this.txtAccount.Text;
                this.Visible = false;
                hp.ShowDialog();
                this.Dispose();
                this.Close();
            }
            else
            {
                MessageBox.Show("Password is incorrect ", "Prompt message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }
    }
}
