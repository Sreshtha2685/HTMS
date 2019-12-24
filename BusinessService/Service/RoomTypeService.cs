using DataModel;
using BusinessService.Interface;
using DataModel.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

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

        public RoomType UpdateRoomType(int Id, RoomType RoomTypeEntity)
        {
            RoomType roomtype = null;
            if (RoomTypeEntity != null)
            {
                using (var scope = new TransactionScope())
                {
                    roomtype = _unitOfWork.RoomTypeRepository.GetByID(Id);
                    if (roomtype != null)
                    {
                        roomtype.id = RoomTypeEntity.id;
                        roomtype.RoomName = RoomTypeEntity.RoomName;
                        roomtype.Description = RoomTypeEntity.Description;
                        roomtype.Max_Adult_No= RoomTypeEntity.Max_Adult_No;
                        roomtype.Max_Child_No = RoomTypeEntity.Max_Child_No;
                        roomtype.RoomStatusId = RoomTypeEntity.RoomStatusId;
                        roomtype.Code = RoomTypeEntity.Code;
                        roomtype.BedId = RoomTypeEntity.BedId;



                        _unitOfWork.RoomTypeRepository.Update(roomtype);
                        _unitOfWork.Save();
                        scope.Complete();
                    }
                }
            }
            return roomtype;
        }
    }
}
