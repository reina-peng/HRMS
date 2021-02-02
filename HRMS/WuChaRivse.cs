using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using HRMS.Properties;

namespace HRMS
{
    public partial class WuChaRivse : Form
    {
        string WuCha_ReceiveID;
        string WuCha_ReceiveItem;
       
        public WuChaRivse(string a, string b)
        {
            InitializeComponent();
            WuCha_ReceiveID = a;
            WuCha_ReceiveItem = b;
           
            LoadReviseItemInfo();


        }
        MyHREntities hrEntities = new MyHREntities();

        private void LoadReviseItemInfo() //顯示想要修改WuCha品項的資訊
        {
            this.comWuCha_To10.Items.Clear();
            for (int i = 0; i <= 9; i++) 
            {
                this.comWuCha_To10.Items.Add(i);
            }


            string strr = Settings.Default.MyHR;
            try
            {
                using (SqlConnection conn = new SqlConnection(strr))
                {
                    SqlCommand command = new SqlCommand();
                    command.CommandText = $"select o.[WuChaOrderNumber],o.StoreID,st.StoreName,[EmployeeID],od.ItemID,ItemName,od.ItemQuantity,o.Date from [dbo].[WuChaOrder] o join [dbo].[OrderStoreDetail] od on o.WuChaOrderNumber = od.WuChaOrderNumber join [dbo].[Item] it on it.ItemID = od.ItemID join [dbo].[Store] st on st.StoreID = o.StoreID where it.ItemName ='{WuCha_ReceiveItem}' and od.WuChaOrderNumber ='{WuCha_ReceiveID}' and o.Date = CONVERT(VARCHAR,GETDATE(),111)----YYYY/MM/DD order by od.ItemID";
                    command.Connection = conn;
                    conn.Open();
                    SqlDataReader reader = command.ExecuteReader();


                    while (reader.Read())
                    {
                        this.labWuCha_ReviseStore.Text = reader["StoreName"].ToString();
                        this.labWuCha_ReviseItem.Text = reader["ItemName"].ToString();
                        this.labWuCha_ReviseItemQuantity.Text = reader["ItemQuantity"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnWuChaRevise_Submit_Click(object sender, EventArgs e)
        {
            int ReviseNumber = int.Parse(this.comWuCha_To10.Text);

            try
            {
                string strr = Settings.Default.MyHR;
                using (SqlConnection conn = new SqlConnection(strr))
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = conn;
                    conn.Open();
                    if (ReviseNumber == 0)
                    {
                        command.CommandText = $"Update [dbo].[OrderStoreDetail] set [ItemQuantity] ={ReviseNumber} where[WuChaOrderNumber] = {WuCha_ReceiveID} and[ItemID] = {ChangToWuChaItemID(WuCha_ReceiveItem)}";
                        command.ExecuteNonQuery();

                        command.CommandText = $"Delete from [dbo].[OrderStoreDetail] where[ItemQuantity] = {ReviseNumber}";
                        command.ExecuteNonQuery();
                    }
                    else
                    {
                        command.CommandText = $"Update [dbo].[OrderStoreDetail] set [ItemQuantity] ={ReviseNumber} where[WuChaOrderNumber] = {WuCha_ReceiveID} and[ItemID] = {ChangToWuChaItemID(WuCha_ReceiveItem)}";
                        command.ExecuteNonQuery();
                    }
                   
                   
                    MessageBox.Show("訂單已修改");

                   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            Frm_HomePage ower = (Frm_HomePage)this.Owner;
            ower.RefreshWuCha();

            this.Close();

            //var q = (this.hrEntities.OrderStoreDetails.AsEnumerable().Where(o => o.WuChaOrderNumber == int.Parse(WuCha_ReceiveID) && o.ItemID == ChangToWuChaItemID(WuCha_ReceiveItem))).FirstOrDefault();

            //if(q != null)
            //{
            //    q.ItemQuantity = 1;
            //    this.hrEntities.SaveChanges();
            //}
           

        }

        public int ChangToWuChaItemID(string s)//品項Name => 品項ID
        {
            var q = from item in this.hrEntities.Items
                    where item.ItemName == s
                    select item.ItemID;

            return q.First();

        }

        private void btnWuChaRevise_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
    }


