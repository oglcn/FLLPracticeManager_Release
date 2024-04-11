namespace FLLPracticeManager.Models
{
    public class HomePageVM
    {
        public List<ReservationVM> Reservations { get; set; }
        public int RemainingMin { get; set; }
        public int RemainingSec { get; set; }
    }
}
