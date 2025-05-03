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
    public class LoginDL
    {
        private DbConnect db;

        // Constructor để khởi tạo đối tượng DbConnect
        public LoginDL()
        {
            db = new DbConnect();
        }

        // Phương thức Login dùng để xác thực người dùng với tên đăng nhập và mật khẩu
        public AccountUser Login(string username, string password)
        {
            // Câu truy vấn SQL để lấy thông tin người dùng từ bảng tbUser
            string sql = "SELECT * FROM tbUser WHERE name = @name AND password = @password";
            AccountUser user = null; // Biến để lưu thông tin người dùng

            try
            {
                db.Connect();

                using (SqlCommand cmd = new SqlCommand(sql, db.MyConnection)) 
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@name", username); // Thêm tham số tên người dùng vào câu truy vấn
                    cmd.Parameters.AddWithValue("@password", password); // Thêm tham số mật khẩu vào câu truy vấn

                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read()) // Nếu có dữ liệu trả về (người dùng hợp lệ)
                    {
                        // Khởi tạo đối tượng AccountUser với thông tin từ cơ sở dữ liệu
                        user = new AccountUser(username, password)
                        {
                            Id = Convert.ToInt32(dr["id"]),
                            Name = dr["name"].ToString(),
                            Address = dr["address"].ToString(),
                            Phone = dr["phone"].ToString(),
                            Role = dr["role"].ToString(),
                            Dob = Convert.ToDateTime(dr["dob"]),
                            Password = dr["password"].ToString()
                        };
                    }
                    dr.Close();
                }

                db.DisConnect();
                return user; // Trả về đối tượng AccountUser chứa thông tin người dùng
            }
            catch (Exception ex)
            {
                db.DisConnect();
                throw ex;
            }
        }
    }
}
