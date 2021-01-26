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
                hp.lblAccount.Text = "員工：" + this.txtAccount.Text;
                hp.lblEmpID.Text = "3345678";
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

        private void btnOnboard_Click(object sender, EventArgs e)
        {
            //TODO
            //HR 提供帳號及預設密碼，彈出新人報到表單視窗，填完表單並修改密碼完成報到作業，帳號才會正式 Enable。
        }
    }
}
