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
    public partial class ProductModule : Form
    {
        private ProductBL productBL;
        private string title = "Pet Shop Management System";
        private bool check = false;
        private ProductForm productForm;

        public ProductModule(ProductForm form)
        {
            InitializeComponent();
            productBL = new ProductBL();
            productForm = form;
            cbCategory.SelectedIndex = 0; // Chọn chỉ mục mặc định trong combo box danh mục sản phẩm
        }

        // Phương thức xử lý sự kiện khi nhấn nút "Close" để đóng form
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        // Phương thức xử lý sự kiện khi nhấn nút "Save" để thêm sản phẩm mới
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                CheckField(); // Kiểm tra các trường nhập liệu
                if (check) // Nếu các trường nhập liệu hợp lệ
                {
                    // Hiển thị hộp thoại xác nhận người dùng có chắc chắn đăng ký sản phẩm không
                    if (MessageBox.Show("Are you sure you want to register this product?", "Product Registration", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        // Tạo đối tượng ProductInfo để lưu thông tin sản phẩm
                        ProductInfo product = new ProductInfo
                        {
                            PName = txtName.Text,
                            PType = txttype.Text,
                            PCategory = cbCategory.Text,
                            PQty = int.Parse(txtQty.Text),
                            PPrice = double.Parse(txtPrice.Text)
                        };

                        productBL.AddProduct(product);
                        MessageBox.Show("Product has been successfully registered!", title);
                        Clear();
                        productForm.LoadProduct();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, title);
            }
        }

        // Phương thức xử lý sự kiện khi nhấn nút "Update" để sửa thông tin sản phẩm
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                CheckField(); // Kiểm tra các trường nhập liệu
                if (check) // Nếu các trường nhập liệu hợp lệ
                {
                    // Hiển thị hộp thoại xác nhận người dùng có chắc chắn chỉnh sửa sản phẩm không
                    if (MessageBox.Show("Are you sure you want to Edit this product?", "Product Edited", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        // Tạo đối tượng ProductInfo để lưu thông tin sản phẩm sau khi sửa
                        ProductInfo product = new ProductInfo
                        {
                            PCode = lblPcode.Text,
                            PName = txtName.Text,
                            PType = txttype.Text,
                            PCategory = cbCategory.Text,
                            PQty = int.Parse(txtQty.Text),
                            PPrice = double.Parse(txtPrice.Text)
                        };

                        productBL.UpdateProduct(product);
                        MessageBox.Show("Product has been successfully updated!", title);
                        productForm.LoadProduct();
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
            Close();
        }

        // Phương thức xử lý sự kiện khi nhấn phím trong ô nhập liệu số lượng sản phẩm
        private void txtQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Kiểm tra nếu người dùng nhập ký tự không phải là số hoặc ký tự điều khiển
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Ngừng sự kiện nếu nhập không phải số
            }
        }

        // Phương thức xử lý sự kiện khi nhấn phím trong ô nhập liệu giá sản phẩm
        private void txtPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Kiểm tra nếu người dùng nhập ký tự không phải là số hoặc dấu chấm
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // Kiểm tra nếu người dùng đã nhập dấu chấm rồi thì không cho nhập dấu chấm lần nữa
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        #region Method
        // Phương thức Clear để xóa các trường nhập liệu sau khi thực hiện thao tác
        public void Clear()
        {
            txtName.Clear();
            txtPrice.Clear();
            txtQty.Clear();
            txttype.Clear();
            cbCategory.SelectedIndex = 0;
            btnUpdate.Enabled = false;
        }

        // Phương thức kiểm tra tính hợp lệ của các trường nhập liệu
        public void CheckField()
        {
            // Kiểm tra nếu một trong các trường bắt buộc bị để trống
            if (txtName.Text == "" | txtPrice.Text == "" | txtQty.Text == "" | txttype.Text == "")
            {
                MessageBox.Show("Required data fields", "Warning");
                return;
            }
            check = true; // Đánh dấu kiểm tra hợp lệ nếu các trường không trống
        }
        #endregion
    }
}
