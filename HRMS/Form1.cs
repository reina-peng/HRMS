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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MyHREntities hREntities = new MyHREntities();
            var q = from n in hREntities.Bulletins
                    select n;
            this.dataGridView1.DataSource = q.ToList();
            DataGridViewDisableButtonColumn co7 = new DataGridViewDisableButtonColumn();

            co7.HeaderText = "按鈕";
            co7.Name = "123";
            co7.Text = "oao";

            dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { co7 });
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (dataGridView1.Columns[e.ColumnIndex].Name == "123")
            {
                DataGridViewDisableButtonCell buttonCell =
                    (DataGridViewDisableButtonCell)dataGridView1.
                    Rows[e.RowIndex].Cells["123"];

                if (buttonCell.Enabled)
                {
                    MessageBox.Show(dataGridView1.Rows[e.RowIndex].
                        Cells[e.ColumnIndex-1].Value.ToString() +
                        " is enabled");
                }
                buttonCell.Enabled = false;
            }
        }
    }
}
