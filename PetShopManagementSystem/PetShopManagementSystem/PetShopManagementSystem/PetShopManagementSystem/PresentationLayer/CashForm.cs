using BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Drawing.Printing;

namespace PresentationLayer
{
    public partial class CashForm : Form
    {
        MainForm main;
        Dashboard dashboard;
        private CashBL cashBL = new CashBL();
        string title = "Pet Shop Management System";

        // Lưu trữ mã giao dịch cuối cùng để in hóa đơn
        private string lastTransNo = "";

        public CashForm(MainForm form, Dashboard dash)
        {
            InitializeComponent();
            dashboard = dash;  // Lưu đối tượng Dashboard vào biến dashboard để cập nhật sau khi giao dịch hoàn tất
            main = form;  // Lưu đối tượng MainForm vào biến main để truy cập thông tin từ form chính
            getTransno();  // Lấy mã giao dịch tiếp theo
            loadCash();  // Tải thông tin giao dịch hiện tại
        }

        // Phương thức xử lý sự kiện khi nhấn nút "Add" để thêm sản phẩm vào giao dịch tiền mặt
        private void btnAdd_Click(object sender, EventArgs e)
        {
            CashProduct product = new CashProduct(this); // Tạo đối tượng CashCustomer để chọn khách hàng cho giao dịch
            product.uname = main.lblUsername.Text;
            product.ShowDialog();
        }

        // Phương thức xử lý sự kiện khi nhấn nút "Cash" để thực hiện thanh toán và chuyển giao dịch tiền mặt thành hóa đơn
        private void btnCash_Click(object sender, EventArgs e)
        {

            CashCustomer customer = new CashCustomer(this);
            customer.ShowDialog();

            // Xác nhận với người dùng xem họ có chắc chắn thanh toán giao dịch không
            if (MessageBox.Show("Are you sure you want to cash this product?", "Cashing", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // Gọi phương thức TransferCashToInvoice để chuyển giao dịch thành hóa đơn
                cashBL.TransferCashToInvoice(lblTransno.Text);
                lastTransNo = lblTransno.Text;
                getTransno();
                dashboard.loadDailySale();

                // Duyệt qua tất cả sản phẩm trong giao dịch và cập nhật số lượng sản phẩm trong kho
                foreach (DataGridViewRow row in dgvCash.Rows)
                {
                    string pcode = row.Cells[2].Value.ToString();
                    int qty = int.Parse(row.Cells[4].Value.ToString());
                    cashBL.UpdateProductQty(pcode, qty);
                }
                dgvCash.Rows.Clear();
                // Hiển thị thông báo thanh toán thành công
                MessageBox.Show("Payment completed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // Phương thức xử lý sự kiện khi nhấn vào các ô trong DataGridView (dgvCash) để thực hiện các thao tác như xóa, tăng/giảm số lượng
        private void dgvCash_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvCash.Columns[e.ColumnIndex].Name;

        removeitem:
            // Nếu người dùng nhấn vào cột "DeleteColumn" để xóa sản phẩm
            if (colName == "DeleteColumn")
            {
                if (MessageBox.Show("Are you sure you want to delete this cash?", "Delete Cash", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string cashId = dgvCash.Rows[e.RowIndex].Cells[1].Value.ToString();
                    cashBL.DeleteCash(cashId);
                    MessageBox.Show("Cash record has been successfully removed!", title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else if (colName == "Increase") // Nếu người dùng nhấn vào cột "Increase" để tăng số lượng sản phẩm
            {
                string pcode = dgvCash.Rows[e.RowIndex].Cells[2].Value.ToString(); // Lấy mã sản phẩm
                int currentQty = int.Parse(dgvCash.Rows[e.RowIndex].Cells[4].Value.ToString()); // Lấy số lượng hiện tại
                int availableQty = cashBL.CheckPqty(pcode); // Kiểm tra số lượng sản phẩm trong kho


                // Nếu số lượng trong kho còn đủ
                if (currentQty < availableQty)
                {
                    string cashId = dgvCash.Rows[e.RowIndex].Cells[1].Value.ToString(); // Lấy ID giao dịch
                    cashBL.UpdateCashQty(cashId, 1, true); // Tăng số lượng sản phẩm
                }
                else
                {
                    MessageBox.Show("Remaining quantity on hand is " + availableQty + "!", "Out of stock", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            else if (colName == "Decrease") // Nếu người dùng nhấn vào cột "Decrease" để giảm số lượng sản phẩm
            {
                int qty = int.Parse(dgvCash.Rows[e.RowIndex].Cells[4].Value.ToString());
                string cashId = dgvCash.Rows[e.RowIndex].Cells[1].Value.ToString();

                if (qty == 1) // Nếu số lượng sản phẩm bằng 1, xóa sản phẩm
                {
                    colName = "DeleteColumn"; // Chuyển cột thành "DeleteColumn"
                    goto removeitem; // Gọi lại thao tác xóa sản phẩm
                }
                else
                {
                    cashBL.UpdateCashQty(cashId, 1, false); // Giảm số lượng sản phẩm
                }
            }

            loadCash();
        }

        #region Method
        // Phương thức getTransno để lấy mã giao dịch tiếp theo
        public void getTransno()
        {
            try
            {
                string transno = cashBL.GetNextTransNo();
                lblTransno.Text = transno;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, title);
            }
        }

        // Phương thức loadCash để tải danh sách các sản phẩm trong giao dịch tiền mặt
        public void loadCash()
        {
            try
            {
                int i = 0;
                double total = 0;
                dgvCash.Rows.Clear(); // Xóa các dòng dữ liệu hiện tại trong DataGridView

                // Lấy danh sách giao dịch tiền mặt từ CashBL
                var cashList = cashBL.LoadCash(lblTransno.Text);

                foreach (var c in cashList)
                {
                    i++;
                    dgvCash.Rows.Add(i, c.CashId, c.PCode, c.PName, c.Qty, c.Price, c.Total, c.CustomerName, c.Cashier); // Thêm các sản phẩm vào DataGridView
                    total += c.Total; // Cộng dồn tổng tiền
                }

                lblTotal.Text = total.ToString("#,##");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, title);
            }
        }

        // Phương thức checkPqty để kiểm tra số lượng sản phẩm trong kho
        public int checkPqty(string pcode)
        {
            try
            {
                // Gọi phương thức CheckPqty trong CashBL để kiểm tra số lượng sản phẩm
                return cashBL.CheckPqty(pcode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, title);
                return 0;
            }
        }
        #endregion Method

        // Phương thức btnPrint_Click xử lý sự kiện khi người dùng nhấn nút "Print" (In hóa đơn)
        private void btnPrint_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem mã giao dịch cuối cùng (lastTransNo) có rỗng không
            if (string.IsNullOrEmpty(lastTransNo))
            {
                // Nếu không có hóa đơn (mã giao dịch trống), hiển thị thông báo
                MessageBox.Show("No invoice to print yet!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Nếu có mã giao dịch, tạo một form mới để in hóa đơn với mã giao dịch cuối cùng
            InvoicePrintForm printForm = new InvoicePrintForm(lastTransNo);
            printForm.Hide();
        }

        // Phương thức btView_Click xử lý sự kiện khi người dùng nhấn nút "View" (Xem lịch sử mua hàng của khách hàng)
        private void btView_Click(object sender, EventArgs e)
        {
            // Tạo một form InvoiceForm để xem thông tin hóa đơn
            InvoiceForm invoiceForm = new InvoiceForm(title);
            // Hiển thị form InvoiceForm dưới dạng hộp thoại
            invoiceForm.ShowDialog();
        }
    }
}
