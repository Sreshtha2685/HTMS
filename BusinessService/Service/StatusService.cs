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
    public class StatusService : IStatusService
    {
        private readonly UnitOfWork _unitOfWork;

        /// <summary>
        /// Public constructor.
        /// </summary>
        public StatusService()
        {
            _unitOfWork = new UnitOfWork();
        }

       

        /// <summary>
        /// Fetches Status by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>

        /// <summary>
        /// Creates a Status
        /// </summary>
        /// <param name="StatusEntity"></param>
        /// <returns></returns>
        public Status CreateStatus(Status StatusEntity)
        {
            using (var scope = new TransactionScope())
            {
                var Status = new Status
                {
                    id = StatusEntity.id,
                    Status1 = StatusEntity.Status1,
                    Description = StatusEntity.Description,
                    InsertedOn = StatusEntity.InsertedOn,
                    InsertedBy = StatusEntity.InsertedBy,
                    IsActive = StatusEntity.IsActive,
                    // IsChecked = StateEntity.IsChecked,
                    IsDelete = StatusEntity.IsDelete,

                };
                _unitOfWork.StatusRepository.Insert(StatusEntity);
                _unitOfWork.Save();
                scope.Complete();
                return StatusEntity;
            }
        }

        public bool DeleteStatus(int Id)
        {
            var success = false;
            if (Id >= 0)
            {
                using (var scope = new TransactionScope())
                {
                    var Status = _unitOfWork.StatusRepository.GetByID(Id);
                    if (Status != null)
                    {
                        _unitOfWork.StatusRepository.Delete(Status);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }

        public IEnumerable<Status> GetAllStatus()
        {
            var Status = _unitOfWork.StatusRepository.GetAll().ToList();
            if (Status.Any())
            {
                return Status;
            }
            return null;
        }

        public Status GetStatusById(int Id)
        {
            var Status = _unitOfWork.StatusRepository.GetByID(Id);
            if (Status != null)
            {
                return Status;
            }
            return null;
        }

        public Status UpdateStatus(int Id, Status StatusEntity)
        {
            Status status = null;
            if (StatusEntity != null)
            {
                using (var scope = new TransactionScope())
                {
                    status = _unitOfWork.StatusRepository.GetByID(Id);
                    if (status != null)
                    {
                        status.id = StatusEntity.id;
                        status.Status1 = StatusEntity.Status1;
                        status.Description = StatusEntity.Description;



                        _unitOfWork.StatusRepository.Update(status);
                        _unitOfWork.Save();
                        scope.Complete();
                    }
                }
            }
            return status;
        }
    }
}
