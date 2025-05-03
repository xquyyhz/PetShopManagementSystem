using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BusinessLayer;
using TransferObject;

namespace PresentationLayer
{
    public partial class CashCustomer : Form
    {
        CashForm cash; 
        private CashBL cashBL = new CashBL(); 
        string title = "Pet Shop Management System";

        public CashCustomer(CashForm form)
        {
            InitializeComponent();
            cash = form; // Lưu đối tượng CashForm vào biến cash để truy cập các thông tin từ form cha
            LoadCustomer(); // Tải danh sách khách hàng khi form được khởi tạo
        }

        // Phương thức xử lý sự kiện khi thay đổi văn bản trong ô tìm kiếm
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadCustomer(); // Gọi lại phương thức LoadCustomer để tìm kiếm và hiển thị khách hàng theo từ khóa
        }

        // Phương thức xử lý sự kiện khi nhấn vào ô "Choice" trong DataGridView để chọn khách hàng
        private void dgvCustomer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvCustomer.Columns[e.ColumnIndex].Name; // Lấy tên cột nơi người dùng đã nhấn

            // Kiểm tra nếu người dùng nhấn vào cột "Choice" và chắc chắn là một hàng hợp lệ
            if (colName == "Choice" && e.RowIndex >= 0)
            {
                try
                {
                    // Lấy ID khách hàng từ cột dữ liệu tương ứng
                    int customerId = Convert.ToInt32(dgvCustomer.Rows[e.RowIndex].Cells[1].Value);
                    string transNo = cash.lblTransno.Text; // Lấy mã giao dịch từ form CashForm

                    // Cập nhật thông tin khách hàng trong giao dịch
                    cashBL.UpdateCustomer(transNo, customerId);
                    cash.loadCash(); 
                    this.Dispose();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, title);
                }
            }
        }

        #region Mehtod
        // Phương thức LoadCustomer để tải danh sách khách hàng từ tầng BusinessLayer và hiển thị lên DataGridView
        public void LoadCustomer()
        {
            try
            {
                int i = 0; // Biến đếm số thứ tự khách hàng
                dgvCustomer.Rows.Clear();

                // Gọi phương thức LoadCustomer từ CashBL để lấy danh sách khách hàng theo từ khóa tìm kiếm
                List<CustomerInfo> customers = cashBL.LoadCustomer(txtSearch.Text); 
                foreach (var c in customers)
                {
                    i++;
                    dgvCustomer.Rows.Add(i, c.Id, c.Name, c.Phone); // Thêm thông tin khách hàng vào DataGridView
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, title);
            }
        }
        #endregion Mehtod
    }
}
