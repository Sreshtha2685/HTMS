using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Interface
{
  public  interface IServiceTypeService
    {
        ServiceType GetServiceTypeById(int Id);
        IEnumerable<ServiceType> GetAllServiceType();
        ServiceType CreateServiceType(ServiceType ServiceTypeEntity);
        ServiceType UpdateServiceType(int Id, ServiceType ServiceTypeEntity);
        bool DeleteServiceType(int Id);

    }
}
