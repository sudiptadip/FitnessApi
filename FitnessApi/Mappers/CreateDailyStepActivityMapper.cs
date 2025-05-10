using FitnessApi.Dto.DailyStepActivity;
using FitnessApi.Mappers.IMappers;
using FitnessApi.Model;

namespace FitnessApi.Mappers
{
    public class CreateDailyStepActivityMapper : IMapper<CreateDailyStepActivityDTO, DailyStepActivity>
    {
        public DailyStepActivity Map(CreateDailyStepActivityDTO source)
        {
            return new DailyStepActivity
            {
                UserId = source.UserId,
                Step = source.Step,
                Kcal = source.Kcal,
                Km = source.Km,
                Minutes = source.Minutes,
                Date = source.Date
            };
        }

        public CreateDailyStepActivityDTO MapBack(DailyStepActivity destination)
        {
            throw new NotImplementedException();
        }
    }
}
