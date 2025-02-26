using System.ComponentModel.DataAnnotations;

namespace FitnessApi.Dto.InternalService
{
    public class CreateInternalServiceDto
    {
        [Required]
        public string ServiceName { get; set; }
        [Required]
        public string ServiceValue { get; set; }
    }
}
