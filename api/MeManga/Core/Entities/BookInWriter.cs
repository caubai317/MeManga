using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MeManga.Core.Entities
{
    [Table("BookInWriter")]
    public class BookInWriter : BaseEntity
    {
        public BookInWriter() : base()
        {

        }

        public Guid BookId { get; set; }

        public Book Book { get; set; }

        public Guid WriterId { get; set; }

        public Writer Writer { get; set; }
    }
}
