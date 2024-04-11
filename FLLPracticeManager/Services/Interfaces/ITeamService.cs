using FLLPracticeManager.Entities;
using FLLPracticeManager.Models;

namespace FLLPracticeManager.Services.Interfaces
{
    public interface ITeamService
    {
        List<Team> GetTeams();
        bool AddTeam(TeamVM team);
        bool DeleteTeam(int teamId);
        Team GetTeam(int teamId);
    }
}
