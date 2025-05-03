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
    public class UserDL
    {
        private DbConnect db;

        // Constructor để khởi tạo đối tượng DbConnect và kết nối cơ sở dữ liệu
        public UserDL()
        {
            db = new DbConnect();
        }

        // Phương thức GetUsers để lấy danh sách người dùng từ cơ sở dữ liệu, có thể tìm kiếm theo từ khóa
        public List<AccountUser> GetUsers(string keyword = "")
        {
            List<AccountUser> list = new List<AccountUser>(); // Danh sách chứa các đối tượng AccountUser

            // Câu truy vấn SQL để tìm kiếm người dùng theo tên, địa chỉ, số điện thoại hoặc vai trò
            string sql = "SELECT * FROM tbUser WHERE name + address + phone + role LIKE @keyword";
            SqlCommand cmd = new SqlCommand(sql, db.MyConnection);

            // Thêm tham số vào câu truy vấn để tìm kiếm từ khóa trong các trường (name, address, phone, role)
            cmd.Parameters.AddWithValue("@keyword", "%" + keyword + "%");

            db.Connect();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                // Thêm người dùng vào danh sách với thông tin lấy từ cơ sở dữ liệu
                list.Add(new AccountUser
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Name = reader["name"].ToString(),
                    Address = reader["address"].ToString(),
                    Phone = reader["phone"].ToString(),
                    Role = reader["role"].ToString(),
                    Dob = Convert.ToDateTime(reader["dob"]),
                    Password = reader["password"].ToString()
                });
            }
            reader.Close();
            db.DisConnect();

            return list; // Trả về danh sách người dùng tìm thấy
        }

        // Phương thức InsertUser để thêm một người dùng mới vào cơ sở dữ liệu
        public int InsertUser(AccountUser user)
        {
            string sql = "INSERT INTO tbUser(name, address, phone, role, dob, password) VALUES (@name, @address, @phone, @role, @dob, @password)";
            SqlCommand cmd = new SqlCommand(sql, db.MyConnection);

            cmd.Parameters.AddWithValue("@name", user.Name);
            cmd.Parameters.AddWithValue("@address", user.Address);
            cmd.Parameters.AddWithValue("@phone", user.Phone);
            cmd.Parameters.AddWithValue("@role", user.Role);
            cmd.Parameters.AddWithValue("@dob", user.Dob);
            cmd.Parameters.AddWithValue("@password", user.Password);

            try
            {
                db.Connect();
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding user: " + ex.Message);
            }
            finally
            {
                db.DisConnect();
            }
        }

        // Phương thức DeleteUser để xóa một người dùng khỏi cơ sở dữ liệu
        public int DeleteUser(int id)
        {
            string sql = "DELETE FROM tbUser WHERE id = @id"; // Câu truy vấn SQL DELETE
            SqlCommand cmd = new SqlCommand(sql, db.MyConnection); // Tạo câu lệnh SQL DELETE

            // Thêm tham số id vào câu truy vấn
            cmd.Parameters.AddWithValue("@id", id);

            db.Connect();
            int result = cmd.ExecuteNonQuery(); // Thực thi câu lệnh DELETE và lấy số dòng bị ảnh hưởng
            db.DisConnect();

            return result; // Trả về số dòng bị ảnh hưởng (số người dùng bị xóa)
        }

        // Phương thức UpdateUser để cập nhật thông tin người dùng trong cơ sở dữ liệu
        public int UpdateUser(AccountUser user)
        {
            string sql = "UPDATE tbUser SET name=@name, address=@address, phone=@phone, role=@role, dob=@dob, password=@password WHERE id=@id";
            SqlCommand cmd = new SqlCommand(sql, db.MyConnection);
            cmd.Parameters.AddWithValue("@name", user.Name);
            cmd.Parameters.AddWithValue("@address", user.Address);
            cmd.Parameters.AddWithValue("@phone", user.Phone);
            cmd.Parameters.AddWithValue("@role", user.Role);
            cmd.Parameters.AddWithValue("@dob", user.Dob);
            cmd.Parameters.AddWithValue("@password", user.Password);
            cmd.Parameters.AddWithValue("@id", user.Id); // Thêm id để xác định người dùng cần cập nhật

            db.Connect();
            int result = cmd.ExecuteNonQuery(); // Thực thi câu lệnh UPDATE và lấy số dòng bị ảnh hưởng
            db.DisConnect();

            return result; // Trả về số dòng bị ảnh hưởng (số người dùng bị cập nhật)
        }
    }
}
