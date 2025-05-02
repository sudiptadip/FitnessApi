namespace FitnessApi.Dto.DailyActivity
{
    public class DailyActivityDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime Date { get; set; }
        public double Water { get; set; }
        public int Step { get; set; }
    }
}
