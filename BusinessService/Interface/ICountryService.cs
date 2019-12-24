

using DataModel;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Interface
{
   public interface ICountryService
    {
        Country GetCountryById(int Id);
        IEnumerable<Country> GetAllCountry();
        Country CreateCountry(Country CountryEntity);
        Country UpdateCountry(int Id, Country CountryEntity);
        bool DeleteCountry(int Id);
    }
}
