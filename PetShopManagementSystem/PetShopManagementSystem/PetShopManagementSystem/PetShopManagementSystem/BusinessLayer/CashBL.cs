using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using TransferObject;

namespace BusinessLayer
{
    public class CashBL
    {
        private CashDL cashDL; // Đối tượng CashDL dùng để truy cập dữ liệu từ tầng DataLayer

        // Constructor của CashBL, khởi tạo đối tượng CashDL
        public CashBL()
        {
            cashDL = new CashDL();
        }

        // Phương thức LoadCustomer để lấy danh sách khách hàng từ CashDL
        public List<CustomerInfo> LoadCustomer(string search)
        {
            return cashDL.LoadCustomer(search);
        }

        // Phương thức LoadProduct để lấy danh sách sản phẩm từ CashDL
        public List<ProductInfo> LoadProduct(string search)
        {
            return cashDL.LoadProduct(search);
        }

        // Phương thức InsertCash để thêm một giao dịch tiền mặt vào cơ sở dữ liệu
        public void InsertCash(CashInfo cash)
        {
            cashDL.InsertCash(cash);
        }

        // Phương thức LoadCash để lấy thông tin giao dịch tiền mặt theo mã giao dịch
        public List<CashInfo> LoadCash(string transno)
        {
            return cashDL.LoadCash(transno);
        }

        // Phương thức DeleteCash để xóa giao dịch tiền mặt theo mã giao dịch
        public void DeleteCash(string cashId)
        {
            cashDL.DeleteCash(cashId);
        }

        // Phương thức UpdateCashQty để cập nhật số lượng sản phẩm trong giao dịch tiền mặt
        public void UpdateCashQty(string cashId, int qty, bool increase)
        {
            cashDL.UpdateCashQty(cashId, qty, increase);
        }

        // Phương thức CheckPqty để kiểm tra số lượng sản phẩm trong kho
        public int CheckPqty(string pcode)
        {
            return cashDL.CheckPqty(pcode);
        }

        // Phương thức UpdateProductQty để cập nhật số lượng sản phẩm trong kho sau mỗi giao dịch
        public void UpdateProductQty(string pcode, int qty)
        {
            cashDL.UpdateProductQty(pcode, qty);
        }

        // Phương thức UpdateCustomer để cập nhật thông tin khách hàng trong giao dịch tiền mặt
        public void UpdateCustomer(string transno, int customerId)
        {
            cashDL.UpdateCustomer(transno, customerId);
        }

        // Phương thức GetNextTransNo để lấy mã giao dịch tiếp theo
        public string GetNextTransNo()
        {
            return cashDL.GetNextTransNo();
        }

        // Phương thức TransferCashToInvoice để chuyển giao dịch tiền mặt sang hóa đơn
        public void TransferCashToInvoice(string transno)
        {
            cashDL.TransferCashToInvoice(transno);
        }
    }
}
