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
    public partial class frmNhapDiem : DevExpress.XtraEditors.XtraForm
    {
        public frmNhapDiem()
        {
            InitializeComponent();
        }

        private void frmNhapDiem_Load(object sender, EventArgs e)
        {
            dS.EnforceConstraints = false;
            // TODO: This line of code loads data into the 'dS.MONHOC' table. You can move, or remove it, as needed.
            this.mONHOCTableAdapter.Connection.ConnectionString = Program.connstr;
            this.mONHOCTableAdapter.Fill(this.dS.MONHOC);
            // TODO: This line of code loads data into the 'dS.LOP' table. You can move, or remove it, as needed.
            this.lOPTableAdapter.Connection.ConnectionString = Program.connstr;
            this.lOPTableAdapter.Fill(this.dS.LOP);

            DataTable dt = new DataTable();
            dt = Program.ExecSqlDataTable("SELECT MAKH FROM KHOA");
            BindingSource bdsCN = new BindingSource();
            bdsCN.DataSource = dt;
            Program.mMAKHOA = ((DataRowView)bdsCN[0])["MAKH"].ToString();
            Program.bds_dspm.Filter = "TENKH LIKE 'KHOA%'";
            cmbTenPhongBan.DataSource = Program.bds_dspm;
            cmbTenPhongBan.DisplayMember = "TENKH";
            cmbTenPhongBan.ValueMember = "TENSERVER";
            cmbTenPhongBan.SelectedIndex = Program.mChinhanh;
            if (Program.mGroup == "PGV")
            {
                cmbTenPhongBan.Enabled = true;
            }
            if (Program.mGroup == "Khoa")
            {
                cmbTenPhongBan.Enabled = false;
                disableMenuBar();
                btnRefresh.Enabled = btnThoat.Enabled = true;

            }

            cmbLanThi.SelectedIndex = 0;
            disableMenuBar();
        }

        private void cmbTenPhongBan_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbTenPhongBan.SelectedValue.ToString() == "System.Data.DataRowView")
                return;
            Program.servername = cmbTenPhongBan.SelectedValue.ToString();
            if (Program.servername == "THONG-PC\\SERVER1")
            {
                Program.mMAKHOA = "CNTT";
            }
            else if (Program.servername == "THONG-PC\\SERVER2")
            {
                Program.mMAKHOA = "VT";
            }
            else
            {
                MessageBox.Show("Bạn không có quyền truy cập vào phòng ban này! ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            if (cmbTenPhongBan.SelectedIndex != Program.mChinhanh)
            {
                Program.mlogin = Program.remotelogin;
                Program.password = Program.remotepassword;
            }
            else
            {
                Program.mlogin = Program.mloginDN;
                Program.password = Program.passwordDN;
            }

            if (Program.KetNoi() == 0)
            {
                MessageBox.Show("Lỗi kết nối về phòng ban mới!");
            }
            else
            {
                this.lOPTableAdapter.Connection.ConnectionString = Program.connstr;
                this.lOPTableAdapter.Fill(this.dS.LOP);
            }
        }

        private void lOPBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bdsLop.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dS);

        }

        private void btnBatDau_Click(object sender, EventArgs e)
        {
            if (cmbLop.Text == "" || cmbMonHoc.Text == "" || cmbLanThi.SelectedIndex == -1)
            {
                MessageBox.Show("Mã lớp hoặc mã môn hoặc lần thi bị trống");
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
            sqlCommand.CommandText = "SP_GETBANGDIEM";
            sqlCommand.Parameters.Add("@MALOP", SqlDbType.NChar, 8).Value = cmbLop.Text;
            sqlCommand.Parameters.Add("@MAMH", SqlDbType.NChar, 5).Value = cmbMonHoc.SelectedValue;
            sqlCommand.Parameters.Add("@LANTHI", SqlDbType.SmallInt).Value = cmbLanThi.SelectedItem;
            sqlCommand.Parameters.Add(returnValue);
            sqlCommand.ExecuteNonQuery();
            int check = (int)returnValue.Value;
            if (check == 1)
            {
                MessageBox.Show("Lớp không có sinh viên! Vui lòng chọn lớp khác!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                con.Close();
                return;
            }
            if (check == 2)
            {
                MessageBox.Show("Bạn cần phải nhập điểm thi lần 1 trước! ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                con.Close();
                return;
            }
            else
            {
                frmDSSV frmDSSV = new frmDSSV(cmbLop.Text, cmbMonHoc.SelectedValue.ToString(), int.Parse(cmbLanThi.Text));
                frmDSSV.Show();
            }
        }

        private void cmbLop_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmbMonHoc_SelectedIndexChanged(object sender, EventArgs e)
        {

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