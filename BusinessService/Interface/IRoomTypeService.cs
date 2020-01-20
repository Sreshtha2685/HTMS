using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Interface
{
  public  interface IRoomTypeService
    {
        
        RoomType GetRoomTypeById(int Id);
        IEnumerable<RoomType> GetAllRoomType();
        RoomType CreateRoomType(RoomType RoomTypeEntity);
        //  RoomTypes UpdateRoomType(int Id, RoomTypes RoomTypeEntity);

        RoomType UpdateRoomType(int Id, RoomType RoomTypeEntity);
        bool DeleteRoomType(int Id);
    }
}
