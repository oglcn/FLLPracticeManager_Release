namespace FLLPracticeManager.Models
{
    public class ReservationVM
    {
        public int Id { get; set; }
        public int TeamNumber { get; set; }
        public int ReservationSlotId { get; set; }
        public int TableNumber { get; set; }
        public string TeamName { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int ReservationTimeout { get; set; }
    }
}
