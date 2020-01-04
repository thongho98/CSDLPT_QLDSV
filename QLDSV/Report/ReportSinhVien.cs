using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace QLDSV.Report
{
    public partial class ReportSinhVien : DevExpress.XtraReports.UI.XtraReport
    {
        public ReportSinhVien(string malop,string tenlop)
        {
            InitializeComponent();
            ds1.EnforceConstraints = false;
            this.sP_INDANHSACHSINHVIENTableAdapter1.Connection.ConnectionString = Program.connstr;
            this.sP_INDANHSACHSINHVIENTableAdapter1.Fill(ds1.SP_INDANHSACHSINHVIEN, malop);
            lbTenLop.Text = tenlop;
        }

    }
}
