using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferObject
{
    public class Revenue
    {
        // Lớp đại diện cho doanh thu
        public DateTime Date { get; set; } // Ngày của giao dịch doanh thu
        public string Transaction { get; set; } // Mã giao dịch
        public string CustomerName { get; set; } // Tên khách hàng
        public int Qty { get; set; } // Số lượng sản phẩm bán ra
        public decimal TotalAmount { get; set; } // Tổng doanh thu của giao dịch
        public string Cashier { get; set; } // Tên thu ngân thực hiện giao dịch
    }

    // Lớp đại diện cho biểu đồ 
    public class Chart
    {
        public string Type { get; set; } // Loại biểu đồ
        public decimal Revenue { get; set; } // Doanh thu theo loại
    }
}
