using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using TransferObject;

namespace BusinessLayer
{
    public class UserBL
    {
        // Đối tượng UserDL để truy cập các phương thức xử lý dữ liệu người dùng từ cơ sở dữ liệu
        private UserDL userDL = new UserDL();

        // Phương thức AddUser để thêm một người dùng mới vào cơ sở dữ liệu
        public bool AddUser(AccountUser user)
        {
            return userDL.InsertUser(user) > 0;
        }

        // Phương thức GetUsers để lấy danh sách người dùng từ cơ sở dữ liệu, có thể tìm kiếm theo từ khóa
        public List<AccountUser> GetUsers(string keyword = "")
        {
            return userDL.GetUsers(keyword);
        }

        // Phương thức DeleteUser để xóa một người dùng khỏi cơ sở dữ liệu theo ID
        public bool DeleteUser(int id)
        {
            return userDL.DeleteUser(id) > 0;
        }

        // Phương thức UpdateUser để cập nhật thông tin của người dùng trong cơ sở dữ liệu
        public bool UpdateUser(AccountUser user)
        {
            return userDL.UpdateUser(user) > 0;
        }
    }
}
