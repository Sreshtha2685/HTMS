using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Interface
{
  public  interface IBedService
    {
        Bed GetBedById(int Id);
        IEnumerable<Bed> GetAllBed();
        Bed CreateBed(Bed BedEntity);
        //  RoomTypes UpdateRoomType(int Id, RoomTypes RoomTypeEntity);

        Bed UpdateBed(int Id, Bed BedEntity);
        bool DeleteBed(int Id);
    }
}
