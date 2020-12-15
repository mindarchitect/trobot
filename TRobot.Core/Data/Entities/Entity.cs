using System.ComponentModel.DataAnnotations;

namespace TRobot.Core.Data.Entities
{
    public abstract class Entity
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string CreatedDate { get; set; }
        public string ModifiedDate { get; set; }
    }
}
