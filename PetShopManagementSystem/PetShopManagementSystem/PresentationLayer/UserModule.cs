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
    public partial class UserModule : Form
    {
        string title = "Pet Shop Management System";
        bool check = false;
        UserForm userForm;
        private UserBL userBUS = new UserBL();

        public UserModule(UserForm user)
        {
            InitializeComponent();
            userForm = user;
            cbRole.SelectedIndex = 1; // Mặc định chọn vai trò là "Employee"
        }

        // Phương thức xử lý sự kiện khi nhấn nút "Save" để thêm người dùng mới
        private void btnSave_Click_1(object sender, EventArgs e)
        {
            try
            {
                CheckField(); // Kiểm tra tính hợp lệ của các trường nhập liệu
                if (check) // Nếu các trường nhập liệu hợp lệ
                {
                    // Hiển thị hộp thoại xác nhận người dùng có chắc chắn đăng ký người dùng không
                    if (MessageBox.Show("Are you sure you want to register this user?", "User Registration", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        // Tạo đối tượng AccountUser và gán giá trị từ các trường nhập liệu
                        AccountUser user = new AccountUser
                        {
                            Name = txtName.Text,
                            Address = txtAddress.Text,
                            Phone = txtPhone.Text,
                            Role = cbRole.Text,
                            Dob = dtDob.Value,
                            Password = txtPass.Text
                        };

                        // Gọi phương thức AddUser trong UserBL để thêm người dùng vào cơ sở dữ liệu
                        bool success = userBUS.AddUser(user);
                        if (success)
                        {
                            MessageBox.Show("User has been successfully registered!", title);
                            Clear();
                            userForm.LoadUser();
                        }
                        else
                        {
                            MessageBox.Show("Failed to register user.", title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        Clear();
                        userForm.LoadUser();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, title);
            }
        }

        // Phương thức xử lý sự kiện khi nhấn nút "Update" để sửa thông tin người dùng
        private void btnUpdate_Click_1(object sender, EventArgs e)
        {
            try
            {
                CheckField(); // Kiểm tra tính hợp lệ của các trường nhập liệu
                if (check) // Nếu các trường nhập liệu hợp lệ
                {
                    if (MessageBox.Show("Are you sure you want to update this record?", "Edit Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        // Tạo đối tượng AccountUser và gán giá trị từ các trường nhập liệu
                        AccountUser user = new AccountUser
                        {
                            Id = Convert.ToInt32(lbluid.Text), // Lấy ID người dùng từ label (được gán khi chỉnh sửa)
                            Name = txtName.Text,
                            Address = txtAddress.Text,
                            Phone = txtPhone.Text,
                            Role = cbRole.Text,
                            Dob = dtDob.Value,
                            Password = txtPass.Text
                        };

                        userBUS.UpdateUser(user);
                        MessageBox.Show("User's data has been successfully updated!", title);
                        Clear();
                        userForm.LoadUser();
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
        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            Clear();
        }

        // Phương thức xử lý sự kiện khi nhấn nút "Close" để đóng form UserModule
        private void btnClose_Click_1(object sender, EventArgs e)
        {
            this.Dispose();
        }

        // Phương thức xử lý sự kiện khi thay đổi giá trị trong combobox "Role" (vai trò)
        private void cbRole_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (cbRole.Text == "Employee") // Nếu vai trò là "Employee"
            {
                this.Height = 514;
                lblPass.Visible = false; // Ẩn label và ô nhập liệu mật khẩu
                txtPass.Visible = false;
            }
            else // Nếu vai trò không phải là "Employee" (tức là "Administrator")
            {
                lblPass.Visible = true; // Hiển thị label và ô nhập liệu mật khẩu
                txtPass.Visible = true;
                this.Height = 544;
            }
        }

        #region Method
        // Phương thức Clear để xóa các trường nhập liệu sau khi thực hiện thao tác
        public void Clear()
        {
            txtName.Clear();
            txtAddress.Clear();
            txtPhone.Clear();
            txtPass.Clear();
            cbRole.SelectedIndex = 0;
            dtDob.Value = DateTime.Now;
            btnUpdate.Enabled = false;
        }

        // Phương thức kiểm tra tính hợp lệ của các trường nhập liệu
        public void CheckField()
        {
            // Kiểm tra nếu các trường nhập liệu tên và địa chỉ bị bỏ trống
            if (txtName.Text == "" || txtAddress.Text == "")
            {
                MessageBox.Show("Required data fields", "Warning");
                return; // Dừng phương thức nếu người dùng dưới 18 tuổi  
            }

            // Kiểm tra nếu người dùng dưới 18 tuổi
            if (CheckAge(dtDob.Value) < 18)
            {
                MessageBox.Show("User is child worker!. Under 18 year", "Warning");
                return; // Dừng phương thức nếu người dùng dưới 18 tuổi  
            }

            check = true;
        }

        // Phương thức tính tuổi của người dùng dựa trên ngày sinh
        private static int CheckAge(DateTime dateofBirth)
        {
            int age = DateTime.Now.Year - dateofBirth.Year; // Tính tuổi dựa trên năm
            if (DateTime.Now.DayOfYear < dateofBirth.DayOfYear) // Nếu chưa đến ngày sinh trong năm hiện tại
            {
                age = age - 1; // Giảm tuổi xuống 1
            }
            return age;
        }
        #endregion

       
    }
}
