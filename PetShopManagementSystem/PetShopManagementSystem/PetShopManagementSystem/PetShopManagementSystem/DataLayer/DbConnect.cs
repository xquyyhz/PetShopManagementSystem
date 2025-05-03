using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace DataLayer
{
    public class DbConnect
    {
        private SqlConnection cn; // Đối tượng SqlConnection lưu trữ kết nối đến cơ sở dữ liệu

        // Constructor của lớp DbConnect, khởi tạo kết nối đến cơ sở dữ liệu
        public DbConnect()
        {
            // Chuỗi kết nối đến SQL Server với cơ sở dữ liệu PetShop
            string cnStr = "Data Source=.;Initial Catalog=PetShop;Integrated Security=True";
            cn = new SqlConnection(cnStr); // Khởi tạo đối tượng SqlConnection với chuỗi kết nối
        }

        // Phương thức Connect để mở kết nối đến cơ sở dữ liệu
        public void Connect()
        {
            try
            {
                // Kiểm tra nếu kết nối chưa mở thì mở kết nối
                if (cn != null && cn.State == ConnectionState.Closed)
                {
                    cn.Open(); // Mở kết nối
                }
            }
            catch (SqlException ex)
            {
                // Nếu xảy ra lỗi trong quá trình kết nối, ném ra ngoại lệ
                throw ex;
            }
        }

        // Phương thức DisConnect để đóng kết nối đến cơ sở dữ liệu
        public void DisConnect()
        {
            // Kiểm tra nếu kết nối đang mở thì đóng kết nối
            if (cn != null && cn.State == ConnectionState.Open)
            {
                cn.Close(); // Đóng kết nối
            }
        }

        // Thuộc tính MyConnection để lấy đối tượng kết nối SqlConnection
        public SqlConnection MyConnection
        {
            get { return cn; } // Trả về đối tượng SqlConnection hiện tại
        }

        // Phương thức MyExecuteReader thực thi câu lệnh SELECT và trả về SqlDataReader
        public SqlDataReader MyExecuteReader(string sql, CommandType type)
        {
            Connect();
            SqlCommand cmd = new SqlCommand(sql, cn); // Tạo đối tượng SqlCommand với câu lệnh SQL
            cmd.CommandType = type; // Chỉ định loại câu lệnh (SELECT, UPDATE, DELETE...)

            try
            {
                // Thực thi câu lệnh và trả về SqlDataReader để đọc dữ liệu
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        // Phương thức MyExecuteScalar thực thi câu lệnh và trả về một giá trị duy nhất
        public object MyExecuteScalar(string sql, CommandType type)
        {
            SqlCommand cmd = new SqlCommand(sql, cn);
            cmd.CommandType = type;
            Connect();

            try
            {
                // Thực thi câu lệnh và trả về giá trị duy nhất
                return cmd.ExecuteScalar();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                DisConnect(); // Đảm bảo đóng kết nối sau khi thực thi
            }
        }

        // Phương thức MyExecuteNonQuery thực thi câu lệnh UPDATE, INSERT, DELETE và trả về số lượng dòng bị ảnh hưởng
        public int MyExecuteNonQuery(string sql, CommandType type)
        {
            SqlCommand cmd = new SqlCommand(sql, cn);
            cmd.CommandType = type;
            try
            {
                Connect();
                // Thực thi câu lệnh và trả về số lượng dòng bị ảnh hưởng
                return cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                DisConnect();
            }
        }
    }
}
