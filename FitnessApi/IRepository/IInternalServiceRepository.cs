using FitnessApi.Dto.InternalService;
using FitnessApi.Model;

namespace FitnessApi.IRepository
{
    public interface IInternalServiceRepository : IRepository<InternalService>
    {
        public Task<InternalService> CreateInternailServiceAsync(CreateInternalServiceDto createInternalServiceDto);
    }
}
