using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BusinessLayer;
using TransferObject;

namespace PresentationLayer
{
    public partial class InvoicePrintForm : Form
    {
        private PrintDocument printDocument = new PrintDocument();
        private Invoice invoice;
        public InvoicePrintForm(string transno)
        {
            InitializeComponent();
            InvoiceBL bl = new InvoiceBL();
            invoice = bl.GetInvoice(transno);

            // Đăng ký sự kiện PrintPage để xử lý việc in hóa đơn
            printDocument.PrintPage += new PrintPageEventHandler(printDocument1_PrintPage);
            ShowPrintPreview();
        }

        // Phương thức hiển thị bản xem trước của hóa đơn
        private void ShowPrintPreview()
        {
            PrintPreviewDialog previewDialog = new PrintPreviewDialog();
            previewDialog.Document = printDocument;
            previewDialog.ShowDialog();
        }

        // Phương thức xử lý sự kiện khi in hóa đơn (PrintPage)
        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            Font headerFont = new Font("Arial", 20, FontStyle.Bold);
            Font bodyFont = new Font("Arial", 14);
            Font boldFont = new Font("Arial", 14, FontStyle.Bold);
            Brush titleBrush = Brushes.DarkBlue;
            int y = 20;

            int pageWidth = e.PageBounds.Width;

            // Tiêu đề căn giữa
            string title = "PET SHOP - INVOICE";
            SizeF titleSize = g.MeasureString(title, headerFont);
            float titleX = (pageWidth - titleSize.Width) / 2;

            g.DrawString(title, headerFont, titleBrush, titleX, y); y += 40;
            g.DrawLine(Pens.Black, 10, y, pageWidth - 10, y);  // Vẽ đường kẻ dưới tiêu đề
            y += 30;


            // Thông tin hóa đơn
            g.DrawString($"Transaction no: {invoice.TransNo}", bodyFont, Brushes.Black, 10, y); y += 25;
            g.DrawString($"Customer: {invoice.CustomerName}", bodyFont, Brushes.Black, 10, y); y += 25;
            g.DrawString($"Date: {invoice.Date:dd/MM/yyyy HH:mm}", bodyFont, Brushes.Black, 10, y); y += 25;
            g.DrawString($"Cashier: {invoice.CashierName}", bodyFont, Brushes.Black, 10, y); y += 35;

            
            // Khởi tạo độ rộng theo tiêu đề
            int colPCodeW = (int)g.MeasureString("Product ID", bodyFont).Width;
            int colPNameW = (int)g.MeasureString("Product Name", bodyFont).Width;
            int colPriceW = (int)g.MeasureString("Price", bodyFont).Width;
            int colQtyW = (int)g.MeasureString("Qty", bodyFont).Width;
            int colTotalW = (int)g.MeasureString("Total", bodyFont).Width;

            // Tính độ rộng lớn nhất cho mỗi cột dựa trên dữ liệu
            foreach (var item in invoice.Details)
            {
                colPCodeW = Math.Max(colPCodeW, (int)g.MeasureString(item.PCode, bodyFont).Width);
                colPNameW = Math.Max(colPNameW, (int)g.MeasureString(item.PName, bodyFont).Width);
                colPriceW = Math.Max(colPriceW, (int)g.MeasureString($"{item.Price:N0}", bodyFont).Width);
                colQtyW = Math.Max(colQtyW, (int)g.MeasureString($"{item.Qty}", bodyFont).Width);
                colTotalW = Math.Max(colTotalW, (int)g.MeasureString($"{item.Total:N0}", bodyFont).Width);
            }

            // Thêm padding 10px cho mỗi cột
            colPCodeW += 10;
            colPNameW += 10;
            colPriceW += 10;
            colQtyW += 10;
            colTotalW += 10;

            // Thiết lập bảng
            int tableStartX = (pageWidth - (colPCodeW + colPNameW + colPriceW + colQtyW + colTotalW)) / 2;
            int rowHeight = 30;

            // Header bảng
            g.DrawRectangle(Pens.Black, tableStartX, y, colPCodeW, rowHeight);
            g.DrawRectangle(Pens.Black, tableStartX + colPCodeW, y, colPNameW, rowHeight);
            g.DrawRectangle(Pens.Black, tableStartX + colPCodeW + colPNameW, y, colPriceW, rowHeight);
            g.DrawRectangle(Pens.Black, tableStartX + colPCodeW + colPNameW + colPriceW, y, colQtyW, rowHeight);
            g.DrawRectangle(Pens.Black, tableStartX + colPCodeW + colPNameW + colPriceW + colQtyW, y, colTotalW, rowHeight);

            g.DrawString("Product ID", boldFont, Brushes.Black, tableStartX + 5, y + 5);
            g.DrawString("Product Name", boldFont, Brushes.Black, tableStartX + colPCodeW + 5, y + 5);
            g.DrawString("Price", boldFont, Brushes.Black, tableStartX + colPCodeW + colPNameW + 5, y + 5);
            g.DrawString("Qty", boldFont, Brushes.Black, tableStartX + colPCodeW + colPNameW + colPriceW + 5, y + 5);
            g.DrawString("Total", boldFont, Brushes.Black, tableStartX + colPCodeW + colPNameW + colPriceW + colQtyW + 5, y + 5);

            y += rowHeight;

            // Dữ liệu sản phẩm
            foreach (var item in invoice.Details)
            {
                g.DrawRectangle(Pens.Black, tableStartX, y, colPCodeW, rowHeight);
                g.DrawRectangle(Pens.Black, tableStartX + colPCodeW, y, colPNameW, rowHeight);
                g.DrawRectangle(Pens.Black, tableStartX + colPCodeW + colPNameW, y, colPriceW, rowHeight);
                g.DrawRectangle(Pens.Black, tableStartX + colPCodeW + colPNameW + colPriceW, y, colQtyW, rowHeight);
                g.DrawRectangle(Pens.Black, tableStartX + colPCodeW + colPNameW + colPriceW + colQtyW, y, colTotalW, rowHeight);

                g.DrawString(item.PCode, bodyFont, Brushes.Black, tableStartX + 5, y + 5);
                g.DrawString(item.PName, bodyFont, Brushes.Black, tableStartX + colPCodeW + 5, y + 5);
                g.DrawString($"{item.Price:N0}", bodyFont, Brushes.Black, tableStartX + colPCodeW + colPNameW + 5, y + 5);
                g.DrawString($"{item.Qty}", bodyFont, Brushes.Black, tableStartX + colPCodeW + colPNameW + colPriceW + 5, y + 5);
                g.DrawString($"{item.Total:N0}", bodyFont, Brushes.Black, tableStartX + colPCodeW + colPNameW + colPriceW + colQtyW + 5, y + 5);

                y += rowHeight;
            }

            y += 20;
            string totalText = $"Total: {invoice.Total:N0} USD";
            SizeF totalTextSize = g.MeasureString(totalText, boldFont);

            // Tổng chiều rộng của bảng
            int tableWidth = colPCodeW + colPNameW + colPriceW + colQtyW + colTotalW;

            // Tính vị trí X để chuỗi nằm sát lề phải bảng
            float totalTextX = tableStartX + tableWidth - totalTextSize.Width;

            g.DrawString(totalText, boldFont, Brushes.Black, totalTextX, y);
            y += 35;
            // Cảm ơn khách hàng
            string thank = "THANKS FOR CHOOSING US!";
            SizeF thankSize = g.MeasureString(thank, bodyFont);
            g.DrawString(thank, bodyFont, Brushes.Black, (pageWidth - thankSize.Width) / 2, y);
        }
    }
}
