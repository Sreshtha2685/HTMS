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
    public class CityService : ICityService
    {
        private readonly UnitOfWork _unitOfWork;

        /// <summary>
        /// Public constructor.
        /// </summary>
        public CityService()
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
        public City CreateCity(City CityEntity)
        {
            using (var scope = new TransactionScope())
            {
                var State = new City
                {
                    Id = CityEntity.Id,
                    CityName = CityEntity.CityName,
                    StateId = CityEntity.StateId,
                    CountryId = CityEntity.CountryId,
                    InsertedOn = CityEntity.InsertedOn,
                    InsertedBy = CityEntity.InsertedBy,
                    IsActive = CityEntity.IsActive,
                    // IsChecked = StateEntity.IsChecked,
                    IsDelete = CityEntity.IsDelete,

                };
                _unitOfWork.CityRepository.Insert(CityEntity);
                _unitOfWork.Save();
                scope.Complete();
                return CityEntity;
            }
        }

        public bool DeleteCity(int Id)
        {
            var success = false;
            if (Id >= 0)
            {
                using (var scope = new TransactionScope())
                {
                    var State = _unitOfWork.CityRepository.GetByID(Id);
                    if (State != null)
                    {
                        _unitOfWork.CityRepository.Delete(State);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }

        public IEnumerable<City> GetAllCity()
        {
            var City = _unitOfWork.CityRepository.GetAll().ToList();
            if (City.Any())
            {
                return City;
            }
            return null;
        }

        public City GetCityById(int Id)
        {
            var City = _unitOfWork.CityRepository.GetByID(Id);
            if (City != null)
            {
                return City;
            }
            return null;
        }

        public City UpdateCity(int Id, City CityEntity)
        {
            City city = null;
            if (CityEntity != null)
            {
                using (var scope = new TransactionScope())
                {
                    city = _unitOfWork.CityRepository.GetByID(Id);
                    if (city != null)
                    {
                        city.Id = CityEntity.Id;
                        city.CityName = CityEntity.CityName;
                        city.StateId = CityEntity.StateId;
                        city.CountryId = CityEntity.CountryId;



                        _unitOfWork.CityRepository.Update(city);
                        _unitOfWork.Save();
                        scope.Complete();
                    }
                }
            }
            return city;
        }
    }
}
