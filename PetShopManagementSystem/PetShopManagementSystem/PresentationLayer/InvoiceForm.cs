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
using System.Windows.Forms.DataVisualization.Charting;
using TransferObject;

namespace PresentationLayer
{
    public partial class InvoiceForm : Form
    {
        private InvoiceBL invoiceBL = new InvoiceBL();
        private string title;
        public InvoiceForm(string title)
        {
            InitializeComponent();
            this.title = title;
            loadInvoices(invoiceBL.GetInvoices());
        }

        // Phương thức loadInvoices để tải và hiển thị danh sách hóa đơn
        public void loadInvoices(List<Invoice> invoices)
        {
            try
            {
                double total = 0; // Biến lưu tổng số tiền của tất cả hóa đơn
                dgvInvoices.Rows.Clear();
                if (invoices == null)
                {
                    return; // Nếu danh sách hóa đơn null, dừng phương thức
                }
                if (invoices != null && invoices.Count > 0)
                {
                    foreach (var invoice in invoices)
                    {
                        dgvInvoices.Rows.Add(invoice.TransNo, invoice.CustomerName, invoice.Date.ToString("dd/MM/yyyy"), invoice.Total, invoice.CashierName);
                        total += (double)invoice.Total; // Cộng dồn tổng tiền của tất cả hóa đơn
                    }
                }

                totalAmountLabel.Text = total.ToString("#,##");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, title);
            }
        }

        // Phương thức xử lý sự kiện khi thay đổi văn bản trong ô tìm kiếm
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string search = txtSearch.Text.Trim();

            // Nếu ô tìm kiếm trống, tải tất cả danh sách hóa đơn
            if (string.IsNullOrEmpty(search))
            {
                // Gọi phương thức GetInvoices trong InvoiceBL để lấy tất cả hóa đơn
                loadInvoices(invoiceBL.GetInvoices());
            }
            else
            {
                // Gọi phương thức GetInvoices với từ khóa tìm kiếm để lọc các hóa đơn
                loadInvoices(invoiceBL.GetInvoices(search));
            }
        }

        // Phương thức xử lý sự kiện khi nhấn nút "Close" để đóng form
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        // Phương thức xử lý sự kiện khi nhấn nút "Bill Detail" để xem chi tiết hóa đơn
        private void btnBillDetail_Click(object sender, EventArgs e)
        {
            if (dgvInvoices.SelectedRows.Count == 0)
            {
                // Kiểm tra nếu người dùng chưa chọn hóa đơn để xem chi tiết
                MessageBox.Show("Please select an invoice to view details.", title);
                return;
            }
            // Lấy mã giao dịch của hóa đơn đã chọn
            string transNo = dgvInvoices.SelectedRows[0].Cells[0].Value.ToString();

            // Tạo form InvoiceDetailForm để hiển thị chi tiết hóa đơn
            var invoiceDetailForm = new InvoiceDetailForm(this, transNo, title);
            invoiceDetailForm.ShowDialog();
        }
    }
}
