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
    public partial class CustomerModule : Form
    {
        private CustomerForm customerForm; 
        private CustomerBL customerBL = new CustomerBL();  
        private string title = "Pet Shop Management System"; 
        private bool check = false;  // Biến kiểm tra các trường nhập liệu có hợp lệ không

        public CustomerModule(CustomerForm form)
        {
            InitializeComponent();
            customerForm = form; // Lưu đối tượng CustomerForm để cập nhật lại dữ liệu khách hàng sau khi thực hiện thao tác
        }

        // Phương thức xử lý sự kiện khi nhấn nút "Save" để thêm khách hàng mới
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                CheckField(); // Kiểm tra các trường nhập liệu
                // Nếu các trường nhập liệu hợp lệ
                if (check)
                {
                    // Hiển thị hộp thoại xác nhận người dùng muốn đăng ký khách hàng không
                    if (MessageBox.Show("Are you sure you want to register this customer?", "Customer Registration", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        // Tạo đối tượng CustomerInfo để lưu thông tin khách hàng
                        CustomerInfo customer = new CustomerInfo
                        {
                            Name = txtName.Text,
                            Address = txtAddress.Text,
                            Phone = txtPhone.Text
                        };

                        customerBL.AddCustomer(customer);
                        MessageBox.Show("Customer has been successfully registered!", title);
                        Clear();
                        customerForm.LoadCustomer();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, title);
            }
        }

        // Phương thức xử lý sự kiện khi nhấn nút "Update" để cập nhật thông tin khách hàng
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                CheckField(); // Kiểm tra các trường nhập liệu
                // Nếu các trường nhập liệu hợp lệ
                if (check)
                {
                    // Hiển thị hộp thoại xác nhận người dùng muốn chỉnh sửa thông tin khách hàng không
                    if (MessageBox.Show("Are you sure you want to edit this record?", "Record Edit", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        // Tạo đối tượng CustomerInfo để lưu thông tin cập nhật của khách hàng
                        CustomerInfo customer = new CustomerInfo
                        {
                            Id = Convert.ToInt32(lblcid.Text),
                            Name = txtName.Text,
                            Address = txtAddress.Text,
                            Phone = txtPhone.Text
                        };

                        customerBL.UpdateCustomer(customer);
                        MessageBox.Show("Customer data has been successfully updated!", title);
                        Clear();
                        customerForm.LoadCustomer();
                        this.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, title);
            }
        }

        // Phương thức xử lý sự kiện khi nhấn nút "Cancel" để hủy bỏ thao tác
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        // Phương thức xử lý sự kiện khi nhấn nút "Close" để đóng form
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        // Phương thức kiểm tra các trường nhập liệu có hợp lệ không
        private void CheckField()
        {
            // Kiểm tra nếu các trường bắt buộc trống
            if (txtName.Text == "" || txtAddress.Text == "" || txtPhone.Text == "")
            {
                MessageBox.Show("Required data fields", "Warning");
                return;
            }
            check = true; // Đánh dấu kiểm tra hợp lệ nếu các trường nhập liệu không trống
        }

        // Phương thức để xóa các trường nhập liệu sau khi lưu hoặc hủy
        private void Clear()
        {
            txtName.Clear();
            txtAddress.Clear();
            txtPhone.Clear();
            btnSave.Enabled = true;
            btnUpdate.Enabled = false;
        }
    }
}
