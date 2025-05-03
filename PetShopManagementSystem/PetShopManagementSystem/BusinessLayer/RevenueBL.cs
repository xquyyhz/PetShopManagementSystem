using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferObject;
using DataLayer;
using System.Data;

namespace BusinessLayer
{
    public class RevenueBL
    {
        // Tạo đối tượng RevenueDL để truy cập các phương thức xử lý dữ liệu doanh thu từ cơ sở dữ liệu
        private RevenueDL dl = new RevenueDL();

        // Phương thức GetDailyRevenueByMonth để lấy doanh thu hàng ngày theo tháng
        public List<Revenue> GetDailyRevenueByMonth(int month)
        {
            return dl.GetDailyRevenueByMonth(month);
        }

        // Phương thức GetRevenueByProductTypeAndMonth để lấy doanh thu theo loại sản phẩm theo tháng
        public List<Chart> GetRevenueByProductTypeAndMonth(int month)
        {
            return dl.GetRevenueByProductTypeAndMonth(month);
        }

        public decimal GetTotalRevenueByMonth(int month)
        {
            return dl.GetTotalRevenueByMonth(month);
        }
    }
}
