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
    public partial class frmDongHP : DevExpress.XtraEditors.XtraForm
    {
        private string masv;
        public frmDongHP(string masv)
        {
            InitializeComponent();
            this.masv = masv;
        }

        private void frmDongHP_Load(object sender, EventArgs e)
        {
            //Đọc lên nhãn
            string strLenh = "EXEC SP_GETINFOSV '" + masv + "'";
            Program.myReader = Program.ExecSqlDataReader(strLenh);
            if (Program.myReader == null) return;
            Program.myReader.Read();
            lbHoTen.Text = Program.myReader.GetString(0);
            lbLop.Text = Program.myReader.GetString(1);
            lbStatus.Text = Program.myReader.GetBoolean(2) ? "Nghỉ học" : "Còn học";
            Program.conn.Close();

            SqlConnection con = new SqlConnection(Program.connstr);
            SqlCommand sqlCommand = new SqlCommand();
            SqlParameter returnValue = new SqlParameter();
            SqlParameter input = new SqlParameter();

            sqlCommand.Connection = con;
            con.Open();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "SP_GETHOCPHISV";
            sqlCommand.Parameters.Add("@MASV", SqlDbType.NChar, 8).Value = masv;
            sqlCommand.ExecuteNonQuery();

            SqlDataAdapter da = new SqlDataAdapter(sqlCommand);
            DataSet ds = new DataSet();
            da.Fill(ds, "BANGHOCPHI");
            con.Close();

            dataGridView1.AutoGenerateColumns = true; // hiển thị các cột tương ứng với các trường dữ liệu có sẵn
            dataGridView1.DataSource = ds;
            dataGridView1.DataMember = "BANGHOCPHI";
            dataGridView1.Columns[0].HeaderText = "Niên khóa";
            dataGridView1.Columns[1].HeaderText = "Học kì";
            dataGridView1.Columns[2].HeaderText = "Học phí";
            dataGridView1.Columns[2].DefaultCellStyle.Format = "#,####0";
            dataGridView1.Columns[3].HeaderText = "Số tiền đã đóng";
            dataGridView1.Columns[3].DefaultCellStyle.Format = "#,####0";
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnGhiHocPhi_Click(object sender, EventArgs e)
        {
            if (lbStatus.Text == "Nghỉ học")
            {
                MessageBox.Show("Sinh viên đã nghỉ học không thể đóng học phí! Vui lòng liên hệ phòng giáo vụ!");
                return;
            }
            else
            {
                SqlConnection con = new SqlConnection(Program.connstr);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM HOCPHI", con);
                da.InsertCommand = new SqlCommand("SP_INSERTHOCPHI", con);
                da.InsertCommand.CommandType = CommandType.StoredProcedure;

                da.InsertCommand.Parameters.Add("@MASV", SqlDbType.NChar, 8, "masv");
                da.InsertCommand.Parameters.Add("@NIENKHOA", SqlDbType.NVarChar, 12, "nienkhoa");
                da.InsertCommand.Parameters.Add("@HOCKY", SqlDbType.Int, 8, "hocky");
                da.InsertCommand.Parameters.Add("@HOCPHI", SqlDbType.Int, 8, "hocphi");
                da.InsertCommand.Parameters.Add("@SOTIENDADONG", SqlDbType.Int, 8, "sotiendadong");

                DataSet ds = new DataSet();
                da.Fill(ds, "BANGHOCPHISV");
                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    dataGridView1.EndEdit();
                    DataRow newRow = ds.Tables["BANGHOCPHISV"].NewRow();
                    newRow["masv"] = masv;

                    //Kiem tra niem khoa
                    newRow["nienkhoa"] = dataGridView1.Rows[i].Cells[0].Value.ToString();

                    //Kiem tra hoc ky
                    if (dataGridView1.Rows[i].Cells[1].Value.ToString() == "")
                    {
                        newRow["hocky"] = 1;
                    }
                    else
                    {
                        newRow["hocky"] = dataGridView1.Rows[i].Cells[1].Value;
                    }


                    int hocphi;
                    if (dataGridView1.Rows[i].Cells[2].Value.ToString() == "")
                    {
                        newRow["hocphi"] = 0;
                        hocphi = 0;
                    }
                    else
                    {
                        hocphi = int.Parse(dataGridView1.Rows[i].Cells[2].Value.ToString());
                        if (hocphi < 0 || hocphi > 20000000)
                        {
                            MessageBox.Show("Học phí từ 0 VND đến 20.000.000 VND !! Vui lòng kiểm tra lại ");
                            return;
                        }
                        else
                        {
                            newRow["hocphi"] = dataGridView1.Rows[i].Cells[2].Value;
                        }
                    }

                    //Kiem tra so tien da dong
                    if (dataGridView1.Rows[i].Cells[3].Value.ToString() == "")
                    {
                        newRow["sotiendadong"] = 0;
                    }
                    else
                    {
                        int sotiendadong = int.Parse(dataGridView1.Rows[i].Cells[3].Value.ToString());
                        if (sotiendadong < 0 || sotiendadong > 20000000)
                        {
                            MessageBox.Show("Số tiền đã đóng từ 0 VND đến 20.000.000 VND !! Vui lòng kiểm tra lại ");
                            return;
                        }
                        if (sotiendadong > hocphi)
                        {
                            MessageBox.Show("Số tiền đã đóng phải nhỏ hơn bằng tiền học phí !! Vui lòng kiểm tra lại ");
                            return;
                        }
                        else
                        {
                            newRow["sotiendadong"] = dataGridView1.Rows[i].Cells[3].Value;
                        }
                    }
                    ds.Tables["BANGHOCPHISV"].Rows.Add(newRow);
                }
                da.Update(ds, "BANGHOCPHISV");
                con.Close();
                MessageBox.Show("Cập nhật học phí thành công!");
                //xóa table dataGridView1 
                dataGridView1.DataSource = null;
                dataGridView1.Columns.Clear();
                this.Close();
            }
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

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}