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
    public partial class UserForm : Form
    {
        private UserBL userBL = new UserBL();
        string title = "Pet Shop Management System";

        public UserForm()
        {
            InitializeComponent();
            LoadUser();
        }

        // Phương thức xử lý sự kiện khi thay đổi văn bản trong ô tìm kiếm
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadUser();
        }

        // Phương thức xử lý sự kiện khi nhấn nút "Add" để thêm người dùng mới
        private void btnAdd_Click(object sender, EventArgs e)
        {
            UserModule module = new UserModule(this);
            module.ShowDialog();
        }

        // Phương thức xử lý sự kiện khi nhấn vào các ô trong DataGridView để chỉnh sửa hoặc xóa người dùng
        private void dgvUser_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colname = dgvUser.Columns[e.ColumnIndex].Name;

            // Nếu người dùng nhấn vào cột "Edit" để chỉnh sửa thông tin người dùng
            if (colname == "Edit")
            {
                // Mở form UserModule để chỉnh sửa người dùng
                UserModule module = new UserModule(this);
                module.lbluid.Text = dgvUser.Rows[e.RowIndex].Cells[1].Value.ToString();
                module.txtName.Text = dgvUser.Rows[e.RowIndex].Cells[2].Value.ToString();
                module.txtAddress.Text = dgvUser.Rows[e.RowIndex].Cells[3].Value.ToString();
                module.txtPhone.Text = dgvUser.Rows[e.RowIndex].Cells[4].Value.ToString();
                module.cbRole.Text = dgvUser.Rows[e.RowIndex].Cells[5].Value.ToString();
                module.dtDob.Text = dgvUser.Rows[e.RowIndex].Cells[6].Value.ToString();
                module.txtPass.Text = dgvUser.Rows[e.RowIndex].Cells[7].Value.ToString();

                module.btnSave.Enabled = false;
                module.btnUpdate.Enabled = true;
                module.ShowDialog();
            }
            else if (colname == "Delete") // Nếu người dùng nhấn vào cột "Delete" để xóa người dùng
            {
                if (MessageBox.Show("Are you sure you want to delete this record?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // Lấy ID người dùng từ DataGridView và gọi phương thức DeleteUser trong UserBL để xóa người dùng
                    int id = Convert.ToInt32(dgvUser.Rows[e.RowIndex].Cells[1].Value);
                    userBL.DeleteUser(id);
                    MessageBox.Show("User data has been successfully removed", title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            LoadUser();
        }

        #region Method
        // Phương thức LoadUser để tải danh sách người dùng từ tầng BusinessLayer và hiển thị lên DataGridView
        public void LoadUser()
        {
            int i = 0; // Biến đếm số thứ tự người dùng
            dgvUser.Rows.Clear();

            // Lấy danh sách người dùng từ UserBL theo từ khóa tìm kiếm
            List<AccountUser> users = userBL.GetUsers(txtSearch.Text);
            foreach (var user in users)
            {
                i++;

                // Thêm thông tin người dùng vào DataGridView
                dgvUser.Rows.Add(i, user.Id, user.Name, user.Address, user.Phone, user.Role, user.Dob.ToShortDateString(), user.Password);
            }
        }
        #endregion Method
    }
}
