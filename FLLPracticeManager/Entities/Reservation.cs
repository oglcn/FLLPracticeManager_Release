using FLLPracticeManager.Entities.Base;

namespace FLLPracticeManager.Entities
{
    public class Reservation:AuditableEntity
    {
        public Team Team { get; set; }
        public ReservationSlot ReservationSlot { get; set; }
        public int TableNumber { get; set; }


    }
}
