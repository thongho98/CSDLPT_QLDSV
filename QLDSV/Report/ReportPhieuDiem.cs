using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace QLDSV.Report
{
    public partial class ReportPhieuDiem : DevExpress.XtraReports.UI.XtraReport
    {
        public ReportPhieuDiem(string masv)
        {
            InitializeComponent();
            ds1.EnforceConstraints = false;
            this.sP_INPHIEUDIEMTableAdapter1.Connection.ConnectionString = Program.connstr;
            this.sP_INPHIEUDIEMTableAdapter1.Fill(ds1.SP_INPHIEUDIEM, masv);
            //Do len header
            string strLenh = "EXEC SP_GETINFOSV '" + masv + "'";
            Program.myReader = Program.ExecSqlDataReader(strLenh);
            if (Program.myReader == null) return;
            Program.myReader.Read();
            lbHoTen.Text = Program.myReader.GetString(0);
            lbLop.Text = Program.myReader.GetString(1);
            Program.conn.Close();
        }

    }
}
