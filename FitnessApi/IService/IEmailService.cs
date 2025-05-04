using FitnessApi.Dto.Meal;
using FitnessApi.Model;

namespace FitnessApi.IService
{
    public interface IEmailService
    {
        public void SendEmail(SendEmailModel request);
    }
}
