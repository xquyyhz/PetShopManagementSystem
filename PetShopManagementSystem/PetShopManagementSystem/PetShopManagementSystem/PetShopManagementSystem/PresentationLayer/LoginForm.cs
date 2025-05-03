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
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        // Phương thức xử lý sự kiện khi nhấn nút "Login" để thực hiện đăng nhập
        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                AccountUser loginUser = new AccountUser()
                {
                    Name = txtName.Text.Trim(),
                    Password = txtPass.Text.Trim()
                };

                LoginBL loginBL = new LoginBL();
                AccountUser result = loginBL.Login(loginUser);

                if (result != null)
                {
                    MessageBox.Show("Welcome " + result.Name + "!", "ACCESS GRANTED", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MainForm main = new MainForm();
                    main.lblUsername.Text = result.Name;
                    main.lblRole.Text = result.Role;
                    if (result.Role == "Administrator")
                    {
                        main.btnUser.Enabled = true;
                        main.btnReport.Enabled = true;
                    }
                    this.Hide();
                    main.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Invalid username or password!", "ACCESS DENIED", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        // Phương thức xử lý sự kiện khi nhấn nút "Forget Password" (quên mật khẩu)
        private void btnForget_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Please contact your BOSS!", "FORGET PASSWORD", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Phương thức xử lý sự kiện khi nhấn Enter khi đang focus ô mật khẩu thì tự động đăng nhập
        private void txtPass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin.PerformClick(); // Gọi sự kiện click của button đăng nhập
            }
        }
    }
}
