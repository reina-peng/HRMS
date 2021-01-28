using HRMS.Properties;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HRMS
{
    public partial class Frm_HomePage : Form
    {
        private Frm_BulletinPublish bp = null;
        //const string cwbAPI = "https://opendata.cwb.gov.tw/api/v1/rest/datastore/F-D0047-063?Authorization=CWB-B0D98AF2-68FB-4F37-B601-17A669CED731&locationName=大安區&elementName=MinT,MaxT,PoP12h,Wx";
        const string cwbAPI = "https://opendata.cwb.gov.tw/api/v1/rest/datastore/F-C0032-001?&Authorization=CWB-B0D98AF2-68FB-4F37-B601-17A669CED731";
        JArray jsondata = null;
        MyHREntities hrEntities = new MyHREntities();
        internal int UserID;//接 login 傳過來的值
        UserInfo userInfo = null;//建立 userInfo 物件
        
        public Frm_HomePage()
        {            

            InitializeComponent();
            this.CenterToScreen();
            this.tabControl1.DrawItem += this.tabControl1_DrawItem;// 註冊 tabControl 事件
            //tabControl改側邊 > Alignment:Left > SizeMode:Fixed > 修改 ItemSize > 加下一行指令 
            this.tabControl1.DrawMode = TabDrawMode.OwnerDrawFixed;            
            this.Load += HomePage_Load;
            LoadBulletin();//載入佈告欄            
        }
        Thread thWeather;//天氣輪播執行緒
        private void HomePage_Load(object sender, EventArgs e)
        {
            jsondata = getJson(cwbAPI);//取得天氣 Json
            LoadWeather(jsondata);
            userInfo = new UserInfo(UserID);// userInfo 賦值
            //顯示右上角員工資料
            ShowImage(UserID);//顯示右上角員工圖片
            this.lblUserID.Text = userInfo.ID.ToString();
            this.lblUserName.Text = userInfo.Name;
            this.lblUserDept.Text = userInfo.Dept.ToString();
            this.lblJobTitle.Text = userInfo.JobTitle.ToString();
            //tabControl1.TabPages.Remove(tabPage1);
            //判斷員工職等設定佈告欄編輯按鈕  Visible
            this.btnPublishInfo.Visible = (userInfo.JobTitle <= 1) ? true : false;
             #region 天氣輪播執行緒
            //參考 https://www.itread01.com/content/1544577918.html
            thWeather = new Thread(delegate ()
            {
                while (true)
                {
                    ChangeImage(Image.FromFile($@"..\..\Photo\Weather\{weatherCode[0].ToString("00")}.png"), time[0], weathdescrible[0] , $"{mintemperature[0]} °c - {maxtemperature[0]} °c" , 3000);
                    ChangeImage(Image.FromFile($@"..\..\Photo\Weather\{weatherCode[1]:00}.png"), time[1], weathdescrible[1], $"{mintemperature[1]} °c - {maxtemperature[1]} °c", 3000);
                    ChangeImage(Image.FromFile($@"..\..\Photo\Weather\{weatherCode[2]:00}.png"), time[2], weathdescrible[2], $"{mintemperature[2]} °c - {maxtemperature[2]} °c", 3000);
                }
            });
            thWeather.IsBackground = true;
            thWeather.Start();
            #endregion
        }
        private void Homepage_FormClosed(object sender, FormClosedEventArgs e)
        {
            thWeather.Abort(); // 關閉時結束天氣輪播執行緒
        }
        private void ShowImage(int imageID)//載入員工圖片
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
            catch (Exception ex)
            {
                this.pictureBox1.Image = this.pictureBox1.ErrorImage;
                MessageBox.Show(ex.Message);
            }
        }
        internal void LoadBulletin()//載入佈告欄
        {            
            this.lsbBulletin.Items.Clear();
            DateTime dtnow = DateTime.Now;
            var qBulletin = (from n in hrEntities.Bulletins
                             where n.Starttime < dtnow && n.Endtime > dtnow
                             select new
                             {
                                 主旨 = n.Title,
                                 內容 = n.ContentofBulletin,
                             }).ToList();
            foreach (var x in qBulletin)
                this.lsbBulletin.Items.Add("主旨：" + x.主旨 + "內容：" + x.內容);
        }
        #region 天氣
        static public JArray getJson(string uri) //向中央氣象局取得 36 小時天氣資料
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uri); //request請求
            req.Timeout = 10000; //request逾時時間
            req.Method = "GET"; //request方式
            HttpWebResponse respone = (HttpWebResponse)req.GetResponse(); //接收respone
            StreamReader streamReader = new StreamReader(respone.GetResponseStream(), Encoding.UTF8); //讀取respone資料
            string result = streamReader.ReadToEnd(); //讀取到最後一行
            respone.Close();
            streamReader.Close();
            JObject jsondata = JsonConvert.DeserializeObject<JObject>(result); //將資料轉為json物件
            return (JArray)jsondata["records"]["location"]; //回傳json陣列
        }

        string[] time = new string[3]; //時間區段
        string[] weathdescrible = new string[3]; //天氣狀況
        string[] pop = new string[3]; //降雨機率
        string[] mintemperature = new string[3]; //最低溫度
        string[] maxtemperature = new string[3]; //最高溫度
        int[] weatherCode = new int[3];

        private void LoadWeather(JArray jsondata)//載入天氣
        {
            foreach (JObject data in jsondata)
            {
                if ((string)data["locationName"] == "臺北市")                    
                {
                    for (int i = 0; i < 3; i++)
                    {
                        time[i] = ((string)data["weatherElement"][0]["time"][i]["startTime"]).Substring(5).Remove(11) + "-" + ((string)data["weatherElement"][0]["time"][i]["endTime"]).Substring(11).Remove(5);
                        weathdescrible[i] = (string)data["weatherElement"][0]["time"][i]["parameter"]["parameterName"];
                        weatherCode[i] = (int)data["weatherElement"][0]["time"][i]["parameter"]["parameterValue"];
                        pop[i] = (string)data["weatherElement"][1]["time"][i]["parameter"]["parameterName"];
                        mintemperature[i] = (string)data["weatherElement"][2]["time"][i]["parameter"]["parameterName"];
                        maxtemperature[i] =(string)data["weatherElement"][4]["time"][i]["parameter"]["parameterName"];
                    }
                }
            }
            for (int i = 0; i < 3; i++) //顯示 3 個時段天氣資料
            {
                this.textBox1.Text += time[i] + " 天氣:" + weathdescrible[i].PadRight(8, '　') + " 溫度:" + mintemperature[i] + "°c-" + maxtemperature[i] + "°c 降雨機率:" + pop[i] + "%" + Environment.NewLine;
                this.textBox1.Text += weatherCode[i] + Environment.NewLine;
            }
        }
        #endregion
        private void ChangeImage(Image img, string time, string describe, string temp, int millisecondTimeOut)
        {
            this.Invoke(new Action(() =>
            {
                pcbWeather.Image = img;
                lblTime.Text = time;
                lblWeather.Text = describe;
                lblT.Text = temp;
            })
                );
            Thread.Sleep(millisecondTimeOut);
        }

        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e) // tabControl 設定
        {
            Graphics g = e.Graphics;
            Brush _textBrush;
            // Get the item from the collection.
            TabPage _tabPage = tabControl1.TabPages[e.Index];

            // Get the real bounds for the tab rectangle.
            Rectangle _tabBounds = tabControl1.GetTabRect(e.Index);

            if (e.State == DrawItemState.Selected)
            {
                // Draw a different background color, and don't paint a focus rectangle.
                _textBrush = new SolidBrush(Color.Red);
                g.FillRectangle(Brushes.Gray, e.Bounds);
            }
            else
            {
                _textBrush = new System.Drawing.SolidBrush(e.ForeColor);
                e.DrawBackground();
            }
            // Use our own font.
            Font _tabFont = new Font("Arial", (float)10.0, FontStyle.Bold, GraphicsUnit.Pixel);
            // Draw string. Center the text.
            StringFormat _stringFlags = new StringFormat();
            _stringFlags.Alignment = StringAlignment.Center;
            _stringFlags.LineAlignment = StringAlignment.Center;
            g.DrawString(_tabPage.Text, _tabFont, _textBrush, _tabBounds, new StringFormat(_stringFlags));
        }

        private void btnLogout_Click(object sender, EventArgs e)//登出按鈕
        {
            Frm_Login lg = new Frm_Login();
            this.Visible = false;
            lg.ShowDialog();
            System.Windows.Forms.Application.Exit();
            //this.Dispose();
            //this.Close();
        }

        private void btnPublishInfo_Click(object sender, EventArgs e)//編輯公佈欄按鈕
        {
            bp = new Frm_BulletinPublish(this);
            bp.ShowDialog();            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int a = int.TryParse(textBox3.Text, out int num1) ? num1 : 0;
            int b = int.TryParse(textBox4.Text, out int num2) ? num2 : 0;
            int c = int.TryParse(textBox5.Text, out int num3) ? num3 : 0;

            MessageBox.Show((a + b + c).ToString());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string x = dateTimePicker1.Value.ToString("yyyy/MM/dd");
            var q = (from o in hrEntities.LeaveApplications.AsEnumerable()
                     where o.EmployeeID == int.Parse(this.textBox2.Text) && o.LeaveEndTime == DateTime.Parse(x)
                     select o).ToList();
        }

        private void pictureBox1_Click(object sender, EventArgs e)//編輯個人資料
        {
            Frm_User f = new Frm_User();
            f.id = UserID;
            f.labID.Text = UserID.ToString();
            f.Show();            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }
    }
}
