using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeManga.Core.Entities
{
    [Table("Comment")]
    public class Comment : BaseEntity
    {
        public Comment() : base()
        {

        }

        [Required]
        public string Content { get; set; }

        public Guid UserId { get; set; }

        public User User { get; set; }

        public Guid BookId { get; set; }

        public Book Book { get; set; }
    }
}
