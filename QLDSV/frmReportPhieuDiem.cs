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
using DevExpress.XtraReports.UI;

namespace QLDSV
{
    public partial class frmReportPhieuDiem : DevExpress.XtraEditors.XtraForm
    {
        public frmReportPhieuDiem()
        {
            InitializeComponent();
        }

        private void frmReportPhieuDiem_Load(object sender, EventArgs e)
        {

        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            if (txtMaSV.Text == "")
            {
                MessageBox.Show("Mã sinh viên không được trống!");
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
                Report.ReportPhieuDiem report = new Report.ReportPhieuDiem(txtMaSV.Text);
                ReportPrintTool printTool = new ReportPrintTool(report);
                printTool.ShowPreview();
            }
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