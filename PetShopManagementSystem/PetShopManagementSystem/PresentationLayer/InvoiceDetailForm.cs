using BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TransferObject;

namespace PresentationLayer
{
    public partial class InvoiceDetailForm : Form
    {
        private string transno;  
        private InvoiceBL invoiceBL = new InvoiceBL();  
        private string title;  
        private Form parent;  

        public InvoiceDetailForm(Form parent, string transno, string title)
        {
            InitializeComponent();
            this.title = title;
            this.transno = transno;
            this.parent = parent;
            loadInvoices(invoiceBL.GetInvoice(transno)); // Tải thông tin hóa đơn dựa trên mã giao dịch
        }

        // Phương thức loadInvoices để tải và hiển thị thông tin chi tiết hóa đơn
        public void loadInvoices(Invoice invoice)
        {
            try
            {
                double total = 0; // Biến lưu tổng số tiền của hóa đơn
                dgvInvoiceDetails.Rows.Clear();
                if(invoice == null)
                {
                    return;
                }
                if(invoice.Details != null && invoice.Details.Count > 0) // Nếu có chi tiết hóa đơn
                {
                    foreach (var invoiceDetail in invoice.Details)
                    {
                        dgvInvoiceDetails.Rows.Add(invoice.TransNo, invoice.CustomerName, invoiceDetail.PName, invoiceDetail.Qty, invoiceDetail.Price, invoiceDetail.Total);
                        total += (double)invoiceDetail.Total; // Cộng dồn tổng tiền của hóa đơn
                    }
                }

                totalAmountLabel.Text = total.ToString("#,##");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, title);
            }
        }

        // Phương thức xử lý sự kiện khi nhấn nút "Back" để quay lại form trước đó
        private void btnBack_Click(object sender, EventArgs e)
        {
            Close();
        }

        // Phương thức xử lý sự kiện khi thay đổi văn bản trong ô tìm kiếm
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string search = txtSearch.Text.Trim();

            // Nếu ô tìm kiếm trống, tải tất cả chi tiết hóa đơn
            if (string.IsNullOrEmpty(search))
            {
                loadInvoices(invoiceBL.GetInvoice(transno)); // Gọi phương thức GetInvoice để tải tất cả chi tiết hóa đơn
            } else
            {
                // Gọi phương thức GetInvoiceWithSearch để tìm kiếm chi tiết hóa đơn theo từ khóa
                loadInvoices(invoiceBL.GetInvoiceWithSearch(transno, search));
            }
        }

        // Phương thức xử lý sự kiện khi nhấn nút "Close" để đóng form
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        // Phương thức xử lý sự kiện khi form đang được đóng
        private void InvoiceDetailForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            parent.Visible = true;
        }

    }
}
