using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Interface
{
  public  interface IHotelService
    {
        Hotel GetHotelById(int Id);
        IEnumerable<Hotel> GetAllHotel();
        Hotel CreateHotel(Hotel HotelEntity);
        Hotel UpdateHotel(int Id, Hotel HotelEntity);
        bool DeleteHotel(int Id);

    }
}
