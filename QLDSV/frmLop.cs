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
    public partial class frmLop : DevExpress.XtraEditors.XtraForm
    {
        int vitri = 0;

        public frmLop()
        {
            InitializeComponent();
            trangthaibandauMenuBar();
        }

        private void lOPBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bdsLop.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dS);

        }

        private void frmLop_Load(object sender, EventArgs e)
        {
            dS.EnforceConstraints = false;

            // TODO: This line of code loads data into the 'dS.SINHVIEN' table. You can move, or remove it, as needed.
            this.sINHVIENTableAdapter.Connection.ConnectionString = Program.connstr;
            this.sINHVIENTableAdapter.Fill(this.dS.SINHVIEN);

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
        }

        private void cmbTenPhongBan_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbTenPhongBan.SelectedValue.ToString() == "System.Data.DataRowView")
                return;
            Program.servername = cmbTenPhongBan.SelectedValue.ToString();
            if(Program.servername == "THONG-PC\\SERVER1")
            {
                Program.mMAKHOA = "CNTT";
            }
            else if(Program.servername == "THONG-PC\\SERVER2")
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

        private void btnThem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Program.Control = "Them";
            vitri = bdsLop.Position;
            bdsLop.AddNew();
            disableMenuBar();
            gcLop.Enabled = true;
            btnGhi.Enabled = btnThoat.Enabled = btnPhucHoi.Enabled = true;
            cmbTenPhongBan.Enabled = false;
            txtMaKhoa.Text = Program.mMAKHOA;
            txtMaLop.Focus();
        }

        private void btnXoa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (bdsSinhVien.Count > 0)
            {
                MessageBox.Show("Lớp có sinh viên đang học! Vui lòng chọn lớp trống!", "", MessageBoxButtons.OK);
                return;
            }
            if (MessageBox.Show("Bạn có muốn xóa lớp " + txtMaLop.Text + " ?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    bdsLop.RemoveCurrent();
                    this.lOPTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.lOPTableAdapter.Update(this.dS.LOP);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xóa Lớp\n" + ex.Message, "", MessageBoxButtons.OK);
                    this.lOPTableAdapter.Fill(this.dS.LOP);
                }
            }
        }

        private void btnHieuChinh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Program.Control = "Sua";
            vitri = bdsLop.Position;
            bdsLop.MoveFirst();
            disableMenuBar();
            btnGhi.Enabled = true;
            btnThoat.Enabled = true;
            btnPhucHoi.Enabled = true;
            cmbTenPhongBan.Enabled = false;
            txtMaLop.Focus();
            txtMaLop.Enabled = false;
        }

        private void btnPhucHoi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MessageBox.Show("Bạn muốn hủy bỏ tất cả thao tác?", "", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                bdsLop.CancelEdit();
                this.lOPTableAdapter.Connection.ConnectionString = Program.connstr;
                this.lOPTableAdapter.Fill(this.dS.LOP);
                if (btnThem.Enabled == false) bdsLop.Position = vitri;
                trangthaibandauMenuBar();
                if (Program.mGroup == "PGV") cmbTenPhongBan.Enabled = true;
                else cmbTenPhongBan.Enabled = false;
            }
        }

        private void btnGhi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (Program.Control == "Them")
                {
                    if (kiemTraTruocKhiGhi())
                    {
                        bdsLop.EndEdit();
                        bdsLop.ResetCurrentItem();
                        this.lOPTableAdapter.Update(this.dS.LOP);
                        bdsLop.MoveLast();
                        cmbTenPhongBan.Enabled = true;
                        trangthaibandauMenuBar();
                        MessageBox.Show("Thêm thành công", "", MessageBoxButtons.OK);
                    }
                }
                if (Program.Control == "Sua")
                {
                    if (kiemtratrungtenlop())
                    {
                        MessageBox.Show("Tên lớp bị trùng!");
                        txtTenLop.Focus();
                        return;
                    }
                    bdsLop.EndEdit();
                    bdsLop.ResetCurrentItem();
                    this.lOPTableAdapter.Update(this.dS.LOP);
                    cmbTenPhongBan.Enabled = true;
                    trangthaibandauMenuBar();
                    MessageBox.Show("Đã sửa thành công", "", MessageBoxButtons.OK);
                    txtMaLop.Enabled = true;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi ghi lớp!" + ex.Message, "", MessageBoxButtons.OK);
            }
        }

        private void btnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.lOPTableAdapter.Connection.ConnectionString = Program.connstr;
            this.lOPTableAdapter.Fill(this.dS.LOP);
        }

        private void btnThoat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MessageBox.Show("Bạn thật sự muốn thoát khỏi form?", "", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                this.Close();
            }
        }

        private bool kiemTraTruocKhiGhi()
        {
            string maLop = txtMaLop.Text;
            if (maLop == "")
            {
                MessageBox.Show("Mã lớp không được rỗng");
                txtMaLop.Focus();
                return false;
            }
            if (maLop.Length > 8)
            {
                MessageBox.Show("Mã lớp tối đa 8 ký tự!");
                txtMaLop.Focus();
                return false;
            }
            //Kiem tra trung ma lop
            if (kiemtratrungmalop())
            {
                MessageBox.Show("Mã lớp bị trùng!");
                txtMaLop.Focus();
                return false;
            }

            string tenLop = txtTenLop.Text;
            if (tenLop == "")
            {
                MessageBox.Show("Tên lớp không được rỗng");
                txtTenLop.Focus();
                return false;
            }
            if (tenLop.Length > 40)
            {
                MessageBox.Show("Tên lớp tối đa 40 ký tự!");
                txtTenLop.Focus();
                return false;
            }

            //Kiem tra trung ten lop
            if (kiemtratrungtenlop())
            {
                MessageBox.Show("Tên lớp bị trùng!");
                txtTenLop.Focus();
                return false;
            }
            return true;
        }

        private bool kiemtratrungmalop()
        {
            SqlConnection con = new SqlConnection(Program.connstr);
            SqlCommand sqlCommand = new SqlCommand();
            SqlParameter returnValue = new SqlParameter();
            SqlParameter input = new SqlParameter();

            returnValue.ParameterName = "@returnValue";
            returnValue.SqlDbType = SqlDbType.Int;
            returnValue.Direction = ParameterDirection.ReturnValue;

            input.ParameterName = "@MALOP";
            input.SqlDbType = SqlDbType.NChar;
            input.Direction = ParameterDirection.Input;
            input.Value = txtMaLop.Text;

            sqlCommand.Connection = con;
            con.Open();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "SP_KIEMTRAMALOPTONTAI";
            sqlCommand.Parameters.Add(returnValue);
            sqlCommand.Parameters.Add(input);
            sqlCommand.ExecuteNonQuery();
            if ((int)returnValue.Value == 1)
            {
                return true;
            }
            return false;
        }

        private bool kiemtratrungtenlop()
        {
            SqlConnection con = new SqlConnection(Program.connstr);
            SqlCommand sqlCommand = new SqlCommand();
            SqlParameter returnValue = new SqlParameter();
            SqlParameter input = new SqlParameter();

            returnValue.ParameterName = "@returnValue";
            returnValue.SqlDbType = SqlDbType.Int;
            returnValue.Direction = ParameterDirection.ReturnValue;

            input.ParameterName = "@TENLOP";
            input.SqlDbType = SqlDbType.NChar;
            input.Direction = ParameterDirection.Input;
            input.Value = txtTenLop.Text;

            sqlCommand.Connection = con;
            con.Open();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "SP_KIEMTRATENLOPTONTAI";
            sqlCommand.Parameters.Add(returnValue);
            sqlCommand.Parameters.Add(input);
            sqlCommand.ExecuteNonQuery();
            if ((int)returnValue.Value == 1)
            {
                return true;
            }
            return false;
        }

        private void disableMenuBar()
        {
            btnThem.Enabled = false;
            btnXoa.Enabled = false;
            btnHieuChinh.Enabled = false;
            btnPhucHoi.Enabled = false;
            btnGhi.Enabled = false;
            btnRefresh.Enabled = false;
            btnThoat.Enabled = false;
        }

        private void trangthaibandauMenuBar()
        {
            btnThem.Enabled = true;
            btnXoa.Enabled = true;
            btnHieuChinh.Enabled = true;
            btnPhucHoi.Enabled = false;
            btnGhi.Enabled = false;
            btnRefresh.Enabled = true;
            btnThoat.Enabled = true;
            txtMaLop.Text = "";
            txtTenLop.Text = "";
        }
    }
}