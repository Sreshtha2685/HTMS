using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Interface
{
  public  interface IFloorService
    {
        Floor GetFloorById(int Id);
        IEnumerable<Floor> GetAllFloor();
        Floor CreateFloor(Floor FloorEntity);

        Floor UpdateFloor(int Id, Floor FloorEntity);
        bool DeleteFloor(int Id);
    }
}
