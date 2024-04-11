using System.ComponentModel.DataAnnotations;

namespace FLLPracticeManager.Entities.Base
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public bool IsDeleted { get; set; }

    }
}
