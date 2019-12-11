using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MeManga.Core.Entities
{
    [Table("Writer")]
    public class Writer : BaseEntity
    {
        public Writer() : base()
        {

        }

        public string Name { get; set; }

        public string Information { get; set; }

        public List<BookInWriter> BookInWriters { get; set; }
    }
}
