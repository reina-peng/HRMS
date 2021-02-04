using HRMS.Properties;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
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
        internal static UserInfo userInfo = null;//建立 userInfo 物件
        internal static DBConnect dbConnect = new DBConnect();
        Thread thWeather;//天氣輪播執行緒
        Thread thLoadBulletin;//載入佈告欄執行緒
        public Frm_HomePage()
        {            
            InitializeComponent();
            this.CenterToScreen();
            this.tabControl1.DrawItem += this.tabControl1_DrawItem;// 註冊 tabControl 事件
            //tabControl改側邊 > Alignment:Left > SizeMode:Fixed > 修改 ItemSize > 加下一行指令            
            this.tabControl1.DrawMode = TabDrawMode.OwnerDrawFixed;
            this.gpbWeather.Paint += this.groupBox_Paint;
            this.gpbSecurity.Paint += this.groupBox_Paint;
            this.gpbBulletin.Paint += this.groupBox_Paint;
            this.Load += HomePage_Load;
            this.FormClosed += Homepage_FormClosed;
            this.tabControl1.SelectedIndexChanged += tabControl2_SelectedIndexChanged;
            LoadBulletin();//載入佈告欄
            this.txtSecuty.Text = "1.使用資訊設備請注意資訊安全，請勿點擊不明信件或網址。"+Environment.NewLine +
                "2.公司資訊請勿任意攜出或傳輸儲存設備或雲端服務。" + Environment.NewLine +
                "3.有任何資安疑慮或問題請洽 IT 人員。";
            #region 可愛教主
            LoadLeaveCategory();
            LoadDays();

            LoadCostCaTEGORY();
            LoadIntOnly();

            LoadWuChaStore();
            SetSomething();
            #endregion
            #region reina
            LoadToTreeView();//載入遺失物
            Loadcbbcombobx(); //載入請假類別

            this.cbbleave.SelectedIndex = 1;
            this.cbblost.SelectedIndex = 1;
            this.cbbfound.SelectedIndex = 1;
            this.cbbget.SelectedIndex = 1;
            this.Read_RefreshDataGridView(); //載入遺失及拾獲名單
            this.getview.SelectionChanged += Getview_SelectionChanged;
            #endregion
        }

        private void HomePage_Load(object sender, EventArgs e)
        {
            userInfo = new UserInfo(UserID);// 建立 userInfo 實體
            #region 顯示右上角員工資料
            ShowImage(UserID);//顯示右上角員工圖片            
            this.lblUserID.Text = userInfo.ID.ToString();
            this.lblUserName.Text = userInfo.Name;
            this.lblUserDept.Text = userInfo.Dept.ToString();
            this.lblDeptName.Text = userInfo.DeptName;
            this.lblJobTitle.Text = userInfo.JobTitle.ToString();
            this.lblJobTitleName.Text = userInfo.JobTitleName;
            #endregion
            //tabControl1.TabPages.Remove(tabPage1);
            //判斷員工職等設定佈告欄編輯按鈕  Visible
            this.btnPublishInfo.Visible = (userInfo.JobTitle <= 1) ? true : false;
            thLoadBulletin = new Thread(delegate ()
            {
                while (true)
                {
                    LoadBulletin(10000);
                }                    
            });
            thLoadBulletin.IsBackground = true;
            thLoadBulletin.Start();
            #region 天氣輪播
            //參考 https://www.itread01.com/content/1544577918.html
            jsondata = getJson(cwbAPI);//取得天氣 Json
            LoadWeather(jsondata, "宜蘭縣");
            thWeather = new Thread(delegate () //執行緒委派方法
            {
                while (true)
                {
                    ChangeImage(Image.FromFile($@"..\..\Photo\Weather\{weatherCode[0].ToString("00")}.png"), time[0], weathdescrible[0], $"{mintemperature[0]} °c",$"{maxtemperature[0]} °c", pop[0]);
                    ChangeImage(Image.FromFile($@"..\..\Photo\Weather\{weatherCode[1]:00}.png"), time[1], weathdescrible[1], $"{mintemperature[1]} °c", $"{maxtemperature[1]} °c", pop[1]);
                    ChangeImage(Image.FromFile($@"..\..\Photo\Weather\{weatherCode[2]:00}.png"), time[2], weathdescrible[2], $"{mintemperature[2]} °c", $"{maxtemperature[2]} °c", pop[2]);
                }
            });            
            thWeather.IsBackground = true;
            thWeather.Start();
            //天氣描述加到 PictureBox，設定透明。
            this.lblWeather.BackColor = Color.Transparent;
            this.lblWeather.Parent = this.pcbWeather;
            this.lblWeather.Location = new Point(0, 90);
            #endregion
            #region Janna
            CheckAuthority();//簽核關卡權限判斷
            GetComboboxValue();//篩選Status, Combobox取值
            labCO_ShowNowTime.Text = DateTime.Now.ToString("HH時mm分ss秒");            
            ShowChart();
            //timer使大逗!!
            timer1.Enabled = true;
            timer1.Interval = 1000;
            timer1.Start();
            tabControl1_SelectedIndexChanged(sender ,e);
            #endregion
            #region wz
            //todo wz 顯示Repair的申請人名字
            this.labRepairName.Text = userInfo.Name;
            //todo wz 顯示Repair的申請人電話
            this.labRepairPhone.Text = userInfo.Phone;
            //todo wz 載入Repair的資料到DataGridView
            RepairtoDGV();
            //todo wz 載入Repair的資料到DataGridView給主管看
            RepairtoSupervisor();
            //todo wz 新增DataGridView的button
            AddButtonofRepair();
            #endregion
            #region Tina
            this.btnWuCha_BossSearch.Visible = (userInfo.JobTitle == 1) ? true : false;   // Todo Tina
            this.btnWuCha_BossSearchDetail.Visible = (userInfo.JobTitle == 1) ? true : false;  // Todo Tina


            Information(userInfo.ID); // >  #region Tina匯入員工資料 (請假、差旅)  // Todo Tina
            #endregion
            #region reina
            LoadTolost();//審核領取物權限
            #endregion

        }
        private void Homepage_FormClosed(object sender, FormClosedEventArgs e)
        {
            thWeather.Abort(); // 關閉時結束天氣輪播執行緒
            thLoadBulletin.Abort();//關閉時結束載入佈告欄執行緒
        }
        #region ShowImage(載入員工圖片)
        private void ShowImage(int imageID)
        {
            try
            {
                //string connstring = Settings.Default.MyHR;
                //using (SqlConnection conn = new SqlConnection())
                //{
                //    conn.ConnectionString = connstring;
                //    conn.Open();

                //    SqlCommand command = new SqlCommand("select* from  [User] where EmployeeID=" + imageID, conn);
                //    SqlDataReader dataReader = command.ExecuteReader();
                //    //=====================
                //    dataReader.Read();
                //    byte[] bytes = (byte[])dataReader["Photo"];
                //    System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes);
                //    this.pictureBox1.Image = Image.FromStream(ms);
                //    //=====================
                //    this.pbLea_Picture.Image = Image.FromStream(ms);
                //    this.pbTra_Picture.Image = Image.FromStream(ms);
                //    this.pbWucha.Image = Image.FromStream(ms);
                //}
                string commStr = "select* from  [User] where EmployeeID=" + imageID;
                SqlDataReader dataReader = dbConnect.DBDataReader(commStr);
                dataReader.Read();
                //byte[] bytes = (byte[])dataReader["Photo"];
                byte[] bytes = (byte[])(dbConnect.DBDataReader(commStr))["Photo"];
                System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes);
                this.pictureBox1.Image = Image.FromStream(ms);
            }
            catch (Exception ex)
            {
                this.pictureBox1.Image = this.pictureBox1.ErrorImage;
            }
        }
        #endregion
        #region 載入佈告欄
        private void LoadBulletin(int a)
        {
            this.Invoke(new Action(() =>
            {
                LoadBulletin();
            }));
            Thread.Sleep(a);
        }
        internal void LoadBulletin()//載入佈告欄
        {            
            this.lsbBulletin.Items.Clear();
            DateTime dtnow = DateTime.Now;
            var qBulletin = (from n in hrEntities.Bulletins
                             where n.Starttime < dtnow && n.Endtime > dtnow
                             select new
                             {
                                 ID = n.Number,
                                 類別 = n.Category,
                                 主旨 = n.Title,
                                 內容 = n.ContentofBulletin,
                             }).ToList();
            foreach (var x in qBulletin)
                this.lsbBulletin.Items.Add(x.ID + x.類別 + x.主旨);
        }
        #endregion
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

        private void LoadWeather(JArray jsondata, string City)//載入天氣
        {
            foreach (JObject data in jsondata)
            {
                if ((string)data["locationName"] == City)                    
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
            //for (int i = 0; i < 3; i++) //顯示 3 個時段天氣資料
            //{
            //    this.txtSecuty.Text += time[i] + " 天氣:" + weathdescrible[i].PadRight(8, '　') + " 溫度:" + mintemperature[i] + "°c-" + maxtemperature[i] + "°c 降雨機率:" + pop[i] + "%" + Environment.NewLine;
            //    this.txtSecuty.Text += weatherCode[i] + Environment.NewLine;
            //}
        }        
        private void ChangeImage(Image img, string time, string describe, string tempMin, string tempMax, string pop, int millisecondTimeOut = 4000)
        {
            this.Invoke(new Action(() =>
            {
                gpbWeather.Text = "";
                pcbWeather.Image = img;
                lblTime.Text = time;
                lblWeather.Text = describe;
                lblTmin.Text = tempMin;
                lblTmax.Text = tempMax;
                lblPop.Text = pop + "%";
            }));
            Thread.Sleep(millisecondTimeOut);
        }
        #endregion
        #region TabControl 顯示設定
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
            Font _tabFont = new Font("Arial", (float)20.0, FontStyle.Bold, GraphicsUnit.Pixel);
            // Draw string. Center the text.
            StringFormat _stringFlags = new StringFormat();
            _stringFlags.Alignment = StringAlignment.Center;
            _stringFlags.LineAlignment = StringAlignment.Center;
            g.DrawString(_tabPage.Text, _tabFont, _textBrush, _tabBounds, new StringFormat(_stringFlags));            
        }
        #endregion
        private void SetSomething()  // Todo Tina
        {
            comWuCha_TempItem.Visible = false;
            comWuCha_TempPrice.Visible = false;
            WuCha_dataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("微軟正黑體", 14);
            WuCha_dataGridView.DefaultCellStyle.Font = new Font("微軟正黑體", 12);
            WuCha_dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            WuCha_dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            WuCha_dataGridView.RowsDefaultCellStyle.ForeColor = Color.Black;
            WuCha_dataGridView.RowsDefaultCellStyle.BackColor = Color.Wheat;

        }
        private void groupBox_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(((GroupBox)sender).BackColor);

            Rectangle Rtg_LT = new Rectangle();
            Rectangle Rtg_RT = new Rectangle();
            Rectangle Rtg_LB = new Rectangle();
            Rectangle Rtg_RB = new Rectangle();
            Rtg_LT.X = 0; Rtg_LT.Y = 7; Rtg_LT.Width = 10; Rtg_LT.Height = 10;
            Rtg_RT.X = e.ClipRectangle.Width - 11; Rtg_RT.Y = 7; Rtg_RT.Width = 10; Rtg_RT.Height = 10;
            Rtg_LB.X = 0; Rtg_LB.Y = e.ClipRectangle.Height - 11; Rtg_LB.Width = 10; Rtg_LB.Height = 10;
            Rtg_RB.X = e.ClipRectangle.Width - 11; Rtg_RB.Y = e.ClipRectangle.Height - 11; Rtg_RB.Width = 10; Rtg_RB.Height = 10;

            Color color = Color.FromArgb(51, 94, 168);
            Pen Pen_AL = new Pen(color, 1);
            Pen_AL.Color = color;
            Brush brush = new HatchBrush(HatchStyle.Divot, color);

            e.Graphics.DrawString(((GroupBox)sender).Text, ((GroupBox)sender).Font, brush, 6, 0);
            e.Graphics.DrawArc(Pen_AL, Rtg_LT, 180, 90);
            e.Graphics.DrawArc(Pen_AL, Rtg_RT, 270, 90);
            e.Graphics.DrawArc(Pen_AL, Rtg_LB, 90, 90);
            e.Graphics.DrawArc(Pen_AL, Rtg_RB, 0, 90);
            e.Graphics.DrawLine(Pen_AL, 5, 7, 6, 7);
            e.Graphics.DrawLine(Pen_AL, e.Graphics.MeasureString(((GroupBox)sender).Text, ((GroupBox)sender).Font).Width + 3, 7, e.ClipRectangle.Width - 7, 7);
            e.Graphics.DrawLine(Pen_AL, 0, 13, 0, e.ClipRectangle.Height - 7);
            e.Graphics.DrawLine(Pen_AL, 6, e.ClipRectangle.Height - 1, e.ClipRectangle.Width - 7, e.ClipRectangle.Height - 1);
            e.Graphics.DrawLine(Pen_AL, e.ClipRectangle.Width - 1, e.ClipRectangle.Height - 7, e.ClipRectangle.Width - 1, 13);
        }
        #region 測試碼
        //private void button1_Click(object sender, EventArgs e)
        //{
        //    int a = int.TryParse(textBox3.Text, out int num1) ? num1 : 0;
        //    int b = int.TryParse(textBox4.Text, out int num2) ? num2 : 0;
        //    int c = int.TryParse(textBox5.Text, out int num3) ? num3 : 0;

        //    MessageBox.Show((a + b + c).ToString());
        //}

        //private void button2_Click(object sender, EventArgs e)
        //{
        //    string x = dateTimePicker1.Value.ToString("yyyy/MM/dd");
        //    var q = (from o in hrEntities.LeaveApplications.AsEnumerable()
        //             where o.EmployeeID == int.Parse(this.textBox2.Text) && o.LeaveEndTime == DateTime.Parse(x)
        //             select o).ToList();
        //    userInfo.resetText(this.tabControl1.SelectedTab);
        //}
        #endregion

        #region 主頁按扭
        private void btnLogout_Click(object sender, EventArgs e)//登出按鈕
        {
            Frm_Login lg = new Frm_Login();
            this.Visible = false;
            lg.ShowDialog();
            System.Windows.Forms.Application.Exit();
            //this.Dispose();
            //this.Close();
            //
        }

        private void btnPublishInfo_Click(object sender, EventArgs e)//編輯公佈欄按鈕
        {
            bp = new Frm_BulletinPublish(this);//因新視窗有功能需要 call 目前視窗的function，傳入 this
            bp.ShowDialog();
        }
        

        private void pictureBox1_Click(object sender, EventArgs e)//按照片跳出編輯個人資料頁
        {
            Frm_User f = new Frm_User();
            f.id = UserID;
            f.labID.Text = UserID.ToString();
            f.Show();
            f.button1.Click += Button1_Click;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            ShowImage(userInfo.ID);
            Information(userInfo.ID);
        }
        #endregion

        #region 打卡
        private void timer1_Tick(object sender, EventArgs e)// 顯示現在時間
        {
            labCO_ShowNowTime.Text = DateTime.Now.ToString("HH時mm分ss秒");
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e) //設定打卡紀錄查詢日期區間 // todo
        {
            using (SqlConnection conn = new SqlConnection(Settings.Default.MyHR))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                conn.Open();
                command.CommandText = "select * from [User] where EmployeeID=" + userInfo.ID;
                SqlDataReader dataReader = command.ExecuteReader();
                dataReader.Read();

                DateTime mindate = DateTime.Parse(dataReader["OnBoardDay"].ToString());
                this.dtpCO_ShearchStart.MinDate = mindate;
                this.dtpCO_ShearchEnd.MinDate = mindate;

                this.dtpCO_ShearchStart.MaxDate = DateTime.Today;
                this.dtpCO_ShearchEnd.MaxDate = DateTime.Today;

                this.dtpCO_ShearchStart.Value = DateTime.Today;
                this.dtpCO_ShearchEnd.Value = DateTime.Today;
            }
        }

        //todo 修改開始------------------------------------------------------------------------------------------
        private void btnClockOn_Click(object sender, EventArgs e) //打上班卡
        {
            var q = from c in hrEntities.Absences.AsEnumerable()
                    let x = DateTime.Today.Date.ToString("yyyy/MM/dd")
                    where c.EmployeeID == userInfo.ID && c.On.Value.Date.ToString("yyyy/MM/dd") == x
                    select c;

            if (q.ToList().Count == 0)//判斷資料庫無打卡紀錄
            {
                labClockTime_ON.Text = "您已打卡成功 !\n" + DateTime.Now;
                Absence absence = new Absence { EmployeeID = userInfo.ID, On = DateTime.Now };
                this.hrEntities.Absences.Add(absence);
                this.hrEntities.SaveChanges();
            }
            else//有打卡紀錄
            {
                labClockTime_ON.Text = "您已打過上班卡! \n無須再打卡";
            }
        }
        private void btnClockOn_OFF_Click(object sender, EventArgs e)//todo 下班卡
        {
            DateTime now = DateTime.Now;
            DateTime off = DateTime.Today.AddHours(18);

            var q = from c in hrEntities.Absences.AsEnumerable()
                    let x = DateTime.Today.Date.ToString("yyyy/MM/dd")
                    where c.EmployeeID == userInfo.ID && c.On.Value.Date.ToString("yyyy/MM/dd") == x
                    select c;

            if (q.ToList().Count() == 0)//尚未打上班卡(新增資料庫)
            {
                MessageBox.Show("今日尚未打上班卡，請先打上班卡", "溫馨小提醒", MessageBoxButtons.OK);
            }
            else//已打過上班卡(修改資料庫)
            {
                if (now > off) //判斷超過六點且已打上班卡
                {
                    labClockTime_OFF.Text = "您已打卡成功 !\n" + DateTime.Now;

                    var q2 = from c in hrEntities.Absences.AsEnumerable()
                             let x = DateTime.Today.Date.ToString("yyyy/MM/dd")
                             where c.EmployeeID == userInfo.ID && c.On.Value.Date.ToString("yyyy/MM/dd") == x
                             select c;
                    //var qq = q2.ToList();
                    //修改該EmployeeID的資料
                    foreach (var item in q2)
                    {
                        item.Off = DateTime.Now;
                    }
                    this.hrEntities.SaveChanges();
                }

                else if (now < off)//非下班時間要打卡
                {
                    if (MessageBox.Show("現在非下班時間\n仍然要打下班卡嗎??", "溫馨小提醒", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        labClockTime_OFF.Text = "您已打卡成功 !\n" + DateTime.Now;
                        var f = from a in hrEntities.Absences.AsEnumerable()
                                let x = DateTime.Today.ToString("yyyy/MM/dd")
                                where a.EmployeeID == userInfo.ID && a.On.Value.Date.ToString("yyyy/MM/dd") == x
                                select a;

                        foreach (var item in f)
                        {
                            item.Off = DateTime.Now;
                        }
                        this.hrEntities.SaveChanges();
                    }
                }
            }
        }
        //todo 修改結束------------------------------------------------------------------------------------------

        private void btnSearch_Click_1(object sender, EventArgs e) //打卡記錄查詢
        {
            this.dgvCO_Search.Columns.Clear();

            DateTime StartDate = this.dtpCO_ShearchStart.Value;
            //設為該日期的23:59:59
            DateTime EndDate = new DateTime(this.dtpCO_ShearchEnd.Value.Year, this.dtpCO_ShearchEnd.Value.Month, this.dtpCO_ShearchEnd.Value.Day, 23, 59, 59);

            if (StartDate > EndDate)
            {
                MessageBox.Show("查詢期間錯誤...\n請重新選擇其他查詢區間");
            }
            else
            {
                var q = from date in hrEntities.Absences.AsEnumerable()
                        join u in hrEntities.Users on date.EmployeeID equals u.EmployeeID
                        where date.EmployeeID == userInfo.ID && date.On.Value.Date >= StartDate && date.On.Value.Date <= EndDate
                        select new
                        {
                            員工編號 = date.EmployeeID,
                            員工姓名 = u.EmployeeName,
                            上班打卡紀錄 = date.On.Value,
                            下班打卡紀錄 = date.Off.Value
                        };

                dgvCO_Search.DataSource = q.ToList();

            }
        }

        #endregion

        #region  wz的Repair 報修申請


        //報修送出
        private void btnSave_Repair_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(comRepairCategory.Text) || string.IsNullOrWhiteSpace(txtRepairLocation.Text) || string.IsNullOrWhiteSpace(txtRepairContent.Text))
            {
                MessageBox.Show("請輸入文字，內容不得為空");
            }
            else
            {
                Repair repair1 = new Repair
                {
                    EmployeeID = userInfo.ID,
                    AppleDate = DateTime.Now,
                    RepairCategory = this.comRepairCategory.Text,
                    Location = this.txtRepairLocation.Text,
                    ContentofRepair = this.txtRepairContent.Text,
                    Phone = userInfo.Phone,
                    RepairStatus = 0,
                };
                this.hrEntities.Repairs.Add(repair1);
                this.hrEntities.SaveChanges();

                MessageBox.Show("申請報修成功，靜待專人前往處理");
                tabControl1.SelectedTab.Refresh();
            }

            this.RepairtoDGV();


            //刷新tab裡面的textbox
            Process(this.tabControl1.SelectedTab.Controls);
            void Process(Control.ControlCollection c)
            //參考網址：https://blog.csdn.net/lubiaopan/article/details/5784846?utm_medium=distribute.pc_relevant_bbs_down.none-task-blog-baidujs-2.nonecase&depth_1-utm_source=distribute.pc_relevant_bbs_down.none-task-blog-baidujs-2.nonecase
            {
                if (c.Count > 0)
                {
                    foreach (Control c1 in c)
                    {
                        if (c1.GetType().ToString().Equals("System.Windows.Forms.TextBox"))
                        {
                            ((TextBox)c1).Clear();
                        }
                        if (c1 != null)
                        {
                            Process(c1.Controls);
                            this.comRepairCategory.Text = "請選擇";
                        }
                        else
                        {
                            return;
                        }
                    }
                }
                else
                {
                    return;
                }
            }
        }
        //報修清空
        private void btnClear_Repair_Click(object sender, EventArgs e)
        {
            this.comRepairCategory.Text = "請選擇";
            this.txtRepairLocation.Text = "";
            this.txtRepairContent.Text = "";
        }

        //Load  Repair to dataGridView1
        private void RepairtoDGV()
        {  //顯示的資料欄位設定
            var q = hrEntities.Repairs.OrderBy(r => r.RepairStatus.Value).ThenByDescending(r => r.RepairNumber).Where(r => r.EmployeeID == userInfo.ID).Select(r => new
            {
                申請單號 = r.RepairNumber,
                申請者 = r.User.EmployeeName,
                申請日期 = r.AppleDate,
                報修類別 = r.RepairCategory,
                地點 = r.Location,
                報修內容 = r.ContentofRepair,
                維修狀態 = r.RepairStatus == 0 ? "維修中" : "完成"
            });
            dgv_Repair.DataSource = q.ToList();

            var q1 = from g in q
                     group g by g.維修狀態 into g1
                     select new
                     {
                         維修狀態 = g1.Key,
                         數量 = (int)g1.Count()
                     };

            //參考網址：https://dotblogs.com.tw/shunnien/2013/04/22/102049
            this.chartRepair.Series[0].Label = "#VALX : #PERCENT{P0}";
            this.chartRepair.DataSource = q1.ToList();
            this.chartRepair.Series[0].XValueMember = "維修狀態";
            this.chartRepair.Series[0].YValueMembers = "數量";
            this.chartRepair.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
            this.chartRepair.Series[0].IsValueShownAsLabel = true;
            this.chartRepair.Legends[0].Title = "維修狀態";
            this.chartRepair.Series[0].LegendText = "#VALX:#VALY 件 ( #PERCENT{P0} )";


            //DataGridView的屬性、顏色設定
          

        }

        #endregion

        #region wz的Repair 報修查詢

        //Load Repair to Supervisor
        private void RepairtoSupervisor()
        {
            if (userInfo.Dept == 4 || userInfo.Dept == 5 && userInfo.Supervisor == userInfo.ID)
            {
                var q = from r in hrEntities.Repairs
                        where (r.RepairCategory.Contains("資訊") && userInfo.Dept == 4) || (r.RepairCategory.Contains("總務") && userInfo.Dept == 5)
                        select new
                        {
                            申請單號 = r.RepairNumber,
                            申請者 = r.User.EmployeeName,
                            申請日期 = r.AppleDate,
                            報修類別 = r.RepairCategory,
                            地點 = r.Location,
                            報修內容 = r.ContentofRepair,
                            維修狀態 = r.RepairStatus == 0 ? "維修中" : "完成"
                        };

                dgv_RepairSup.DataSource = q.ToList();
            }
            //todo wz 若整合請將下方的tabpage5該功能放置的數
            else //除了部門4、5可看到此tabpage(維修通知)
            {
                tabControl1.TabPages.Remove(報修審核);
            }
        }

        //新增結案按鈕
        private void AddButtonofRepair()
        {
            DataGridViewButtonColumn column = new DataGridViewButtonColumn();
            //DataGridViewDisableButtonColumn column = new DataGridViewDisableButtonColumn();
            //設定列的名字
            column.Name = "是否結案";
            //設定所有在按鈕上"結案"的名字
            column.UseColumnTextForButtonValue = true;
            column.Text = "結案";
            //向DataGridView追加
            dgv_RepairSup.Columns.Add(column);


            //for (int i = 0; i < dgv_RepairSup.Rows.Count - 1; i++)
            //{
            //    DataGridViewDisableButtonCell btncell = (DataGridViewDisableButtonCell)dgv_RepairSup.Rows[i].Cells["是否結案"];
            //    string states = dgv_RepairSup.Rows[i].Cells["維修狀態"].Value.ToString();

            //    if (states == "維修中")
            //    {
            //        btncell.Enabled = true;

            //    }

            //    if (btncell.Enabled)
            //    {
            //        btncell.Enabled = false;
            //    }

            //}



        }


        //點選結案按鈕進行狀態更改
        private void dgv_RepairSup_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int row, column;

            /*Read the button position which is pressed*/
            row = e.RowIndex;
            column = e.ColumnIndex;
            int num = (int)dgv_RepairSup.Rows[row].Cells["申請單號"].Value;
            //如果是"是否結案"列，按鈕被點擊
            if (dgv_RepairSup.Columns[column].Name == "是否結案")//此處索引列可以使name、也可以使headertext...
            {
                var q = from r in hrEntities.Repairs
                        where r.RepairNumber == num
                        select r;

                foreach (var i in q)
                {
                    i.RepairStatus = 1;
                }
                hrEntities.SaveChanges();
            }

            //點選完結案重新載入資料進來
            RepairtoSupervisor();

            //button Enabled
            //DataGridViewDisableButtonCell btncell = (DataGridViewDisableButtonCell)dgv_RepairSup.Rows[e.RowIndex].Cells["是否結案"];
            //string states = dgv_RepairSup.Rows[row].Cells["維修狀態"].Value.ToString();
            //if (states == "完成")
            //{
            //    btncell.Enabled = false;
            //}
            //if (btncell.Enabled)
            //{
            //    btncell.Enabled = false;
            //}

        }

        //datagridview顏色
        private void dgv_RepairSup_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //參考網址：https://www.366service.com/zh-tw/qa/4ae5bda2b10ce73fc97bd23e31e2e34e

            for (int i = 0; i < dgv_RepairSup.Rows.Count; i++)
            {
                DataGridViewRow dgvr1 = dgv_RepairSup.Rows[i];
                if (i % 2 == 0)
                {
                    dgv_RepairSup.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(255, 196, 120);
                }
                else
                {
                    dgv_RepairSup.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(240, 229, 216);
                }
            }

            for (int i = 0; i < dgv_RepairSup.Rows.Count; i++)
            {
                DataGridViewRow dgvr = dgv_RepairSup.Rows[i];
                if (dgvr.Cells[7].Value.ToString() == "維修中")
                {
                    dgvr.Cells[7].Style.BackColor = Color.FromArgb(170, 58, 58);
                    dgvr.Cells[7].Style.ForeColor = Color.White;
                    //dgvr.Cells[7].Style.Font= new Font(dgv_RepairSup.Font, FontStyle.Bold);
                }
                else if (dgvr.Cells[7].Value.ToString() == "完成")
                {
                    dgvr.Cells[7].Style.BackColor = Color.FromArgb(94, 168, 135);
                    dgvr.Cells[7].Style.ForeColor = Color.White;
                    //dgvr.Cells[7].Style.Font = new Font(dgv_RepairSup.Font, FontStyle.Bold);
                }
            }
            //標題和內容文字置中
            dgv_RepairSup.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv_RepairSup.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

        }

        //一鍵審核通過
        private void btnRepairAllCheck_Click(object sender, EventArgs e)
        {
            if (userInfo.Dept == 4 || userInfo.Dept == 5 && userInfo.Supervisor == userInfo.ID)
            {
                var q = from r in hrEntities.Repairs
                        where (r.RepairCategory.Contains("資訊") && userInfo.Dept == 4) || (r.RepairCategory.Contains("總務") && userInfo.Dept == 5)
                        select r;

                foreach (var i in q)
                {
                    i.RepairStatus = 1;
                }
                hrEntities.SaveChanges();

            }
            //點選完結案重新載入資料進來
            RepairtoSupervisor();
        }



        #endregion

        #region 簽核關卡
        private void ShowChart()
        {
            //每月請假人數圖表
            var qL = from l in hrEntities.LeaveApplications
                     group l by l.LeaveStartTime.Value.Month into g
                     select new
                     {
                         月份 = g.Key,
                         人數 = g.Count()
                     };
            this.chartLeave.DataSource = qL.ToList();
            this.chartLeave.Series[0].XValueMember = "月份";
            this.chartLeave.Series[0].YValueMembers = "人數";
            this.chartLeave.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
            this.chartLeave.Series[0].Label = "#VALY 人";

            //每月出差申請人數圖表
            var qT = from t in hrEntities.Travel_Expense_Application
                     group t by t.TravelStartTime.Month into g
                     select new
                     {
                         月份 = g.Key,
                         人數 = g.Count()
                     };
            this.chartTravel.DataSource = qT.ToList();
            this.chartTravel.Series[0].XValueMember = "月份";
            this.chartTravel.Series[0].YValueMembers = "人數";
            this.chartTravel.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
            this.chartTravel.Series[0].Label = "#VALY 人";
        }

        private void CheckAuthority() //簽核關卡權限判斷
        {
            if (userInfo.JobTitle > 2)
            {
                tabControl1.TabPages.Remove(簽核關卡);
            }
        }

        private void GetComboboxValue() //leave & traval combobox的值
        {
            this.comBoxLeave.Items.Clear();
            this.comboBoxTravel.Items.Clear();

            //leave
            var qL = (from l in hrEntities.LeaveApplications
                      join c in hrEntities.CheckStatus on l.CheckStatus equals c.CheckStatusID
                      where l.Department == userInfo.Dept
                      orderby l.CheckStatus ascending
                      select c.CheckStatus).Distinct();

            foreach (var item in qL)
            {
                this.comBoxLeave.Items.Add(item);
            }

            //traval
            var qT = (from t in hrEntities.Travel_Expense_Application
                      join c in hrEntities.CheckStatus on t.CheckStatus equals c.CheckStatusID
                      where t.Department == userInfo.Dept
                      orderby t.CheckStatus ascending
                      select c.CheckStatus).Distinct();

            foreach (var item in qT)
            {
                this.comboBoxTravel.Items.Add(item);
            }
        }

        private void btnCS_SearchLeave_Click(object sender, EventArgs e)//查詢請假申請 
        {
            this.dgvCS_Leave.Columns.Clear();

            DateTime StartDate = this.dtpCS_SearchLeaveStart.Value;
            DateTime EndDate = this.dtpCS_SearchLeaveEnd.Value;
            dgvCS_Leave.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            if (StartDate > EndDate)
            {
                MessageBox.Show("查詢期間錯誤...\n請重新選擇其他查詢區間");
            }
            else
            {
                try
                {
                    if (comBoxLeave.Text == "審核中") //審核中
                    {
                        var qL = from a in hrEntities.LeaveApplications
                                 join l in hrEntities.Leaves on a.LeaveCategory equals l.LeaveID
                                 join ch in hrEntities.CheckStatus on a.CheckStatus equals ch.CheckStatusID
                                 join d in hrEntities.Departments on a.Department equals d.DepartmentID
                                 orderby a.LeaveStartTime.Value ascending
                                 where a.Department == userInfo.Dept && ch.CheckStatusID == 1 && a.LeaveStartTime.Value >= StartDate && a.LeaveStartTime.Value <= EndDate
                                 select new
                                 {
                                     假單編號 = a.ApplyNumber,
                                     請假類別 = a.Leave.LeaveCategory,
                                     員工編號 = a.EmployeeID,
                                     員工姓名 = a.User.EmployeeName,
                                     申請時間 = a.ApplyDate,
                                     請假起始日 = a.LeaveStartTime,
                                     請假結束日 = a.LeaveEndTime,
                                     事由 = a.Reason,
                                     審核狀態 = ch.CheckStatus
                                 };

                        dgvCS_Leave.DataSource = qL.ToList();
                        dgvButton();
                    }
                    else if (comBoxLeave.Text == "審核通過") //通過
                    {
                        var qL = from a in hrEntities.LeaveApplications
                                 join l in hrEntities.Leaves on a.LeaveCategory equals l.LeaveID
                                 join ch in hrEntities.CheckStatus on a.CheckStatus equals ch.CheckStatusID
                                 join d in hrEntities.Departments on a.Department equals d.DepartmentID
                                 orderby a.LeaveStartTime.Value ascending
                                 where a.Department == userInfo.Dept && ch.CheckStatusID == 2 && a.LeaveStartTime.Value >= StartDate && a.LeaveStartTime.Value <= EndDate
                                 select new
                                 {
                                     假單編號 = a.ApplyNumber,
                                     請假類別 = a.Leave.LeaveCategory,
                                     員工編號 = a.EmployeeID,
                                     員工姓名 = a.User.EmployeeName,
                                     申請時間 = a.ApplyDate,
                                     請假起始日 = a.LeaveStartTime,
                                     請假結束日 = a.LeaveEndTime,
                                     事由 = a.Reason,
                                     審核狀態 = ch.CheckStatus
                                 };

                        dgvCS_Leave.DataSource = qL.ToList();
                    }
                    else //退件
                    {
                        var qL = from a in hrEntities.LeaveApplications
                                 join l in hrEntities.Leaves on a.LeaveCategory equals l.LeaveID
                                 join ch in hrEntities.CheckStatus on a.CheckStatus equals ch.CheckStatusID
                                 join d in hrEntities.Departments on a.Department equals d.DepartmentID
                                 orderby a.LeaveStartTime.Value ascending
                                 where a.Department == userInfo.Dept && ch.CheckStatusID == 3 && a.LeaveStartTime.Value >= StartDate && a.LeaveStartTime.Value <= EndDate
                                 select new
                                 {
                                     假單編號 = a.ApplyNumber,
                                     請假類別 = a.Leave.LeaveCategory,
                                     員工編號 = a.EmployeeID,
                                     員工姓名 = a.User.EmployeeName,
                                     申請時間 = a.ApplyDate,
                                     請假起始日 = a.LeaveStartTime,
                                     請假結束日 = a.LeaveEndTime,
                                     事由 = a.Reason,
                                     審核狀態 = ch.CheckStatus
                                 };

                        dgvCS_Leave.DataSource = qL.ToList();                        
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            this.btnCS_AllPassLeave.Enabled = true;
            GetComboboxValue();
        }

        private void dgvButton()// build datagridview buttons
        {
            if (tabcCS.SelectedIndex == 0)
            {
                DataGridViewButtonColumn btnColumnPass = new DataGridViewButtonColumn();
                btnColumnPass.Name = "通過";
                btnColumnPass.Text = "通過";
                btnColumnPass.UseColumnTextForButtonValue = true;
                dgvCS_Leave.Columns.Insert(9, btnColumnPass);

                DataGridViewButtonColumn btnColumnFail = new DataGridViewButtonColumn();
                btnColumnFail.Name = "退件";
                btnColumnFail.Text = "退件";
                btnColumnFail.UseColumnTextForButtonValue = true;
                dgvCS_Leave.Columns.Insert(10, btnColumnFail);
            }
            else if (tabcCS.SelectedIndex == 1)
            {
                DataGridViewButtonColumn btnColumnPass = new DataGridViewButtonColumn();
                btnColumnPass.Name = "通過";
                btnColumnPass.Text = "通過";
                btnColumnPass.UseColumnTextForButtonValue = true;
                dgvCS_Travel.Columns.Insert(9, btnColumnPass);

                DataGridViewButtonColumn btnColumnFail = new DataGridViewButtonColumn();
                btnColumnFail.Name = "退件";
                btnColumnFail.Text = "退件";
                btnColumnFail.UseColumnTextForButtonValue = true;
                dgvCS_Travel.Columns.Insert(10, btnColumnFail);
            }
        }

        private void dgvCS_Leave_CellContentClick(object sender, DataGridViewCellEventArgs e) //點擊dgvCS_Leave button
        {
            int an = (int)dgvCS_Leave.Rows[e.RowIndex].Cells["假單編號"].Value;

            if (dgvCS_Leave.Columns[e.ColumnIndex].Name == "通過")
            {
                var q = from l in hrEntities.LeaveApplications
                        where l.ApplyNumber == an
                        select l;
                foreach (var item in q)
                {
                    item.CheckStatus = 2;
                }
                hrEntities.SaveChanges();
            }
            else if (dgvCS_Leave.Columns[e.ColumnIndex].Name == "退件")
            {
                var q = from l in hrEntities.LeaveApplications
                        where l.ApplyNumber == an
                        select l;
                foreach (var item in q)
                {
                    item.CheckStatus = 3;
                }
                hrEntities.SaveChanges();
            }
            btnCS_SearchLeave_Click(sender, e);
            GetComboboxValue();
        }

        private void btnCS_AllPassLeave_Click(object sender, EventArgs e) // 請假申請一鍵通過
        {
            var q = from l in hrEntities.LeaveApplications
                    where l.Department == userInfo.Dept && l.CheckStatus == 1
                    select l;
            foreach (var item in q)
            {
                item.CheckStatus = 2;
            }
            hrEntities.SaveChanges();

            btnCS_SearchLeave_Click(sender, e);
            GetComboboxValue();
        }

        private void btnCS_SearchTravel_Click(object sender, EventArgs e)//查詢出差申請
        {
            this.dgvCS_Travel.Columns.Clear();

            DateTime StartDate = this.dtpCS_SearchTravelStart.Value;
            DateTime EndDate = this.dtpCS_SearchTravelEnd.Value;
            dgvCS_Travel.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            if (StartDate > EndDate)
            {
                MessageBox.Show("查詢期間錯誤...\n請重新選擇其他查詢區間");
            }
            else
            {
                try
                {
                    if (comboBoxTravel.Text == "審核中") //審核中
                    {
                        var qT = from t in hrEntities.Travel_Expense_Application
                                 join ch in hrEntities.CheckStatus on t.CheckStatus equals ch.CheckStatusID
                                 join d in hrEntities.Departments on t.Department equals d.DepartmentID
                                 orderby t.TravelStartTime ascending
                                 where t.Department == userInfo.Dept && ch.CheckStatusID == 1 && t.TravelStartTime >= StartDate && t.TravelStartTime <= EndDate
                                 select new
                                 {
                                     差旅單號 = t.ApplyNumber,
                                     員工編號 = t.EmployeeID,
                                     員工姓名 = t.User.EmployeeName,
                                     申請時間 = t.ApplyDate,
                                     差旅起始日 = t.TravelStartTime,
                                     差旅結束日 = t.TravelEndTime,
                                     申請費用 = t.Amont,
                                     事由 = t.Reason,
                                     審核狀態 = ch.CheckStatus
                                 };

                        dgvCS_Travel.DataSource = qT.ToList();
                        dgvButton();
                    }
                    else if (comboBoxTravel.Text == "審核通過") //通過
                    {
                        var qT = from t in hrEntities.Travel_Expense_Application
                                 join ch in hrEntities.CheckStatus on t.CheckStatus equals ch.CheckStatusID
                                 join d in hrEntities.Departments on t.Department equals d.DepartmentID
                                 orderby t.TravelStartTime ascending
                                 where t.Department == userInfo.Dept && ch.CheckStatusID == 2 && t.TravelStartTime >= StartDate && t.TravelStartTime <= EndDate
                                 select new
                                 {
                                     差旅單號 = t.ApplyNumber,
                                     員工編號 = t.EmployeeID,
                                     員工姓名 = t.User.EmployeeName,
                                     申請時間 = t.ApplyDate,
                                     差旅起始日 = t.TravelStartTime,
                                     差旅結束日 = t.TravelEndTime,
                                     申請費用 = t.Amont,
                                     事由 = t.Reason,
                                     審核狀態 = ch.CheckStatus
                                 };

                        dgvCS_Travel.DataSource = qT.ToList();

                    }
                    else //退件
                    {
                        var qT = from t in hrEntities.Travel_Expense_Application
                                 join ch in hrEntities.CheckStatus on t.CheckStatus equals ch.CheckStatusID
                                 join d in hrEntities.Departments on t.Department equals d.DepartmentID
                                 orderby t.TravelStartTime ascending
                                 where t.Department == userInfo.Dept && ch.CheckStatusID == 3 && t.TravelStartTime >= StartDate && t.TravelStartTime <= EndDate
                                 select new
                                 {
                                     差旅單號 = t.ApplyNumber,
                                     員工編號 = t.EmployeeID,
                                     員工姓名 = t.User.EmployeeName,
                                     申請時間 = t.ApplyDate,
                                     差旅起始日 = t.TravelStartTime,
                                     差旅結束日 = t.TravelEndTime,
                                     申請費用 = t.Amont,
                                     事由 = t.Reason,
                                     審核狀態 = ch.CheckStatus
                                 };

                        dgvCS_Travel.DataSource = qT.ToList();                        
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            this.btnCS_AllPassTravel.Enabled = true;
            GetComboboxValue();
        }

        private void dgvCS_Travel_CellContentClick(object sender, DataGridViewCellEventArgs e)//點擊dgvCS_Travel button
        {
            int tn = (int)dgvCS_Travel.Rows[e.RowIndex].Cells["差旅單號"].Value;

            if (dgvCS_Travel.Columns[e.ColumnIndex].Name == "通過")
            {
                var q = from t in hrEntities.Travel_Expense_Application
                        where t.ApplyNumber == tn
                        select t;
                foreach (var item in q)
                {
                    item.CheckStatus = 2;
                }
                hrEntities.SaveChanges();
            }
            else if (dgvCS_Travel.Columns[e.ColumnIndex].Name == "退件")
            {
                var q = from t in hrEntities.Travel_Expense_Application
                        where t.ApplyNumber == tn
                        select t;
                foreach (var item in q)
                {
                    item.CheckStatus = 3;
                }
                hrEntities.SaveChanges();
            }
            btnCS_SearchTravel_Click(sender, e);
            GetComboboxValue();
        }

        private void btnCS_AllPassTravel_Click(object sender, EventArgs e) // 出差申請一鍵通過
        {
            var q = from t in hrEntities.Travel_Expense_Application
                    where t.Department == userInfo.Dept && t.CheckStatus == 1
                    select t;
            foreach (var item in q)
            {
                item.CheckStatus = 2;
            }
            hrEntities.SaveChanges();

            btnCS_SearchTravel_Click(sender, e);
            GetComboboxValue();
        }

        #region datagridview 顏色
        private void dgvCS_Travel_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            for (int i = 0; i < dgvCS_Travel.Rows.Count; i++)
            {
                DataGridViewRow dgvr1 = dgvCS_Travel.Rows[i];
                if (i % 2 == 0)
                {
                    dgvCS_Travel.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(255, 196, 120);
                }
                else
                {
                    dgvCS_Travel.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(240, 229, 216);
                }
            }

            //for (int i = 0; i < dgvCS_Travel.Rows.Count; i++)
            //{
            //    DataGridViewRow dgvr = dgvCS_Travel.Rows[i];
            //    if (dgvr.Cells[7].Value.ToString() == "維修中")
            //    {
            //        dgvr.Cells[7].Style.BackColor = Color.FromArgb(170, 58, 58);
            //        dgvr.Cells[7].Style.ForeColor = Color.White;
            //        //dgvr.Cells[7].Style.Font= new Font(dgv_RepairSup.Font, FontStyle.Bold);
            //    }
            //    else if (dgvr.Cells[7].Value.ToString() == "完成")
            //    {
            //        dgvr.Cells[7].Style.BackColor = Color.FromArgb(94, 168, 135);
            //        dgvr.Cells[7].Style.ForeColor = Color.White;
            //        //dgvr.Cells[7].Style.Font = new Font(dgv_RepairSup.Font, FontStyle.Bold);
            //    }
            //}

            //標題和內容文字置中
            dgvCS_Travel.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCS_Travel.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;


        }

        private void dgvCS_Leave_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            for (int i = 0; i < dgvCS_Leave.Rows.Count; i++)
            {
                DataGridViewRow dgvr1 = dgvCS_Leave.Rows[i];
                if (i % 2 == 0)
                {
                    dgvCS_Leave.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(255, 196, 120);
                }
                else
                {
                    dgvCS_Leave.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(240, 229, 216);
                }
            }
            //for (int i = 0; i < dgvCS_Leave.Rows.Count; i++)
            //{
            //    DataGridViewRow dgvr = dgvCS_Leave.Rows[i];
            //    if (dgvr.Cells[7].Value.ToString() == "維修中")
            //    {
            //        dgvr.Cells[7].Style.BackColor = Color.FromArgb(170, 58, 58);
            //        dgvr.Cells[7].Style.ForeColor = Color.White;
            //        //dgvr.Cells[7].Style.Font= new Font(dgv_RepairSup.Font, FontStyle.Bold);
            //    }
            //    else if (dgvr.Cells[7].Value.ToString() == "完成")
            //    {
            //        dgvr.Cells[7].Style.BackColor = Color.FromArgb(94, 168, 135);
            //        dgvr.Cells[7].Style.ForeColor = Color.White;
            //        //dgvr.Cells[7].Style.Font = new Font(dgv_RepairSup.Font, FontStyle.Bold);
            //    }
            //}


            dgvCS_Leave.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCS_Leave.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;


        }

        private void dgvCO_Search_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            for (int i = 0; i < dgvCO_Search.Rows.Count; i++)
            {
                DataGridViewRow dgvr1 = dgvCO_Search.Rows[i];
                if (i % 2 == 0)
                {
                    dgvCO_Search.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(255, 196, 120);
                }
                else
                {
                    dgvCO_Search.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(240, 229, 216);
                }
            }

            //for (int i = 0; i < dgvCO_Search.Rows.Count; i++)
            //{
            //    DataGridViewRow dgvr = dgvCO_Search.Rows[i];
            //    if (dgvr.Cells[7].Value.ToString() == "維修中")
            //    {
            //        dgvr.Cells[7].Style.BackColor = Color.FromArgb(170, 58, 58);
            //        dgvr.Cells[7].Style.ForeColor = Color.White;
            //        //dgvr.Cells[7].Style.Font= new Font(dgv_RepairSup.Font, FontStyle.Bold);
            //    }
            //    else if (dgvr.Cells[7].Value.ToString() == "完成")
            //    {
            //        dgvr.Cells[7].Style.BackColor = Color.FromArgb(94, 168, 135);
            //        dgvr.Cells[7].Style.ForeColor = Color.White;
            //        //dgvr.Cells[7].Style.Font = new Font(dgv_RepairSup.Font, FontStyle.Bold);
            //    }
            //}

            //標題和內容文字置中
            dgvCO_Search.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCO_Search.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

        }

        #endregion

        

        private void dgv_Repair_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            dgv_Repair.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            for (int i = 0; i < dgv_Repair.Rows.Count; i++)
            {
                DataGridViewRow dgvr1 = dgv_Repair.Rows[i];
                if (dgvr1.Cells[3].Value.ToString() == "總務修繕")
                {
                    dgv_Repair.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(255, 196, 120);
                }
                else if (dgvr1.Cells[3].Value.ToString() == "資訊修繕")
                {
                    dgv_Repair.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(240, 229, 216);
                }
            }

            for (int i = 0; i < dgv_Repair.Rows.Count; i++)
            {
                DataGridViewRow dgvr = dgv_Repair.Rows[i];
                if (dgvr.Cells[6].Value.ToString() == "維修中")
                {
                    dgvr.Cells[6].Style.BackColor = Color.FromArgb(170, 58, 58);
                    dgvr.Cells[6].Style.ForeColor = Color.White;
                    //dgvr.Cells[6].Style.Font= new Font(dgv_RepairSup.Font, FontStyle.Bold);
                }
                else if (dgvr.Cells[6].Value.ToString() == "完成")
                {
                    dgvr.Cells[6].Style.BackColor = Color.FromArgb(94, 168, 135);
                    dgvr.Cells[6].Style.ForeColor = Color.White;
                    //dgvr.Cells[6].Style.Font = new Font(dgv_RepairSup.Font, FontStyle.Bold);
                }
            }
            //標題和內容文字置中
            dgv_Repair.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv_Repair.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }
        #endregion

        #region  Tina 匯入員工資料
        private void Information(int UserID)
        {
            try
            {
                string strr = Settings.Default.MyHR;

                using (SqlConnection conn = new SqlConnection(strr))
                {

                    SqlCommand command = new SqlCommand();

                    command.Connection = conn;
                    command.CommandText = $"Select * from [User] where EmployeeID ={UserID}";
                    conn.Open();


                    SqlDataReader dataReader = command.ExecuteReader();
                    dataReader.Read();

                    this.labTra_EmployeeName.Text = dataReader["EmployeeName"].ToString();
                    this.labLea_EmployeeName.Text = dataReader["EmployeeName"].ToString();
                    this.labWucha_EmpName.Text = dataReader["EmployeeName"].ToString();




                    this.labLea_EmployeeID.Text = dataReader["EmployeeID"].ToString();
                    this.labTra_EmployeeID.Text = dataReader["EmployeeID"].ToString();
                    this.labWucha_EmpID.Text = dataReader["EmployeeID"].ToString();




                    //byte[] bytes = (byte[])dataReader["Photo"];
                    //System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes);
                    ShowImage(userInfo.ID);

                    var q = from i in this.hrEntities.Users
                            where i.EmployeeID == UserID
                            select i.Department1.DepartmentName;

                    this.labTra_EmpDep.Text = q.First();
                    this.labLea_EmpDep.Text = q.First();
                    this.labWucha_EmpDep.Text = q.First();



                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region 請假申請


        private void LoadDays() //請假天數
        {
            this.comLea_Days.Items.Add("半天");
            for (int i = 1; i <= 30; i++)
            {
                this.comLea_Days.Items.Add($"{i}天");
            }
        }

        private void LoadLeaveCategory()  //匯入假別類別
        {
            string strr = Settings.Default.MyHR;
            try
            {
                using (SqlConnection conn = new SqlConnection(strr))
                {
                    SqlCommand comm = new SqlCommand();
                    comm.CommandText = "select * from Leave";
                    comm.Connection = conn;

                    conn.Open();
                    SqlDataReader reader = comm.ExecuteReader();
                    while (reader.Read())
                    {
                        this.comLea_Cate.Items.Add(reader["LeaveCategory"]);
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnLeave_Submit_Click(object sender, EventArgs e)
        {
            try
            {
                string strr = Settings.Default.MyHR;

                using (SqlConnection conn = new SqlConnection(strr))
                {

                    int EmployeeID = 2;


                    string startDay = dtpLea_Start.Value.ToString("yyyy/MM/dd"); //將時間轉換成字串，SQL讀懂
                    string endDay = dtp_Lea_End.Value.ToString("yyyy/MM/dd");
                    string reason = txtLea_Reason.Text;

                    //確認視窗設定
                    string message = $" 姓名:{this.labLea_EmployeeName.Text}{Environment.NewLine} 員工編號:{this.labLea_EmployeeID.Text}{Environment.NewLine} 所屬部門:{this.labLea_EmpDep.Text}{Environment.NewLine}請假類別:{this.comLea_Cate.Text}{Environment.NewLine}請假天數:{this.comLea_Days.Text}{Environment.NewLine}請假起始日:{this.dtpLea_Start.Text}{Environment.NewLine}請假結束日:{this.dtp_Lea_End.Text}{Environment.NewLine}請假原因:{this.txtLea_Reason.Text}{Environment.NewLine}";
                    string caption = "Confirm";
                    var result = MessageBox.Show(message, caption, MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

                    if (result == DialogResult.OK)
                    {

                        int FinallyLeaveResult = ChangeToLeaveID(comLea_Cate.Text);
                        SqlCommand command = new SqlCommand($"Insert into LeaveApplication (EmployeeID,ApplyDate,LeaveCategory,LeaveStartTime,LeaveEndTime,Reason) values('{EmployeeID}','{DateTime.Now.ToShortDateString()}','{FinallyLeaveResult}','{startDay}','{endDay}','{reason}')", conn);
                        conn.Open();
                        command.ExecuteNonQuery();
                        MessageBox.Show("請假申請成功");

                    }
                    else
                    {
                        //能幹嘛我也不知道
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }//申請假別

        public int ChangeToLeaveID(string s)  //對應假別名稱為數值回資料庫~~~ (病假=>1 事假=>2 生理假=>3 喪假=>4 特休=>5)
        {
            var q = from L in this.hrEntities.Leaves
                    where L.LeaveCategory == s
                    select L.LeaveID;

            return q.First();

        }



        #endregion

        #region 差旅費申請

        private void LoadCostCaTEGORY()
        {
            var q = from i in this.hrEntities.CostCategories
                    select i.CostCategory1;
            foreach (var a in q)
            {
                this.comTra_Catogory1.Items.Add(a);
                this.comTra_Catogory2.Items.Add(a);
                this.comTra_Catogory3.Items.Add(a);
            }
        }

        private void LoadIntOnly() //指向方法，讓欄位只能輸入int 和 差旅費計算總金額
        {
            this.txtTra_Cost2.KeyPress += txtTra_Cost1_KeyPress;
            this.txtTra_Cost3.KeyPress += txtTra_Cost1_KeyPress;

            this.txtTra_Cost1.TextChanged += SumCost;
            this.txtTra_Cost2.TextChanged += SumCost;
            this.txtTra_Cost3.TextChanged += SumCost;

        }

        private void txtTra_Cost1_KeyPress(object sender, KeyPressEventArgs e) //差旅費3個金額欄只能輸入int
        {
            if (((int)e.KeyChar < 48 | (int)e.KeyChar > 57) & (int)e.KeyChar != 8)
            {
                e.Handled = true;
            }
        }

        int COSTSUM;

        private void btnTra_Submit_Click(object sender, EventArgs e)//申請差旅費
        {
            try
            {
                string strr = Settings.Default.MyHR;

                using (SqlConnection conn = new SqlConnection(strr))
                {

                    //宣告一些有的沒的
                    int EmployeeID = 2;


                    int Amont1, Amont2, Amont3;
                    string TravelstartDay = dtpTra_Start.Value.ToString("yyyy/MM/dd");
                    string TravelendDay = dtpTra_End.Value.ToString("yyyy/MM/dd");
                    string Travelreason = txtTra_Reason.Text;

                    //確認資料視窗設定
                    string message = $"姓名:{this.labTra_EmployeeName.Text}{Environment.NewLine} 員工編號:{this.labTra_EmployeeID.Text}{Environment.NewLine} 所屬部門:{this.labTra_EmpDep.Text}{Environment.NewLine}{Environment.NewLine}申請出差總金額為:{this.label26.Text}{Environment.NewLine}";
                    string caption = "Confirm";
                    var result = MessageBox.Show(message, caption, MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

                    if (result == DialogResult.OK) // 如果按了確認執行的事情
                    {
                        SqlCommand command2 = new SqlCommand();
                        conn.Open();
                        command2.Connection = conn;


                        command2.CommandText = $"Insert into Travel_Expense_Application(EmployeeID,Reason,ApplyDate,TravelStartTime,TravelEndTime,Amont) values('{EmployeeID}','{Travelreason}','{DateTime.Now.ToShortDateString()}','{TravelstartDay}','{TravelendDay}',{COSTSUM})";
                        command2.ExecuteNonQuery();

                        //插入申請差旅費主表
                        command2.CommandText = "SELECT top 1 [ApplyNumber]  FROM [dbo].[Travel_Expense_Application] ORDER BY [ApplyNumber] DESC";
                        int i = (int)command2.ExecuteScalar();


                        if (txtTra_Cost1.Text != "") //如果類別欄位不是空字串 1.轉型 2.插入差旅費詳細資料
                        {

                            Amont1 = int.Parse(txtTra_Cost1.Text);
                            int Finallyresult = ChangToCostCategoryID(comTra_Catogory1.Text);
                            command2.CommandText = $"Insert into ApplyDetail(ApplyNumber,CostID,Amont) values({i},{Finallyresult},{Amont1})";
                            command2.ExecuteNonQuery();
                        }
                        if (txtTra_Cost2.Text != "")
                        {

                            Amont2 = int.Parse(txtTra_Cost2.Text);
                            int Finallyresult = ChangToCostCategoryID(comTra_Catogory2.Text);
                            command2.CommandText = $"Insert into ApplyDetail(ApplyNumber,CostID,Amont) values({i},{Finallyresult},{Amont2})";
                            command2.ExecuteNonQuery();
                        }

                        if (txtTra_Cost3.Text != "")
                        {

                            Amont3 = int.Parse(txtTra_Cost3.Text);
                            int Finallyresult = ChangToCostCategoryID(comTra_Catogory3.Text);
                            command2.CommandText = $"Insert into ApplyDetail(ApplyNumber,CostID,Amont) values({i},{Finallyresult},{Amont3})";
                            command2.ExecuteNonQuery();
                        }
                        MessageBox.Show("差旅費用申請成功");
                     
                    }
                    else
                    {
                        //能幹嘛我也不知道
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SumCost(object sender, EventArgs e)  //申請差旅費呈現總金額
        {
            try
            {
                int cost1, cost2, cost3;
                if (Int32.TryParse(txtTra_Cost1.Text, out int number1))
                {
                    cost1 = number1;
                }
                else
                {
                    cost1 = 0;
                }

                if (Int32.TryParse(txtTra_Cost2.Text, out int number2))
                {
                    cost2 = number2;
                }
                else
                {
                    cost2 = 0;
                }

                if (Int32.TryParse(txtTra_Cost3.Text, out int number3))
                {
                    cost3 = number3;
                }
                else
                {
                    cost3 = 0;
                }

                label26.Text = (($"{cost1 + cost2 + cost3:c}").ToString());
                COSTSUM = cost1 + cost2 + cost3;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }



        }

        public int ChangToCostCategoryID(string s) //對應差旅費名稱為數值回資料庫~~~ (交通 =>1 食宿 =>2 其他 =>3)
        {
            var q = from C in this.hrEntities.CostCategories
                    where C.CostCategory1 == s
                    select C.CostID;

            return q.First();

        }



        #endregion

        #region WUCHAAAA


        private void LoadWuChaStore() //匯入店家名稱
        {
            var q = from i in this.hrEntities.Stores
                    select i.StoreName;

            foreach (var Store in q)
            {
                this.comWuCha_Store.Items.Add(Store);
            }

        }
        private void comWuCha_Store_SelectedIndexChanged(object sender, EventArgs e) //增加店家的品項
        {
            btnWuCha_Submit.Enabled = true;
            this.WuCha_dataGridView.Columns.Clear();
            this.WuCha_dataGridView.DataSource = null;

            this.WuCha_dataGridView.Columns.Add("店家", "店家");
            this.WuCha_dataGridView.Columns.Add("品項", "品項");
            this.WuCha_dataGridView.Columns.Add("單價", "單價");
            this.WuCha_dataGridView.Columns.Add("數量", "數量");
            this.WuCha_dataGridView.Columns.Add("小計", "小計");

            this.labWuCha_Text.Text = "本筆訂單金額:";

            //this.WuCha_dataGridView.Columns[0].HeaderCell.Value = "店家";
            //this.WuCha_dataGridView.Columns[1].HeaderCell.Value = "品項";
            //this.WuCha_dataGridView.Columns[2].HeaderCell.Value = "單價";
            //this.WuCha_dataGridView.Columns[3].HeaderCell.Value = "數量";
            //this.WuCha_dataGridView.Columns[4].HeaderCell.Value = "小計";

            this.WuCha_dataGridView.Columns[0].ReadOnly = true;
            this.WuCha_dataGridView.Columns[1].ReadOnly = true;
            this.WuCha_dataGridView.Columns[2].ReadOnly = true;
            this.WuCha_dataGridView.Columns[3].ReadOnly = true;
            this.WuCha_dataGridView.Columns[4].ReadOnly = true;


            var q = from item in this.hrEntities.Items
                    join store in this.hrEntities.Stores
                    on item.StoreID equals store.StoreID
                    where this.comWuCha_Store.Text == store.StoreName
                    select item.ItemName;


            this.comWuCha_TempItem.Items.Clear();
            foreach (var i in q)
            {
                this.comWuCha_TempItem.Items.Add(i);
            }

            var q1 = from i in this.hrEntities.Items
                     join s in this.hrEntities.Stores
                     on i.StoreID equals s.StoreID
                     where this.comWuCha_Store.Text == s.StoreName
                     select (int)i.ItemPrice;

            this.comWuCha_TempPrice.Items.Clear();
            foreach (var price in q1)
            {
                this.comWuCha_TempPrice.Items.Add(price);
            }



            this.panel1.Controls.Clear();   //建立WuCha品項按鈕
            Button[] buttons = new Button[this.comWuCha_TempItem.Items.Count];

            for (int i = 0; i <= this.comWuCha_TempItem.Items.Count - 1; i++)
            {
                buttons[i] = new Button();
                buttons[i].Size = new Size(120, 120);
                buttons[i].Location = new Point(50 + 130 * i, 10);
                buttons[i].BackColor = Color.Olive;
                buttons[i].Font = new Font("微軟正黑體", 14, FontStyle.Bold);
                panel1.Controls.Add(buttons[i]);
                buttons[i].Name = this.comWuCha_TempItem.Items[i].ToString();  //放品項
                buttons[i].Tag = this.comWuCha_TempPrice.Items[i];  //放價錢
                buttons[i].Text = $"{this.comWuCha_TempItem.Items[i]}{Environment.NewLine}" + $"{this.comWuCha_TempPrice.Items[i]:c2}";
                buttons[i].Click += AddWuChaItem;

            }

            this.WuCha_dataGridView.DataSource = null;
            Total = 0;
            labWuCha_money.Text = Total.ToString();

        }
        int Total;
        private void AddWuChaItem(object sender, EventArgs e) // 新增所選品項欄位
        {
            this.WuCha_dataGridView.DataSource = null;

            this.WuCha_dataGridView.Rows.Add(this.comWuCha_Store.SelectedItem, ((Button)sender).Name, $"{((Button)sender).Tag/*:c2*/}", 1, $"{((Button)sender).Tag.ToString().Replace(".0000", "")}");
            CountWuChaTotal();
            ((Button)sender).Click -= AddWuChaItem;
            ((Button)sender).Click += AddWuChaItemQuantity;
        }
        private void AddWuChaItemQuantity(object sender, EventArgs e) //計算數量 和 小計
        {
            if (this.WuCha_dataGridView.Rows.Count > 0)
            {
                for (int i = 0; i < this.WuCha_dataGridView.Rows.Count; i++)
                {

                    if (((Button)sender).Name == this.WuCha_dataGridView.Rows[i].Cells[1].Value.ToString())
                    {
                        this.WuCha_dataGridView.Rows[i].Cells[3].Value = (int)this.WuCha_dataGridView.Rows[i].Cells[3].Value + 1;
                        int price = int.Parse(this.WuCha_dataGridView.Rows[i].Cells[2].Value.ToString().Replace(".0000", ""));
                        int Quantity = int.Parse(this.WuCha_dataGridView.Rows[i].Cells[3].Value.ToString());
                        this.WuCha_dataGridView.Rows[i].Cells[4].Value = $"{price * Quantity/*:c2*/}";    //一定會要改
                        CountWuChaTotal();
                    }
                }
            }

        }
        private void CountWuChaTotal() //計算WuCha合計值
        {
            Total = 0;
            labWuCha_money.Text = "";
            for (int i = 0; i < this.WuCha_dataGridView.Rows.Count; i++)
            {
                Total += int.Parse(this.WuCha_dataGridView.Rows[i].Cells["小計"].Value.ToString());
                labWuCha_money.Text = Total.ToString();
            }
        }
        private void btnWuCha_Submit_Click(object sender, EventArgs e) //提交WuCha訂單
        {

            string strr = Settings.Default.MyHR;
            string WuchaGroupday = DateTime.Now.ToString("MMdd");
            string Wuchaday = DateTime.Now.ToString("yyyy/MM/dd");
            //int ItemID;

            try
            {
                using (SqlConnection conn = new SqlConnection(strr))
                {

                    SqlCommand command = new SqlCommand();


                    if (this.WuCha_dataGridView.Rows.Count == 0)
                    {
                        MessageBox.Show("白癡嗎? 並未下訂任何WuCha品項");
                    }
                    else
                    {
                        int storeId = ChangToWuChaStoreID(this.WuCha_dataGridView.Rows[0].Cells[0].Value.ToString());
                        int WuChaTotal = int.Parse(this.labWuCha_money.Text);

                        command.Connection = conn;
                        conn.Open();
                        command.CommandText = $"Insert into WuchaOrder(GroupID,StoreID,EmployeeID,Date,TotalPirce) values('{WuchaGroupday}-{storeId.ToString()}',{storeId},{userInfo.ID},'{Wuchaday}',{WuChaTotal})";
                        command.ExecuteNonQuery();//插入WuCha主表

                        command.CommandText = "select top 1 WuChaOrderNumber from WuChaOrder order by WuChaOrderNumber desc";
                        int lastWuChaOrderNum = (int)command.ExecuteScalar(); //撈出最後一筆訂單編號

                        //command.Parameters.Add("ItemID", SqlDbType.Int, 16).Value = ItemID;

                        for (int i = 0; i < this.WuCha_dataGridView.Rows.Count; i++)
                        {

                            int ItemID = ChangToWuChaItemID(this.WuCha_dataGridView.Rows[i].Cells[1].Value.ToString());
                            int ItemQuantity = (int)this.WuCha_dataGridView.Rows[i].Cells[3].Value;
                            command.Parameters.Clear();
                            command.CommandText = $"Insert into OrderStoreDetail(WuChaOrderNumber,StoreID,ItemID,ItemQuantity) values({lastWuChaOrderNum},{storeId},{@ItemID},{@ItemQuantity})";
                            command.ExecuteNonQuery();//訂單插入Order Detail


                        }

                        MessageBox.Show("WuCha待會就來");


                        this.WuCha_dataGridView.Columns.Clear();   //清空資料再加上欄位
                        this.WuCha_dataGridView.DataSource = null;
                        this.WuCha_dataGridView.Columns.Add("店家", "店家");
                        this.WuCha_dataGridView.Columns.Add("品項", "品項");
                        this.WuCha_dataGridView.Columns.Add("單價", "單價");
                        this.WuCha_dataGridView.Columns.Add("數量", "數量");
                        this.WuCha_dataGridView.Columns.Add("小計", "小計");

                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnWuCha_Search_Click(object sender, EventArgs e) //查詢當月總金額
        {
            btnWuCha_Submit.Enabled = false;
            this.panel1.Controls.Clear();
            //this.comWuCha_Store.Items.Clear();
            this.WuCha_dataGridView.Columns.Clear();
            this.WuCha_dataGridView.DataSource = null;



            var q = from x in hrEntities.WuChaOrders
                    where x.EmployeeID == userInfo.ID && x.Date.Month == DateTime.Now.Month
                    select new
                    {
                        日期 = x.Date,
                        店家 = x.Store.StoreName,
                        小計 = (int)x.TotalPirce
                    };
            this.WuCha_dataGridView.DataSource = q.ToList();
            this.labWuCha_Text.Text = $"{DateTime.Now.Month}月總訂購金額:";
            CountWuChaTotal();


        }

        private void btnWuCha_SearchRange_Click(object sender, EventArgs e) //查詢區間總金額
        {

            btnWuCha_Submit.Enabled = false;
            string WuChaStart = this.dtpWuCha_Start.Value.ToString("yyyy/MM/dd");
            string WuChaEnd = this.dtpWuCha_End.Value.ToString("yyyy/MM/dd");
            this.panel1.Controls.Clear();
            //this.comWuCha_Store.Items.Clear();
            this.WuCha_dataGridView.Columns.Clear();
            this.WuCha_dataGridView.DataSource = null;


            var q = from i in this.hrEntities.WuChaOrders.AsEnumerable()
                    where i.EmployeeID == userInfo.ID && DateTime.Parse(WuChaStart) <= i.Date && i.Date <= DateTime.Parse(WuChaEnd)
                    orderby i.Date ascending
                    select new
                    {
                        日期 = i.Date,
                        店家 = i.Store.StoreName,
                        小計 = (int)i.TotalPirce
                    };

            this.WuCha_dataGridView.DataSource = q.ToList();
            this.labWuCha_Text.Text = $"查詢總金額:";
            CountWuChaTotal();

        }

        private void btnWuCha_BossSearch_Click(object sender, EventArgs e) //檢視"當日"訂單總額
        {
            this.btnWuCha_Submit.Enabled = false;

            this.WuCha_dataGridView.Columns.Clear();
            this.WuCha_dataGridView.DataSource = null;
            this.panel1.Controls.Clear();

            string Wuchaday = DateTime.Now.ToString("yyyy/MM/dd");

            var q = from i in this.hrEntities.WuChaOrders.AsEnumerable()
                    where i.Date == DateTime.Parse(Wuchaday) //當日判斷
                    orderby i.GroupID
                    group i by i.GroupID into g
                    select new
                    {
                        店家 = ChangeGroupIDtoStore(g.Key.Substring(5)), //擷取部分GroupID轉換成店家名稱
                        總訂單數量 = g.Count(),
                        小計 = g.Sum(x => x.TotalPirce),

                    };

            this.WuCha_dataGridView.DataSource = q.ToList();
            CountWuChaTotal();
            this.labWuCha_Text.Text = Wuchaday + "共:";


        }

        private void btnWuCha_BossSearchDetail_Click(object sender, EventArgs e) //限定職權查詢訂單Detail
        {
            this.btnWuCha_Submit.Enabled = false;
            this.WuCha_dataGridView.Columns.Clear();
            this.WuCha_dataGridView.DataSource = null;
            this.panel1.Controls.Clear();

            string Wuchaday = DateTime.Now.ToString("yyyy/MM/dd");

            var q = from i in this.hrEntities.WuChaOrders.AsEnumerable()
                    join y in this.hrEntities.OrderStoreDetails
                    on i.WuChaOrderNumber equals y.WuChaOrderNumber
                    where i.Date == DateTime.Parse(Wuchaday) //當天訂單
                    group new { i, y } by new { i.StoreID, i.EmployeeID, y.ItemID } into g
                    orderby g.Key.EmployeeID
                    select new
                    {
                        員工 = ChangeToEmployeeName(g.Key.EmployeeID),
                        店家 = ChangeGroupIDtoStore(g.Key.StoreID.ToString()),
                        項目 = ChangeToWuChaItemName(g.Key.ItemID),
                        數量 = g.Sum(c => c.y.ItemQuantity)
                    };

            this.WuCha_dataGridView.DataSource = q.ToList();
            //CountWuChaTotal();
            this.labWuCha_Text.Text = "-----------";
            this.labWuCha_money.Text = "-----";

        }

        public int ChangToWuChaStoreID(string s) //午茶店家轉換
        {
            var q = from Store in this.hrEntities.Stores
                    where Store.StoreName == s
                    select Store.StoreID;

            return q.First();

        }

        public int ChangToWuChaItemID(string s)//品項Name => 品項ID
        {
            var q = from item in this.hrEntities.Items
                    where item.ItemName == s
                    select item.ItemID;

            return q.First();

        }

        public string ChangeToWuChaItemName(int i) //品項ID => 品項Name
        {
            var q = from a in this.hrEntities.Items
                    where a.ItemID == i
                    select a.ItemName;

            return q.First();
        }

        public string ChangeGroupIDtoStore(string s) //GroupID 轉換店家名稱
        {

            var q = from i in this.hrEntities.Stores
                    where i.StoreID.ToString() == s
                    select i.StoreName;

            return q.First();
        }

        public string ChangeToEmployeeName(int i) //EmployeeID => EmployeeName
        {
            var q = from n in this.hrEntities.Users
                    where n.EmployeeID == i
                    select n.EmployeeName;

            return q.First();

        }

        private void btnWuCha_TodayOrder_Click(object sender, EventArgs e) //查詢今日訂單 且可以修改
        {



            this.btnWuCha_Submit.Enabled = false;
            this.WuCha_dataGridView.Columns.Clear();
            this.WuCha_dataGridView.DataSource = null;
            this.panel1.Controls.Clear();


            string Wuchaday = DateTime.Now.ToString("yyyy/MM/dd");

            var q = from o in this.hrEntities.WuChaOrders.AsEnumerable()
                    join od in this.hrEntities.OrderStoreDetails
                    on o.WuChaOrderNumber equals od.WuChaOrderNumber
                    join it in this.hrEntities.Items
                    on od.ItemID equals it.ItemID
                    orderby od.ItemID
                    where o.EmployeeID == userInfo.ID && o.Date == DateTime.Parse(Wuchaday)
                    select new
                    {

                        //增加的button - index[0]
                        店家 = o.Store.StoreName,
                        品項 = ChangeToWuChaItemName(od.ItemID),
                        數量 = od.ItemQuantity,
                        小計 = od.ItemQuantity * it.ItemPrice,
                        單號 = o.WuChaOrderNumber

                    };

            AddBtnToReviseItem();

            this.WuCha_dataGridView.DataSource = q.ToList();
            this.WuCha_dataGridView.Columns[5].Visible = false;
            CountWuChaTotal();
            this.labWuCha_Text.Text = "今日訂單金額:";

            //var q = (this.hrEntities.OrderStoreDetails.AsEnumerable().Where(o => o.WuChaOrderNumber == int.Parse(WuCha_ReceiveID) && o.ItemID == ChangToWuChaItemID(WuCha_ReceiveItem))).FirstOrDefault();

            //if(q != null)
            //{
            //    q.ItemQuantity = 1;
            //    this.hrEntities.SaveChanges();
            //}

        }

        private void AddBtnToReviseItem() //增加修改訂單按鈕
        {
            DataGridViewButtonColumn column = new DataGridViewButtonColumn();
            column.Name = "修改";
            column.UseColumnTextForButtonValue = true;
            column.Text = "修改";
            this.WuCha_dataGridView.Columns.Add(column);

        }

        private void WuCha_dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e) //修改訂單
        {
            //MessageBox.Show(sender.ToString());

            string WuCha_ShouldReviseID, WuCha_ShouldReviseItem;
            if (e.ColumnIndex == 0)
            {
                //this.WuCha_dataGridView.Rows[e.RowIndex].Cells[3].ReadOnly = false;
                this.btnWuCha_Submit.Enabled = true;

                WuCha_ShouldReviseID = this.WuCha_dataGridView.Rows[e.RowIndex].Cells[5].Value.ToString();
                WuCha_ShouldReviseItem = this.WuCha_dataGridView.Rows[e.RowIndex].Cells[2].Value.ToString();
                WuChaRivse wcr = new WuChaRivse(WuCha_ShouldReviseID, WuCha_ShouldReviseItem);
                wcr.Owner = this;
                wcr.ShowDialog();
            }
            else
            {
                //沒事
            }

        }

        public void RefreshWuCha()
        {
            this.btnWuCha_TodayOrder.PerformClick();
        }

        #endregion

        #region 查詢功能
        private void tabControl2_SelectedIndexChanged(object sender, EventArgs e) //更新頁面
        {
            //todo update
            try
            {
                if (tabControl1.SelectedTab == 請假查詢 )
                {
                    var q = from l in this.hrEntities.Losts
                            where l.EmployeeID == userInfo.ID
                            select l.LostCategory;
                    if(q.ToList().Count() > 0)
                    {
                        TreeNode treeNode1 = this.losttreeView.Nodes.Add(q.ToList().First().ToString());
                    }
                    
                }
                var a = from l in hrEntities.Losts.AsEnumerable()
                        select new
                        {
                            LostID = l.LostID,
                            類別 = l.LostCategory,
                            遺失物品 = l.LostProperty,
                            遺失物照片 = l.LostPropertyPhoto,
                            遺失時間 = l.LostDate,
                            遺失地點 = l.LostSpace,
                            遺失物描述 = l.LostPropertyDescription,
                            狀態 = l.LostCheckStatus
                        };
                this.lostview.DataSource = a.ToList();

                var b = from f in hrEntities.Founds.AsEnumerable()
                        select new
                        {
                            FoundID = f.FoundID,
                            類別 = f.FoundSubject,
                            遺失物品 = f.FoundCategory,
                            遺失物照片 = f.FoundPropertyPhoto,
                            遺失時間 = f.FoundDate,
                            遺失地點 = f.FoundSpace,
                            遺失物描述 = f.FoundPropertyDescription,
                            狀態 = f.FoundCheckStatus
                        };
                this.foundview.DataSource = b.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }
        private void LoadToTreeView()
        {
            //todo lsottreeview
            try
            {
                var q = from tree in this.hrEntities.Losts
                        orderby tree.LostCategory descending
                        group tree by tree.LostCategory into g
                        select new
                        {
                            遺失類別 = g.Key,
                            MyCount = g.Count(),
                            MyGroup = g
                        };

                //treeview
                foreach (var group in q)
                {
                    string s = $"{group.遺失類別} ({group.MyCount})";
                    TreeNode x = this.losttreeView.Nodes.Add(s);

                    foreach (var item in group.MyGroup)
                    {
                        x.Nodes.Add($"{item.LostProperty}".ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void Loadcbbcombobx() //載入請假類別
        {
            try
            {
                var q = (from cbb in this.hrEntities.Leaves
                         select cbb.LeaveCategory).Distinct();

                foreach (var n in q)
                {
                    this.cbbleave.Items.Add(n);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        int g;
        private void btncheckleave_Click(object sender, EventArgs e) //查詢請假
        {
            this.dataGridView1.Columns.Clear();
            try
            {
                switch (this.cbbleave.SelectedItem.ToString())
                {
                    case "病假":
                        g = 1;
                        break;
                    case "事假":
                        g = 2;
                        break;
                    case "生理假":
                        g = 3;
                        break;
                    case "喪假":
                        g = 4;
                        break;
                    case "特休":
                        g = 5;
                        break;
                }

                var x = leavetimepicker.Value.ToString("yyyy/MM/dd");

                var q = from l in this.hrEntities.LeaveApplications.AsEnumerable()
                        where l.LeaveStartTime == DateTime.Parse(x) && g == l.LeaveCategory
                        select new
                        {
                            員工編號 = userInfo.ID,
                            員工姓名 = userInfo.Name,
                            請假類別 = l.LeaveCategory,
                            請假開始時間 = l.LeaveStartTime,
                            請假結束時間 = l.LeaveEndTime,
                            審核狀態 = l.CheckStatus
                        };
                this.dataGridView1.DataSource = q.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnleavclear_Click(object sender, EventArgs e)
        {
            this.dataGridView1.Columns.Clear();
            this.cbbleave.SelectedIndex = 1;

        }

        private void btncheckcost_Click(object sender, EventArgs e) //查詢差旅費
        {
            this.dataGridView2.Columns.Clear();

            try
            {
                var y = costtimepicker.Value.ToString("yyyy/MM/dd");

                var q2 = from t in hrEntities.Travel_Expense_Application.AsEnumerable()
                         where t.TravelStartTime == DateTime.Parse(y)
                         select new
                         {
                             員工編號 = userInfo.ID,
                             員工姓名 = userInfo.Name,
                             出差開始時間 = t.TravelStartTime,
                             出差結束時間 = t.TravelEndTime,
                             差旅費 = t.Amont,
                             審核狀態 = t.CheckStatus
                         };

                this.dataGridView2.DataSource = q2.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btncostclear_Click(object sender, EventArgs e)
        {
            this.dataGridView2.Columns.Clear();
        }
        #endregion

        #region 失物招領
        private void Getview_SelectionChanged(object sender, EventArgs e)
        {
            int id = (int)this.getview.Rows[getview.CurrentRow.Index].Cells[0].Value;
            var g = (from n in this.hrEntities.Losts
                     where n.LostID == id
                     select n).FirstOrDefault();
            if (g == null)
            {
                MessageBox.Show("請重新查詢");
                return;
            }

            g.LostCheckStatus = 2;
            this.hrEntities.SaveChanges();
        }
        private void LoadTolost() //領取物權限
        {
            if (userInfo.Dept < 5)
            {
                tabControl1.TabPages.Remove(取回登記);
            }
        }

        private void btnfilechose_Click(object sender, EventArgs e) //選擇遺失物圖片檔案
        {
            if (this.openFilelost.ShowDialog() == DialogResult.OK)
            {
                this.lostpicture.Image = Image.FromFile(this.openFilelost.FileName);
            }
        }


        private void btnlostclear_Click(object sender, EventArgs e) //清除填寫資料
        {
            this.txtlostproperty.Text = null;
            this.txtdec1.Text = null;
            this.txtlostspace.Text = null;
            this.lostpicture.Image = null;
        }

        public byte[] ImageToByteArray(System.Drawing.Image image) //轉換圖片型別
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, image.RawFormat);
                return ms.ToArray();
            }
        }

        private void btnchecklost_Click(object sender, EventArgs e) //遺失物登記
        {
            try
            {
                string lostsubject = "遺失物";
                var photo = this.lostpicture.Image;
                byte[] imagebinary = ImageToByteArray(photo);
                string lostspace = this.txtlostspace.Text;
                var lostdate = lostTimePicker4.Value.ToString("yyyy/MM/dd");
                string des = this.txtdec1.Text;
                byte loststatus = 1;

                Lost lost = new Lost
                {
                    EmployeeID = int.Parse(this.lblUserID.Text),
                    Deparment = int.Parse(this.lblUserDept.Text),
                    LostSubject = lostsubject,
                    LostCategory = this.cbblost.Text,
                    LostPropertyPhoto = imagebinary,
                    LostProperty = this.txtlostproperty.Text,
                    LostDate = DateTime.Parse(lostdate),
                    LostSpace = lostspace,
                    LostPropertyDescription = des,
                    LostCheckStatus = loststatus
                };
                this.hrEntities.Losts.Add(lost);

                this.hrEntities.SaveChanges();

                MessageBox.Show("新增成功!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        void Read_RefreshDataGridView() //匯入遺失及拾獲資料
        {
            //todo lost and found datagridview
            try
            {
                var a = from l in hrEntities.Losts.AsEnumerable()
                        select new
                        {
                            LostID = l.LostID,
                            類別 = l.LostCategory,
                            遺失物品 = l.LostProperty,
                            遺失物照片 = l.LostPropertyPhoto,
                            遺失時間 = l.LostDate,
                            遺失地點 = l.LostSpace,
                            遺失物描述 = l.LostPropertyDescription,
                            狀態 = l.LostCheckStatus
                        };
                this.lostview.DataSource = a.ToList();

                var b = from f in hrEntities.Founds.AsEnumerable()
                        select new
                        {
                            FoundID = f.FoundID,
                            類別 = f.FoundSubject,
                            遺失物品 = f.FoundCategory,
                            遺失物照片 = f.FoundPropertyPhoto,
                            遺失時間 = f.FoundDate,
                            遺失地點 = f.FoundSpace,
                            遺失物描述 = f.FoundPropertyDescription,
                            狀態 = f.FoundCheckStatus
                        };
                this.foundview.DataSource = b.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnfilefound_Click_1(object sender, EventArgs e) //加入拾獲照片
        {
            if (this.openFilefound.ShowDialog() == DialogResult.OK)
            {
                this.foundpicture.Image = Image.FromFile(this.openFilefound.FileName);
            }
        }

        private void btnfoundclear_Click(object sender, EventArgs e) //清除
        {
            this.cbbfound.SelectedIndex = 1;
            this.txtfounddec.Text = null;
            this.txtfoundp.Text = null;
            this.txtfoundsp.Text = null;
            this.foundpicture.Image = null;
        }

        private void btnfoundcheck_Click(object sender, EventArgs e) //加入拾獲物
        {
            try
            {
                string foundsubject = "拾獲物";
                var photo = this.foundpicture.Image;
                byte[] imagebinary2 = ImageToByteArray(photo);
                string foundspace = this.txtfoundsp.Text;
                var founddate = foundTimePicker5.Value.ToString("yyyy/MM/dd");
                string founddes = this.txtfounddec.Text;
                byte foundstatus = 1;

                Found found = new Found
                {
                    EmployeeID = int.Parse(this.lblUserID.Text),
                    Deparment = int.Parse(this.lblUserDept.Text),

                    FoundSubject = foundsubject,
                    FoundCategory = this.cbbfound.Text,
                    FoundPropertyPhoto = imagebinary2,
                    FoundProperty = this.txtfoundp.Text,
                    FoundDate = DateTime.Parse(founddate),
                    FoundSpace = foundspace,
                    FoundPropertyDescription = founddes,
                    FoundCheckStatus = foundstatus
                };
                this.hrEntities.Founds.Add(found);

                this.hrEntities.SaveChanges();

                MessageBox.Show("新增成功!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btngetclear_Click(object sender, EventArgs e) //清除
        {
            this.txtgetp.Text = null;
            this.txtgetsp.Text = null;
            this.getview.Columns.Clear();
            this.cbbget.SelectedIndex = 1;

        }

        private void btngetcheck_Click(object sender, EventArgs e) //查詢遺失物
        {
            this.getview.Columns.Clear();
            try
            {
                var q3 = from l in hrEntities.Losts.AsEnumerable()
                         where l.LostCategory == this.cbbget.Text && l.LostProperty == this.txtgetp.Text
                         select new
                         {
                             LostID = l.LostID,
                             類別 = l.LostCategory,
                             遺失物品 = l.LostProperty,
                             遺失物照片 = l.LostPropertyPhoto,
                             遺失時間 = l.LostDate,
                             遺失地點 = l.LostSpace,
                             遺失物描述 = l.LostPropertyDescription,
                             狀態 = l.LostCheckStatus
                         };
                this.getview.DataSource = q3.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


        private void btngetchose_Click(object sender, EventArgs e) //領取遺失物
        {
            this.getview.Columns.Clear();

            try
            {
                var q5 = from o in hrEntities.Losts.AsEnumerable()
                         where o.LostCategory == this.cbbget.Text && o.LostProperty == this.txtgetp.Text
                         select new
                         {
                             LostID = o.LostID,
                             類別 = o.LostCategory,
                             遺失物品 = o.LostProperty,
                             遺失物照片 = o.LostPropertyPhoto,
                             遺失時間 = o.LostDate,
                             遺失地點 = o.LostSpace,
                             遺失物描述 = o.LostPropertyDescription,
                             狀態 = o.LostCheckStatus
                         };

                this.hrEntities.SaveChanges();
                MessageBox.Show("變更成功");
                this.getview.DataSource = q5.ToList();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        #endregion

        private void lsbBulletin_SelectedIndexChanged(object sender, EventArgs e)
        {
            string str = lsbBulletin.SelectedItem.ToString();
            int ID = int.Parse(str.Replace(str.Substring(str.IndexOf('[')),""));
            var q = (from n in hrEntities.Bulletins
                             where n.Number == ID
                             select new
                             {                                 
                                 內容 = n.ContentofBulletin,
                             }).FirstOrDefault();
            MessageBox.Show(q.內容);
        }
    }
}
