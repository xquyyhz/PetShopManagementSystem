using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferObject;


namespace DataLayer
{
    public class InvoiceDL
    {
        private DbConnect db = new DbConnect();

        // Phương thức GetInvoiceData để lấy thông tin hóa đơn và chi tiết hóa đơn theo mã giao dịch
        public Invoice GetInvoiceData(string transno)
        {
            var invoice = new Invoice(); // Tạo đối tượng hóa đơn mới
            invoice.Details = new List<InvoiceDetail>(); // Khởi tạo danh sách chi tiết hóa đơn

            // Câu truy vấn SQL để lấy thông tin hóa đơn theo mã giao dịch
            string queryInvoice = "SELECT i.transno, c.name AS customername, i.date, i.cashiername, i.total " +
                                  "FROM tbInvoice i LEFT JOIN tbCustomer c ON i.customerid = c.id WHERE i.transno = @transno";

            // Câu truy vấn SQL để lấy chi tiết hóa đơn
            string queryDetails = "SELECT pcode, pname, price, qty, total FROM tbInvoiceDetails WHERE transno = @transno";

            // Lấy SqlConnection từ DbConnect
            SqlConnection conn = db.MyConnection;

            conn.Open();

            // Thực thi câu truy vấn để lấy thông tin hóa đơn
            using (SqlCommand cmd = new SqlCommand(queryInvoice, conn))
            {
                cmd.Parameters.AddWithValue("@transno", transno);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        invoice.TransNo = dr["transno"].ToString();
                        invoice.CustomerName = dr["customername"].ToString();
                        invoice.Date = Convert.ToDateTime(dr["date"]);
                        invoice.CashierName = dr["cashiername"].ToString();
                        invoice.Total = Convert.ToDecimal(dr["total"]);
                    }
                }
            }

            // Thực thi câu truy vấn để lấy chi tiết các sản phẩm trong hóa đơn
            using (SqlCommand cmd = new SqlCommand(queryDetails, conn))
            {
                cmd.Parameters.AddWithValue("@transno", transno);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        // Thêm thông tin chi tiết vào danh sách hóa đơn
                        invoice.Details.Add(new InvoiceDetail
                        {
                            PCode = dr["pcode"].ToString(),
                            PName = dr["pname"].ToString(),
                            Price = Convert.ToDecimal(dr["price"]),
                            Qty = Convert.ToInt32(dr["qty"]),
                            Total = Convert.ToDecimal(dr["total"])
                        });
                    }
                }
            }
            conn.Close();
            return invoice; // Trả về thông tin hóa đơn và chi tiết
        }

        // Phương thức GetInvoiceData để lấy thông tin hóa đơn và chi tiết hóa đơn theo mã giao dịch và tên sản phẩm tìm kiếm
        public Invoice GetInvoiceData(string transno, string search)
        {
            var invoice = new Invoice(); // Tạo đối tượng hóa đơn mới
            invoice.Details = new List<InvoiceDetail>(); // Khởi tạo danh sách chi tiết hóa đơn

            // Câu truy vấn SQL để lấy thông tin hóa đơn theo mã giao dịch
            string queryInvoice = "SELECT i.transno, c.name AS customername, i.date, i.cashiername, i.total " +
                                  "FROM tbInvoice i LEFT JOIN tbCustomer c ON i.customerid = c.id WHERE i.transno = @transno";

            // Câu truy vấn SQL để lấy chi tiết hóa đơn theo mã giao dịch và tên sản phẩm
            string queryDetails = "SELECT pcode, pname, price, qty, total FROM tbInvoiceDetails WHERE transno = @transno AND pname LIKE @search";

            // Lấy SqlConnection từ DbConnect
            SqlConnection conn = db.MyConnection;

            conn.Open();

            // Thực thi câu truy vấn để lấy thông tin hóa đơn
            using (SqlCommand cmd = new SqlCommand(queryInvoice, conn))
            {
                cmd.Parameters.AddWithValue("@transno", transno);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        invoice.TransNo = dr["transno"].ToString();
                        invoice.CustomerName = dr["customername"].ToString();
                        invoice.Date = Convert.ToDateTime(dr["date"]);
                        invoice.CashierName = dr["cashiername"].ToString();
                        invoice.Total = Convert.ToDecimal(dr["total"]);
                    }
                }
            }

            // Thực thi câu truy vấn để lấy chi tiết các sản phẩm trong hóa đơn
            using (SqlCommand cmd = new SqlCommand(queryDetails, conn))
            {
                cmd.Parameters.AddWithValue("@transno", transno);
                cmd.Parameters.AddWithValue("@search", $"%{search}%");
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        // Thêm thông tin chi tiết vào danh sách hóa đơn
                        invoice.Details.Add(new InvoiceDetail
                        {
                            PCode = dr["pcode"].ToString(),
                            PName = dr["pname"].ToString(),
                            Price = Convert.ToDecimal(dr["price"]),
                            Qty = Convert.ToInt32(dr["qty"]),
                            Total = Convert.ToDecimal(dr["total"])
                        });
                    }
                }
            }
            conn.Close();
            return invoice; // Trả về thông tin hóa đơn và chi tiết
        }

        // Phương thức GetInvoices để lấy danh sách tất cả các hóa đơn
        public List<Invoice> GetInvoices()
        {
            List<Invoice> list = new List<Invoice>(); // Khởi tạo danh sách hóa đơn
            string query = "SELECT i.transno, c.name AS customername, i.date, i.cashiername, i.total " +
                           "FROM tbInvoice i LEFT JOIN tbCustomer c ON i.customerid = c.id";

            using (SqlCommand cm = new SqlCommand(query, db.MyConnection))
            {
                db.MyConnection.Open();
                using (SqlDataReader dr = cm.ExecuteReader())
                {
                    // Đọc từng dòng dữ liệu và thêm vào danh sách hóa đơn
                    while (dr.Read())
                    {
                        list.Add(new Invoice()
                        {
                            TransNo = dr["transno"].ToString(),
                            CustomerName = dr["customername"].ToString(),
                            Date = Convert.ToDateTime(dr["date"]),
                            CashierName = dr["cashiername"].ToString(),
                            Total = Convert.ToDecimal(dr["total"]),
                        });
                    }
                }
                db.MyConnection.Close();
            }

            return list;
        }

        // Phương thức GetInvoices để lấy danh sách hóa đơn theo từ khóa tìm kiếm
        public List<Invoice> GetInvoices(string search)
        {
            List<Invoice> list = new List<Invoice>(); // Khởi tạo danh sách hóa đơn
            string query = "SELECT i.transno, c.name AS customername, i.date, i.cashiername, i.total " +
                           "FROM tbInvoice i LEFT JOIN tbCustomer c ON i.customerid = c.id WHERE i.transno LIKE @search OR c.name LIKE @search";

            using (SqlCommand cm = new SqlCommand(query, db.MyConnection))
            {
                cm.Parameters.AddWithValue("@search", $"%{search}%");
                db.MyConnection.Open();
                using (SqlDataReader dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new Invoice()
                        {
                            TransNo = dr["transno"].ToString(),
                            CustomerName = dr["customername"].ToString(),
                            Date = Convert.ToDateTime(dr["date"]),
                            CashierName = dr["cashiername"].ToString(),
                            Total = Convert.ToDecimal(dr["total"]),
                        });
                    }
                }
                db.MyConnection.Close();
            }

            return list; // Trả về danh sách các hóa đơn tìm thấy theo từ khóa
        }
    }
}

