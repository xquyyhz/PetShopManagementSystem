using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using TransferObject;

namespace BusinessLayer
{
    public class ProductBL
    {
        // Tạo đối tượng ProductDL để truy cập dữ liệu từ cơ sở dữ liệu
        private ProductDL dl = new ProductDL();

        // Phương thức AddProduct để thêm một sản phẩm mới vào cơ sở dữ liệu
        public void AddProduct(ProductInfo p)
        {
            dl.AddProduct(p);
        }

        // Phương thức UpdateProduct để cập nhật thông tin của sản phẩm trong cơ sở dữ liệu
        public void UpdateProduct(ProductInfo p)
        {
            dl.UpdateProduct(p);
        }

        // Phương thức DeleteProduct để xóa sản phẩm khỏi cơ sở dữ liệu theo mã sản phẩm
        public void DeleteProduct(string pcode)
        {
            dl.DeleteProduct(pcode);
        }

        // Phương thức GetProducts để lấy danh sách sản phẩm từ cơ sở dữ liệu theo từ khóa tìm kiếm
        public List<ProductInfo> GetProducts(string keyword)
        {
            return dl.GetProducts(keyword);
        }
    }
}
