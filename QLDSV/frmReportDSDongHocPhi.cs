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
    public partial class frmReportDSDongHocPhi : DevExpress.XtraEditors.XtraForm
    {
        public frmReportDSDongHocPhi()
        {
            InitializeComponent();
        }

        private void frmReportDSDongHocPhi_Load(object sender, EventArgs e)
        {
            dS.EnforceConstraints = false;           
            // TODO: This line of code loads data into the 'dS.LOP' table. You can move, or remove it, as needed.
            //this.lOPTableAdapter.Connection.ConnectionString = Program.connstr;
            //this.lOPTableAdapter.Fill(this.dS.LOP);

            cmbHocKy.SelectedIndex = 0;
        }

        private void lOPBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bdsLop.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dS);

        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            if(txtMaLop.Text == "")
            {
                MessageBox.Show("Mã lớp không được trống!");
                return;
            }
            
            if (txtNienKhoa.Text == "")
            {
                MessageBox.Show("Niên khóa không được trống!");
                return;
            }

            //Bat ma lop ton tai!, nien khoa dinh dang sai

            Report.ReportDSDongHocPhi report = new Report.ReportDSDongHocPhi(txtMaLop.Text, txtNienKhoa.Text,int.Parse(cmbHocKy.Text));
            ReportPrintTool printTool = new ReportPrintTool(report);
            printTool.ShowPreview();
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