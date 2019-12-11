using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeManga.Core.Entities
{
    [Table("Role")]
    public class Role : BaseEntity
    {
        public Role() : base()
        {

        }

        [StringLength(512)]
        [Required]
        public string Name { get; set; }

        public List<User> Users { get; set; }
    }
}
