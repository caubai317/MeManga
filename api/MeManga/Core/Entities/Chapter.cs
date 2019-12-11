using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeManga.Core.Entities
{
    [Table("Chapter")]
    public class Chapter : BaseEntity
    {
        public Chapter() : base()
        {

        }

        public string Name { get; set; }

        public int View { get; set; }

        public Guid BookId { get; set; }
        public Book Book { get; set; }

        public List<FilePath> PathImages { get; set; }
    }
}