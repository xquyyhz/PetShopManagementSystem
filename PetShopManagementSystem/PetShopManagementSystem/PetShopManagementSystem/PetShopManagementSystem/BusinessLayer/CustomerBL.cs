using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using TransferObject;

namespace BusinessLayer
{
    public class CustomerBL
    {
        // Đối tượng CustomerDL để truy cập các phương thức xử lý dữ liệu khách hàng từ cơ sở dữ liệu
        private CustomerDL cus = new CustomerDL();

        // Phương thức AddCustomer để thêm một khách hàng mới vào cơ sở dữ liệu
        public void AddCustomer(CustomerInfo customer)
        {
            cus.AddCustomer(customer); // Gọi phương thức AddCustomer trong CustomerDL để thực hiện thêm khách hàng
        }

        // Phương thức UpdateCustomer để cập nhật thông tin của khách hàng
        public void UpdateCustomer(CustomerInfo customer)
        {
            cus.UpdateCustomer(customer);
        }
        // Phương thức DeleteCustomer để xóa khách hàng khỏi cơ sở dữ liệu theo ID
        public void DeleteCustomer(int id)
        {
            cus.DeleteCustomer(id);
        }

        // Phương thức GetCustomers để lấy danh sách khách hàng từ cơ sở dữ liệu theo từ khóa tìm kiếm
        public List<CustomerInfo> GetCustomers(string keyword)
        {
            return cus.GetCustomers(keyword);
        }
    }
}
