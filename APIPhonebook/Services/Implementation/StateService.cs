using APIPhonebook.Data.Contract;
using APIPhonebook.Dtos;
using APIPhonebook.Services.Contract;

namespace APIPhonebook.Services.Implementation
{
    public class StateService : IStateService
    {
        private readonly IStateRepository _stateRepository;

        public StateService(IStateRepository stateRepository)
        {
            _stateRepository = stateRepository;
        }

        public ServiceResponse<IEnumerable<StateDto>> GetStatesByCountryId(int countryId)
        {
            var response = new ServiceResponse<IEnumerable<StateDto>>();
            var states = _stateRepository.GetStatesByCountryId(countryId);

            if (states != null && states.Any())
            {
                List<StateDto> stateDtoList = new List<StateDto>();
                foreach (var state in states)
                {
                    StateDto stateDto = new StateDto();
                    stateDto.StateId = state.StateId;
                    stateDto.StateName = state.StateName;
                    stateDto.CountryId = countryId;

                    stateDtoList.Add(stateDto);
                }

                response.Data = stateDtoList;
                response.Success = true;
                response.Message = "Success";
            }
            else
            {
                response.Success = false;
                response.Message = "No record found";
            }

            return response;
        }

        public ServiceResponse<StateDto> GetStateById(int stateId)
        {
            var response = new ServiceResponse<StateDto>();
            var state = _stateRepository.GetStateById(stateId);

            if (state != null)
            {
                var stateDto = new StateDto()
                {
                    StateId = state.StateId,
                    StateName = state.StateName,
                    CountryId = state.CountryId,
                };

                response.Data = stateDto;
                response.Success = true;
                response.Message = "Success";
            }
            else
            {
                response.Success = false;
                response.Message = "No record found";
            }

            return response;
        }
    }
}
