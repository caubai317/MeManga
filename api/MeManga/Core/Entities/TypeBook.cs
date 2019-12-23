using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MeManga.Core.Entities
{
    [Table("TypeBook")]
    public class TypeBook : BaseEntity
    {
        public TypeBook() : base()
        {

        }

        public string Name { get; set; }

        //public int? Count { get; set; }

        public List<BookInType> BookInTypes { get; set; }
    }
}
