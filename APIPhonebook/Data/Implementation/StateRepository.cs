using APIPhonebook.Data.Contract;
using APIPhonebook.Models;

namespace APIPhonebook.Data.Implementation
{
    public class StateRepository : IStateRepository
    {
        private readonly IAppDbContext _appDbContext;

        public StateRepository(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<State> GetStatesByCountryId(int countryId)
        {
            List<State> states = _appDbContext.States.Where(s => s.CountryId == countryId).ToList();
            return states;
        }

        public State GetStateById(int stateId)
        {
            return _appDbContext.States.FirstOrDefault(s => s.StateId == stateId);
        }
    }
}
