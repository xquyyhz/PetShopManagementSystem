using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace DataLayer
{
    public class DashboardDL
    {
        private DbConnect db = new DbConnect();

        // Phương thức ExtractData để lấy tổng số lượng sản phẩm theo danh mục
        public int ExtractData(string category)
        {
            // Khởi tạo giá trị mặc định của kết quả là 0
            int result = 0;
            try
            {
                db.Connect();
                // Câu truy vấn SQL để tính tổng số lượng sản phẩm theo danh mục
                string sql = "SELECT ISNULL(SUM(pqty),0) AS qty FROM tbProduct WHERE pcategory = @category";
                using (SqlCommand cmd = new SqlCommand(sql, db.MyConnection))
                {
                    // Thêm tham số vào câu truy vấn để tìm theo danh mục sản phẩm
                    cmd.Parameters.AddWithValue("@category", category);
                    // Thực thi câu truy vấn và lấy kết quả trả về
                    object res = cmd.ExecuteScalar();
                    if (res != null) // Kiểm tra nếu kết quả không phải null
                        result = Convert.ToInt32(res); // Chuyển kết quả về kiểu int
                }
            }
            finally
            {
                db.DisConnect();
            }
            return result;
        }

        // Phương thức GetDailySale để lấy tổng doanh thu hàng ngày theo ngày cụ thể
        public double GetDailySale(string sdate)
        {
            // Khởi tạo giá trị mặc định của kết quả là 0
            double result = 0;
            try
            {
                db.Connect();
                // Câu truy vấn SQL để tính tổng doanh thu trong ngày theo mã giao dịch bắt đầu với ngày được cung cấp
                string sql = "SELECT ISNULL(SUM(total),0) AS total FROM tbCash WHERE transno LIKE @sdate + '%'";
                using (SqlCommand cmd = new SqlCommand(sql, db.MyConnection))
                {
                    // Thêm tham số vào câu truy vấn để tìm theo ngày giao dịch
                    cmd.Parameters.AddWithValue("@sdate", sdate);
                    object res = cmd.ExecuteScalar();
                    if (res != null)
                        result = Convert.ToDouble(res);
                }
            }
            finally
            {
                db.DisConnect();
            }
            return result; // Trả về tổng doanh thu trong ngày
        }
    }
}
