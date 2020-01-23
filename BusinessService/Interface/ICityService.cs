using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Interface
{
  public interface ICityService
    {
        City GetCityById(int Id);
        IEnumerable<City> GetAllCity();
        City CreateCity(City CityEntity);
        City UpdateCity(int Id, City CityEntity);
        bool DeleteCity(int Id);
    }
}
