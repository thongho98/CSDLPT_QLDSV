using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace QLDSV.Report
{
    public partial class ReportBangDiemTongKet : DevExpress.XtraReports.UI.XtraReport
    {
        public ReportBangDiemTongKet(string malop)
        {
            InitializeComponent();
            ds1.EnforceConstraints = false;
            this.sP_INDANHSACHBANGDIEMTONGKETTableAdapter.Connection.ConnectionString = Program.connstr;
            this.sP_INDANHSACHBANGDIEMTONGKETTableAdapter.Fill(ds1.SP_INDANHSACHBANGDIEMTONGKET, malop);
            lbLop.Text = malop;
            
        }

    }
}
