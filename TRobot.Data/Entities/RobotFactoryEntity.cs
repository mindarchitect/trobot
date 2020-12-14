using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TRobot.Data.Entities
{
    [Table("Factories")]
    public class RobotFactoryEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<RobotEntity> Robots { get; private set; } = new ObservableCollection<RobotEntity>();
    }
}
