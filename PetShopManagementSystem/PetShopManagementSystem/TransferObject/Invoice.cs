using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferObject
{
    // Lớp đại diện cho hóa đơn
    public class Invoice
    {
        public string TransNo { get; set; } // Mã giao dịch của hóa đơn
        public string CustomerName { get; set; } // Tên khách hàng
        public DateTime Date { get; set; } // Ngày tạo hóa đơn
        public string CashierName { get; set; } // Tên thu ngân thực hiện giao dịch
        public decimal Total { get; set; } // Tổng tiền của hóa đơn
        public List<InvoiceDetail> Details { get; set; } // Danh sách các chi tiết hóa đơn
    }

    // Lớp đại diện cho chi tiết hóa đơn
    public class InvoiceDetail
    {
        public string PCode { get; set; } // Mã sản phẩm
        public string PName { get; set; } // Tên sản phẩm
        public decimal Price { get; set; } // Giá của sản phẩm
        public int Qty { get; set; } // Số lượng sản phẩm
        public decimal Total { get; set; } // Tổng tiền cho sản phẩm (số lượng * giá)
    }
}
