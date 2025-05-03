using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;

namespace BusinessLayer
{
    public class DashboardBL
    {
        // Tạo đối tượng DashboardDL để truy cập các phương thức lấy dữ liệu từ cơ sở dữ liệu
        private DashboardDL dl = new DashboardDL();

        // Phương thức ExtractData để lấy tổng số lượng sản phẩm theo danh mục từ tầng DataLayer
        public int ExtractData(string category)
        {
            return dl.ExtractData(category);
        }

        // Phương thức GetDailySale để lấy tổng doanh thu theo ngày từ tầng DataLayer
        public double GetDailySale(string sdate)
        {
            return dl.GetDailySale(sdate);
        }
    }
}
