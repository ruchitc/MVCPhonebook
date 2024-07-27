using APIPhonebook.Data.Contract;
using APIPhonebook.Dtos;

namespace APIPhonebook.Services.Contract
{
    public interface IStateService
    {
        ServiceResponse<IEnumerable<StateDto>> GetStatesByCountryId(int countryId);

        ServiceResponse<StateDto> GetStateById(int stateId);
    }
}
