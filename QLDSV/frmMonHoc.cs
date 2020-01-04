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
    public partial class frmMonHoc : DevExpress.XtraEditors.XtraForm
    {
        int vitri = 0;

        public frmMonHoc()
        {
            InitializeComponent();
            trangthaibandauMenuBar();
        }

        private void frmMonHoc_Load(object sender, EventArgs e)
        {
            dS.EnforceConstraints = false;
            // TODO: This line of code loads data into the 'dS.DIEM' table. You can move, or remove it, as needed.
            this.dIEMTableAdapter.Connection.ConnectionString = Program.connstr;
            this.dIEMTableAdapter.Fill(this.dS.DIEM);
            // TODO: This line of code loads data into the 'dS.MONHOC' table. You can move, or remove it, as needed.
            this.mONHOCTableAdapter.Connection.ConnectionString = Program.connstr;
            this.mONHOCTableAdapter.Fill(this.dS.MONHOC);

            if (Program.mGroup == "Khoa")
            {
                disableMenuBar();
                btnRefresh.Enabled = btnThoat.Enabled = true;
            }
        }

        private void mONHOCBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bdsMonHoc.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dS);

        }

        private void btnThem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Program.Control = "Them";
            vitri = bdsMonHoc.Position;
            bdsMonHoc.AddNew();
            disableMenuBar();
            gcMonHoc.Enabled = true;
            btnGhi.Enabled = btnThoat.Enabled = btnPhucHoi.Enabled = true;
            txtMaMH.Focus();
        }

        private void btnXoa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (bdsDiem.Count > 0)
            {
                MessageBox.Show("Không thể xóa môn học đã có điểm thi!", "", MessageBoxButtons.OK);
                return;
            }
            if (MessageBox.Show("Bạn có muốn môn " + txtTenMH.Text + " ?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    bdsMonHoc.RemoveCurrent();
                    this.mONHOCTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.mONHOCTableAdapter.Update(this.dS.MONHOC);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xóa môn học\n" + ex.Message, "", MessageBoxButtons.OK);
                    this.mONHOCTableAdapter.Fill(this.dS.MONHOC);
                }
            }
        }

        private void btnHieuChinh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Program.Control = "Sua";
            disableMenuBar();
            btnGhi.Enabled = true;
            btnThoat.Enabled = true;
            btnPhucHoi.Enabled = true;
            txtMaMH.Focus();
        }

        private void btnPhucHoi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MessageBox.Show("Bạn muốn hủy bỏ tất cả thao tác?", "", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                bdsMonHoc.CancelEdit();
                this.mONHOCTableAdapter.Connection.ConnectionString = Program.connstr;
                this.mONHOCTableAdapter.Fill(this.dS.MONHOC);
                if (btnThem.Enabled == false) bdsMonHoc.Position = vitri;
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
                        bdsMonHoc.EndEdit();
                        bdsMonHoc.ResetCurrentItem();
                        this.mONHOCTableAdapter.Connection.ConnectionString = Program.connstr;
                        this.mONHOCTableAdapter.Update(this.dS.MONHOC);
                        bdsMonHoc.MoveLast();
                        trangthaibandauMenuBar();
                        MessageBox.Show("Thêm thành công", "", MessageBoxButtons.OK);
                    }
                }
                if (Program.Control == "Sua")
                {
                    if (kiemTraTrungTenMonHoc())
                    {
                        MessageBox.Show("Tên môn học bị trùng!");
                        txtTenMH.Focus();
                        return;
                    }
                    bdsMonHoc.EndEdit();
                    bdsMonHoc.ResetCurrentItem();
                    this.mONHOCTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.mONHOCTableAdapter.Update(this.dS.MONHOC);
                    trangthaibandauMenuBar();
                    MessageBox.Show("Đã sửa thành công", "", MessageBoxButtons.OK);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi ghi lớp!" + ex.Message, "", MessageBoxButtons.OK);
            }
        }

        private void btnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.mONHOCTableAdapter.Fill(this.dS.MONHOC);
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
            string maMonHoc = txtMaMH.Text;
            if (maMonHoc == "")
            {
                MessageBox.Show("Mã môn học không được rỗng");
                txtMaMH.Focus();
                return false;
            }
            if (maMonHoc.Length > 5)
            {
                MessageBox.Show("Mã môn học tối đa 5 ký tự!");
                txtMaMH.Focus();
                return false;
            }
            //Kiem tra trung ma lop
            if (kiemTraTrungMaMonHoc())
            {
                MessageBox.Show("Mã môn học bị trùng!");
                txtMaMH.Focus();
                return false;
            }

            string tenMonHoc = txtTenMH.Text;
            if (tenMonHoc == "")
            {
                MessageBox.Show("Tên môn học không được rỗng");
                txtTenMH.Focus();
                return false;
            }
            if (tenMonHoc.Length > 40)
            {
                MessageBox.Show("Tên môn học tối đa 40 ký tự!");
                txtTenMH.Focus();
                return false;
            }

            //Kiem tra trung ten lop
            if (kiemTraTrungTenMonHoc())
            {
                MessageBox.Show("Tên môn học bị trùng!");
                txtTenMH.Focus();
                return false;
            }
            return true;
        }

        private bool kiemTraTrungMaMonHoc()
        {
            SqlConnection con = new SqlConnection(Program.connstr);
            SqlCommand sqlCommand = new SqlCommand();
            SqlParameter returnValue = new SqlParameter();
            SqlParameter input = new SqlParameter();

            returnValue.ParameterName = "@returnValue";
            returnValue.SqlDbType = SqlDbType.Int;
            returnValue.Direction = ParameterDirection.ReturnValue;

            input.ParameterName = "@MAMH";
            input.SqlDbType = SqlDbType.NChar;
            input.Direction = ParameterDirection.Input;
            input.Value = txtMaMH.Text;

            sqlCommand.Connection = con;
            con.Open();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "SP_KIEMTRAMAMONHOCTONTAI";
            sqlCommand.Parameters.Add(returnValue);
            sqlCommand.Parameters.Add(input);
            sqlCommand.ExecuteNonQuery();
            if ((int)returnValue.Value == 1)
            {
                return true;
            }
            return false;
        }

        private bool kiemTraTrungTenMonHoc()
        {
            SqlConnection con = new SqlConnection(Program.connstr);
            SqlCommand sqlCommand = new SqlCommand();
            SqlParameter returnValue = new SqlParameter();
            SqlParameter input = new SqlParameter();

            returnValue.ParameterName = "@returnValue";
            returnValue.SqlDbType = SqlDbType.Int;
            returnValue.Direction = ParameterDirection.ReturnValue;

            input.ParameterName = "@TENMH";
            input.SqlDbType = SqlDbType.NChar;
            input.Direction = ParameterDirection.Input;
            input.Value = txtTenMH.Text;

            sqlCommand.Connection = con;
            con.Open();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "SP_KIEMTRATENMONHOCTONTAI";
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
            txtMaMH.Text = "";
            txtTenMH.Text = "";
        }
    }
}