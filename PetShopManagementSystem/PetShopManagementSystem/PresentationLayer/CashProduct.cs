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
    public partial class CashProduct : Form
    {
        public string uname;  // Biến để lưu tên người thu ngân
        CashForm cash; 
        private CashBL cashBL = new CashBL();  
        string title = "Pet Shop Management System";  

        public CashProduct(CashForm form)
        {
            InitializeComponent();
            cash = form;
            LoadProduct();
        }

        // Phương thức xử lý sự kiện khi thay đổi văn bản trong ô tìm kiếm
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadProduct(); // Gọi lại phương thức LoadProduct để tìm kiếm và hiển thị sản phẩm theo từ khóa
        }

        // Phương thức xử lý sự kiện khi nhấn nút "Cash" để thêm sản phẩm vào giao dịch tiền mặt
        private void btnCash_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvProduct.Rows)
            {
                // Kiểm tra nếu người dùng chọn sản phẩm (cột "Select" được chọn)
                if (Convert.ToBoolean(row.Cells["Select"].Value))
                {
                    try
                    {
                        // Lấy giá sản phẩm từ cột "Price"
                        double price = Convert.ToDouble(row.Cells[5].Value);

                        // Tạo đối tượng CashInfo để lưu thông tin giao dịch
                        var cashInfo = new CashInfo
                        {
                            TransNo = cash.lblTransno.Text,
                            PCode = row.Cells[1].Value.ToString(),
                            PName = row.Cells[2].Value.ToString(),
                            Qty = 1,
                            Price = price,
                            Total = price,
                            Cashier = uname,
                        };

                        // Thêm thông tin giao dịch vào cơ sở dữ liệu
                        cashBL.InsertCash(cashInfo);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, title);
                    }
                }
            }
            // Cập nhật lại thông tin giao dịch sau khi thêm sản phẩm
            cash.loadCash();
            this.Dispose();
        }

        #region Method
        // Phương thức LoadProduct để tải danh sách sản phẩm từ tầng BusinessLayer và hiển thị lên DataGridView
        public void LoadProduct()
        {
            try
            {
                int i = 0;
                dgvProduct.Rows.Clear();

                // Gọi phương thức LoadProduct từ CashBL để lấy danh sách sản phẩm theo từ khóa tìm kiếm
                var products = cashBL.LoadProduct(txtSearch.Text.Trim());

                // Duyệt qua danh sách sản phẩm và thêm vào DataGridView
                foreach (var p in products)
                {
                    i++;
                    dgvProduct.Rows.Add(i, p.PCode, p.PName, p.PType, p.PCategory, p.PPrice.ToString("0.00")); // Thêm thông tin sản phẩm vào DataGridView
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, title);
            }
        }
        #endregion Method
    }
}
