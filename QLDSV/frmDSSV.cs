using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.SqlClient;

namespace QLDSV
{
    public partial class frmDSSV : DevExpress.XtraEditors.XtraForm
    {
        private string lop;
        private string monhoc;
        private int lanthi;
        public frmDSSV(string lop, string monhoc, int lanthi)
        {
            InitializeComponent();
            this.lop = lop;
            this.monhoc = monhoc;
            this.lanthi = lanthi;
        }

        private void frmDSSV_Load(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(Program.connstr);
            SqlCommand sqlCommand = new SqlCommand();
            SqlParameter returnValue = new SqlParameter();
            SqlParameter input = new SqlParameter();

            sqlCommand.Connection = con;
            con.Open();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "SP_GETBANGDIEM";
            sqlCommand.Parameters.Add("@MALOP", SqlDbType.NChar, 8).Value = lop;
            sqlCommand.Parameters.Add("@MAMH", SqlDbType.NChar, 5).Value = monhoc;
            sqlCommand.Parameters.Add("@LANTHI", SqlDbType.SmallInt).Value = lanthi;
            sqlCommand.ExecuteNonQuery();

            SqlDataAdapter da = new SqlDataAdapter(sqlCommand);
            DataSet ds = new DataSet();
            da.Fill(ds, "BANGDIEM");
            con.Close();

            dataGridView1.AutoGenerateColumns = true; // hiển thị các cột tương ứng với các trường dữ liệu có sẵn
            dataGridView1.DataSource = ds;
            dataGridView1.DataMember = "BANGDIEM";
            dataGridView1.Columns[0].HeaderText = "Mã sinh viên";
            dataGridView1.Columns[0].ReadOnly = true;
            dataGridView1.Columns[1].HeaderText = "Họ tên";
            dataGridView1.Columns[1].ReadOnly = true;
            dataGridView1.Columns[2].HeaderText = "Điểm";
        }

        private void btnGhiDiem_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(Program.connstr);
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM DIEM", con);
            da.InsertCommand = new SqlCommand("SP_INSERTBANGDIEM", con);
            da.InsertCommand.CommandType = CommandType.StoredProcedure;

            da.InsertCommand.Parameters.Add("@MASV", SqlDbType.NChar, 8, "masv");
            da.InsertCommand.Parameters.Add("@MAMH", SqlDbType.NChar, 5, "mamh");
            da.InsertCommand.Parameters.Add("@LANTHI", SqlDbType.SmallInt, 1, "lan");
            da.InsertCommand.Parameters.Add("@DIEM", SqlDbType.Float, 8, "diem");

            DataSet ds = new DataSet();
            da.Fill(ds, "BANGDIEMSV");
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                dataGridView1.EndEdit();
                DataRow newRow = ds.Tables["BANGDIEMSV"].NewRow();
                newRow["masv"] = dataGridView1.Rows[i].Cells[0].Value.ToString();
                newRow["mamh"] = monhoc;
                newRow["lan"] = lanthi;

                //Kiem tra diem nhap vao
                if(dataGridView1.Rows[i].Cells[2].Value.ToString() == "")
                {
                    MessageBox.Show("Điểm không được để trống!");
                    return;
                }
                if (IsDigitsOnly(dataGridView1.Rows[i].Cells[2].Value.ToString()) == false)
                {
                    MessageBox.Show("Điểm chỉ từ 0 đến 10 và chỉ có số !! Vui lòng kiểm tra lại ");
                    return;
                }
                float diem = float.Parse(dataGridView1.Rows[i].Cells[2].Value.ToString());
                if (diem >= 0 && diem <= 10)
                {
                    newRow["diem"] = dataGridView1.Rows[i].Cells[2].Value;
                }
                else
                {
                    MessageBox.Show("Điểm chỉ từ 0 đến 10 !! Vui lòng kiểm tra lại ");
                    return;
                }
                ds.Tables["BANGDIEMSV"].Rows.Add(newRow);
            }
            da.Update(ds, "BANGDIEMSV");
            con.Close();
            MessageBox.Show("Cập nhật điểm thành công!");
            //xóa dataGridView1 
            dataGridView1.DataSource = null;
            dataGridView1.Columns.Clear();
            this.Close();
        }

        private bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '.' || c > '9')
                    return false;
            }
            return true;
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}