using FLLPracticeManager.Data;
using FLLPracticeManager.Entities;
using FLLPracticeManager.Models;
using FLLPracticeManager.Services.Interfaces;

namespace FLLPracticeManager.Services
{
    public class TeamService : ITeamService
    {
        private readonly ApplicationDBContext _context;

        public TeamService(ApplicationDBContext context)
        {
            _context = context;
        }
        public bool AddTeam(TeamVM team)
        {
            // Add the team to the database
            var newTeam = new Team
            {
                TeamNumber = team.TeamNumber,
                TeamName = team.TeamName
            };
            _context.Teams.Add(newTeam);
            var result = _context.SaveChanges();
            return result > 0;
        }

        public bool DeleteTeam(int teamId)
        {
            // Delete the team from the database
            var team = _context.Teams.Find(teamId);
            if (team != null)
            {
                team.IsDeleted = true;
                var result = _context.SaveChanges();
                return result > 0;
            }
            return false;
        }

        public Team GetTeam(int teamId)
        {
            // Get the team from the database
            var team = _context.Teams.Find(teamId);
            if (team != null && team.IsDeleted==false)
            {
                return team;
            }
            return null;
        }

        public List<Team> GetTeams()
        {
            // Get the teams from the database
            return _context.Teams.Where(x=>x.IsDeleted==false).ToList();
        }
    }
}
