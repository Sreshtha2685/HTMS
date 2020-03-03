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
    public class GuestService : IGuestService
    {
        private readonly UnitOfWork _unitOfWork;

        /// <summary>
        /// Public constructor.
        /// </summary>
        public GuestService()
        {
            _unitOfWork = new UnitOfWork();
        }

        /// <summary>
        /// Fetches GuestService by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>

        /// <summary>
        /// Creates a GuestService
        /// </summary>
        /// <param name="GuestServiceEntity"></param>
        /// <returns></return>
        public Guest CreateGuest(Guest GuestEntity)
        {
            using (var scope = new TransactionScope())
            {
                var State = new Guest
                {
                    Id = GuestEntity.Id,
                    GuestName =GuestEntity.GuestName,
                    GuestContactNumber =GuestEntity.GuestContactNumber,
                    GuestAddress =GuestEntity.GuestAddress,
                    IdProof = GuestEntity.IdProof,
                    RoomId =GuestEntity.RoomId,
                    ServiceTypeId=GuestEntity.ServiceTypeId,
                    Description = GuestEntity.Description,
                    InsertedOn = GuestEntity.InsertedOn,
                    InsertedBy = GuestEntity.InsertedBy,
                    IsActive = GuestEntity.IsActive,
                    IsDelete = GuestEntity.IsDelete,

                };
                _unitOfWork.GuestRepository.Insert(GuestEntity);
                _unitOfWork.Save();
                scope.Complete();
                return GuestEntity;
            }
        }

        public bool DeleteGuest(int Id)
        {
            var success = false;
            if (Id >= 0)
            {
                using (var scope = new TransactionScope())
                {
                    var Guest = _unitOfWork.GuestRepository.GetByID(Id);
                    if (Guest != null)
                    {
                        _unitOfWork.GuestRepository.Delete(Guest);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }

        public IEnumerable<Guest> GetAllGuest()
        {
            var Guest = _unitOfWork.GuestRepository.GetAll().ToList();
            if (Guest.Any())
            {
                return Guest;
            }
            return null;
        }

        public Guest GetGuestById(int Id)
        {

            var Guest = _unitOfWork.GuestRepository.GetByID(Id);
            if (Guest != null)
            {
                return Guest;
            }
            return null;
        }

        public Guest UpdateGuest(int Id, Guest GuestEntity)
        {
            Guest guest = null;
            if (GuestEntity != null)
            {
                using (var scope = new TransactionScope())
                {
                    guest = _unitOfWork.GuestRepository.GetByID(Id);
                    if (guest != null)
                    {
                        guest.Id = GuestEntity.Id;
                        guest.GuestName = GuestEntity.GuestName;
                        guest.GuestAddress = GuestEntity.GuestAddress;
                        guest.GuestContactNumber = GuestEntity.GuestContactNumber;
                        guest.IdProof = GuestEntity.IdProof;
                        guest.RoomId = GuestEntity.RoomId;
                        guest.ServiceTypeId = GuestEntity.ServiceTypeId;
                        guest.Description = GuestEntity.Description;
                        _unitOfWork.GuestRepository.Update(guest);
                        _unitOfWork.Save();
                        scope.Complete();
                    }
                }
            }
            return guest;
        }
    }
}
