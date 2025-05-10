using FitnessApi.Dto.DailyStepActivity;
using FitnessApi.Mappers.IMappers;
using FitnessApi.Model;

namespace FitnessApi.Mappers
{
    public class DailyStepActivityMapper : IMapper<DailyStepActivity, DailyStepActivityDTO>
    {
        public DailyStepActivityDTO Map(DailyStepActivity source)
        {
            return new DailyStepActivityDTO
            {
                Id = source.Id,
                UserId = source.UserId,
                Step = source.Step,
                Kcal = source.Kcal,
                Km = source.Km,
                Minutes = source.Minutes,
                Date = source.Date
            };
        }

        public DailyStepActivity MapBack(DailyStepActivityDTO destination)
        {
            throw new NotImplementedException();
        }
    }
}
