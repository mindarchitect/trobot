using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TRobot.Core.Data.Entities
{
    [Table("Factories")]
    public class FactoryEntity : Entity
    {
        public string Name { get; set; }
        [ForeignKey("FactoryId")]
        public ICollection<RobotEntity> Robots { get; set; }
    }
}
