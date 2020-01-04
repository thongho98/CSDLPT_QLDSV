using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace QLDSV.Report
{
    public partial class ReportDSThiHetMon : DevExpress.XtraReports.UI.XtraReport
    {
        public ReportDSThiHetMon(string malop, string tenlop, string monhoc, string ngaythi)
        {
            InitializeComponent();
            ds1.EnforceConstraints = false;
            this.sP_INDANHSACHTHIHETMONTableAdapter.Connection.ConnectionString = Program.connstr;
            this.sP_INDANHSACHTHIHETMONTableAdapter.Fill(ds1.SP_INDANHSACHTHIHETMON, malop);
            lbLop.Text = tenlop;
            lbMonHoc.Text = monhoc;
            lbNgayThi.Text = ngaythi;
        }

    }
}
