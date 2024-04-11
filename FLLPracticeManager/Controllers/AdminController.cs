using FLLPracticeManager.Entities;
using FLLPracticeManager.Models;
using FLLPracticeManager.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FLLPracticeManager.Controllers
{
    public class AdminController : Controller
    {
        private readonly ITeamService _teamService;
        private readonly IReservationService _reservationService;
        public AdminController(IReservationService reservationService, ITeamService teamService)
        {

            _reservationService = reservationService;
            _teamService = teamService;

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddTeam()
        {
            return View();
        }

        [HttpPost]
        public JsonResult AddTeam(TeamVM team)
        {
            // Add the team to the database
            bool result = _teamService.AddTeam(team);

            return Json(result);

        }



        [AllowAnonymous]
        public JsonResult GetTeams()
        {
            // Get the teams from the database
            var teams = _teamService.GetTeams();

            // Sort the teams by team number
            teams.Sort((x, y) => x.TeamNumber.CompareTo(y.TeamNumber));

            return Json(teams);
        }

        public IActionResult GenerateReservationSlots()
        {
            // Delete all the reservation slots
            _reservationService.DeleteAllReservationSlots();


            // Generate the reservation slots for the specified time period and date
            // Use default values for the start and end time
            // Use current date for the start date and end date
            // Start time should be 9:00 AM
            // End time should be 6:00 PM
            // Slot duration should be 10 minutes


            // Make a startTime and endTime variable with the default values
            // Start time should be 9:00 AM Turkey time
            // End time should be 6:00 PM Turkey time
            // Slot duration should be 10 minutes

            // Server should be set to Turkey time





            DateTime startTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 9, 0, 0);

            DateTime endTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 18, 0, 0);

            // Call the GenerateReservationSlots method from the reservation service
            var result = _reservationService.GenerateReservationSlots(startTime, endTime, 10);

            return Content(result.ToString());
        }

        public IActionResult DeleteAllReservations()
        {
            // Delete all the reservations
            var result = _reservationService.DeleteAllReservations();
            return Content(result.ToString());
        }
        


    }
}
