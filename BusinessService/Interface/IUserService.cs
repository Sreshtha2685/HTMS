using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Interface
{
  public interface IUserService
    {
        User GetUserById(int Id);
        IEnumerable<User> GetAllUser();
        User CreateUser(User UserEntity);
        User UpdateUser(int Id, User UserEntity);
        bool DeleteUser(int Id);
    }
}
