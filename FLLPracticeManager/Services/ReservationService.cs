using FLLPracticeManager.Data;
using FLLPracticeManager.Entities;
using FLLPracticeManager.Models;
using FLLPracticeManager.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FLLPracticeManager.Services
{
    public class ReservationService : IReservationService
    {
        private readonly ApplicationDBContext _context;
        private readonly IConfiguration _configuration;

        public ReservationService(ApplicationDBContext dBContext, IConfiguration configuration)
        {
            _context = dBContext;
            _configuration = configuration;
        }



        public bool AddReservation(ReservationVM reservation)
        {

            var newReservation = new Reservation
            {
                Team = _context.Teams.FirstOrDefault(x => x.TeamNumber == reservation.TeamNumber),
                ReservationSlot = _context.ReservationSlots.FirstOrDefault(x => x.Id == reservation.ReservationSlotId),
                TableNumber = reservation.TableNumber,
            };
            if (newReservation.Team == null || newReservation.ReservationSlot == null)
            {
                return false;
            }

            _context.Reservations.Add(newReservation);
            var result = _context.SaveChanges();

            return result > 0;
        }

        public bool DeleteReservation(int reservationId)
        {
            var reservation = _context.Reservations.FirstOrDefault(x => x.Id == reservationId);
            if (reservation != null)
            {
                reservation.IsDeleted = true;

            }
            var result = _context.SaveChanges();
            return result > 0;
        }



        public Reservation GetReservation(int reservationId)
        {
            Reservation reservation = _context.Reservations.FirstOrDefault(x => x.Id == reservationId);
            if (reservation != null)
            {
                // Check if the reservation is deleted
                if (reservation.IsDeleted == false)
                {
                    return reservation;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }

        }

        public List<Reservation> GetReservations()
        {
            return _context.Reservations.Where(x => x.IsDeleted == false).Include(x => x.Team).Include(x => x.ReservationSlot).ToList();
        }
        public List<Reservation> GetReservationsByTable(int tableNumber)
        {
            return _context.Reservations.Where(x => x.TableNumber == tableNumber && x.IsDeleted == false).Include(x => x.Team).Include(x => x.ReservationSlot).ToList();
        }

        public List<Reservation> GetReservationsByTeam(int teamNumber)
        {
            return _context.Reservations.Where(x => x.Team.TeamNumber == teamNumber && x.IsDeleted == false).Include(x => x.Team).Include(x => x.ReservationSlot).ToList();
        }

        public bool UpdateReservation(ReservationVM reservation)
        {
            var reservationToUpdate = _context.Reservations.FirstOrDefault(x => x.Id == reservation.Id);
            if (reservationToUpdate != null)
            {
                reservationToUpdate.Team = _context.Teams.FirstOrDefault(x => x.TeamNumber == reservation.TeamNumber);
                reservationToUpdate.ReservationSlot = _context.ReservationSlots.FirstOrDefault(x => x.Id == reservation.ReservationSlotId);
                reservationToUpdate.TableNumber = reservation.TableNumber;
            }
            var result = _context.SaveChanges();
            return result > 0;

        }


        public int GenerateReservationSlots(DateTime startDate, DateTime endDate, int slotDuration)
        {
            var slots = new List<ReservationSlot>();
            var currentTime = startDate;
            while (currentTime < endDate)
            {
                var newSlot = new ReservationSlot
                {
                    StartTime = currentTime,
                    EndTime = currentTime.AddMinutes(slotDuration)
                };
                slots.Add(newSlot);
                currentTime = currentTime.AddMinutes(slotDuration);
            }
            _context.ReservationSlots.AddRange(slots);
            var result = _context.SaveChanges();

            // return the number of slots generated
            return result;
        }

        public List<ReservationSlot> GetAllReservationSlots()
        {
            return _context.ReservationSlots.ToList();
        }

        public int DeleteAllReservationSlots()
        {
            _context.ReservationSlots.RemoveRange(_context.ReservationSlots);
            return _context.SaveChanges();
        }

        public List<Reservation> GetReservationsBySlot(int slotId)
        {

            return _context.Reservations.Where(x => x.ReservationSlot.Id == slotId && x.IsDeleted == false).Include(x => x.Team).Include(x => x.ReservationSlot).ToList();
        }



        public List<int> GetAvailableTables(int slotId)
        {
            var reservedTables = _context.Reservations.Where(x => x.ReservationSlot.Id == slotId && x.IsDeleted == false).Select(x => x.TableNumber).ToList();
            //var allTables = _context.Reservations.Select(x => x.TableNumber).Distinct().ToList();

            // Get Table Count from appsettings.json

            var tableCount = int.Parse(_configuration["AppSettings:TableCount"]); // Assuming the key in appsettings.json is "TableCount"

            var allTables = Enumerable.Range(1, tableCount).ToList();
            return allTables.Except(reservedTables).ToList();

        }

        public int GetSlotId(DateTime startTime, DateTime endTime)
        {
            var slot = _context.ReservationSlots.FirstOrDefault(x => x.StartTime == startTime && x.EndTime == endTime);
            if (slot != null)
            {
                return slot.Id;
            }
            else
            {
                return -1;
            }
        }

        public bool DeleteReservationBySlot(int slotId, int table)
        {

            var reservation = _context.Reservations.FirstOrDefault(x => x.ReservationSlot.Id == slotId && x.TableNumber == table);

            reservation.IsDeleted = true;

            var result = _context.SaveChanges();
            return result > 0;
        }

        public int DeleteAllReservations()
        {
            var reservations = _context.Reservations;
            foreach (var reservation in reservations)
            {
                reservation.IsDeleted = true;
            }
            return _context.SaveChanges();
        }

        public ReservationSlot GetCurrentReservationSlot()
        {
            // Find the current slot by comparing the current time with the slot start and end time
            var currentTime = DateTime.Now;
            var slot = _context.ReservationSlots.FirstOrDefault(x => x.StartTime <= currentTime && x.EndTime >= currentTime);
            return slot;
        }

        public bool CheckTimeout(int timeoutMinutes, int TeamNumber, DateTime requestedStartTime)
        {
            // Check if the team has a reservation in the last timeoutMinutes
            var lastReservation = _context.Reservations.Where(x => x.Team.TeamNumber == TeamNumber && x.IsDeleted == false).Include(x => x.Team).Include(x => x.ReservationSlot).OrderByDescending(x => x.ReservationSlot.StartTime).FirstOrDefault();

            if (lastReservation != null)
            {
                var timeoutTime = lastReservation.ReservationSlot.StartTime.AddMinutes(timeoutMinutes);
                if (timeoutTime > requestedStartTime)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }

        }

        public ReservationSlot GetReservationSlot(int slotId)
        {
            return _context.ReservationSlots.FirstOrDefault(x => x.Id == slotId);
        }
    }
}
