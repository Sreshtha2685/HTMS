using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Interface
{
 public  interface IRoomService
    {
        Room GetRoomById(int Id);
        IEnumerable<Room> GetAllRoom();
        Room CreateRoom(Room RoomEntity);
        Room UpdateRoom(int Id, Room RoomEntity);
        bool DeleteRoom(int Id);
    }
}
