using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using TransferObject;
using System.Data.SqlClient;

namespace DataLayer
{
    public class CustomerDL
    {
        private DbConnect db = new DbConnect();

        // Phương thức AddCustomer dùng để thêm khách hàng vào cơ sở dữ liệu
        public void AddCustomer(CustomerInfo customer)
        {
            // Câu truy vấn SQL để thêm thông tin khách hàng vào bảng tbCustomer
            using (SqlCommand cm = new SqlCommand("INSERT INTO tbCustomer(name,address,phone) VALUES(@name,@address,@phone)", db.MyConnection))
            {
                cm.Parameters.AddWithValue("@name", customer.Name);
                cm.Parameters.AddWithValue("@address", customer.Address);
                cm.Parameters.AddWithValue("@phone", customer.Phone);

                // Mở kết nối đến cơ sở dữ liệu
                db.MyConnection.Open();
                cm.ExecuteNonQuery(); // Thực thi câu truy vấn INSERT
                db.MyConnection.Close();
            }
        }

        // Phương thức UpdateCustomer dùng để cập nhật thông tin khách hàng
        public void UpdateCustomer(CustomerInfo customer)
        {
            // Câu truy vấn SQL để cập nhật thông tin khách hàng trong bảng tbCustomer
            using (SqlCommand cm = new SqlCommand("UPDATE tbCustomer SET name=@name, address=@address, phone=@phone WHERE id=@id", db.MyConnection))
            {
                // Thêm các tham số vào câu truy vấn để tránh SQL Injection
                cm.Parameters.AddWithValue("@id", customer.Id);
                cm.Parameters.AddWithValue("@name", customer.Name);
                cm.Parameters.AddWithValue("@address", customer.Address);
                cm.Parameters.AddWithValue("@phone", customer.Phone);

                db.MyConnection.Open();
                cm.ExecuteNonQuery(); // Thực thi câu truy vấn UPDATE
                db.MyConnection.Close();
            }
        }

        // Phương thức DeleteCustomer dùng để xóa khách hàng khỏi cơ sở dữ liệu theo ID
        public void DeleteCustomer(int id)
        {
            // Câu truy vấn SQL để xóa thông tin khách hàng trong bảng tbCustomer
            using (SqlCommand cm = new SqlCommand("DELETE FROM tbCustomer WHERE id=@id", db.MyConnection))
            {
                cm.Parameters.AddWithValue("@id", id); // Tham số @id sẽ được thay thế bằng giá trị id

                db.MyConnection.Open();
                cm.ExecuteNonQuery(); // Thực thi câu lệnh DELETE, xóa khách hàng có id tương ứng
                db.MyConnection.Close();
            }
        }

        // Phương thức GetCustomers dùng để tìm kiếm khách hàng theo từ khóa
        public List<CustomerInfo> GetCustomers(string keyword)
        {
            List<CustomerInfo> list = new List<CustomerInfo>(); // Khởi tạo danh sách khách hàng để lưu kết quả tìm kiếm
            string query = "SELECT * FROM tbCustomer WHERE CONCAT(name, address, phone) LIKE @keyword"; // Câu truy vấn SQL để tìm kiếm khách hàng

            using (SqlCommand cm = new SqlCommand(query, db.MyConnection))
            {
                cm.Parameters.AddWithValue("@keyword", $"%{keyword}%"); // Tham số @keyword sẽ được thay thế bằng từ khóa tìm kiếm với ký tự '%' cho tìm kiếm gần đúng
                db.MyConnection.Open();
                using (SqlDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        // Thêm đối tượng CustomerInfo vào danh sách với các giá trị lấy từ cơ sở dữ liệu
                        list.Add(new CustomerInfo
                        {
                            Id = Convert.ToInt32(dr["id"]),
                            Name = dr["name"].ToString(),
                            Address = dr["address"].ToString(),
                            Phone = dr["phone"].ToString()
                        });
                    }
                }
                db.MyConnection.Close();
            }
            // Trả về danh sách khách hàng tìm thấy
            return list;
        }
    }
}
