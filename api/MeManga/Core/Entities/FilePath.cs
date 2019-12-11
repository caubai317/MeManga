using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeManga.Core.Entities
{
    [Table("FilePath")]
    public class FilePath
    {
        public FilePath()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }

        public int PageNumber { get; set; }

        public string Path { get; set; }

        public Guid ChapterId { get; set; }

        public Chapter Chapter { get; set; }
    }
}
