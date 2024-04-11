using FLLPracticeManager.Entities;
using FLLPracticeManager.Models;

namespace FLLPracticeManager.Services.Interfaces
{
    public interface IReservationService
    {
        public bool AddReservation(ReservationVM reservation);
        public bool DeleteReservation(int reservationId);
        public bool DeleteReservationBySlot(int slotId, int table);
        public bool UpdateReservation(ReservationVM reservation);
        public Reservation GetReservation(int reservationId);
        public ReservationSlot GetReservationSlot(int slotId);
        
        public List<Reservation> GetReservations();
        public List<Reservation> GetReservationsByTeam(int teamNumber);
        public List<Reservation> GetReservationsBySlot(int slotId);
        public List<Reservation> GetReservationsByTable(int tableNumber);
        public List<int> GetAvailableTables(int slotId);
        public int GetSlotId(DateTime startTime, DateTime endTime);


        public int GenerateReservationSlots(DateTime startDate, DateTime endDate, int slotDuration);
        public List<ReservationSlot> GetAllReservationSlots();
        public int DeleteAllReservationSlots();
        public int DeleteAllReservations();

        public ReservationSlot GetCurrentReservationSlot();
        public bool CheckTimeout(int timeoutMinutes, int TeamNumber, DateTime requestedStartTime);


    }
}
