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
  public  class BedService : IBedService
    {
        private readonly UnitOfWork _unitOfWork;
        
        /// <summary>
        /// Public constructor.
        /// </summary>
        public BedService()
        {
            _unitOfWork = new UnitOfWork();
        }

        /// <summary>
        /// Fetches Bed by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>

        /// <summary>
        /// Creates a Bed
        /// </summary>
        /// <param name="BedEntity"></param>
        /// <returns></returns>

        public Bed CreateBed(Bed BedEntity)
        {
            using (var scope = new TransactionScope())
            {
                var Bed = new Bed
                {
                    id = BedEntity.id,
                    Bed_Code = BedEntity.Bed_Code,
                    Description = BedEntity.Description,
                    Bed_Number = BedEntity.Bed_Number,

                    InsertedOn = BedEntity.InsertedOn,
                    InsertedBy = BedEntity.InsertedBy,
                    IsActive = BedEntity.IsActive,
                    IsDelete = BedEntity.IsDelete


                };
                _unitOfWork.BedRepository.Insert(BedEntity);
                _unitOfWork.Save();
                scope.Complete();
                return BedEntity;
            }
        }

        public bool DeleteBed(int Id)
        {
            var success = false;
            if (Id >= 0)
            {
                using (var scope = new TransactionScope())
                {
                    var Bed = _unitOfWork.BedRepository.GetByID(Id);
                    if (Bed != null)
                    {
                        _unitOfWork.BedRepository.Delete(Bed);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }

        public IEnumerable<Bed> GetAllBed()
        {
            var Bed = _unitOfWork.BedRepository.GetAll().ToList();
            if (Bed.Any())
            {
                return Bed;
            }
            return null;
        }

        public Bed GetBedById(int Id)
        {
            var Bed = _unitOfWork.BedRepository.GetByID(Id);
            if (Bed != null)
            {
                return Bed;
            }
            return null;
        }

        public Bed UpdateBed(int id, Bed BedEntity)
        {
            Bed Bed = null;
            if (BedEntity != null)
            {
                using (var scope = new TransactionScope())
                {
                    Bed = _unitOfWork.BedRepository.GetByID(id);
                    if (Bed != null)
                    {
                        Bed.id = BedEntity.id;
                        Bed.Bed_Number= BedEntity.Bed_Number;
                        Bed.Bed_Code = BedEntity.Bed_Code;
                        Bed.Description = BedEntity.Description;
                        _unitOfWork.BedRepository.Update(Bed);
                        _unitOfWork.Save();
                        scope.Complete();
                    }
                }
            }
            return Bed;
        }
    }
}
