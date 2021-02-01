using HRMS.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HRMS
{
    public partial class Frm_Login : Form
    {
        public Frm_Login()
        {            
            InitializeComponent();
            this.CenterToScreen();
            UsernameTextBox.Text = "1";
            PasswordTextBox.Text = "789";
        }
        int count = 1;//計算登入次數
        
        private void btnOnboard_Click(object sender, EventArgs e)
        {
            //TODO
            //HR 提供帳號及預設密碼，彈出新人報到表單視窗，填完表單並修改密碼完成報到作業，帳號才會正式 Enable。
        }

        private void OK_Click(object sender, EventArgs e)
        {
            int UserID = int.Parse(this.UsernameTextBox.Text);
            string password = this.PasswordTextBox.Text;

            string connstring = Settings.Default.MyHR;
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = connstring;

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandText = $"select AccountEnable,EmployeeName from [User] where EmployeeID=@UserID and PassWord=@Password";

                command.Parameters.Add("@UserID", SqlDbType.NVarChar, 16).Value = UserID;
                command.Parameters.Add("@Password", SqlDbType.NVarChar, 40).Value = password;

                conn.Open();
                SqlDataReader dataReader = command.ExecuteReader();
                int x = 0;

                MyHREntities hr = new MyHREntities();

                dataReader.Read();

                if (dataReader.HasRows)
                {
                    x = int.Parse(dataReader[0].ToString());

                    if (x == 1)
                    {
                        MessageBox.Show($"登入成功，歡迎~{dataReader[1]}，祝您有個美好的一天");
                        
                        Frm_HomePage hp = new Frm_HomePage();                        
                        hp.UserID = UserID;//傳 UserID 到 Hompage
                        this.Visible = false;
                        hp.ShowDialog();
                        this.Dispose();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("帳號未啟用[AccountEnable]，請通知管理員");
                    }
                }
                else
                {
                    MessageBox.Show("登入失敗，請確認帳號密碼是否輸入錯誤");

                    count++;

                    if (count > 3)
                    {
                        MessageBox.Show("帳號已關閉，請聯絡管理員!");

                        var q = from p in hr.Users
                                where p.EmployeeID == UserID
                                select p;

                        foreach (var i in q)
                        {
                            i.AccountEnable = 0;
                        }
                        hr.SaveChanges();
                    }
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
            //this.Dispose();
            //this.Close();
        }
    }
}
