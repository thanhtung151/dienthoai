using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using dienthoai.Models;

namespace dienthoai.Controllers
{
    public class NguoidungController : Controller
    {
        DbqldienthoaiDataContext data = new DbqldienthoaiDataContext();
        // GET: Nguoidung
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Dangky()
        {
            return View();
        }
        //post:Hàm đăng ký
        [HttpPost]
     public ActionResult Dangky(FormCollection collection, KhachHang kHACHHANG)
        {
            var hoten = collection["HotenKH"];
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];
            var nhaplaimatkhau = collection["Nhaplaimatkhau"];
            var diachi = collection["Diachi"];
            var email = collection["Email"];
            var dienthoai = collection["Dienthoai"];
            
            if (string.IsNullOrEmpty(hoten))
            {
                ViewData["loi1"] = " Họ Tên Không Được Để Trống";
            }
            else if(string.IsNullOrEmpty(tendn))
            {
                ViewData["loi2"] = "Phải Nhập Tên Đăng Nhập ";
            }
            else if (string.IsNullOrEmpty(matkhau))
            {
                ViewData["loi3"] = "Phải Nhập Mật Khẩu ";
            }
            else if (string.IsNullOrEmpty(nhaplaimatkhau))
            {
                ViewData["loi4"] = "Phải Nhập Lại Mật Khẩu ";
            }
            if(string.IsNullOrEmpty(email))
            {
                ViewData["loi5"] = "Email không được bỏ trống";
            }
            if (string.IsNullOrEmpty(dienthoai))
            {
                ViewData["loi6"] = "Phải nhập điện thọi";
            }
            else
            {
                //Gán giá trị cho đối tượng mới (kh)
                kHACHHANG.Ten_KH = hoten;
                kHACHHANG.Ten_DangNhapKH = tendn;
                kHACHHANG.Matkhau = matkhau;
                kHACHHANG.Email_KH = email;
                kHACHHANG.Dia_Chi = diachi;
                kHACHHANG.SDT = dienthoai;
                data.KhachHangs.InsertOnSubmit(kHACHHANG);
                data.SubmitChanges();
                return RedirectToAction("DANGNHAP");

                                        
            }
            return this.Dangky();


        }
        [HttpGet]
        public ActionResult Dangnhap()
        {
            return View();
        }
        public ActionResult Dangnhap(FormCollection collection)
        {
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];
            if (string.IsNullOrEmpty(tendn))
            {
                ViewData["loi1"] = "Phải nhập tên đăng nhập";
            }
            else if (string.IsNullOrEmpty(matkhau))
            {
                ViewData["loi2"] = "Phải nhập mật khẩu";
            }
            else
            {
                KhachHang khachHang = data.KhachHangs.SingleOrDefault(n => n.Ten_DangNhapKH == tendn && n.Matkhau == matkhau);
                if (khachHang != null)
                {
                    ViewBag.Thongbao = "Đăng nhập thành công";
                    Session["Ten_dangnhapKH"] = khachHang;
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();

        }
    }
}