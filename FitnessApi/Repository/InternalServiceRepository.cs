using FitnessApi.Data;
using FitnessApi.Dto.InternalService;
using FitnessApi.IRepository;
using FitnessApi.Model;
using Microsoft.EntityFrameworkCore;

namespace FitnessApi.Repository
{
    public class InternalServiceRepository : Repository<InternalService>, IInternalServiceRepository
    {
        private readonly ApplicationDbContext _db;
        public InternalServiceRepository(ApplicationDbContext db) : base (db)
        {
            _db = db;
        }

        public async Task<InternalService> CreateInternailServiceAsync(CreateInternalServiceDto createInternalServiceDto)
        {
            var service = new InternalService()
            {
                ServiceName = createInternalServiceDto.ServiceName,
                ServiceValue = createInternalServiceDto.ServiceValue,
            };

            var result = (await _db.InternalServices.AddAsync(service)).Entity;
            await _db.SaveChangesAsync();

            return result;

        }
        

    }
}
