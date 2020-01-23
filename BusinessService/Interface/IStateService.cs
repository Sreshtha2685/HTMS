using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Interface
{
  public interface IStateService
    {
        State GetStateById(int Id);
        IEnumerable<State> GetAllState();
        State CreateState(State StateEntity);
        State UpdateState(int Id, State StateEntity);
        bool DeleteState(int Id);
    }
}
