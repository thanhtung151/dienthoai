using dienthoai.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.IO;

namespace dienthoai.Controllers
{
    public class AdminController : Controller
    {
        DbqldienthoaiDataContext data = new DbqldienthoaiDataContext();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult sanpham(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 5;

            return View(data.sanphams.ToList().OrderBy(n => n.Ma_SP).ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        //[HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            DbqldienthoaiDataContext data = new DbqldienthoaiDataContext();

            // gan gia tri
            var tendn = collection["username"];
            var matkhau = collection["password"];
            if (string.IsNullOrEmpty(tendn))
            {
                ViewData["loi1"] = "Phải nhập tên đăng nhập";
            }
            if (string.IsNullOrEmpty(matkhau))
            {
                ViewData["loi2"] = "Phải nhập mật khẩu";
            }
            else
            {
                //gắn giá trị tạo mới (ad)
                Admin ad = data.Admins.SingleOrDefault(n => n.Ten_Admin == tendn && n.Mat_Khau == matkhau);
                if (ad != null)
                {
                    //viewbag.Thongbao = "Chúc mừng đăng nhập thành công"
                    Session["TaikhoanAdmin"] = ad;
                    return RedirectToAction("Index", "Admin");
                }
                else
                {

                    ViewBag.Thongbao1 = "Tên đăng nhập hoặc mật khẩu không đúng";
                }
            }
            return View();
        }
        [HttpGet]
        public ActionResult Themmoisanpham()
        {
            ViewBag.Ma_Loai_SP = new SelectList(data.loaisanphams.ToList().OrderBy(n => n.Ten_Loai_SP), "Ma_Loai_SP", "Ten_Loai_SP");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themmoisanpham(sanpham sanpham, HttpPostedFileBase fileupload)

        {
            ViewBag.Ma_Loai_SP = new SelectList(data.loaisanphams.ToList().OrderBy(n => n.Ten_Loai_SP), "Ma_Loai_SP", "Ten_Loai_SP");
            if (fileupload == null)
            {
                ViewBag.Thongbao = "Vui Lòng Chọn Hình Ảnh";
                return View();
            }

            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileupload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/image"), fileName);
                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Hình Ảnh Đã Tồn Tại";
                    //else
                    //{
                    //    fileupload.SaveAs(path);
                    //}
                    sanpham.Hinh_Anh = fileName;
                    data.sanphams.InsertOnSubmit(sanpham);
                    data.SubmitChanges();
                }
                return RedirectToAction("sanpham");
            }


        }




        //xóa sản phẩm
        [HttpGet]
        public ActionResult Xoasanpham(int id)
        {
            sanpham sanpham = data.sanphams.SingleOrDefault(n => n.Ma_SP == id);
            ViewBag.Ma_SP = sanpham.Ma_SP;
            if (sanpham == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sanpham);
        }
        public ActionResult chitietsanpham(int id)
        {
            sanpham sanpham = data.sanphams.SingleOrDefault(n => n.Ma_SP == id);
            if (sanpham == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sanpham);
        }
        [HttpPost, ActionName("Xoasanpham")]
        public ActionResult Xacnhanxoa(int id)
        {
            sanpham sanpham = data.sanphams.SingleOrDefault(n => n.Ma_SP == id);
            ViewBag.Ma_SP = sanpham.Ma_SP;
            if (sanpham == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.sanphams.DeleteOnSubmit(sanpham);
            data.SubmitChanges();
            return RedirectToAction("sanpham");
        }
        //[HttpGet]    
        public ActionResult Suasanpham(int id)
        {
            if (Session["TaikhoanAdmin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                sanpham sanpham = data.sanphams.SingleOrDefault(n => n.Ma_SP == id);
                //Lay du liệu tư table Chude để đổ vào Dropdownlist, kèm theo chọn MaCD tương tưng 
                ViewBag.Ma_Loai_SP = new SelectList(data.loaisanphams.ToList().OrderBy(n => n.Ten_Loai_SP), "Ma_Loai_SP", "Ten_Loai_SP", sanpham.Ma_Loai_SP);
                
                return View(sanpham);
            }
        }

        [HttpPost, ActionName("Suasanpham")]
        public ActionResult Suasanpham(int id, HttpPostedFileBase fileUpload)
        {
            if (Session["TaikhoanAdmin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                sanpham sanpham = data.sanphams.SingleOrDefault(n => n.Ma_SP == id);
                ViewBag.Ma_Loai_SP = new SelectList(data.loaisanphams.ToList().OrderBy(n => n.Ten_Loai_SP), "Ma_Loai_SP", "Ten_Loai_SP");
                
                //Kiem tra duong dan file
                if (fileUpload == null)
                {
                    ViewBag.Thongbao1 = "Vui lòng chọn ảnh";
                    return View(sanpham);
                }
                //Them vao CSDL
                else
                {
                    if (ModelState.IsValid)
                    {
                        //Luu ten fie, luu y bo sung thu vien using System.IO;
                        var fileName = Path.GetFileName(fileUpload.FileName);

                        //Luu duong dan cua file
                        var path = Path.Combine(Server.MapPath("~/Content/image/"), fileName);

                        //Kiem tra hình anh ton tai chua?
                        if (System.IO.File.Exists(path))
                            ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                        else
                        {
                            //Luu hinh anh vao duong dan
                            fileUpload.SaveAs(path);

                        }
                        sanpham.Hinh_Anh = fileName;
                        //Luu vao CSDL
                        UpdateModel(sanpham);
                        data.SubmitChanges();
                    }
                    return RedirectToAction("Sanpham", "Admin");
                }
            }
        }
    }
}