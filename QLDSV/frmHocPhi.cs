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
    public partial class frmHocPhi : DevExpress.XtraEditors.XtraForm
    {
        public frmHocPhi()
        {
            InitializeComponent();
        }

        private void btnThongTin_Click(object sender, EventArgs e)
        {
            if (txtMaSV.Text == "")
            {
                MessageBox.Show("Mã sinh viên không được trống!");
                txtMaSV.Focus();
                return;
            }
            string maSV = txtMaSV.Text;
            if (maSV.Length > 8)
            {
                MessageBox.Show("Mã sinh viên tối đa 8 ký tự!");
                txtMaSV.Focus();
                return;
            }
            SqlConnection con = new SqlConnection(Program.connstr);
            SqlCommand sqlCommand = new SqlCommand();
            SqlParameter returnValue = new SqlParameter();
            SqlParameter input = new SqlParameter();

            returnValue.ParameterName = "@returnValue";
            returnValue.SqlDbType = SqlDbType.Int;
            returnValue.Direction = ParameterDirection.ReturnValue;

            sqlCommand.Connection = con;
            con.Open();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "SP_GETINFOSV";
            sqlCommand.Parameters.Add("@MASV", SqlDbType.NChar, 8).Value = txtMaSV.Text;
            sqlCommand.Parameters.Add(returnValue);
            sqlCommand.ExecuteNonQuery();
            int check = (int)returnValue.Value;
            if (check == 1)
            {
                MessageBox.Show("Mã sinh viên không tồn tại! ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                con.Close();
                return;
            }
            else
            {
                
                frmDongHP frmDongHP = new frmDongHP(txtMaSV.Text);
                frmDongHP.Show();
                con.Close();
            }
        }

        private void frmHocPhi_Load(object sender, EventArgs e)
        {
            disableMenuBar();
        }

        private void disableMenuBar()
        {
            btnThem.Enabled = false;
            btnXoa.Enabled = false;
            btnHieuChinh.Enabled = false;
            btnPhucHoi.Enabled = false;
            btnGhi.Enabled = false;
            btnRefresh.Enabled = false;
            btnThoat.Enabled = true;
        }

        private void btnThoat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MessageBox.Show("Bạn thật sự muốn thoát khỏi form?", "", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                this.Close();
            }
        }
    }
}