using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferObject;

namespace DataLayer
{
    public class RevenueDL
    {
        private DbConnect db;

        // Phương thức GetDailyRevenueByMonth để lấy doanh thu hàng ngày theo tháng
        public List<Revenue> GetDailyRevenueByMonth(int month)
        {
            List<Revenue> list = new List<Revenue>(); // Danh sách chứa các đối tượng Revenue

            // Câu truy vấn SQL để lấy doanh thu hàng ngày theo tháng
            string sql = @"
            SELECT 
                i.date,
                i.transno AS [transaction],
                c.name AS customername,
                SUM(d.qty) AS totalQty,
                i.total AS totalAmount,
                i.cashiername
            FROM 
                tbInvoice i
            INNER JOIN 
                tbInvoiceDetails d ON i.transno = d.transno
            INNER JOIN 
                tbCustomer c ON i.customerid = c.id
            WHERE 
                MONTH(i.date) = @month
            GROUP BY 
                i.date, i.transno, i.total, c.name, i.cashiername
            ORDER BY 
                i.date";

            db = new DbConnect();
            SqlCommand cmd = new SqlCommand(sql, db.MyConnection);
            cmd.Parameters.AddWithValue("@month", month); // Thêm tham số tháng vào câu truy vấn

            db.Connect();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Revenue r = new Revenue
                {
                    Date = Convert.ToDateTime(dr["date"]),
                    Transaction = dr["Transaction"].ToString(),
                    CustomerName = dr["customername"].ToString(),
                    Qty = Convert.ToInt32(dr["totalQty"]),
                    TotalAmount = Convert.ToDecimal(dr["totalAmount"]),
                    Cashier = dr["cashiername"].ToString()
                };
                list.Add(r); // Thêm đối tượng Revenue vào danh sách
            }
            dr.Close();
            db.DisConnect();
            return list; // Trả về danh sách doanh thu hàng ngày
        }

        // Phương thức GetRevenueByProductTypeAndMonth để lấy doanh thu theo loại sản phẩm theo tháng
        public List<Chart> GetRevenueByProductTypeAndMonth(int month)
        {
            List<Chart> list = new List<Chart>(); // Danh sách chứa các đối tượng Chart (biểu đồ doanh thu)

            // Câu truy vấn SQL để lấy doanh thu theo loại sản phẩm theo tháng
            string sql = @"SELECT p.pcategory AS Category, SUM(d.total) AS Revenue
                   FROM tbInvoiceDetails d
                   JOIN tbInvoice i ON d.transno = i.transno
                   JOIN tbProduct p ON d.pcode = p.pcode
                   WHERE MONTH(i.date) = @month
                   GROUP BY p.pcategory";

            DbConnect db = new DbConnect(); // Khởi tạo kết nối đến cơ sở dữ liệu
            SqlCommand cmd = new SqlCommand(sql, db.MyConnection);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@month", month); // Thêm tham số tháng vào câu truy vấn

            db.Connect(); // Mở kết nối đến cơ sở dữ liệu
            SqlDataReader dr = cmd.ExecuteReader(); // Thực thi câu truy vấn

            // Duyệt qua kết quả và thêm vào danh sách các đối tượng Chart
            while (dr.Read())
            {
                Chart dto = new Chart();
                dto.Type = dr["Category"].ToString(); // Gán loại sản phẩm (ptype) vào thuộc tính Type
                dto.Revenue = Convert.ToDecimal(dr["Revenue"]); // Gán doanh thu vào thuộc tính Revenue
                list.Add(dto); // Thêm đối tượng Chart vào danh sách
            }

            dr.Close(); // Đóng kết nối dữ liệu
            db.DisConnect(); // Ngắt kết nối cơ sở dữ liệu

            return list; // Trả về danh sách kết quả
        }

        // Thêm vào lớp RevenueDL
        public decimal GetTotalRevenueByMonth(int month)
        {
            decimal totalRevenue = 0;
            db = new DbConnect();


            // Câu truy vấn SQL để tính tổng doanh thu theo tháng
            string sql = @"
                SELECT SUM(i.total) AS TotalRevenue
                FROM tbInvoice i
                WHERE MONTH(i.date) = @month";

            SqlCommand cmd = new SqlCommand(sql, db.MyConnection);
            cmd.Parameters.AddWithValue("@month", month); // Thêm tham số tháng vào câu truy vấn

            db.Connect();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                totalRevenue = Convert.ToDecimal(dr["TotalRevenue"]);
            }
            dr.Close();
            db.DisConnect();

            return totalRevenue;
        }

    }
}
