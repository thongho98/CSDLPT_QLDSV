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
    public partial class frmTaoTaiKhoan : DevExpress.XtraEditors.XtraForm
    {
        public frmTaoTaiKhoan()
        {
            InitializeComponent();
        }

        private void gIANGVIENBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bdsGiangVien.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dS);

        }

        private void frmTaoTaiKhoan_Load(object sender, EventArgs e)
        {
            dS.EnforceConstraints = false;
            // TODO: This line of code loads data into the 'dS.GIANGVIEN' table. You can move, or remove it, as needed.
            this.gIANGVIENTableAdapter.Connection.ConnectionString = Program.connstr;
            this.gIANGVIENTableAdapter.Fill(this.dS.GIANGVIEN);

            cmbTenPhongBan.DataSource = Program.bds_dspm;
            cmbTenPhongBan.DisplayMember = "TENKH";
            cmbTenPhongBan.ValueMember = "TENSERVER";
            cmbTenPhongBan.SelectedIndex = Program.mChinhanh;
            if (Program.mGroup == "PGV")
            {
                cmbTenPhongBan.Enabled = true;
                cmbChucVu.SelectedIndex = 0;
            }
            if(Program.mGroup == "Khoa")
            {
                cmbTenPhongBan.Enabled = false;
                cmbChucVu.Enabled = false;
                cmbChucVu.SelectedIndex = 1;
            }
            if (Program.mGroup == "PKeToan")
            {
                cmbTenPhongBan.Enabled = false;
                cmbChucVu.Enabled = false;
                cmbChucVu.SelectedIndex = 2;
            }

            Program.bds_dspm.Filter = "";
        }

        private void mAGVComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

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

        private void btnTaoTaiKhoan_Click(object sender, EventArgs e)
        {
            if (kiemTraTruocKhiGhi())
            {
                try
                {
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
                    sqlCommand.CommandText = "SP_TAOLOGIN";
                    sqlCommand.Parameters.Add("@LGNAME", SqlDbType.VarChar, 50).Value = txtUsername.Text;
                    sqlCommand.Parameters.Add("@PASS", SqlDbType.VarChar, 50).Value = txtPass.Text;
                    sqlCommand.Parameters.Add("@USERNAME", SqlDbType.VarChar, 50).Value = cmbMaGV.Text;
                    sqlCommand.Parameters.Add("@ROLE", SqlDbType.VarChar, 50).Value = cmbChucVu.Text;
                    sqlCommand.Parameters.Add(returnValue);
                    sqlCommand.ExecuteNonQuery();
                    int check = (int)returnValue.Value;
                    if (check == 1)
                    {
                        MessageBox.Show("Tên tài khoản bị trùng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        con.Close();
                        return;
                    }
                    if (check == 2)
                    {
                        MessageBox.Show("Giảng viên này đã có tài khoản!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        con.Close();
                        return;
                    }
                    if (check == 0)
                    {
                        MessageBox.Show("Tạo tài khoản thành công!");
                        con.Close();
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: "+ex);
                }
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn thật sự muốn thoát khỏi form?", "", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                this.Close();
            }
        }

        private bool kiemTraTruocKhiGhi()
        {
            string username = txtUsername.Text;
            if (username == "")
            {
                MessageBox.Show("Tên tài khoản không được trống");
                txtUsername.Focus();
                return false;
            }
            if (username.Length > 50)
            {
                MessageBox.Show("Tên tài khoản tối đa 50 ký tự!");
                txtUsername.Focus();
                return false;
            }
            string pass = txtPass.Text;
            if(pass == "")
            {
                MessageBox.Show("Mật khẩu không được để trống");
                txtUsername.Focus();
                return false;
            }
            if (pass.Length > 50)
            {
                MessageBox.Show("Mật khẩu tối đa 50 ký tự!");
                txtUsername.Focus();
                return false;
            }

            return true;
        }
    }
}