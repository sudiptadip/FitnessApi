using FitnessApi.Dto.UserDetails;
using FitnessApi.Mappers.IMappers;
using FitnessApi.Model;

namespace FitnessApi.Mappers.UserDetails
{
    public class UpdateUserDetailsMapper : IMapper<UpdateUserDetailsDTO, UserDetail>
    {
        public UserDetail Map(UpdateUserDetailsDTO source)
        {
            return new UserDetail
            {
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

        public UpdateUserDetailsDTO MapBack(UserDetail destination)
        {
            return new UpdateUserDetailsDTO
            {
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
