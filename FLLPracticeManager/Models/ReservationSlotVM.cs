namespace FLLPracticeManager.Models
{
    public class ReservationSlotVM
    {
        public int Id { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public bool IsAvailable { get; set; }
    }
}
