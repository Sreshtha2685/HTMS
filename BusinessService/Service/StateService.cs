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
    public class StateService : IStateService
    {
        private readonly UnitOfWork _unitOfWork;

        /// <summary>
        /// Public constructor.
        /// </summary>
        public StateService()
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
        public State CreateState(State StateEntity)
        {
            using (var scope = new TransactionScope())
            {
                var State = new State
                {
                    Id = StateEntity.Id,
                    StateName = StateEntity.StateName,
                    CityId = StateEntity.CityId,
                    InsertedOn = StateEntity.InsertedOn,
                    InsertedBy = StateEntity.InsertedBy,
                    IsActive = StateEntity.IsActive,
                   // IsChecked = StateEntity.IsChecked,
                    IsDelete = StateEntity.IsDelete,

                };
                _unitOfWork.StateRepository.Insert(StateEntity);
                _unitOfWork.Save();
                scope.Complete();
                return StateEntity;
            }
        }

        public bool DeleteState(int Id)
        {
            var success = false;
            if (Id >= 0)
            {
                using (var scope = new TransactionScope())
                {
                    var State = _unitOfWork.StateRepository.GetByID(Id);
                    if (State != null)
                    {
                        _unitOfWork.StateRepository.Delete(State);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }

        public IEnumerable<State> GetAllState()
        {
            var State = _unitOfWork.StateRepository.GetAll().ToList();
            if (State.Any())
            {
                return State;
            }
            return null;
        }

        public State GetStateById(int Id)
        {

            var State = _unitOfWork.StateRepository.GetByID(Id);
            if (State != null)
            {
                return State;
            }
            return null;
        }

        public State UpdateState(int Id, State StateEntity)
        {
            State state = null;
            if (StateEntity != null)
            {
                using (var scope = new TransactionScope())
                {
                    state = _unitOfWork.StateRepository.GetByID(Id);
                    if (state != null)
                    {
                        state.Id = StateEntity.Id;
                        state.StateName = StateEntity.StateName;
                        state.CityId = StateEntity.CityId;
                        


                        _unitOfWork.StateRepository.Update(state);
                        _unitOfWork.Save();
                        scope.Complete();
                    }
                }
            }
            return state;
        }
    }
}
