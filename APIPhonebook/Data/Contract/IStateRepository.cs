using APIPhonebook.Models;

namespace APIPhonebook.Data.Contract
{
    public interface IStateRepository
    {
        IEnumerable<State> GetStatesByCountryId(int countryId);
        State GetStateById(int stateId);
    }
}
