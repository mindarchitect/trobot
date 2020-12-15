using System;
using System.ComponentModel.DataAnnotations;

namespace TRobot.Core.Data.Entities
{
    public abstract class Entity
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
