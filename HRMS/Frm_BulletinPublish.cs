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
    public partial class Frm_BulletinPublish : Form
    {
        private Frm_HomePage hp;
        MyHREntities HREntities = new MyHREntities();
        public Frm_BulletinPublish(Frm_HomePage form)
        {
            InitializeComponent();
            hp = form;
            this.CenterToScreen();
            string[] categoryItems = new string[] { "[公告]", "[活動]", "[系統維護]" };
            this.cbbCategory.Items.AddRange(categoryItems);
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            if(this.txtTitle.Text == "" || this.txtContent.Text =="")
            {
                MessageBox.Show("公告主旨或內容不可為空!!", "警告");
                return;
            }
            //TODO 判斷設定時間起始
            string s = "主旨：" + this.txtTitle.Text + "\n" + "內容：" + this.txtContent.Text;
            DialogResult result = MessageBox.Show($"確認發送{this.cbbCategory.Text}公告?\n" + s, "發送公告", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(result == DialogResult.Yes)
            {
                Bulletin bulletin = new Bulletin
                {
                    Number = 2,
                    Title = this.txtTitle.Text,
                    Department = 1,
                    ContentofBulletin = this.txtContent.Text,
                    Starttime = this.dateTimePicker1.Value,
                    Endtime = this.dateTimePicker2.Value,
                };
                this.HREntities.Bulletins.Add(bulletin);
                this.HREntities.SaveChanges();
            }
            hp.LoadBulletin();//主視窗重載佈告欄
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }
    }
}
