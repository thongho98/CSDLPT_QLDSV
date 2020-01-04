using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace QLDSV.Report
{
    public partial class ReportDSDongHocPhi : DevExpress.XtraReports.UI.XtraReport
    {
        public ReportDSDongHocPhi(string malop, string nienkhoa, int hocky)
        {
            InitializeComponent();
            ds1.EnforceConstraints = false;
            this.sP_INDANHSACHHOCPHITableAdapter.Connection.ConnectionString = Program.connstr;
            this.sP_INDANHSACHHOCPHITableAdapter.Fill(ds1.SP_INDANHSACHHOCPHI, malop,nienkhoa, hocky);
            lbMaLop.Text = malop;
            lbNienKhoa.Text = nienkhoa;
        }

    }
}
