using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QLDSV
{
    public partial class frmMain : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public frmMain()
        {
            InitializeComponent();
            stripstatusMaGV.Text = "Mã GV: " + Program.mMaGV;
            stripstatusTen.Text = "Họ tên: " + Program.mHoten;
            stripstatusChucVu.Text = "Chức vụ: " + Program.mGroup;
        }

        private bool CheckExistForm(string name)
        {
            bool check = false;
            foreach (Form frm in this.MdiChildren)
            {
                if (frm.Name == name)
                {
                    check = true;
                    break;
                }
            }
            return check;
        }

        private void ActiveChildForm(string name)
        {
            foreach (Form frm in this.MdiChildren)
            {
                if (frm.Name == name)
                {
                    frm.Activate();
                    break;
                }
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            if (Program.mGroup == "PGV")
            {
                btnLop.Enabled = btnMonHoc.Enabled = btnSinhVien.Enabled = btnNhapDiem.Enabled = true;
                btnDSDongHocPhi.Enabled = false;
                btnHocPhi.Enabled = false;
            }
            if(Program.mGroup == "Khoa")
            {
                btnLop.Enabled = btnMonHoc.Enabled = btnSinhVien.Enabled = btnNhapDiem.Enabled = true;
                btnDSDongHocPhi.Enabled = false;
                btnHocPhi.Enabled = false;
            }
            if (Program.mGroup == "PKeToan")
            {
                btnLop.Enabled = btnMonHoc.Enabled = btnSinhVien.Enabled = btnNhapDiem.Enabled = false;
                btnDSSV.Enabled = btnDSThiHetMon.Enabled = btnBangDiemMon.Enabled = btnBangDiemTongKet.Enabled = btnPhieuDiem.Enabled = false;
                btnHocPhi.Enabled = true;
                
            }
        }

        private void btnLop_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!CheckExistForm("frmLop"))
            {
                frmLop frmLop = new frmLop();
                frmLop.MdiParent = this;
                frmLop.Show();
            }
            else
            {
                ActiveChildForm("frmLop");
            }
        }

        private void btnMonHoc_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!CheckExistForm("frmMonHoc"))
            {
                frmMonHoc frmMonHoc = new frmMonHoc();
                frmMonHoc.MdiParent = this;
                frmMonHoc.Show();
            }
            else
            {
                ActiveChildForm("frmMonHoc");
            }
        }

        private void btnSinhVien_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!CheckExistForm("frmSinhVien"))
            {
                frmSinhVien frmSinhVien = new frmSinhVien();
                frmSinhVien.MdiParent = this;
                frmSinhVien.Show();
            }
            else
            {
                ActiveChildForm("frmSinhVien");
            }
        }

        private void btnNhapDiem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!CheckExistForm("frmNhapDiem"))
            {
                frmNhapDiem frmNhapDiem = new frmNhapDiem();
                frmNhapDiem.MdiParent = this;
                frmNhapDiem.Show();
            }
            else
            {
                ActiveChildForm("frmNhapDiem");
            }
        }

        private void ribbonControl1_Click(object sender, EventArgs e)
        {

        }

        private void btnHocPhi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!CheckExistForm("frmHocPhi"))
            {
                frmHocPhi frmHocPhi = new frmHocPhi();
                frmHocPhi.MdiParent = this;
                frmHocPhi.Show();
            }
            else
            {
                ActiveChildForm("frmHocPhi");
            }
        }

        private void btnDSSV_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!CheckExistForm("frmReportSinhVien"))
            {
                frmReportSinhVien frmReportSinhVien = new frmReportSinhVien();
                frmReportSinhVien.MdiParent = this;
                frmReportSinhVien.Show();
            }
            else
            {
                ActiveChildForm("frmReportSinhVien");
            }
        }

        private void btnDSThiHetMon_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!CheckExistForm("frmReportDSThiHetMon"))
            {
                frmReportDSThiHetMon frmReportDSThiHetMon = new frmReportDSThiHetMon();
                frmReportDSThiHetMon.MdiParent = this;
                frmReportDSThiHetMon.Show();
            }
            else
            {
                ActiveChildForm("frmReportDSThiHetMon");
            }
        }

        private void btnBangDiemMon_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!CheckExistForm("frmReportBangDiemMonHoc"))
            {
                frmReportBangDiemMonHoc frmReportBangDiemMonHoc = new frmReportBangDiemMonHoc();
                frmReportBangDiemMonHoc.MdiParent = this;
                frmReportBangDiemMonHoc.Show();
            }
            else
            {
                ActiveChildForm("frmReportBangDiemMonHoc");
            }
        }

        private void btnDSDongHocPhi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!CheckExistForm("frmReportDSDongHocPhi"))
            {
                frmReportDSDongHocPhi frmReportDSDongHocPhi = new frmReportDSDongHocPhi();
                frmReportDSDongHocPhi.MdiParent = this;
                frmReportDSDongHocPhi.Show();
            }
            else
            {
                ActiveChildForm("frmReportDSDongHocPhi");
            }
        }

        private void btnPhieuDiem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!CheckExistForm("frmReportPhieuDiem"))
            {
                frmReportPhieuDiem frmReportPhieuDiem = new frmReportPhieuDiem();
                frmReportPhieuDiem.MdiParent = this;
                frmReportPhieuDiem.Show();
            }
            else
            {
                ActiveChildForm("frmReportPhieuDiem");
            }
        }

        private void btnBangDiemTongKet_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!CheckExistForm("frmReportBangDiemTongKet"))
            {
                frmReportBangDiemTongKet frmReportBangDiemTongKet = new frmReportBangDiemTongKet();
                frmReportBangDiemTongKet.MdiParent = this;
                frmReportBangDiemTongKet.Show();
            }
            else
            {
                ActiveChildForm("frmReportBangDiemTongKet");
            }
        }

        private void btnDangXuat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
            Program.bds_dspm.Filter = "";
            Program.frmDangNhap.Visible = true;

        }

        private void btnTaoTaiKhoan_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!CheckExistForm("frmTaoTaiKhoan"))
            {
                frmTaoTaiKhoan frmTaoTaiKhoan = new frmTaoTaiKhoan();
                frmTaoTaiKhoan.MdiParent = this;
                frmTaoTaiKhoan.Show();
            }
            else
            {
                ActiveChildForm("frmTaoTaiKhoan");
            }
        }

        private void btnGiangVien_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!CheckExistForm("frmGiangVien"))
            {
                frmGiangVien frmGiangVien = new frmGiangVien();
                frmGiangVien.MdiParent = this;
                frmGiangVien.Show();
            }
            else
            {
                ActiveChildForm("frmGiangVien");
            }
        }
    }
}
