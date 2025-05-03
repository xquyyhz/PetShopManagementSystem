using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PresentationLayer
{
    public partial class SplashForm : Form
    {
        public SplashForm()
        {
            InitializeComponent();
        }

        // Biến lưu trữ giá trị điểm bắt đầu của thanh tiến trình (progress bar)
        int startPoint = 0;

        // Phương thức xử lý sự kiện khi timer tick (mỗi lần đồng hồ đếm giây)
        private void timer1_Tick(object sender, EventArgs e)
        {
            startPoint += 2; // Tăng giá trị điểm bắt đầu của thanh tiến trình lên 2 mỗi lần tick
            guna2ProgressBar1.Value = startPoint;

            // Nếu thanh tiến trình đạt 100%, dừng timer và hiển thị form Login
            if (guna2ProgressBar1.Value == 100)
            {
                guna2ProgressBar1.Value = 0;
                timer1.Stop();
                LoginForm login = new LoginForm();
                login.ShowDialog();
                this.Hide();
            }
        }

        // Phương thức xử lý sự kiện khi form SplashForm được tải
        private void SplashForm_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }
    }
}
