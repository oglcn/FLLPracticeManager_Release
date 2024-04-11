using FLLPracticeManager.Entities.Base;

namespace FLLPracticeManager.Entities
{
    public class ReservationSlot:BaseEntity
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
