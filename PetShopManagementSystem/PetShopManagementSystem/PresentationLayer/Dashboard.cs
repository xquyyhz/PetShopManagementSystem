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

namespace PresentationLayer
{
    public partial class Dashboard : Form
    {
        private DashboardBL dashboardBL = new DashboardBL();

        public Dashboard()
        {
            InitializeComponent();
            loadDailySale(); // Tải thông tin doanh thu hàng ngày ngay khi form được khởi tạo
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            // Lấy dữ liệu số lượng sản phẩm cho các loại động vật và hiển thị lên các label
            lblDog.Text = dashboardBL.ExtractData("Dog").ToString();
            lblCat.Text = dashboardBL.ExtractData("Cat").ToString();
            lblBird.Text = dashboardBL.ExtractData("Bird").ToString();
            lblFish.Text = dashboardBL.ExtractData("Fish").ToString();
        }

        #region Method

        // Phương thức loadDailySale để tải doanh thu của ngày hiện tại
        public void loadDailySale()
        {
            string sdate = DateTime.Now.ToString("yyyyMMdd");
            double total = dashboardBL.GetDailySale(sdate);
            lblDailySale.Text = total.ToString("#,##");
        }
        #endregion Method
    }
}
