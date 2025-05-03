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
    public partial class ReportForm: Form
    {
        private RevenueBL revenueBL = new RevenueBL();

        public ReportForm()
        {
            InitializeComponent();
            chartRevenue.Visible = false;
            dgvRevenue.Visible = false;// Ẩn biểu đồ lúc đầu
        }

        // Phương thức loadRevenueChart dùng để hiển thị biểu đồ doanh thu theo loại sản phẩm
        private void LoadRevenueChart(List<Chart> list)
        {
            chartRevenue.Series.Clear();

            // Tạo một series mới cho biểu đồ với kiểu Pie (biểu đồ tròn)
            var series = new System.Windows.Forms.DataVisualization.Charting.Series("Revenue by type")
            {
                ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie,
                IsValueShownAsLabel = true,
                Font = new Font("Century Gothic", 9, FontStyle.Bold),
            };

            // Biến để lưu tổng doanh thu của các phần nhỏ
            decimal otherRevenue = 0;

            // Duyệt qua các sản phẩm trong danh sách
            foreach (var item in list)
            {
                if (item.Revenue < 500) // Nếu doanh thu nhỏ hơn 200 (hoặc giá trị bạn chọn)
                {
                    otherRevenue += item.Revenue;  // Cộng doanh thu vào nhóm "Other"
                }
                else
                {
                    int pointIndex = series.Points.AddXY(item.Type, item.Revenue);
                    series.Points[pointIndex].Label = "#PERCENT";  // Hiển thị tỷ lệ phần trăm bên trong biểu đồ
                    series.Points[pointIndex].LegendText = $"{item.Type}: {"#PERCENT"} ({item.Revenue:N2})"; // Hiển thị trong legend: phần trăm và giá trị tiền
                }
            }

            // Nếu có các phần nhỏ, gộp chúng lại thành nhóm "Other"
            if (otherRevenue > 0)
            {
                // Thêm phần "Other" vào biểu đồ
                series.Points.AddXY("Other", otherRevenue);
                int otherIndex = series.Points.Count - 1;

                // Chỉ hiển thị tỷ lệ phần trăm cho nhóm "Other" bên trong biểu đồ
                series.Points[otherIndex].Label = $"{(otherRevenue / list.Sum(x => x.Revenue) * 100):N2}%";

                // Hiển thị cả tỷ lệ phần trăm và giá tiền cho nhóm "Other" trong phần chú thích
                series.Points[otherIndex].LegendText = $"Other: {(otherRevenue / list.Sum(x => x.Revenue) * 100):N2}% ({otherRevenue:N2})";
            }

            chartRevenue.Series.Add(series);
            chartRevenue.Legends[0].Enabled = true; // Hiển thị chú thích
            chartRevenue.Legends[0].Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Right;
        }

        // Phương thức xử lý sự kiện khi nhấn nút "Show" để hiển thị báo cáo doanh thu
        private void btnShow_Click(object sender, EventArgs e)
        {
            // Kiểm tra nếu chưa chọn tháng từ combobox
            if (cbMonth.SelectedIndex < 0)
            {
                MessageBox.Show("Please choose the month to see the revenue.", "Pet Shop Management System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return; // Dừng phương thức nếu không chọn tháng
            }

            // Lấy tháng đã chọn (index cộng thêm 1)
            int selectedMonth = cbMonth.SelectedIndex + 1;

            decimal totalRevenue = revenueBL.GetTotalRevenueByMonth(selectedMonth);
            totalAmountLabel.Text = $"{totalRevenue}"; // Hiển thị tổng doanh thu


            if (cbRevenue.SelectedItem != null && cbRevenue.SelectedItem.ToString() == "Date")
            {
                List<Revenue> data = revenueBL.GetDailyRevenueByMonth(selectedMonth);
                dgvRevenue.Visible = true;
                chartRevenue.Visible = false;
                dgvRevenue.DataSource = data; // Liên kết dữ liệu với DataGridView
            }
            else if (cbRevenue.SelectedItem != null && cbRevenue.SelectedItem.ToString() == "Chart")
            {
                // Lấy doanh thu theo loại sản phẩm của tháng đã chọn và hiển thị biểu đồ
                List<Chart> data = revenueBL.GetRevenueByProductTypeAndMonth(selectedMonth);
                dgvRevenue.Visible = false;
                chartRevenue.Visible = true;
                LoadRevenueChart(data);
            }
        }

        // Phương thức xử lý sự kiện khi nhấn nút "Close" để đóng form
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
