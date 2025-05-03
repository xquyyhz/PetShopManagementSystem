using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferObject
{
    public class ProductInfo
    {
        // Các thuộc tính liên quan đến thông tin sản phẩm
        public string PCode { get; set; } // Mã sản phẩm
        public string PName { get; set; } // Tên sản phẩm
        public string PType { get; set; } // Loại sản phẩm
        public string PCategory { get; set; } // Danh mục sản phẩm
        public int PQty { get; set; } // Số lượng sản phẩm trong kho
        public double PPrice { get; set; } // Giá của sản phẩm
    }
}
