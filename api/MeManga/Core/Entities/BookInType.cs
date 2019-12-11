using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeManga.Core.Entities
{
    [Table("BookInType")]
    public class BookInType : BaseEntity
    {
        public BookInType() : base()
        {

        }

        public Guid BookId { get; set; }

        public Book Book { get; set; }

        public Guid TypeBookId { get; set; }

        public TypeBook TypeBook { get; set; }
    }
}