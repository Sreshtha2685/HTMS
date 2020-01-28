using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Interface
{
   public interface IStatusService
    {
        Status GetStatusById(int Id);
        
        IEnumerable<Status> GetAllStatus();
        Status CreateStatus(Status StatusEntity);
       
        Status UpdateStatus(int Id, Status StatusEntity);
        bool DeleteStatus(int Id);
    }
}
