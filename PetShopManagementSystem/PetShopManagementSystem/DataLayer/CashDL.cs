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
    public class CashDL
    {
        private DbConnect db = new DbConnect(); // Khởi tạo đối tượng kết nối cơ sở dữ liệu

        // Phương thức để tải danh sách khách hàng dựa trên từ khóa tìm kiếm
        public List<CustomerInfo> LoadCustomer(string search)
        {
            List<CustomerInfo> list = new List<CustomerInfo>(); // Danh sách khách hàng
            try
            {
                db.Connect(); // Kết nối đến cơ sở dữ liệu

                // Câu truy vấn SQL để tìm khách hàng có tên giống với từ khóa tìm kiếm
                string query = "SELECT id, name, phone FROM tbCustomer WHERE name LIKE @search";
                
                // Sử dụng SqlCommand để thực thi câu truy vấn SQL
                using (SqlCommand cm = new SqlCommand(query, db.MyConnection)) 
                {
                    cm.Parameters.AddWithValue("@search", "%" + search + "%"); // Thêm tham số tìm kiếm vào câu truy vấn
                    // Thực thi câu truy vấn và lấy dữ liệu bằng SqlDataReader
                    using (SqlDataReader dr = cm.ExecuteReader(CommandBehavior.CloseConnection)) 
                    {
                        // Đọc từng dòng kết quả từ cơ sở dữ liệu
                        while (dr.Read())
                        {
                            // Đọc từng dòng dữ liệu và thêm vào danh sách khách hàng
                            list.Add(new CustomerInfo
                            {
                                Id = Convert.ToInt32(dr["id"]),
                                Name = dr["name"].ToString(),
                                Phone = dr["phone"].ToString()
                            });
                        }
                    }
                }
            }
            finally
            {
                db.DisConnect(); // Đảm bảo rằng kết nối được đóng dù có lỗi hay không
            }

            return list; // Trả về danh sách khách hàng tìm thấy
        }

        // Phương thức LoadProduct để lấy danh sách sản phẩm theo từ khóa tìm kiếm
        public List<ProductInfo> LoadProduct(string search)
        {
            List<ProductInfo> list = new List<ProductInfo>(); // Khởi tạo danh sách sản phẩm
            try
            {
                db.Connect();

                // Câu truy vấn SQL tìm kiếm sản phẩm có tên, loại hoặc danh mục chứa từ khóa và số lượng lớn hơn 0
                string query = "SELECT pcode, pname, ptype, pcategory, pprice FROM tbProduct WHERE CONCAT(pname,ptype,pcategory) LIKE @search AND pqty > 0";
                
                // Sử dụng SqlCommand để thực thi câu truy vấn SQL
                using (SqlCommand cm = new SqlCommand(query, db.MyConnection))
                {
                    cm.Parameters.AddWithValue("@search", "%" + search + "%"); // Thêm tham số tìm kiếm vào câu truy vấn
                    // Thực thi câu truy vấn và lấy dữ liệu bằng SqlDataReader
                    using (SqlDataReader dr = cm.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        // Đọc từng dòng dữ liệu và thêm vào danh sách sản phẩm
                        while (dr.Read())
                        {
                            list.Add(new ProductInfo
                            {
                                PCode = dr["pcode"].ToString(),
                                PName = dr["pname"].ToString(),
                                PType = dr["ptype"].ToString(),
                                PCategory = dr["pcategory"].ToString(),
                                PPrice = Convert.ToDouble(dr["pprice"])
                            });
                        }
                    }
                }
            }
            finally
            {
                db.DisConnect();
            }

            return list;
        }

        // Phương thức InsertCash để thêm một giao dịch mới vào cơ sở dữ liệu
        public void InsertCash(CashInfo cash)
        {
            try
            {
                db.Connect();

                // Câu truy vấn SQL để thêm thông tin giao dịch 
                string query = "INSERT INTO tbCash(transno, pcode, pname, qty, price, total, cid, cashier) VALUES (@transno, @pcode, @pname, @qty, @price, @total, @cid, @cashier)";

                // Sử dụng SqlCommand để thực thi câu truy vấn SQL
                using (SqlCommand cm = new SqlCommand(query, db.MyConnection))
                {
                    // Thêm tham số vào câu truy vấn để chèn thông tin giao dịch
                    cm.Parameters.AddWithValue("@transno", cash.TransNo);
                    cm.Parameters.AddWithValue("@pcode", cash.PCode);
                    cm.Parameters.AddWithValue("@pname", cash.PName);
                    cm.Parameters.AddWithValue("@qty", cash.Qty);
                    cm.Parameters.AddWithValue("@price", cash.Price);
                    cm.Parameters.AddWithValue("@total", cash.Total);
                    cm.Parameters.AddWithValue("@cid", cash.CustomerId ?? (object)DBNull.Value); // Nếu không có CustomerId thì cho giá trị DBNull
                    cm.Parameters.AddWithValue("@cashier", cash.Cashier);
                    cm.ExecuteNonQuery();
                }

            }
            finally
            {

                db.DisConnect();
            }
        }

        // Phương thức LoadCash để lấy các thông tin giao dịch theo mã giao dịch (transno)
        public List<CashInfo> LoadCash(string transno)
        {
            List<CashInfo> list = new List<CashInfo>(); // Khởi tạo danh sách giao dịch 
            try
            {
                db.Connect();

                // Câu truy vấn SQL lấy chi tiết giao dịch
                string query = @"SELECT cashid, pcode, pname, qty, price, total, c.name, cashier 
                         FROM tbCash AS cash 
                         LEFT JOIN tbCustomer c ON cash.cid = c.id 
                         WHERE transno LIKE @transno";
                using (SqlCommand cm = new SqlCommand(query, db.MyConnection))
                {
                    cm.Parameters.AddWithValue("@transno", transno); // Thêm tham số vào câu truy vấn
                    using (SqlDataReader dr = cm.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dr.Read())
                        {
                            list.Add(new CashInfo
                            {
                                CashId = Convert.ToInt32(dr["cashid"]),
                                PCode = dr["pcode"].ToString(),
                                PName = dr["pname"].ToString(),
                                Qty = Convert.ToInt32(dr["qty"]),
                                Price = Convert.ToDouble(dr["price"]),
                                Total = Convert.ToDouble(dr["total"]),
                                CustomerName = dr["name"].ToString(),
                                Cashier = dr["cashier"].ToString()
                            });
                        }
                    }
                }
            }
            finally
            {
                db.DisConnect();
            }

            return list;
        }

        // Phương thức DeleteCash dùng để xóa một giao dịch tiền mặt khỏi bảng tbCash dựa trên mã giao dịch (cashId)
        public void DeleteCash(string cashId)
        {
            try
            {
                db.Connect();

                // Câu truy vấn SQL để xóa giao dịch 
                string query = "DELETE FROM tbCash WHERE cashid LIKE @cashid";
                using (SqlCommand cm = new SqlCommand(query, db.MyConnection))
                {
                    cm.Parameters.AddWithValue("@cashid", cashId); // Thêm tham số cashId vào câu truy vấn
                    cm.ExecuteNonQuery();
                }
            }
            finally
            {
                db.DisConnect();
            }
        }

        // Phương thức UpdateCashQty dùng để cập nhật số lượng sản phẩm trong giao dịch
        public void UpdateCashQty(string cashId, int qty, bool increase)
        {
            try
            {
                db.Connect();

                // Quyết định có tăng hay giảm số lượng
                string op = increase ? "+" : "-";

                // Câu truy vấn SQL để cập nhật số lượng sản phẩm trong giao dịch
                string query = $"UPDATE tbCash SET qty = qty {op} @qty WHERE cashid LIKE @cashid";
                using (SqlCommand cm = new SqlCommand(query, db.MyConnection))
                {
                    cm.Parameters.AddWithValue("@qty", qty); // Thêm số lượng vào tham số câu truy vấn
                    cm.Parameters.AddWithValue("@cashid", cashId); // Thêm mã giao dịch vào tham số
                    cm.ExecuteNonQuery();
                }
            }
            finally
            {
                db.DisConnect();
            }
        }

        // Phương thức CheckPqty dùng để kiểm tra số lượng sản phẩm trong kho dựa trên mã sản phẩm (pcode)
        public int CheckPqty(string pcode)
        {
            int quantity = 0;
            try
            {
                db.Connect();

                // Câu truy vấn SQL lấy số lượng sản phẩm
                string query = "SELECT pqty FROM tbProduct WHERE pcode LIKE @pcode";
                using (SqlCommand cm = new SqlCommand(query, db.MyConnection))
                {
                    cm.Parameters.AddWithValue("@pcode", pcode); // Thêm mã sản phẩm vào câu truy vấn
                    object result = cm.ExecuteScalar(); // Thực thi truy vấn và lấy kết quả
                    if (result != null)
                    {
                        quantity = Convert.ToInt32(result); // Chuyển đổi kết quả sang kiểu int
                    }
                }
            }
            finally
            {
                db.DisConnect();
            }

            return quantity;
        }

        // Phương thức UpdateProductQty dùng để cập nhật số lượng sản phẩm trong kho sau mỗi giao dịch bán hàng
        public void UpdateProductQty(string pcode, int qty)
        {
            try
            {
                db.Connect();

                // Cập nhật số lượng sản phẩm trong kho
                string query = "UPDATE tbProduct SET pqty = pqty - @qty WHERE pcode LIKE @pcode";
                using (SqlCommand cm = new SqlCommand(query, db.MyConnection))
                {
                    cm.Parameters.AddWithValue("@qty", qty); // Thêm số lượng vào câu truy vấn
                    cm.Parameters.AddWithValue("@pcode", pcode); // Thêm mã sản phẩm vào câu truy vấn
                    cm.ExecuteNonQuery();
                }
            }
            finally
            {
                db.DisConnect();
            }
        }

        // Phương thức UpdateCustomer dùng để cập nhật ID khách hàng trong bảng tbCash khi có giao dịch mới
        public void UpdateCustomer(string transno, int customerId)
        {
            try
            {
                db.Connect();

                // Cập nhật ID khách hàng vào giao dịch
                string query = "UPDATE tbCash SET cid = @cid WHERE transno = @transno";
                using (SqlCommand cm = new SqlCommand(query, db.MyConnection))
                {
                    cm.Parameters.AddWithValue("@cid", customerId); // Thêm ID khách hàng vào câu truy vấn
                    cm.Parameters.AddWithValue("@transno", transno); // Thêm mã giao dịch vào câu truy vấn
                    cm.ExecuteNonQuery();
                }
            }
            finally
            {
                db.DisConnect();
            }
        }

        // Phương thức GetNextTransNo dùng để tạo mã giao dịch mới dựa trên ngày hiện tại và số thứ tự giao dịch trong ngày
        public string GetNextTransNo()
        {
            string sdate = DateTime.Now.ToString("yyyyMMdd");
            string transno = sdate + "1001";

            try
            {
                db.Connect();
                string query = "SELECT TOP 1 transno FROM tbCash WHERE transno LIKE @sdate + '%' ORDER BY cashid DESC"; // Lấy giao dịch mới nhất
                using (SqlCommand cm = new SqlCommand(query, db.MyConnection))
                {
                    cm.Parameters.AddWithValue("@sdate", sdate); // Thêm ngày vào câu truy vấn
                    object result = cm.ExecuteScalar(); // Thực thi truy vấn và lấy kết quả

                    if (result != null && result.ToString() != "")
                    {
                        string lastTrans = result.ToString(); // Lấy mã giao dịch cuối cùng
                        int count = int.Parse(lastTrans.Substring(8, 4)); // Lấy phần số thứ tự giao dịch
                        transno = sdate + (count + 1).ToString("D4"); // Tăng số thứ tự lên 1 và tạo mã giao dịch mới
                    }
                }
            }
            finally
            {
                db.DisConnect();
            }

            return transno;
        }

        // Phương thức TransferCashToInvoice dùng để chuyển giao dịch tiền mặt từ bảng tbCash sang bảng tbInvoice và tbInvoiceDetails khi giao dịch được hoàn thành
        public void TransferCashToInvoice(string transno)
        {
            try
            {
                db.Connect();

                // Bước 1: Lấy dữ liệu từ tbCash
                string queryCash = @"SELECT cashid, transno, pcode, pname, qty, price, total, cid, cashier 
                             FROM tbCash 
                             WHERE transno = @transno";

                SqlCommand cm = new SqlCommand(queryCash, db.MyConnection);
                cm.Parameters.AddWithValue("@transno", transno); // Lấy dữ liệu giao dịch tiền mặt
                SqlDataAdapter da = new SqlDataAdapter(cm);
                DataTable dtCash = new DataTable();
                da.Fill(dtCash);

                if (dtCash.Rows.Count == 0)
                {
                    throw new Exception("No data found in tbCash with this transaction code.");
                }

                DataRow firstRow = dtCash.Rows[0];

                // Bước 2: Thêm hóa đơn vào bảng tbInvoice
                string insertInvoiceQuery = @"INSERT INTO tbInvoice (transno, customerid, date, total, cashiername) 
                                      VALUES (@transno, @customerid, @date, @total, @cashiername)";

                SqlCommand insertInvoiceCmd = new SqlCommand(insertInvoiceQuery, db.MyConnection);
                insertInvoiceCmd.Parameters.AddWithValue("@transno", firstRow["transno"]);
                insertInvoiceCmd.Parameters.AddWithValue("@customerid", firstRow["cid"]);
                insertInvoiceCmd.Parameters.AddWithValue("@date", DateTime.Now);
                insertInvoiceCmd.Parameters.AddWithValue("@total", dtCash.AsEnumerable().Sum(r => Convert.ToDecimal(r["total"])));
                insertInvoiceCmd.Parameters.AddWithValue("@cashiername", firstRow["cashier"]);
                insertInvoiceCmd.ExecuteNonQuery();

                // Bước 3: Thêm từng dòng sản phẩm vào tbInvoiceDetails
                foreach (DataRow dr in dtCash.Rows)
                {
                    string insertDetailsQuery = @"INSERT INTO tbInvoiceDetails (transno, pcode, pname, price, qty, total) 
                                          VALUES (@transno, @pcode, @pname, @price, @qty, @total)";
                    SqlCommand insertDetailsCmd = new SqlCommand(insertDetailsQuery, db.MyConnection);
                    insertDetailsCmd.Parameters.AddWithValue("@transno", dr["transno"]);
                    insertDetailsCmd.Parameters.AddWithValue("@pcode", dr["pcode"]);
                    insertDetailsCmd.Parameters.AddWithValue("@pname", dr["pname"]);
                    insertDetailsCmd.Parameters.AddWithValue("@price", dr["price"]);
                    insertDetailsCmd.Parameters.AddWithValue("@qty", dr["qty"]);
                    insertDetailsCmd.Parameters.AddWithValue("@total", dr["total"]);
                    insertDetailsCmd.ExecuteNonQuery();
                }
            }

            finally
            {
                db.DisConnect();
            }
        }

    }
}