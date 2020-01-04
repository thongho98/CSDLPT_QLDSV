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
    public partial class frmGiangVien : DevExpress.XtraEditors.XtraForm
    {
        int vitri = 0;

        public frmGiangVien()
        {
            InitializeComponent();
            trangthaibandauMenuBar();
        }

        private void gIANGVIENBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bdsGiangVien.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dS);

        }

        private void frmGiangVien_Load(object sender, EventArgs e)
        {
            dS.EnforceConstraints = false;
            // TODO: This line of code loads data into the 'dS.GIANGVIEN' table. You can move, or remove it, as needed.
            this.gIANGVIENTableAdapter.Connection.ConnectionString = Program.connstr;
            this.gIANGVIENTableAdapter.Fill(this.dS.GIANGVIEN);

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
                this.gIANGVIENTableAdapter.Connection.ConnectionString = Program.connstr;
                this.gIANGVIENTableAdapter.Fill(this.dS.GIANGVIEN);
            }
        }

        private void btnThem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Program.Control = "Them";
            vitri = bdsGiangVien.Position;
            bdsGiangVien.AddNew();
            disableMenuBar();
            gcGiangVien.Enabled = true;
            btnGhi.Enabled = btnThoat.Enabled = btnPhucHoi.Enabled = true;
            cmbTenPhongBan.Enabled = false;
            txtMaKhoa.Text = Program.mMAKHOA;
            txtMaGV.Focus();
        }

        private void btnXoa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn xóa giảng viên " + txtHo.Text + " " + txtTen.Text + " ?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    bdsGiangVien.RemoveCurrent();
                    this.gIANGVIENTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.gIANGVIENTableAdapter.Update(this.dS.GIANGVIEN);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xóa giảng viên\n" + ex.Message, "", MessageBoxButtons.OK);
                    this.gIANGVIENTableAdapter.Fill(this.dS.GIANGVIEN);
                }
            }
        }

        private void btnHieuChinh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Program.Control = "Sua";
            vitri = bdsGiangVien.Position;
            bdsGiangVien.MoveFirst();
            disableMenuBar();
            btnGhi.Enabled = true;
            btnThoat.Enabled = true;
            btnPhucHoi.Enabled = true;
            cmbTenPhongBan.Enabled = false;
            txtMaGV.Focus();
            txtMaGV.Enabled = false;
        }

        private void btnPhucHoi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MessageBox.Show("Bạn muốn hủy bỏ tất cả thao tác?", "", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                bdsGiangVien.CancelEdit();
                this.gIANGVIENTableAdapter.Connection.ConnectionString = Program.connstr;
                this.gIANGVIENTableAdapter.Fill(this.dS.GIANGVIEN);
                if (btnThem.Enabled == false) bdsGiangVien.Position = vitri;
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
                        bdsGiangVien.EndEdit();
                        bdsGiangVien.ResetCurrentItem();
                        this.gIANGVIENTableAdapter.Update(this.dS.GIANGVIEN);
                        bdsGiangVien.MoveLast();
                        cmbTenPhongBan.Enabled = true;
                        trangthaibandauMenuBar();
                        MessageBox.Show("Thêm thành công", "", MessageBoxButtons.OK);
                    }
                }
                if (Program.Control == "Sua")
                {
                    bdsGiangVien.EndEdit();
                    bdsGiangVien.ResetCurrentItem();
                    this.gIANGVIENTableAdapter.Update(this.dS.GIANGVIEN);
                    cmbTenPhongBan.Enabled = true;
                    trangthaibandauMenuBar();
                    MessageBox.Show("Đã sửa thành công", "", MessageBoxButtons.OK);
                    txtMaGV.Enabled = true;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi ghi giảng viên!" + ex.Message, "", MessageBoxButtons.OK);
            }
        }

        private void btnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.gIANGVIENTableAdapter.Connection.ConnectionString = Program.connstr;
            this.gIANGVIENTableAdapter.Fill(this.dS.GIANGVIEN);
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
            string maGV = txtMaGV.Text;
            if (maGV == "")
            {
                MessageBox.Show("Mã giảng viên không được rỗng");
                txtMaGV.Focus();
                return false;
            }
            if (maGV.Length > 10)
            {
                MessageBox.Show("Mã giảng viên tối đa 10 ký tự!");
                txtMaGV.Focus();
                return false;
            }
            //Kiem tra trung ma giang vien
            if (kiemtratrungmagiangvien())
            {
                MessageBox.Show("Mã giảng viên bị trùng!");
                txtMaGV.Focus();
                return false;
            }

            string ho = txtHo.Text;
            if (ho == "")
            {
                MessageBox.Show("Họ giảng viên không được rỗng");
                txtHo.Focus();
                return false;
            }
            if (ho.Length > 50)
            {
                MessageBox.Show("Họ giảng viên tối đa 50 ký tự!");
                txtHo.Focus();
                return false;
            }
            string ten = txtTen.Text;
            if (ten == "")
            {
                MessageBox.Show("Tên giảng viên không được rỗng");
                txtTen.Focus();
                return false;
            }
            if (ten.Length > 10)
            {
                MessageBox.Show("Tên giảng viên tối đa 10 ký tự!");
                txtTen.Focus();
                return false;
            }
            return true;
        }

        private bool kiemtratrungmagiangvien()
        {
            SqlConnection con = new SqlConnection(Program.connstr);
            SqlCommand sqlCommand = new SqlCommand();
            SqlParameter returnValue = new SqlParameter();
            SqlParameter input = new SqlParameter();

            returnValue.ParameterName = "@returnValue";
            returnValue.SqlDbType = SqlDbType.Int;
            returnValue.Direction = ParameterDirection.ReturnValue;

            input.ParameterName = "@MAGV";
            input.SqlDbType = SqlDbType.NChar;
            input.Direction = ParameterDirection.Input;
            input.Value = txtMaGV.Text;

            sqlCommand.Connection = con;
            con.Open();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "SP_KIEMTRAMAGIANGVIENTONTAI";
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
            txtMaGV.Text = "";
            txtHo.Text = "";
            txtTen.Text = "";
        }
    }
}