using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferObject
{
    public class AccountUser
    {
        // Các thuộc tính của người dùng
        public int Id { get; set; } // Mã ID của người dùng
        public string Name { get; set; } // Tên người dùng
        public string Password { get; set; } // Mật khẩu của người dùng
        public string Address { get; set; } // Địa chỉ của người dùng
        public string Phone { get; set; } // Số điện thoại của người dùng
        public string Role { get; set; } // Vai trò của người dùng (admin, cashier, employee)
        public DateTime Dob { get; set; } // Ngày sinh của người dùng

        // Constructor mặc định
        public AccountUser() { }

        // Constructor với tên và mật khẩu
        public AccountUser(string name, string password)
        {
            this.Name = name;
            this.Password = password;
        }
    }
}
