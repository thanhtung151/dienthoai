using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using dienthoai.Models;
using PagedList;
using PagedList.Mvc;

namespace dienthoai.Controllers
{
    public class PHONEController : Controller
    {
        // Tạo 1 đối tượng chứa toàn bộ CSDL từ dbqldienthoai
        DbqldienthoaiDataContext data = new DbqldienthoaiDataContext();

        private List<sanpham> laysanpham(int count)
        {
            return data.sanphams.OrderByDescending(a => a.Ma_SP).Take(count).ToList();
        }


        public ActionResult Index(int? page, string searchString)
        {
            ViewBag.Keyword = searchString;

            int pageSize = 6;
            int pageNum = (page ?? 1);
            //lấy 5 sản phẩm 
            //var sanpham = laysanpham(60);

            //return View(sanpham.ToPagedList(pageNum, pageSize));
            return View(sanpham.GetAll(searchString).ToPagedList(pageNum, pageSize));

        }

       






        //}
        public ActionResult loaisanpham()
        {
            var loaisanpham = from cd in data.loaisanphams select cd;
            return PartialView(loaisanpham);
        }
        //public ActionResult nhacungcap()
        //{
        //    var nhacungcap = from cd in data.nhacungcaps select cd;
        //    return PartialView(nhacungcap);

        //}
        public ActionResult SPTheoloaisanpham(int id)
        {
            var sanpham = from s in data.sanphams where s.Ma_Loai_SP==id select s;
           
            return View(sanpham);
        }
        public ActionResult Details(int id)
        {
            var sanpham = from s in data.sanphams
                          where s.Ma_SP == id
                          select s;
            return View(sanpham.Single());
        }
        
    }
}