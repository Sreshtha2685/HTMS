using BusinessService.Interface;
using DataModel;
using DataModel.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace BusinessService.Service
{
    public class ServiceTypeService : IServiceTypeService
    {
        private readonly UnitOfWork _unitOfWork;

        /// <summary>
        /// Public constructor.
        /// </summary>
        public ServiceTypeService()
        {
            _unitOfWork = new UnitOfWork();
        }

        /// <summary>
        /// Fetches AddressLink by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>

        /// <summary>
        /// Creates a AddressLink
        /// </summary>
        /// <param name="AddressLinkEntity"></param>
        /// <returns></returns>
        public ServiceType CreateServiceType(ServiceType ServiceTypeEntity)
        {
            using (var scope = new TransactionScope())
            {
                var State = new ServiceType
                {
                    id = ServiceTypeEntity.id,
                    ServiceTypeName = ServiceTypeEntity.ServiceTypeName,
                    Description = ServiceTypeEntity.Description,
                    InsertedOn = ServiceTypeEntity.InsertedOn,
                    InsertedBy = ServiceTypeEntity.InsertedBy,
                    IsActive = ServiceTypeEntity.IsActive,
                    // IsChecked = StateEntity.IsChecked,
                    IsDelete = ServiceTypeEntity.IsDelete,

                };
                _unitOfWork.ServiceTypeRepository.Insert(ServiceTypeEntity);
                _unitOfWork.Save();
                scope.Complete();
                return ServiceTypeEntity;
            }
        }

        public bool DeleteServiceType(int Id)
        {
            var success = false;
            if (Id >= 0)
            {
                using (var scope = new TransactionScope())
                {
                    var State = _unitOfWork.ServiceTypeRepository.GetByID(Id);
                    if (State != null)
                    {
                        _unitOfWork.ServiceTypeRepository.Delete(State);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }

        public IEnumerable<ServiceType> GetAllServiceType()
        {
            var ServiceType = _unitOfWork.ServiceTypeRepository.GetAll().ToList();
            if (ServiceType.Any())
            {
                return ServiceType;
            }
            return null;
        }

        public ServiceType GetServiceTypeById(int Id)
        {

            var ServiceType = _unitOfWork.ServiceTypeRepository.GetByID(Id);
            if (ServiceType != null)
            {
                return ServiceType;
            }
            return null;
        }

        public ServiceType UpdateServiceType(int Id, ServiceType ServiceTypeEntity)
        {
            ServiceType service = null;
            if (ServiceTypeEntity != null)
            {
                using (var scope = new TransactionScope())
                {
                    service = _unitOfWork.ServiceTypeRepository.GetByID(Id);
                    if (service != null)
                    {
                        service.id = ServiceTypeEntity.id;
                        service.ServiceTypeName = ServiceTypeEntity.ServiceTypeName;
                        service.Description = ServiceTypeEntity.Description;



                        _unitOfWork.ServiceTypeRepository.Update(service);
                        _unitOfWork.Save();
                        scope.Complete();
                    }
                }
            }
            return service;
        }
    }
}
