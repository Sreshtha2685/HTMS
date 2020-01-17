using DataModel;
using BusinessService.Interface;
using DataModel.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Http;
using System.Data.Entity;

namespace BusinessService.Service
{
    public class RoomTypeService : IRoomTypeService
    {
        private readonly UnitOfWork _unitOfWork;

        /// <summary>
        /// Public constructor.
        /// </summary>
        public RoomTypeService()
        {
            _unitOfWork = new UnitOfWork();
        }

        /// <summary>
        /// Fetches RoomType by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>

        /// <summary>
        /// Creates a RoomType
        /// </summary>
        /// <param name="RoomTypeEntity"></param>
        /// <returns></returns>
        public RoomType CreateRoomType(RoomType RoomTypeEntity)
        {
            using (var scope = new TransactionScope())
            {
                var RoomType = new RoomType
                {
                    id = RoomTypeEntity.id,
                    BedId = RoomTypeEntity.BedId,
                    Description = RoomTypeEntity.Description,
                    Code=RoomTypeEntity.Code,
                    Max_Adult_No=RoomTypeEntity.Max_Adult_No,
                    Max_Child_No=RoomTypeEntity.Max_Child_No,
                    RoomStatusId=RoomTypeEntity.RoomStatusId,
                    RoomName=RoomTypeEntity.RoomName,
                    InsertedOn=RoomTypeEntity.InsertedOn,
                    InsertedBy=RoomTypeEntity.InsertedBy,
                    IsActive=RoomTypeEntity.IsActive,
                    IsDelete=RoomTypeEntity.IsDelete
                    

                };
                _unitOfWork.RoomTypeRepository.Insert(RoomTypeEntity);
                _unitOfWork.Save();
                scope.Complete();
                return RoomTypeEntity;
            }
        }

        public bool DeleteRoomType(int Id)
        {
            var success = false;
            if (Id >= 0)
            {
                using (var scope = new TransactionScope())
                {
                    var RoomType = _unitOfWork.RoomTypeRepository.GetByID(Id);
                    if (RoomType != null)
                    {
                        _unitOfWork.RoomTypeRepository.Delete(RoomType);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }

        public IEnumerable<RoomType> GetAllRoomType()
        {
            var RoomType = _unitOfWork.RoomTypeRepository.GetAll().ToList();
            if (RoomType.Any())
            {
                return RoomType;
            }
            return null;
        }

        public RoomType GetRoomTypeById(int Id)
        {
            var RoomType = _unitOfWork.RoomTypeRepository.GetByID(Id);
            if (RoomType != null)
            {
                return RoomType;
            }
            return null;
        }

        public RoomType UpdateRoomType(int id, RoomType RoomTypeEntity)
        {
            RoomType Room = null;
            if (RoomTypeEntity != null)
            {
                using (var scope = new TransactionScope())
                {
                    Room = _unitOfWork.RoomTypeRepository.GetByID(id);
                    if (Room != null)
                    {
                        Room.id = RoomTypeEntity.id;
                        Room.BedId = RoomTypeEntity.BedId;
                        Room.RoomName= RoomTypeEntity.RoomName;
                        Room.Max_Adult_No = RoomTypeEntity.Max_Adult_No;
                        Room.Max_Child_No = RoomTypeEntity.Max_Child_No;
                        Room.Description = RoomTypeEntity.Description;
                        _unitOfWork.RoomTypeRepository.Update(Room);
                        _unitOfWork.Save();
                        scope.Complete();
                    }
                }
            }
            return Room;
        }


    }
}
