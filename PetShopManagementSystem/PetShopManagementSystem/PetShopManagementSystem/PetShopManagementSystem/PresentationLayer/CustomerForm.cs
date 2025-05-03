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
    public partial class CustomerForm : Form
    {
        private CustomerBL customerBL = new CustomerBL();
        string title = "Pet Shop Management System";

        public CustomerForm()
        {
            InitializeComponent();
            LoadCustomer(); // Tải danh sách khách hàng khi form được tạo
        }

        // Phương thức xử lý sự kiện khi nhấn nút "Add" để thêm khách hàng mới
        private void btnAdd_Click(object sender, EventArgs e)
        {
            CustomerModule module = new CustomerModule(this);
            module.ShowDialog();
        }

        // Phương thức xử lý sự kiện khi thay đổi văn bản trong ô tìm kiếm
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadCustomer(); // Gọi lại phương thức LoadCustomer để tìm kiếm và hiển thị khách hàng theo từ khóa
        }

        // Phương thức xử lý sự kiện khi nhấn vào các ô trong DataGridView để chỉnh sửa hoặc xóa khách hàng
        private void dgvCustomer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colname = dgvCustomer.Columns[e.ColumnIndex].Name;

            // Nếu người dùng nhấn vào cột "Edit" để chỉnh sửa thông tin khách hàng
            if (colname == "Edit")
            {
                // Tạo đối tượng CustomerModule để chỉnh sửa khách hàng
                CustomerModule module = new CustomerModule(this);

                // Lấy thông tin khách hàng từ các ô trong DataGridView và truyền vào form CustomerModule
                module.lblcid.Text = dgvCustomer.Rows[e.RowIndex].Cells[1].Value.ToString();
                module.txtName.Text = dgvCustomer.Rows[e.RowIndex].Cells[2].Value.ToString();
                module.txtAddress.Text = dgvCustomer.Rows[e.RowIndex].Cells[3].Value.ToString();
                module.txtPhone.Text = dgvCustomer.Rows[e.RowIndex].Cells[4].Value.ToString();

                // Chuyển trạng thái của các nút trong form CustomerModule (chỉnh sửa thay vì thêm mới)
                module.btnSave.Enabled = false;
                module.btnUpdate.Enabled = true;
                module.ShowDialog();
            }
            else if (colname == "Delete") // Nếu người dùng nhấn vào cột "Delete" để xóa khách hàng
            {
                // Hiển thị thông báo xác nhận xóa khách hàng
                if (MessageBox.Show("Are you sure you want to delete this customer record?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // Lấy ID khách hàng từ DataGridView và gọi phương thức DeleteCustomer trong CustomerBL để xóa
                    int id = Convert.ToInt32(dgvCustomer.Rows[e.RowIndex].Cells[1].Value);
                    customerBL.DeleteCustomer(id);
                    MessageBox.Show("Customer data has been successfully removed", title);
                }
            }
            LoadCustomer(); // Tải lại danh sách khách hàng sau khi thực hiện các thao tác
        }

        #region Method
        // Phương thức LoadCustomer để tải danh sách khách hàng từ tầng BusinessLayer và hiển thị lên DataGridView
        public void LoadCustomer()
        {
            int i = 0; // Biến đếm số thứ tự khách hàng
            dgvCustomer.Rows.Clear();

            // Gọi phương thức GetCustomers từ CustomerBL để lấy danh sách khách hàng theo từ khóa tìm kiếm
            List<CustomerInfo> customers = customerBL.GetCustomers(txtSearch.Text);
            foreach (var customer in customers)
            {
                i++;
                dgvCustomer.Rows.Add(i, customer.Id, customer.Name, customer.Address, customer.Phone); // Thêm thông tin khách hàng vào DataGridView
            }
        }
        #endregion Method
    }
}
