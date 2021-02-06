using HRMS.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace HRMS
{
    public class MyUserInfo
    {        
        public int ID { get; }//員工ID
        public byte[] Photo { get; }//員工圖片
        public string Name { get; }//員工姓名
        public string EnglishName { get; }//員工英文名
        public int Dept { get; }//員工部門代號
        public string DeptName { get; }//員工部門名稱
        public int JobTitle { get; }//員工職稱代號
        public string JobTitleName { get; }//員工職稱名稱
        public string Phone { get; } //員工電話
        public int Supervisor { get; } //員工主管        
        public void ErrorMsg(Exception ex)
        {
            MessageBox.Show(ex.Message + "\n請檢查程式碼或輸入值");
        }
        public void resetText(Control a)//清除頁面欄位
        {            
            {
                foreach (Control ctrl in a.Controls)
                {
                    if (ctrl is TextBox || ctrl is ComboBox || ctrl is PictureBox || ctrl is RadioButton || ctrl is DateTimePicker)
                        ctrl.Text = "";
                    else if (ctrl is GroupBox)
                        resetText(ctrl);
                }
            }
        }

        public MyUserInfo(int userID)
        {
            try
            {
                MyHREntities hrEntities = new MyHREntities();
                //抓員工資料
                var q = (hrEntities.Users.Where(o => o.EmployeeID == userID).Select(o => new { o.Photo, o.EmployeeName, o.EmployeeEnglishName, o.Department, o.JobTitle, o.Phone, Supervisor = o.Supervisor == null ? o.EmployeeID : o.Supervisor })).ToList();
                //var q = (hrEntities.Users
                //    .Join(hrEntities.Departments, u => u.Department, d => d.DepartmentID, (u, d) => new { u.EmployeeID, u.EmployeeName, u.EmployeeEnglishName, u.Department, d.DepartmentName, u.JobTitle, u.Phone, u.Supervisor })
                //    //.Join(hrEntities.Departments, u => u.Department, d => d.DepartmentID, (u, d) => new { U = u, D = d})
                //    .Join(hrEntities.JobTitles, u => u.JobTitle, j => j.JobTitleID, (u, j) => new { u, j.JobTitle1 })
                //    .Where(o => o.u.EmployeeID == userID))
                //    .ToList();
                //抓員工部門名稱
                var q2 = (hrEntities.Users
                    .Join(hrEntities.Departments,
                    u => u.Department,
                    d => d.DepartmentID,
                    (u, d) => new
                    {
                        u.EmployeeID,
                        DepartmentName = d.DepartmentName
                    })
                    .Where(ud => ud.EmployeeID == userID))
                    .ToList();
                //抓員工職稱名稱
                var q3 = (hrEntities.Users
                    .Join(hrEntities.JobTitles,
                    u => u.JobTitle,
                    j => j.JobTitleID,
                    (u, j) => new
                    {
                        u.EmployeeID,
                        JobTitleName = j.JobTitle1,
                    })
                    .Where(uj => uj.EmployeeID == userID))
                    .ToList();

                ID = userID;
                Photo = q[0].Photo;
                Name = q[0].EmployeeName;
                EnglishName = q[0].EmployeeEnglishName;
                Dept = (int)q[0].Department;
                DeptName = q2[0].DepartmentName;
                JobTitle = (int)q[0].JobTitle;
                JobTitleName = q3[0].JobTitleName;
                Phone = q[0].Phone;
                Supervisor = (int)q[0].Supervisor;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);                
            }
        }
    }
    #region DataGridViewDisableButton
    public class DataGridViewDisableButtonColumn : DataGridViewButtonColumn
    {
        public DataGridViewDisableButtonColumn()
        {
            this.CellTemplate = new DataGridViewDisableButtonCell();
        }
    }

    public class DataGridViewDisableButtonCell : DataGridViewButtonCell
    {
        private bool enabledValue;
        public bool Enabled
        {
            get
            {
                return enabledValue;
            }
            set
            {
                enabledValue = value;
            }
        }

        // Override the Clone method so that the Enabled property is copied.
        public override object Clone()
        {
            DataGridViewDisableButtonCell cell =
                (DataGridViewDisableButtonCell)base.Clone();
            cell.Enabled = this.Enabled;
            return cell;
        }

        // By default, enable the button cell.
        public DataGridViewDisableButtonCell()
        {
            this.enabledValue = true;
        }

        protected override void Paint(Graphics graphics,
            Rectangle clipBounds, Rectangle cellBounds, int rowIndex,
            DataGridViewElementStates elementState, object value,
            object formattedValue, string errorText,
            DataGridViewCellStyle cellStyle,
            DataGridViewAdvancedBorderStyle advancedBorderStyle,
            DataGridViewPaintParts paintParts)
        {
            // The button cell is disabled, so paint the border,
            // background, and disabled button for the cell.
            if (!this.enabledValue)
            {
                // Draw the cell background, if specified.
                if ((paintParts & DataGridViewPaintParts.Background) ==
                    DataGridViewPaintParts.Background)
                {
                    SolidBrush cellBackground =
                        new SolidBrush(cellStyle.BackColor);
                    graphics.FillRectangle(cellBackground, cellBounds);
                    cellBackground.Dispose();
                }

                // Draw the cell borders, if specified.
                if ((paintParts & DataGridViewPaintParts.Border) ==
                    DataGridViewPaintParts.Border)
                {
                    PaintBorder(graphics, clipBounds, cellBounds, cellStyle,
                        advancedBorderStyle);
                }

                // Calculate the area in which to draw the button.
                Rectangle buttonArea = cellBounds;
                Rectangle buttonAdjustment =
                    this.BorderWidths(advancedBorderStyle);
                buttonArea.X += buttonAdjustment.X;
                buttonArea.Y += buttonAdjustment.Y;
                buttonArea.Height -= buttonAdjustment.Height;
                buttonArea.Width -= buttonAdjustment.Width;

                // Draw the disabled button.
                ButtonRenderer.DrawButton(graphics, buttonArea,
                    PushButtonState.Disabled);

                // Draw the disabled button text.
                if (this.FormattedValue is String)
                {
                    TextRenderer.DrawText(graphics,
                        (string)this.FormattedValue,
                        this.DataGridView.Font,
                        buttonArea, SystemColors.GrayText);
                }
            }
            else
            {
                // The button cell is enabled, so let the base class
                // handle the painting.
                base.Paint(graphics, clipBounds, cellBounds, rowIndex,
                    elementState, value, formattedValue, errorText,
                    cellStyle, advancedBorderStyle, paintParts);
            }
        }
    }
    #endregion

    public class DBConnect
    {
        public DataTable DBDataReader(String commStr)
        {            
            string connstring = Settings.Default.MyHR;
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = connstring;
                conn.Open();
                SqlCommand comm = new SqlCommand(commStr, conn);
                SqlDataReader dataReader = comm.ExecuteReader();
                dataReader.Read();
                DataTable dt = new DataTable();
                                
                return dt;
            }            
        }        
    }    
}
