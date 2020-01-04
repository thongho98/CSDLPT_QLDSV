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
    public partial class frmSinhVien : DevExpress.XtraEditors.XtraForm
    {
        int vitri = 0;
        private DateTime ngaysinhmacdinh = new DateTime(1999, 01, 01);

        string malopdangchon = "";
        public frmSinhVien()
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

        private void frmSinhVien_Load(object sender, EventArgs e)
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

            dateNgaySinh.Format = DateTimePickerFormat.Custom;
            dateNgaySinh.CustomFormat = "dd/MM/yyyy";

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

        private void lOPGridControl_Click(object sender, EventArgs e)
        {
            malopdangchon = ((DataRowView)bdsLop.Current)["MALOP"].ToString();
            bdsSinhVien.Filter = "MALOP='" + malopdangchon + "'";
            txtMaLop.Text = malopdangchon;
        }

        private void lOPGridControl_FocusedViewChanged(object sender, DevExpress.XtraGrid.ViewFocusEventArgs e)
        {
            malopdangchon = ((DataRowView)bdsLop.Current)["MALOP"].ToString();
            bdsSinhVien.Filter = "MALOP='" + malopdangchon + "'";
        }

        private void btnThem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (malopdangchon == "")
            {
                MessageBox.Show("Vui lòng chọn lớp!");
                return;
            }
            Program.Control = "Them";
            vitri = bdsSinhVien.Position;
            bdsSinhVien.AddNew();
            disableMenuBar();
            datGiaTriMacDinh();
            gcSinhVien.Enabled = true;
            btnGhi.Enabled = btnThoat.Enabled = btnPhucHoi.Enabled = true;
            cmbTenPhongBan.Enabled = false;
            txtMaLop.Text = malopdangchon;
            txtMaSV.Focus();
        }

        private void btnXoa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (malopdangchon == "")
            {
                MessageBox.Show("Vui lòng chọn lớp!");
                return;
            }

            if (MessageBox.Show("Bạn có muốn xóa sinh viên " + txtHo.Text + " " + txtTen.Text + " ?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    bdsSinhVien.RemoveCurrent();
                    this.sINHVIENTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.sINHVIENTableAdapter.Update(this.dS.SINHVIEN);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xóa Lớp\n" + ex.Message, "", MessageBoxButtons.OK);
                    this.lOPTableAdapter.Fill(this.dS.LOP);
                    this.sINHVIENTableAdapter.Fill(this.dS.SINHVIEN);
                }
            }
        }

        private void btnHieuChinh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (malopdangchon == "")
            {
                MessageBox.Show("Vui lòng chọn lớp!");
                return;
            }
            Program.Control = "Sua";
            disableMenuBar();
            txtMaSV.Enabled = false;
            btnGhi.Enabled = btnThoat.Enabled = btnPhucHoi.Enabled = true;
            cmbTenPhongBan.Enabled = false;
            txtMaSV.Focus();
        }

        private void btnPhucHoi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MessageBox.Show("Bạn muốn hủy bỏ tất cả thao tác?", "", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                bdsSinhVien.CancelEdit();
                this.sINHVIENTableAdapter.Connection.ConnectionString = Program.connstr;
                this.sINHVIENTableAdapter.Fill(this.dS.SINHVIEN);
                this.lOPTableAdapter.Connection.ConnectionString = Program.connstr;
                this.lOPTableAdapter.Fill(this.dS.LOP);
                if (btnThem.Enabled == false) bdsLop.Position = vitri;
                trangthaibandauMenuBar();
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
                        bdsSinhVien.EndEdit();
                        bdsSinhVien.ResetCurrentItem();
                        this.sINHVIENTableAdapter.Connection.ConnectionString = Program.connstr;
                        this.sINHVIENTableAdapter.Update(this.dS.SINHVIEN);
                        bdsSinhVien.MoveLast();
                        cmbTenPhongBan.Enabled = true;
                        trangthaibandauMenuBar();
                        MessageBox.Show("Thêm thành công", "", MessageBoxButtons.OK);
                    }
                }
                if (Program.Control == "Sua")
                {
                    bdsSinhVien.EndEdit();
                    bdsSinhVien.ResetCurrentItem();
                    this.sINHVIENTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.sINHVIENTableAdapter.Update(this.dS.SINHVIEN);
                    cmbTenPhongBan.Enabled = true;
                    trangthaibandauMenuBar();
                    txtMaSV.Enabled = true;
                    MessageBox.Show("Đã sửa thành công", "", MessageBoxButtons.OK);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi ghi sinh viên!" + ex.Message, "", MessageBoxButtons.OK);
            }
        }

        private void btnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.lOPTableAdapter.Connection.ConnectionString = Program.connstr;
            this.lOPTableAdapter.Fill(this.dS.LOP);
            this.sINHVIENTableAdapter.Connection.ConnectionString = Program.connstr;
            this.sINHVIENTableAdapter.Fill(this.dS.SINHVIEN);
        }

        private void btnChuyenLop_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void btnThoat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MessageBox.Show("Bạn thật sự muốn thoát khỏi form?", "", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                this.Close();
            }
        }

        private bool kiemTraTrungMaSV()
        {
            SqlConnection con = new SqlConnection(Program.connstr);
            SqlCommand sqlCommand = new SqlCommand();
            SqlParameter returnValue = new SqlParameter();
            SqlParameter input = new SqlParameter();

            returnValue.ParameterName = "@returnValue";
            returnValue.SqlDbType = SqlDbType.Int;
            returnValue.Direction = ParameterDirection.ReturnValue;

            input.ParameterName = "@MASV";
            input.SqlDbType = SqlDbType.NChar;
            input.Direction = ParameterDirection.Input;
            input.Value = txtMaSV.Text;

            sqlCommand.Connection = con;
            con.Open();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "SP_KIEMTRAMASINHVIENTONTAI";
            sqlCommand.Parameters.Add(returnValue);
            sqlCommand.Parameters.Add(input);
            sqlCommand.ExecuteNonQuery();
            if ((int)returnValue.Value == 1)
            {
                return true;
            }
            return false;
        }

        private bool kiemTraTruocKhiGhi()
        {
            string maSV = txtMaSV.Text;
            if (maSV == "")
            {
                MessageBox.Show("Mã sinh viên không được rỗng");
                txtMaSV.Focus();
                return false;
            }
            if (maSV.Length > 8)
            {
                MessageBox.Show("Mã sinh viên tối đa 8 ký tự!");
                txtMaSV.Focus();
                return false;
            }

            if (kiemTraTrungMaSV())
            {
                MessageBox.Show("Mã sinh viên bị trùng!");
                txtMaSV.Focus();
                return false;
            }
            string ho = txtHo.Text;
            if (ho == "")
            {
                MessageBox.Show("Họ sinh viên không được rỗng");
                txtHo.Focus();
                return false;
            }
            if (ho.Length > 40)
            {
                MessageBox.Show("Họ sinh viên tối đa 40 ký tự!");
                txtHo.Focus();
                return false;
            }

            string ten = txtTen.Text;
            if (ten == "")
            {
                MessageBox.Show("Tên sinh viên không được rỗng");
                txtHo.Focus();
                return false;
            }
            if (ten.Length > 10)
            {
                MessageBox.Show("Tên sinh viên tối đa 10 ký tự!");
                txtHo.Focus();
                return false;
            }

            string noisinh = txtNoiSinh.Text;
            if (noisinh == "")
            {
                MessageBox.Show("Nơi sinh không được rỗng");
                txtNoiSinh.Focus();
                return false;
            }
            if (noisinh.Length > 40)
            {
                MessageBox.Show("Nơi sinh tối đa 40 ký tự!");
                txtNoiSinh.Focus();
                return false;
            }

            string diachi = txtDiaChi.Text;
            if (diachi == "")
            {
                MessageBox.Show("Địa chỉ không được rỗng");
                txtDiaChi.Focus();
                return false;
            }
            if (diachi.Length > 40)
            {
                MessageBox.Show("Địa chỉ tối đa 80 ký tự!");
                txtNoiSinh.Focus();
                return false;
            }
            return true;
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
            txtMaSV.Text = "";
            txtHo.Text = "";
            txtTen.Text = "";
            txtNoiSinh.Text = "";
            txtDiaChi.Text = "";
            txtMaLop.Text = "";
        }

        private void datGiaTriMacDinh()
        {
            dateNgaySinh.Value = ngaysinhmacdinh;
            checkPhai.Checked = false;
            checkNghi.Checked = false;
        }

        //Context menu
        private void contextThemSV_Click(object sender, EventArgs e)
        {
            if (malopdangchon == "")
            {
                MessageBox.Show("Vui lòng chọn lớp!");
                return;
            }
            Program.Control = "Them";
            vitri = bdsSinhVien.Position;
            bdsSinhVien.AddNew();
            disableMenuBar();
            datGiaTriMacDinh();
            gcSinhVien.Enabled = true;
            btnGhi.Enabled = btnThoat.Enabled = btnPhucHoi.Enabled = true;
            cmbTenPhongBan.Enabled = false;
            txtMaLop.Text = malopdangchon;
            txtMaSV.Focus();
        }

        private void contextXoaSV_Click(object sender, EventArgs e)
        {
            if (malopdangchon == "")
            {
                MessageBox.Show("Vui lòng chọn lớp!");
                return;
            }
            if (MessageBox.Show("Bạn có muốn xóa sinh viên " + txtHo.Text + " " + txtTen.Text + " ?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    bdsSinhVien.RemoveCurrent();
                    this.sINHVIENTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.sINHVIENTableAdapter.Update(this.dS.SINHVIEN);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xóa Lớp\n" + ex.Message, "", MessageBoxButtons.OK);
                    this.lOPTableAdapter.Fill(this.dS.LOP);
                    this.sINHVIENTableAdapter.Fill(this.dS.SINHVIEN);
                }
            }
        }

        private void contextHieuChinh_Click(object sender, EventArgs e)
        {
            if (malopdangchon == "")
            {
                MessageBox.Show("Vui lòng chọn lớp!");
                return;
            }
            Program.Control = "Sua";
            disableMenuBar();
            txtMaSV.Enabled = false;
            btnGhi.Enabled = btnThoat.Enabled = btnPhucHoi.Enabled = true;
            cmbTenPhongBan.Enabled = false;
            txtMaSV.Focus();
        }

        private void contextGhiSV_Click(object sender, EventArgs e)
        {
            try
            {
                if (Program.Control == "Them")
                {
                    if (kiemTraTruocKhiGhi())
                    {
                        bdsSinhVien.EndEdit();
                        bdsSinhVien.ResetCurrentItem();
                        this.sINHVIENTableAdapter.Connection.ConnectionString = Program.connstr;
                        this.sINHVIENTableAdapter.Update(this.dS.SINHVIEN);
                        bdsSinhVien.MoveLast();
                        cmbTenPhongBan.Enabled = true;
                        trangthaibandauMenuBar();
                        MessageBox.Show("Thêm thành công", "", MessageBoxButtons.OK);
                    }
                }
                if (Program.Control == "Sua")
                {
                    bdsSinhVien.EndEdit();
                    bdsSinhVien.ResetCurrentItem();
                    this.sINHVIENTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.sINHVIENTableAdapter.Update(this.dS.SINHVIEN);
                    cmbTenPhongBan.Enabled = true;
                    trangthaibandauMenuBar();
                    txtMaSV.Enabled = true;
                    MessageBox.Show("Đã sửa thành công", "", MessageBoxButtons.OK);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi ghi sinh viên!" + ex.Message, "", MessageBoxButtons.OK);
            }
        }
    }
}