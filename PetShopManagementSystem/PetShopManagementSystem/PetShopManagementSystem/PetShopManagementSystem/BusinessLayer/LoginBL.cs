using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using TransferObject;

namespace BusinessLayer
{
    public class LoginBL
    {
        // Đối tượng LoginDL để truy cập các phương thức liên quan đến đăng nhập từ cơ sở dữ liệu
        private LoginDL loginDL;

        // Constructor khởi tạo đối tượng LoginDL
        public LoginBL()
        {
            loginDL = new LoginDL();
        }

        // Phương thức Login dùng để xác thực người dùng với tên đăng nhập và mật khẩu
        public AccountUser Login(AccountUser acc)
        {
            try
            {
                // Gọi phương thức Login trong LoginDL để thực hiện đăng nhập và trả về thông tin người dùng nếu đăng nhập thành công
                return loginDL.Login(acc.Name, acc.Password);
            }
            catch (Exception ex)
            {
                // Nếu có lỗi trong quá trình đăng nhập, ném ngoại lệ với thông báo lỗi
                throw new Exception("Login Failed: " + ex.Message);
            }
        }
    }
}
