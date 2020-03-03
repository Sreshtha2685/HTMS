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
    public class RoomService : IRoomService
    {
        private readonly UnitOfWork _unitOfWork;

        /// <summary>
        /// Public constructor.
        /// </summary>
        public RoomService()
        {
            _unitOfWork = new UnitOfWork();
        }

        /// <summary>
        /// Fetches RoomService by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>

        /// <summary>
        /// Creates a RoomService
        /// </summary>
        /// <param name="RoomEntity"></param>
        /// <returns></ret
        public Room CreateRoom(Room RoomEntity)
        {
            using (var scope = new TransactionScope())
            {
                var State = new Room
                {
                    id = RoomEntity.id,
                    RoomNumber = RoomEntity.RoomNumber,
                    RoomTypeId = RoomEntity.RoomTypeId,
                    BedId = RoomEntity.BedId,
                    RoomStatusId = RoomEntity.RoomStatusId,
                    FloorId = RoomEntity.FloorId,
                    HotelId = RoomEntity.HotelId,
                    Description = RoomEntity.Description,
                    InsertedOn = RoomEntity.InsertedOn,
                    InsertedBy = RoomEntity.InsertedBy,
                    IsActive = RoomEntity.IsActive,
                    IsDelete = RoomEntity.IsDelete,

                };
                _unitOfWork.RoomRepository.Insert(RoomEntity);
                _unitOfWork.Save();
                scope.Complete();
                return RoomEntity;
            }
        }

        public bool DeleteRoom(int Id)
        {
            var success = false;
            if (Id >= 0)
            {
                using (var scope = new TransactionScope())
                {
                    var Room = _unitOfWork.RoomRepository.GetByID(Id);
                    if (Room != null)
                    {
                        _unitOfWork.RoomRepository.Delete(Room);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }

        public IEnumerable<Room> GetAllRoom()
        {
            var Room = _unitOfWork.RoomRepository.GetAll().ToList();
            if (Room.Any())
            {
                return Room;
            }
            return null;
        }

        public Room GetRoomById(int Id)
        {
            var Room = _unitOfWork.RoomRepository.GetByID(Id);
            if (Room != null)
            {
                return Room;
            }
            return null;
        }

        public Room UpdateRoom(int Id, Room RoomEntity)
        {
            Room room = null;
            if (RoomEntity != null)
            {
                using (var scope = new TransactionScope())
                {
                    room = _unitOfWork.RoomRepository.GetByID(Id);
                    if (room != null)
                    {
                        room.id = RoomEntity.id;
                        room.RoomNumber = RoomEntity.RoomNumber;
                        room.RoomStatusId = RoomEntity.RoomStatusId;
                        room.RoomTypeId = RoomEntity.RoomTypeId;
                        room.BedId = RoomEntity.BedId;
                        room.FloorId = RoomEntity.FloorId;
                        room.HotelId = RoomEntity.HotelId;
                        room.Description = RoomEntity.Description;
                        _unitOfWork.RoomRepository.Update(room);
                        _unitOfWork.Save();
                        scope.Complete();
                    }
                }
            }
            return room;
        }
    }
}
