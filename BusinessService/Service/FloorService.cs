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
    public class FloorService : IFloorService
    {
        private readonly UnitOfWork _unitOfWork;

        /// <summary>
        /// Public constructor.
        /// </summary>
        public FloorService()
        {
            _unitOfWork = new UnitOfWork();
        }



        /// <summary>
        /// Fetches Status by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>

        /// <summary>
        /// Creates a Floor
        /// </summary>
        /// <param name="FloorEntity"></param>
        /// <returns></returns>
        public Floor CreateFloor(Floor FloorEntity)
        {
            using (var scope = new TransactionScope())
            {
                var Floor = new Floor
                {
                    id = FloorEntity.id,
                    Floor_Number = FloorEntity.Floor_Number,
                    Description = FloorEntity.Description,
                    InsertedOn = FloorEntity.InsertedOn,
                    InsertedBy = FloorEntity.InsertedBy,
                    IsActive = FloorEntity.IsActive,
                    // IsChecked = StateEntity.IsChecked,
                    IsDelete = FloorEntity.IsDelete,

                };
                _unitOfWork.FloorRepository.Insert(FloorEntity);
                _unitOfWork.Save();
                scope.Complete();
                return FloorEntity;
            }
        }

        public bool DeleteFloor(int Id)
        {
            var success = false;
            if (Id >= 0)
            {
                using (var scope = new TransactionScope())
                {
                    var Floor = _unitOfWork.FloorRepository.GetByID(Id);
                    if (Floor != null)
                    {
                        _unitOfWork.FloorRepository.Delete(Floor);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }

        public IEnumerable<Floor> GetAllFloor()
        {
            var Floor = _unitOfWork.FloorRepository.GetAll().ToList();
            if (Floor.Any())
            {
                return Floor;
            }
            return null;
        }

        public Floor GetFloorById(int Id)
        {
            var Floor = _unitOfWork.FloorRepository.GetByID(Id);
            if (Floor != null)
            {
                return Floor;
            }
            return null;
        }

        public Floor UpdateFloor(int Id, Floor FloorEntity)
        {
            Floor status = null;
            if (FloorEntity != null)
            {
                using (var scope = new TransactionScope())
                {
                    status = _unitOfWork.FloorRepository.GetByID(Id);
                    if (status != null)
                    {
                        status.id = FloorEntity.id;
                        status.Floor_Number = FloorEntity.Floor_Number;
                        status.Description = FloorEntity.Description;
                        _unitOfWork.FloorRepository.Update(status);
                        _unitOfWork.Save();
                        scope.Complete();
                    }
                }
            }
            return status;
        }
    }
}
