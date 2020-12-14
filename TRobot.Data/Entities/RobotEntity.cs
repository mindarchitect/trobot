using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TRobot.Data.Entities
{
    [Table("Robots")]
    public class RobotEntity
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        [ForeignKey("Id")]
        public int FactoryId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Guid { get; set; }
    }
}
