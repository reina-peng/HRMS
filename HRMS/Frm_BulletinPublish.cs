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
        private Frm_HomePage hp; //儲存上層傳來的form，以便 call 上層的function
        MyHREntities HREntities = new MyHREntities();
       
        public Frm_BulletinPublish(Frm_HomePage form)//傳入上層傳來的 this 參數
        {
            InitializeComponent();
            hp = form;
            this.CenterToScreen();
            string[] categoryItems = new string[] { "[重要公告]", "[活動通知]", "[系統維護]" };
            this.cbbCategory.Items.AddRange(categoryItems);
            this.cbbCategory1.Items.AddRange(categoryItems);
            LoadDgv();
            this.dataGridView1.SelectionChanged += DataGridView1_SelectionChanged;
        }
        int number;//放文章Number
        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {            
            try
            {
                var q = (from n in HREntities.Bulletins.AsEnumerable()
                         where n.Number == (int)dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells["Number"].Value
                         select n).ToList();
                if(q.Count > 0)
                {
                    number = q[0].Number;
                    this.cbbCategory1.Text = q[0].Category;
                    this.txtTitle1.Text = q[0].Title;
                    this.txtContent1.Text = q[0].ContentofBulletin;
                    this.dateTimePicker11.Value = (DateTime)q[0].Starttime;
                    this.dateTimePicker22.Value = (DateTime)q[0].Endtime;
                }                
            }
            catch (Exception ex)
            {
                Frm_HomePage.userInfo.ErrorMsg(ex);
            }
        }

        private void LoadDgv()
        {
            try
            {
                var q = from n in HREntities.Bulletins
                        where n.Department == Frm_HomePage.userInfo.Dept
                        select n;
                this.dataGridView1.DataSource = q.ToList();
            }
            catch (Exception ex)
            {
                Frm_HomePage.userInfo.ErrorMsg(ex);
            }
        }
        private void btnApply_Click(object sender, EventArgs e)
        {            
            if(this.txtTitle.Text == "" || this.txtContent.Text =="")//判斷有沒有輸入
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
                    //Number = 2,
                    Category = this.cbbCategory.Text,
                    Title = this.txtTitle.Text,
                    Department = Frm_HomePage.userInfo.Dept,
                    ContentofBulletin = this.txtContent.Text,
                    Starttime = this.dateTimePicker1.Value,
                    Endtime = this.dateTimePicker2.Value,
                };
                this.HREntities.Bulletins.Add(bulletin);
                this.HREntities.SaveChanges();
                MessageBox.Show("公告發佈成功");
                hp.LoadBulletin();//主視窗重載佈告欄
            }
            else
                MessageBox.Show("公告發佈取消");

            Frm_HomePage.userInfo.resetText(this);
            LoadDgv();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {            
            this.Dispose();
            this.Close();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show($"確認編輯公告?", "編輯公告", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No) return;

                var q = (from n in this.HREntities.Bulletins
                         where n.Number == number
                         select n).FirstOrDefault();

                if (q == null) return;
                q.Category = this.cbbCategory1.Text;
                q.Title = this.txtTitle1.Text;
                q.ContentofBulletin = this.txtContent1.Text;
                q.Starttime = this.dateTimePicker11.Value;
                q.Endtime = this.dateTimePicker22.Value;
                this.HREntities.SaveChanges();
                MessageBox.Show("公告已修改");
                LoadDgv();
                hp.LoadBulletin();//主視窗重載佈告欄
            }
            catch (Exception ex)
            {
                Frm_HomePage.userInfo.ErrorMsg(ex);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show($"確認刪除公告?", "刪除公告", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No) return;

            var q = (from n in this.HREntities.Bulletins
                     where n.Number == number
                     select n).FirstOrDefault();

            if (q == null) return;
            this.HREntities.Bulletins.Remove(q);
            this.HREntities.SaveChanges();
            MessageBox.Show("公告已刪除");
            LoadDgv();
            hp.LoadBulletin();//主視窗重載佈告欄
        }
    }
}
