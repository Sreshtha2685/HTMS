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

    public class HotelService : IHotelService
    {
        private readonly UnitOfWork _unitOfWork;

        /// <summary>
        /// Public constructor.
        /// </summary>
        public HotelService()
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
        /// <param name="AddressLinkEntity"></param>
        /// <returns></returns>
        /// 
        public Hotel CreateHotel(Hotel HotelEntity)
        {
            using (var scope = new TransactionScope())
            {
                var Hotel = new Hotel
                {
                    id = HotelEntity.id,
                    Hotel_Name = HotelEntity.Hotel_Name,
                    Hotel_Number = HotelEntity.Hotel_Number,
                    Description =HotelEntity.Description,
                    InsertedOn = HotelEntity.InsertedOn,
                    InsertedBy = HotelEntity.InsertedBy,
                    IsActive = HotelEntity.IsActive,
                    IsDelete = HotelEntity.IsDelete,

                };
                _unitOfWork.HotelRepository.Insert(HotelEntity);
                _unitOfWork.Save();
                scope.Complete();
                return HotelEntity;
            }
        }

        public bool DeleteHotel(int Id)
        {
            var success = false;
            if (Id >= 0)
            {
                using (var scope = new TransactionScope())
                {
                    var Hotel = _unitOfWork.HotelRepository.GetByID(Id);
                    if (Hotel != null)
                    {
                        _unitOfWork.HotelRepository.Delete(Hotel);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }

        public IEnumerable<Hotel> GetAllHotel()
        {
            var Hotel = _unitOfWork.HotelRepository.GetAll().ToList();
            if (Hotel.Any())
            {
                return Hotel;
            }
            return null;
        }

        public Hotel GetHotelById(int Id)
        {
            var Hotel = _unitOfWork.HotelRepository.GetByID(Id);
            if (Hotel != null)
            {
                return Hotel;
            }
            return null;
        }

        public Hotel UpdateHotel(int Id, Hotel HotelEntity)
        {
            Hotel state = null;
            if (HotelEntity != null)
            {
                using (var scope = new TransactionScope())
                {
                    state = _unitOfWork.HotelRepository.GetByID(Id);
                    if (state != null)
                    {
                        state.id= HotelEntity.id;
                        state.Hotel_Number= HotelEntity.Hotel_Number;
                        state.Hotel_Name = HotelEntity.Hotel_Name;
                        state.InsertedBy = HotelEntity.InsertedBy;
                        state.InsertedOn = HotelEntity.InsertedOn;
                        state.IsActive = HotelEntity.IsActive;
                        state.IsDelete = HotelEntity.IsDelete;



                        _unitOfWork.HotelRepository.Update(state);
                        _unitOfWork.Save();
                        scope.Complete();
                    }
                }
            }
            return state;
        }
    }
}
