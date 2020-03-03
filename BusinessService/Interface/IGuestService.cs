using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Interface
{
   public interface IGuestService
    {
        Guest GetGuestById(int Id);
        IEnumerable<Guest> GetAllGuest();
        Guest CreateGuest(Guest GuestEntity);
        Guest UpdateGuest(int Id, Guest GuestEntity);
        bool DeleteGuest(int Id);
    }
}
