using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PresentationLayer
{
    public partial class MainForm : Form
    {
        Dashboard dashboard = new Dashboard();
        private System.Windows.Forms.Timer timer;

        public MainForm()
        {
            InitializeComponent();
        }

        // Phương thức xử lý sự kiện khi form MainForm được tải
        private void MainForm_Load(object sender, EventArgs e)
        {
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000; // 1 giây
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        // Xử lý Tick (thay thế Elapsed)
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (!progress.IsDisposed)
            {
                progress.Text = DateTime.Now.ToString("hh:mm:ss");
                progress.Value = DateTime.Now.Second;
            }
        }

        // Phương thức xử lý sự kiện khi nhấn nút "Close" để thoát khỏi ứng dụng
        private void btnClose_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Exit Application", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        // Phương thức xử lý sự kiện khi nhấn nút "Logout" để đăng xuất
        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Logout Application", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                LoginForm login = new LoginForm();
                this.Dispose();
                login.ShowDialog();
            }
        }

        // Phương thức xử lý sự kiện khi nhấn nút "Dashboard" để mở form Dashboard
        private void btnDashboard_Click(object sender, EventArgs e)
        {
             openChildForm(new Dashboard());
        }

        // Phương thức xử lý sự kiện khi nhấn nút "Customer" để mở form CustomerForm
        private void btnCustomer_Click(object sender, EventArgs e)
        {
            openChildForm(new CustomerForm());
        }

        // Phương thức xử lý sự kiện khi nhấn nút "User" để mở form UserForm
        private void btnUser_Click(object sender, EventArgs e)
        {
            openChildForm(new UserForm());
        }

        // Phương thức xử lý sự kiện khi nhấn nút "Product" để mở form ProductForm
        private void btnProduct_Click(object sender, EventArgs e)
        {
            openChildForm(new ProductForm());
        }

        // Phương thức xử lý sự kiện khi nhấn nút "Cash" để mở form CashForm
        private void btnCash_Click(object sender, EventArgs e)
        {
            openChildForm(new CashForm(this, dashboard));
        }

        // Phương thức xử lý sự kiện khi nhấn nút "Report" để mở form ReportForm
        private void btnReport_Click(object sender, EventArgs e)
        {
            ReportForm reportForm = new ReportForm();
            reportForm.ShowDialog();
        }

        #region Method
        // Biến lưu trữ form con đang hoạt động
        private Form activeForm = null;

        // Phương thức openChildForm dùng để mở một form con trong panelChild
        public void openChildForm(Form childForm)
        {
            // Kiểm tra nếu đã có form con đang mở
            if (activeForm != null)
                activeForm.Close(); // Đóng form con hiện tại
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            lblTitle.Text = childForm.Text;
            panelChild.Controls.Add(childForm);
            panelChild.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        #endregion Method
        
    }
}
