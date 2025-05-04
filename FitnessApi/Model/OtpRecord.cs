namespace FitnessApi.Model
{
    public class OtpRecord
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Otp { get; set; }
        public DateTime ExpirationTime { get; set; }
    }
}
