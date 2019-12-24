
using DataModel;
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
    public class CountryService : ICountryService
    {
        private readonly UnitOfWork _unitOfWork;

        /// <summary>
        /// Public constructor.
        /// </summary>
        public CountryService()
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
        public Country CreateCountry(Country CountryEntity)
        {
            using (var scope = new TransactionScope())
            {
                var Country = new Country
                {
                    Id = CountryEntity.Id,
                    CountryName = CountryEntity.CountryName,
                    Description = CountryEntity.Description,
                  
                };
                _unitOfWork.CountryRepository.Insert(CountryEntity);
                _unitOfWork.Save();
                scope.Complete();
                return CountryEntity;
            }
        }

        public bool DeleteCountry(int Id)
        {
            var success = false;
            if (Id >= 0)
            {
                using (var scope = new TransactionScope())
                {
                    var Country = _unitOfWork.CountryRepository.GetByID(Id);
                    if (Country != null)
                    {
                        _unitOfWork.CountryRepository.Delete(Country);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }

        public IEnumerable<Country> GetAllCountry()
        {
            var Country = _unitOfWork.CountryRepository.GetAll().ToList();
            if (Country.Any())
            {
                return Country;
            }
            return null;
        }

        public Country GetCountryById(int Id)
        {
            var Country = _unitOfWork.CountryRepository.GetByID(Id);
            if (Country != null)
            {
                return Country;
            }
            return null;
        }

        public Country UpdateCountry(int Id, Country CountryEntity)
        {
            Country country = null;
            if (CountryEntity != null)
            {
                using (var scope = new TransactionScope())
                {
                    country = _unitOfWork.CountryRepository.GetByID(Id);
                    if (country != null)
                    {
                        country.Id = CountryEntity.Id;
                        country.CountryName = CountryEntity.CountryName;
                        country.Description = CountryEntity.Description;
                       
                        _unitOfWork.CountryRepository.Update(country);
                        _unitOfWork.Save();
                        scope.Complete();
                    }
                }
            }
            return country;
        }


    }
}
