﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TRobot.Core.Data.Entities
{
    [Table("Robots")]
    public class RobotEntity : Entity
    {
        [Required]
        public int FactoryId { get; set; }

        [ForeignKey("FactoryId")]
        public FactoryEntity Factory { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Guid { get; set; }
    }
}
