using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using BusinessLayer;
using TransferObject;

namespace PresentationLayer
{
    public partial class ProductForm : Form
    {
        private ProductBL productBL = new ProductBL();
        string title = "Pet Shop Management System";

        public ProductForm()
        {
            InitializeComponent();
            LoadProduct(); // Tải danh sách sản phẩm khi form được tạo
        }

        // Phương thức xử lý sự kiện khi nhấn nút "Add" để thêm sản phẩm mới
        private void btnAdd_Click(object sender, EventArgs e)
        {
            ProductModule module = new ProductModule(this);
            module.ShowDialog();
        }

        // Phương thức xử lý sự kiện khi thay đổi văn bản trong ô tìm kiếm
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadProduct();
        }

        // Phương thức xử lý sự kiện khi nhấn vào các ô trong DataGridView để chỉnh sửa hoặc xóa sản phẩm
        private void dgvProduct_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvProduct.Columns[e.ColumnIndex].Name;
            if (colName == "Edit") // Nếu người dùng nhấn vào cột "Edit" để chỉnh sửa sản phẩm
            {
                // Mở form ProductModule để chỉnh sửa sản phẩm
                ProductModule module = new ProductModule(this);
                module.lblPcode.Text = dgvProduct.Rows[e.RowIndex].Cells[1].Value.ToString();
                module.txtName.Text = dgvProduct.Rows[e.RowIndex].Cells[2].Value.ToString();
                module.txttype.Text = dgvProduct.Rows[e.RowIndex].Cells[3].Value.ToString();
                module.cbCategory.Text = dgvProduct.Rows[e.RowIndex].Cells[4].Value.ToString();
                module.txtQty.Text = dgvProduct.Rows[e.RowIndex].Cells[5].Value.ToString();
                module.txtPrice.Text = dgvProduct.Rows[e.RowIndex].Cells[6].Value.ToString();

                module.btnSave.Enabled = false;
                module.btnUpdate.Enabled = true;
                module.ShowDialog();
            }
            else if (colName == "Delete") // Nếu người dùng nhấn vào cột "Delete" để xóa sản phẩm
            {
                if (MessageBox.Show("Are you sure you want to delete this item?", "Delete Records", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // Gọi phương thức DeleteProduct trong ProductBL để xóa sản phẩm
                    productBL.DeleteProduct(dgvProduct.Rows[e.RowIndex].Cells[1].Value.ToString());
                    MessageBox.Show("Item record has been successfully removed!", title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    LoadProduct();
                }
            }
        }

        #region Mehtod
        // Phương thức LoadProduct để tải danh sách sản phẩm từ tầng BusinessLayer và hiển thị lên DataGridView
        public void LoadProduct()
        {
            dgvProduct.Rows.Clear();
            var products = productBL.GetProducts(txtSearch.Text);
            int i = 0; // Biến đếm số thứ tự sản phẩm
            foreach (var product in products)
            {
                i++;
                // Thêm thông tin sản phẩm vào DataGridView
                dgvProduct.Rows.Add(i, product.PCode, product.PName, product.PType, product.PCategory, product.PQty, product.PPrice);
            }
        }
        #endregion Mehtod
    }
}
