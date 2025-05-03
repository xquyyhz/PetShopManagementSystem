using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferObject
{
    public class CustomerInfo
    {
        // Các thuộc tính liên quan đến thông tin khách hàng
        public int Id { get; set; } // Mã ID của khách hàng
        public string Name { get; set; } // Tên của khách hàng
        public string Address { get; set; } // Địa chỉ của khách hàng
        public string Phone { get; set; } // Số điện thoại của khách hàng
    }
}
