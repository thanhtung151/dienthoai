using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using dienthoai.Models;

namespace dienthoai.Controllers
{
    public class GiohangController : Controller
    {
        DbqldienthoaiDataContext data = new DbqldienthoaiDataContext();
        //public ActionResult Index()
        //{
        //    return View();
        //}
        // GET: Giohang
        public List<Giohang> Laygiohang()
        {
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            if (lstGiohang == null)
            {
                lstGiohang = new List<Giohang>();
                Session["Giohang"] = lstGiohang;
            }
            return lstGiohang;
        }
        //Thêm giỏ hàng
        public ActionResult ThemGiohang(int iMa_SP, string strURL)
        {
            List<Giohang> lstGiohang = Laygiohang();
            Giohang sanpham = lstGiohang.Find(n => n.iMa_SP == iMa_SP);
            if (sanpham == null)
            {
                sanpham = new Giohang(iMa_SP);
                lstGiohang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.isoluong++;
                return Redirect(strURL);
            }
        }
        //tong so luong
        private int Tongsoluong()
        {
            int iTongsoluong = 0;
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            if (lstGiohang != null)
            {
                iTongsoluong = lstGiohang.Sum(n => n.isoluong);
            }
            return iTongsoluong;
        }
        //tinh tong tien
        private double TongTien()
        {
            double iTongtien = 0;
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            if (lstGiohang != null)
            {
                iTongtien = lstGiohang.Sum(n => n.dThanhtien);
            }
            return iTongtien;
        }
        public ActionResult Giohang()
        {
            List<Giohang> lstGiohang = Laygiohang();
            if (lstGiohang.Count == 0)
            {
                return RedirectToAction("Index", "PHONE");
            }
            ViewBag.Tongsoluong = Tongsoluong();
            ViewBag.Tongtien = TongTien();
            return View(lstGiohang);

        }
        public ActionResult GiohangPartial()
        {
            ViewBag.Tongsoluong = Tongsoluong();
            ViewBag.Tongtien = TongTien();
            return PartialView();

        }
        //Xoa Giohang
        public ActionResult XoaGiohang(int iMa_SP)
        {
            //lay gio hang tu session
            List<Giohang> lstGiohang = Laygiohang();
            // kiem tra san pham trong gio hang
            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.iMa_SP == iMa_SP);
            //ton tai thi cho sua so luong
            if (sanpham != null)
            {
                lstGiohang.RemoveAll(n => n.iMa_SP == iMa_SP);
                return RedirectToAction("Giohang");
            }
            if (lstGiohang.Count == 0)
            {
                return RedirectToAction("Index", "PHONE");
            }
            return RedirectToAction("Giohang");
        }
        //cap nhat gio hang
        public ActionResult CapnhatGiohang(int iMa_SP, FormCollection f)
        {
            //lay gio hang tu session
            List<Giohang> lstGiohang = Laygiohang();
            // kiem tra san pham trong gio hang
            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.iMa_SP == iMa_SP);
            //ton tai thi cho sua so luong
            if (sanpham != null)
            {
                sanpham.isoluong = int.Parse(f["txtsoluong"].ToString());
            }
            return RedirectToAction("Giohang");
        }
        public ActionResult XoaTatcaGiohang()
        {
            List<Giohang> lstGiohang = Laygiohang();
            lstGiohang.Clear();
            return RedirectToAction("Index", "PHONE");
        }
        //hien thi thong tin de cap nhat don hang
        [HttpGet]
        public ActionResult DatHang()
        {
            if (Session["Ten_dangnhapKH"] == null || Session["Ten_dangnhapKH"].ToString() == "")
            {
                return RedirectToAction("Dangnhap", "Nguoidung");
            }
            if (Session["Giohang"] == null)
            {
                return RedirectToAction("Index", "PHONE");
            }
            //lay gio hang tu session
            List<Giohang> lstGiohang = Laygiohang();
            ViewBag.Tongsoluong = Tongsoluong();
            ViewBag.Tongtien = TongTien();
            return View(lstGiohang);
        }

        public ActionResult DatHang(FormCollection collection)
        {
            HOADON hOADON = new HOADON();
            KhachHang kh = (KhachHang)Session["Ten_DangNhapKH"];
            List<Giohang> gh = Laygiohang();
            hOADON.Ma_KH = kh.Ma_KH;
            hOADON.Ngay_Lap = DateTime.Now;
            var Ngay_Giao_Hang = string.Format("{0:MM/dd/yyyy}", collection["Ngay_Giao_Hang"]);           
            hOADON.Ngay_Giao_Hang = DateTime.Parse(Ngay_Giao_Hang);
            data.HOADONs.InsertOnSubmit(hOADON);
            data.SubmitChanges();
            //them chi tiet don hang
            foreach (var item in gh)
            {
                chitiethoadon chitiethoadon = new chitiethoadon();
                chitiethoadon.Ma_HD = hOADON.Ma_HD;
                chitiethoadon.Ma_SP = item.iMa_SP;
                chitiethoadon.So_Luong = item.isoluong;
                chitiethoadon.Don_Gia = (decimal)item.dDon_Gia;
                data.chitiethoadons.InsertOnSubmit(chitiethoadon);

            }
            data.SubmitChanges();
            Session["Giohang"] = null;
            return RedirectToAction("Xacnhandonhang", "Giohang");
        }
        public ActionResult Xacnhandonhang()
        {
            return View();
        }
    }
}
