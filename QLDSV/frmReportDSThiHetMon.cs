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
using DevExpress.XtraReports.UI;

namespace QLDSV
{
    public partial class frmReportDSThiHetMon : DevExpress.XtraEditors.XtraForm
    {
        public frmReportDSThiHetMon()
        {
            InitializeComponent();
        }

        private void mONHOCBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bdsMonHoc.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dS);

        }

        private void frmReportDSThiHetMon_Load(object sender, EventArgs e)
        {
            dS.EnforceConstraints = false;
            // TODO: This line of code loads data into the 'dS.LOP' table. You can move, or remove it, as needed.
            this.lOPTableAdapter.Connection.ConnectionString = Program.connstr;
            this.lOPTableAdapter.Fill(this.dS.LOP);
            // TODO: This line of code loads data into the 'dS.MONHOC' table. You can move, or remove it, as needed.
            this.mONHOCTableAdapter.Connection.ConnectionString = Program.connstr;
            this.mONHOCTableAdapter.Fill(this.dS.MONHOC);


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
            if (Program.mGroup == "PGV") cmbTenPhongBan.Enabled = true;
            else cmbTenPhongBan.Enabled = false;

            cmbLanThi.SelectedIndex = 0;
            dtpNgayThi.Format = DateTimePickerFormat.Custom;
            dtpNgayThi.CustomFormat = "dd/MM/yyyy";
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            Report.ReportDSThiHetMon report = new Report.ReportDSThiHetMon(txtMaLop.Text, cmbTenLop.Text,cmbMonHoc.Text, dtpNgayThi.Text);
            ReportPrintTool printTool = new ReportPrintTool(report);
            printTool.ShowPreview();
        }

        private void cmbTenPhongBan_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbTenPhongBan.SelectedValue.ToString() == "System.Data.DataRowView")
                return;
            Program.servername = cmbTenPhongBan.SelectedValue.ToString();
            if (Program.servername == "THONG-PC\\SERVER3")
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

        private void dtpNgayThi_ValueChanged(object sender, EventArgs e)
        {

        }

        private void cmbTenLop_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtMaLop_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn thật sự muốn thoát khỏi form?", "", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                this.Close();
            }
        }
    }
}