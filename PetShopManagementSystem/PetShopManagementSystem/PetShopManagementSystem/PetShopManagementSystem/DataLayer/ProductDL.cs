using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using TransferObject;

namespace DataLayer
{
    public class ProductDL
    {
        private DbConnect db = new DbConnect();

        // Phương thức AddProduct để thêm một sản phẩm mới vào cơ sở dữ liệu
        public void AddProduct(ProductInfo p)
        {
            // Câu truy vấn SQL để thêm sản phẩm vào bảng tbProduct
            using (SqlCommand cm = new SqlCommand("INSERT INTO tbProduct(pname, ptype, pcategory, pqty, pprice) VALUES(@pname, @ptype, @pcategory, @pqty, @pprice)", db.MyConnection))
            {
                cm.Parameters.AddWithValue("@pname", p.PName);
                cm.Parameters.AddWithValue("@ptype", p.PType);
                cm.Parameters.AddWithValue("@pcategory", p.PCategory);
                cm.Parameters.AddWithValue("@pqty", p.PQty);
                cm.Parameters.AddWithValue("@pprice", p.PPrice);

                db.MyConnection.Open();
                cm.ExecuteNonQuery();
                db.MyConnection.Close();
            }
        }

        // Phương thức UpdateProduct để cập nhật thông tin sản phẩm trong cơ sở dữ liệu
        public void UpdateProduct(ProductInfo p)
        {
            // Câu truy vấn SQL để cập nhật thông tin sản phẩm trong bảng tbProduct
            using (SqlCommand cm = new SqlCommand("UPDATE tbProduct SET pname=@pname, ptype=@ptype, pcategory=@pcategory, pqty=@pqty, pprice=@pprice WHERE pcode=@pcode", db.MyConnection))
            {
                cm.Parameters.AddWithValue("@pname", p.PName);
                cm.Parameters.AddWithValue("@ptype", p.PType);
                cm.Parameters.AddWithValue("@pcategory", p.PCategory);
                cm.Parameters.AddWithValue("@pqty", p.PQty);
                cm.Parameters.AddWithValue("@pprice", p.PPrice);
                cm.Parameters.AddWithValue("@pcode", p.PCode);

                db.MyConnection.Open();
                cm.ExecuteNonQuery();
                db.MyConnection.Close();
            }
        }

        // Phương thức DeleteProduct để xóa một sản phẩm khỏi cơ sở dữ liệu
        public void DeleteProduct(string pcode)
        {
            // Câu truy vấn SQL để xóa sản phẩm theo mã sản phẩm (pcode)
            using (SqlCommand cm = new SqlCommand("DELETE FROM tbProduct WHERE pcode=@pcode", db.MyConnection))
            {
                cm.Parameters.AddWithValue("@pcode", pcode); // Thêm mã sản phẩm vào câu truy vấn

                db.MyConnection.Open();
                cm.ExecuteNonQuery();
                db.MyConnection.Close();
            }
        }

        // Phương thức GetProducts để lấy danh sách sản phẩm theo từ khóa tìm kiếm
        public List<ProductInfo> GetProducts(string keyword)
        {
            List<ProductInfo> list = new List<ProductInfo>(); // Khởi tạo danh sách sản phẩm

            // Câu truy vấn SQL để tìm kiếm sản phẩm dựa trên tên, loại và danh mục sản phẩm
            string query = "SELECT * FROM tbProduct WHERE CONCAT(pname, ptype, pcategory) LIKE @keyword";

            using (SqlCommand cm = new SqlCommand(query, db.MyConnection))
            {
                cm.Parameters.AddWithValue("@keyword", $"%{keyword}%"); // Thêm tham số tìm kiếm vào câu truy vấn

                db.MyConnection.Open();
                using (SqlDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        // Thêm thông tin sản phẩm vào danh sách
                        list.Add(new ProductInfo
                        {
                            PCode = dr["pcode"].ToString(),
                            PName = dr["pname"].ToString(),
                            PType = dr["ptype"].ToString(),
                            PCategory = dr["pcategory"].ToString(),
                            PQty = Convert.ToInt32(dr["pqty"]),
                            PPrice = Convert.ToDouble(dr["pprice"])
                        });
                    }
                }
                db.MyConnection.Close();
            }

            return list; // Trả về danh sách sản phẩm tìm thấy
        }
    }
}
