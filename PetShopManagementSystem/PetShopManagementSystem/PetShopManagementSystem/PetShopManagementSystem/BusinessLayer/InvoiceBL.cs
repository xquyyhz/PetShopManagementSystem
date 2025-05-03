using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferObject;
using DataLayer;

namespace BusinessLayer
{
    public class InvoiceBL
    {
        // Tạo đối tượng InvoiceDL để truy cập dữ liệu từ cơ sở dữ liệu
        private InvoiceDL dl = new InvoiceDL();

        // Phương thức GetInvoice để lấy thông tin hóa đơn theo mã giao dịch (transno)
        public Invoice GetInvoice(string transno)
        {
            return dl.GetInvoiceData(transno);
        }

        // Phương thức GetInvoiceWithSearch để lấy thông tin hóa đơn theo mã giao dịch và từ khóa tìm kiếm
        public Invoice GetInvoiceWithSearch(string transno, string search)
        {
            return dl.GetInvoiceData(transno, search);
        }

        // Phương thức GetInvoices để lấy danh sách các hóa đơn, có thể lọc theo từ khóa tìm kiếm
        public List<Invoice> GetInvoices(string search = null)
        {
            if(string.IsNullOrEmpty(search))
            {
                // Nếu không có từ khóa tìm kiếm, lấy tất cả các hóa đơn
                return dl.GetInvoices(); 
            }
            else
            {
                // Nếu có từ khóa tìm kiếm, lấy các hóa đơn phù hợp với từ khóa
                return dl.GetInvoices(search);
            }
        }
    }
}
