using FLLPracticeManager.Entities.Base;

namespace FLLPracticeManager.Entities
{
    public class Team:BaseEntity
    {
        public int TeamNumber { get; set; }
        public string? TeamName { get; set; }
    }
}
