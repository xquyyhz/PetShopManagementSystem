using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferObject
{
    public class CashInfo
    {
        // Các thuộc tính liên quan đến giao dịch thanh toán
        public int CashId { get; set; } // Mã id của giao dịch thanh toán
        public string TransNo { get; set; } // Mã số giao dịch
        public string PCode { get; set; } // Mã sản phẩm
        public string PName { get; set; } // Tên sản phẩm
        public int Qty { get; set; } // Số lượng sản phẩm
        public double Price { get; set; } // Giá của mỗi sản phẩm
        public double Total { get; set; } // Tổng tiền giao dịch
        public string Cashier { get; set; } // Tên thu ngân thực hiện giao dịch
        public int? CustomerId { get; set; } // ID khách hàng (có thể null nếu không có khách hàng)
        public string CustomerName { get; set; } // Tên khách hàng
    }
}
