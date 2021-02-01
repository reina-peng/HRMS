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
    public partial class Frm_User : Form
    {
        public Frm_User()
        {
            InitializeComponent();
            this.CenterToScreen();
        }
        
        //設定員工編號為1
        internal int id = 1;

        MyHREntities HREntities = new MyHREntities();

        private void Frm_User_Load(object sender, EventArgs e)
        {
            LoadUser(id);
            ShowImage(id);
            this.pictureBox1.AllowDrop = true;
            this.pictureBox1.DragDrop += PictureBox1_DragDrop;
            this.pictureBox1.DragEnter += PictureBox1_DragEnter;
        }


        //todo wz 增加部門ID轉換成名字、 職位ID轉換成名字、 主管職位ID轉換成名字、 在職狀態ID轉換成名字
        //從資料庫撈出員工基本資料User到各欄位上面
        #region LoadUser
        private void LoadUser(int ID)
        {
            string connstring = Settings.Default.MyHR;

            try
            {

                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = connstring;
                    conn.Open();

                    SqlCommand command = new SqlCommand($"select * from [User] where EmployeeID='{ID}'", conn);
                    SqlDataReader dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        this.labID.Text = (dataReader["EmployeeID"].ToString());
                        this.labName.Text = (dataReader["EmployeeName"].ToString());
                        this.txtEnName.Text = (dataReader["EmployeeEnglishName"].ToString());
                        this.txtPassWord.Text = (dataReader["PassWord"].ToString());

                        this.DTOBD.Text = (dataReader["OnBoardDay"].ToString());
                        this.DTBBD.Text = (dataReader["ByeByeDay"].ToString());
                        this.comSex.Text = (dataReader["Gender"].ToString());
                        this.txtEmail.Text = (dataReader["Email"].ToString());
                        this.txtAdd.Text = (dataReader["Address"].ToString());
                        //this.labDept.Text = (dataReader["Department"].ToString());
                        //this.labJobTitle.Text = (dataReader["JobTitle"].ToString());
                        //this.labSup.Text = (dataReader["Supervisor"].ToString());
                        this.DTBir.Text = (dataReader["Birthday"].ToString());
                        this.txtPhone.Text = (dataReader["Phone"].ToString());
                        this.txtEmergencyP.Text = (dataReader["EmergencyPerson"].ToString());
                        this.txtEmergencyC.Text = (dataReader["EmergencyContact"].ToString());
                        this.labOBS.Text = (dataReader["OnBoardState"].ToString());
                        this.labAccS.Text = (dataReader["AccountEnable"].ToString());
                    }

                    //部門ID轉換成名字
                    dataReader.Close();
                    SqlCommand command1 = new SqlCommand($"select d.DepartmentName from [User] as u join[Department] as d on u.Department = d.DepartmentID where EmployeeID ={ID}", conn);
                    SqlDataReader dataReader1 = command1.ExecuteReader();
                    while (dataReader1.Read())
                    {
                        this.labDept.Text = (dataReader1[0].ToString());
                    }

                    //職位ID轉換成名字
                    dataReader1.Close();
                    SqlCommand command2 = new SqlCommand($"select j.JobTitle from [User] as u join [JobTitle] as j on u.JobTitle=j.JobTitleID where u.EmployeeID={ID}", conn);
                    SqlDataReader dataReader2 = command2.ExecuteReader();
                    while (dataReader2.Read())
                    {
                        this.labJobTitle.Text = (dataReader2[0].ToString());
                    }


                    //主管職位ID轉換成名字
                    dataReader2.Close();
                    SqlCommand command3 = new SqlCommand($"select j.EmployeeName from [User] as u join [User] as j on u.Supervisor=j.EmployeeID where u.EmployeeID={ID}", conn);
                    SqlDataReader dataReader3 = command3.ExecuteReader();
                    while (dataReader3.Read())
                    {
                        this.labSup.Text = (dataReader3[0].ToString());
                    }

                    //在職狀態ID轉換成名字
                    dataReader3.Close();
                    SqlCommand command4 = new SqlCommand($"select o.OnBoardStatus from [User] as u join [OnBoardStatus] as o on u.OnBoardState=o.OnBoardStatusID where u.EmployeeID={ID}", conn);
                    SqlDataReader dataReader4 = command4.ExecuteReader();
                    while (dataReader4.Read())
                    {
                        this.labOBS.Text = (dataReader4[0].ToString());
                    }



                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        #endregion


        //從資料庫抓取該員工的照片photo
        #region LoadPhoto
        private void ShowImage(int imageID)
        {


            try
            {
                string connstring = Settings.Default.MyHR;
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = connstring;
                    conn.Open();

                    SqlCommand command = new SqlCommand("select* from  [User] where EmployeeID=" + imageID, conn);
                    SqlDataReader dataReader = command.ExecuteReader();
                    //=====================
                    dataReader.Read();
                    byte[] bytes = (byte[])dataReader["Photo"];
                    System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes);
                    this.pictureBox1.Image = Image.FromStream(ms);
                    //=====================
                }


            }
            catch (Exception)
            {
                this.pictureBox1.Image = this.pictureBox1.ErrorImage;
                // MessageBox.Show(ex.Message);

            }
        }
        #endregion


        //儲存各欄位資料+圖片
        #region Save
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string Password = this.txtPassWord.Text;
                string EmployeeEnglishName = this.txtEnName.Text;

                string Gender = this.comSex.Text;
                string Email = this.txtEmail.Text;
                string Address = this.txtAdd.Text;
                string Birthday = this.DTBir.Value.ToString("yyyy/MM/dd");
                //DateTime.Today.ToString("yyyy/MM/dd");
                //DateTime.Parse();

                int Phone = int.Parse(this.txtPhone.Text);
                string EmergencyPerson = this.txtEmergencyP.Text;
                int EmergencyContact = int.Parse(this.txtEmergencyC.Text);
                //DateTime OnBoardDay =this.DTOBD.Value;
                //DateTime ByeByeDay = this.DTBBD.Value;
                // string Department = this.comDept.Text;
                // string JobTitle = this.comJobTit.Text;
                //string Supervisor = this.comSup.Text;
                // string OnBoardState = this.comOBS.Text;
                //string AccountEnable = this.comAccEnable.Text;


                string connstring = Settings.Default.MyHR;
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = connstring;

                    SqlCommand command = new SqlCommand();
                    command.CommandText =
                    $"Update [User] set PassWord='{Password}'," +
                    $"EmployeeEnglishName='{EmployeeEnglishName}'," +
                    $"Gender='{Gender}'," +
                    $"Email='{Email}'," +
                    $"Address='{Address}'," +
                    $"[Birthday]='{Birthday}'," +
                    $"Phone={Phone}," +
                    $"EmergencyPerson='{EmergencyPerson}'," +
                    $"EmergencyContact={EmergencyContact}" +
                    $" where EmployeeID='{id}'";

                    command.Connection = conn;

                    conn.Open();
                    command.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            //儲存修改的圖片
            try
            {

                string connstring = Settings.Default.MyHR;
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = connstring;


                    SqlCommand command = new SqlCommand();
                    command.CommandText = $"Update [User] set Photo=@Image where EmployeeName=@Desc";
                    command.Connection = conn;

                    //============================
                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    this.pictureBox1.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    byte[] bytes = ms.GetBuffer();
                    //============================
                    command.Parameters.Add("@Desc", SqlDbType.NVarChar).Value = this.labName.Text;
                    command.Parameters.Add("@Image", SqlDbType.Image).Value = bytes;

                    conn.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("修改成功");
                }


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }


        #endregion


        //點選PicBox跳出檔案夾去選其他圖片+拖曳圖片
        #region change picture

        //跳出檔案夾去選其他圖片
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.pictureBox1.Image = Image.FromFile(this.openFileDialog1.FileName);
            }
        }


        private void PictureBox1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        //拉圖片到PicBox裡面呈現
        private void PictureBox1_DragDrop(object sender, DragEventArgs e)
        {
            string[] filename = (string[])e.Data.GetData(DataFormats.FileDrop);
            this.pictureBox1.Image = Image.FromFile(filename[0]);

        }
        #endregion



    }
}
