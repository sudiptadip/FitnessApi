using FitnessApi.Dto.DailyStepActivity;
using FitnessApi.Mappers.IMappers;
using FitnessApi.Model;

namespace FitnessApi.Mappers
{
    public class UpdateDailyStepActivityMapper : IMapper<UpdateDailyStepActivityDTO, DailyStepActivity>
    {
        public DailyStepActivity Map(UpdateDailyStepActivityDTO source)
        {
            return new DailyStepActivity
            {
                Step = source.Step,
                Kcal = source.Kcal,
                Km = source.Km,
                Minutes = source.Minutes,
                Date = source.Date
            };
        }

        public UpdateDailyStepActivityDTO MapBack(DailyStepActivity destination)
        {
            throw new NotImplementedException();
        }
    }
}
