using FLLPracticeManager.Entities;
using FLLPracticeManager.Models;
using FLLPracticeManager.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FLLPracticeManager.Controllers
{
    public class ReservationController : Controller
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        public IActionResult Index()
        {
            //return View();
            // Redirect to ListReservations action
            return RedirectToAction("ListReservations");
        }

        public JsonResult GetHomePageInfo()
        {
            // Find the current reservation slot
            var currentReservationSlot = _reservationService.GetCurrentReservationSlot();

            int remainingMin = 0;
            int remainingSec = 0;

            List<ReservationVM> reservationVMs = new List<ReservationVM>();

            if (currentReservationSlot == null)
            {
                // If there is no current reservation slot, return default values
                remainingSec = 30;
                remainingMin = 0;

                // Fill the reservationVMs with default values
                for (int i = 1; i <= 10; i++)
                {
                    reservationVMs.Add(new ReservationVM
                    {
                        Id = -1,
                        TeamNumber = 000,
                        ReservationSlotId = -1,
                        TableNumber = i,
                        TeamName = "Boş Masa",
                    });
                }
            }
            else
            {
                // Calculate the remaining time for the current reservation slot
                var currentTime = System.DateTime.Now;
                var remainingTime = currentReservationSlot.EndTime - currentTime;
                remainingMin = remainingTime.Minutes;
                remainingSec = remainingTime.Seconds;

                // Get the reservations for the current reservation slot
                var reservations = _reservationService.GetReservationsBySlot(currentReservationSlot.Id);
                // Map the Reservation to ReservationVM with Linq
                reservationVMs = reservations.Select(x => new ReservationVM
                {
                    Id = x.Id,
                    TeamNumber = x.Team.TeamNumber,
                    ReservationSlotId = x.ReservationSlot.Id,
                    TableNumber = x.TableNumber,
                    TeamName = x.Team.TeamName
                }).ToList();

                // Order the reservationVMs by TableNumber
                reservationVMs = reservationVMs.OrderBy(x => x.TableNumber).ToList();

                // Add the missing tables with default values
                for (int i = 1; i <= 10; i++)
                {
                    if (reservationVMs.Count(x => x.TableNumber == i) == 0)
                    {
                        reservationVMs.Add(new ReservationVM
                        {
                            Id = -1,
                            TeamNumber = 000,
                            ReservationSlotId = -1,
                            TableNumber = i,
                            TeamName = "Boş Masa",
                        });
                    }
                }

                // Order the reservationVMs by TableNumber again
                reservationVMs = reservationVMs.OrderBy(x => x.TableNumber).ToList();

            }

            HomePageVM homePageVM = new HomePageVM
            {
                RemainingMin = remainingMin,
                RemainingSec = remainingSec,
                Reservations = reservationVMs
            };


            return Json(homePageVM);

        }

        public IActionResult Search()
        {
            return View();
        }

        public JsonResult GetReservationSlots()
        {
            var reservationSlots = _reservationService.GetAllReservationSlots();
            // Filter todays reservation slots
            //reservationSlots = reservationSlots.Where(x => x.StartTime.Date == DateTime.Today).ToList();

            List<ReservationSlotVM> reservationSlotVMs = new List<ReservationSlotVM>();

            // Map the ReservationSlot to ReservationSlotVM with Linq
            reservationSlotVMs = reservationSlots.Select(x => new ReservationSlotVM
            {
                Id = x.Id,
                StartTime = x.StartTime.ToString("HH:mm"),
                EndTime = x.EndTime.ToString("HH:mm")
            }).ToList();

            // Check if the reservation slot is available or not and set the IsAvailable property
            foreach (var item in reservationSlotVMs)
            {
                var availableTables = _reservationService.GetAvailableTables(item.Id);
                item.IsAvailable = (availableTables.Count > 0);

            }
            return Json(reservationSlotVMs);
        }

        public JsonResult GetReservations(int tableNumber)
        {

            var reservations = _reservationService.GetReservationsByTable(tableNumber);
            List<ReservationVM> reservationVMs = new List<ReservationVM>();
            // Map the Reservation to ReservationVM with Linq
            reservationVMs = reservations.Select(x => new ReservationVM
            {
                Id = x.Id,
                TeamNumber = x.Team.TeamNumber,
                ReservationSlotId = x.ReservationSlot.Id,
                TableNumber = x.TableNumber,
                TeamName = x.Team.TeamName

            }).ToList();
            return Json(reservationVMs);
        }

        public JsonResult GetReservationsByTeam(int teamNumber)
        {

            var reservations = _reservationService.GetReservationsByTeam(teamNumber);
            List<ReservationVM> reservationVMs = new List<ReservationVM>();
            // Map the Reservation to ReservationVM with Linq
            reservationVMs = reservations.Select(x => new ReservationVM
            {
                Id = x.Id,
                TeamNumber = x.Team.TeamNumber,
                ReservationSlotId = x.ReservationSlot.Id,
                TableNumber = x.TableNumber,
                TeamName = x.Team.TeamName,
                StartTime = x.ReservationSlot.StartTime.ToString("HH:mm"),
                EndTime = x.ReservationSlot.EndTime.ToString("HH:mm")

            }).ToList();
            return Json(reservationVMs);
        }

        public JsonResult GetTables(int reservationSlotId)
        {
            var tables = _reservationService.GetAvailableTables(reservationSlotId);
            return Json(tables);
        }

        public IActionResult ListReservations()
        {

            return View();
        }

        public JsonResult DeleteReservation(int slotId, int table)
        {
            var result = _reservationService.DeleteReservationBySlot(slotId, table);

            if (result)
            {
                return Json(new { success = true, message = "Reservation deleted successfully" });
            }
            else
            {
                return Json(new { success = false, message = "Reservation could not be deleted" });
            }
        }

        public JsonResult AddReservation(ReservationVM reservation)
        {
            ReservationSlot reservationSlot = _reservationService.GetReservationSlot(reservation.ReservationSlotId);
            // Convert the string StartTime to DateTime
            DateTime startTime = reservationSlot.StartTime;

            // Check if the team is out of reservation timeout
            bool isEligible = _reservationService.CheckTimeout(reservation.ReservationTimeout, reservation.TeamNumber, startTime);

            if (!isEligible)
            {
                return Json(new { success = false, message = "Aynı Takımın Yakında Başka Rezervasyonu Var" });
            }


            var result = _reservationService.AddReservation(reservation);
            if (result)
            {
                return Json(new { success = true, message = "Rezervasyon Başarı ile Eklendi" });
            }
            else
            {
                return Json(new { success = false, message = "Bir Hata Oluştu" });
            }
        }


    }
}
