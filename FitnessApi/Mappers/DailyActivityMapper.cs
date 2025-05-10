using FitnessApi.Dto.DailyActivity;
using FitnessApi.Mappers.IMappers;
using FitnessApi.Model;

namespace FitnessApi.Mappers
{
    public class CreateDailyActivityMapper : IMapper<CreateDailyActivityDTO, DailyActivity>
    {
        public DailyActivity Map(CreateDailyActivityDTO source)
        {
            return new DailyActivity
            {
                UserId = source.UserId,
                Date = source.Date,
                Water = source.Water,
            };
        }

        public CreateDailyActivityDTO MapBack(DailyActivity destination)
        {
            throw new NotImplementedException();
        }
    }

    public class UpdateDailyActivityMapper : IMapper<UpdateDailyActivityDTO, DailyActivity>
    {
        public DailyActivity Map(UpdateDailyActivityDTO source)
        {
            return new DailyActivity
            {
                Id = source.Id,
                UserId = source.UserId,
                Date = source.Date,
                Water = source.Water,
            };
        }

        public UpdateDailyActivityDTO MapBack(DailyActivity destination)
        {
            throw new NotImplementedException();
        }
    }

    public class DailyActivityToDtoMapper : IMapper<DailyActivity, DailyActivityDTO>
    {
        public DailyActivityDTO Map(DailyActivity source)
        {
            return new DailyActivityDTO
            {
                Id = source.Id,
                UserId = source.UserId,
                Date = source.Date,
                Water = source.Water,
            };
        }

        public DailyActivity MapBack(DailyActivityDTO destination)
        {
            throw new NotImplementedException();
        }
    }
}
