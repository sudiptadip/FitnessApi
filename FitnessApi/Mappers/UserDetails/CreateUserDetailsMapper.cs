using FitnessApi.Dto.UserDetails;
using FitnessApi.Mappers.IMappers;
using FitnessApi.Model;

namespace FitnessApi.Mappers.UserDetails
{
    public class CreateUserDetailsMapper : IMapper<CreateUserDetailsDTO, UserDetail>
    {
        public UserDetail Map(CreateUserDetailsDTO source)
        {
            return new UserDetail
            {
                UserId = source.UserId,
                FitnessGoal = source.FitnessGoal,
                Gender = source.Gender,
                Weight = source.Weight,
                Height = source.Height,
                PreviousFitnessExperience = source.PreviousFitnessExperience,
                SpecificDiet = source.SpecificDiet,
                DaysCommit = source.DaysCommit,
                SpecificExperiencePreferance = source.SpecificExperiencePreferance,
                CalorieyGoal = source.CalorieyGoal,
                SleepQuality = source.SleepQuality
            };
        }

        public CreateUserDetailsDTO MapBack(UserDetail destination)
        {
            return new CreateUserDetailsDTO
            {
                UserId = destination.UserId,
                FitnessGoal = destination.FitnessGoal,
                Gender = destination.Gender,
                Weight = destination.Weight,
                Height = destination.Height,
                PreviousFitnessExperience = destination.PreviousFitnessExperience,
                SpecificDiet = destination.SpecificDiet,
                DaysCommit = destination.DaysCommit,
                SpecificExperiencePreferance = destination.SpecificExperiencePreferance,
                CalorieyGoal = destination.CalorieyGoal,
                SleepQuality = destination.SleepQuality
            };
        }
    }
}
