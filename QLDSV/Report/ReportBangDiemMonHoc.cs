using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace QLDSV.Report
{
    public partial class ReportBangDiemMonHoc : DevExpress.XtraReports.UI.XtraReport
    {
        public ReportBangDiemMonHoc(string malop,string tenlop,string mamon,string tenmon,int lanthi)
        {
            InitializeComponent();
            ds1.EnforceConstraints = false;
            this.sP_INDIEMMONHOCTableAdapter.Connection.ConnectionString = Program.connstr;
            this.sP_INDIEMMONHOCTableAdapter.Fill(ds1.SP_INDIEMMONHOC, malop,mamon,lanthi);
            lbLop.Text = tenlop;
            lbMon.Text = tenmon;
            lbLanThi.Text = lanthi.ToString();
        }

    }
}
