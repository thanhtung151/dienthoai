using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using dienthoai.Models;


namespace dienthoai.Models
{
    public class Giohang
    {
        DbqldienthoaiDataContext data = new DbqldienthoaiDataContext();
        public int iMa_SP { set; get; }
        public string sTen_SP { set; get; }
        public string sHinh_Anh { set; get; }
        public double dDon_Gia { set; get; }
        public int isoluong { set; get; }
        public double dThanhtien
        {
            get { return isoluong * dDon_Gia; }
        }
        public Giohang(int Ma_SP)
        {
            iMa_SP = Ma_SP;
            sanpham sanpham = data.sanphams.Single(n => n.Ma_SP == iMa_SP);
            sTen_SP = sanpham.Ten_SP;
            sHinh_Anh = sanpham.Hinh_Anh; ;

            dDon_Gia = double.Parse(sanpham.Don_Gia.ToString());
            isoluong = 1;
        }
    }
}