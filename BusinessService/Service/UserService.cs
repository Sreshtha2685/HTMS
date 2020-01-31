using BusinessService.Interface;
using DataModel;
using DataModel.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace BusinessService.Service
{

    public class UserService : IUserService
    {
        private readonly UnitOfWork _unitOfWork;

        /// <summary>
        /// Public constructor.
        /// </summary>
        public UserService()
        {
            _unitOfWork = new UnitOfWork();
        }

        /// <summary>
        /// Fetches AddressLink by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>

        /// <summary>
        /// Creates a AddressLink
        /// </summary>
        /// <param name="UserEntity"></param>
        /// <returns></returns>
        public User CreateUser(User UserEntity)
        {
            using (var scope = new TransactionScope())
            {
                var User = new User
                {
                    UserId = UserEntity.UserId,
                    UserRole = UserEntity.UserRole,
                    UserName =UserEntity.UserName,
                    Password = UserEntity.Password,
                    Description = UserEntity.Description,
                    InsertedOn = UserEntity.InsertedOn,
                    InsertedBy = UserEntity.InsertedBy,
                    IsActive = UserEntity.IsActive,
                   
                    IsDelete = UserEntity.IsDelete,

                };
                _unitOfWork.UserRepository.Insert(UserEntity);
                _unitOfWork.Save();
                scope.Complete();
                return UserEntity;
            }
        }

        public bool DeleteUser(int Id)
        {
            var success = false;
            if (Id >= 0)
            {
                using (var scope = new TransactionScope())
                {
                    var User = _unitOfWork.UserRepository.GetByID(Id);
                    if (User != null)
                    {
                        _unitOfWork.UserRepository.Delete(User);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }

        public IEnumerable<User> GetAllUser()
        {
            var User = _unitOfWork.UserRepository.GetAll().ToList();
            if (User.Any())
            {
                return User;
            }
            return null;
        }

        public User GetUserById(int Id)
        {
            var User = _unitOfWork.UserRepository.GetByID(Id);
            if (User != null)
            {
                return User;
            }
            return null;
        }

        public User UpdateUser(int Id, User UserEntity)
        {
            User user = null;
            if (UserEntity != null)
            {
                using (var scope = new TransactionScope())
                {
                    user = _unitOfWork.UserRepository.GetByID(Id);
                    if (user != null)
                    {
                        user.UserId = UserEntity.UserId;
                        user.UserRole = UserEntity.UserRole;
                        user.Description = UserEntity.Description;
                        user.UserName = UserEntity.UserName;
                        user.Password = UserEntity.Password;
                        _unitOfWork.UserRepository.Update(user);
                        _unitOfWork.Save();
                        scope.Complete();
                    }
                }
            }
            return user;
        }
    }
}
