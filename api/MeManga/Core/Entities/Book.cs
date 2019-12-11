using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MeManga.Core.Entities
{
    [Table("Book")]
    public class Book : BaseEntity
    {
        public Book() : base()
        {

        }

        public int? View
        {
            get
            {
                if (Chapters?.Any() != false)
                {
                    int View = 0;
                    foreach (Chapter chapter in Chapters)
                    {
                        View += chapter.View;
                    }
                    return View;
                }

                return null;
            }
        }

        [StringLength(512)]
        public string Name { get; set; }

        public string Avatar { get; set; }

        public string Overall { get; set; }

        public Guid? WriterId { get; set; }

        public Writer Writer { get; set; }

        public List<UserCollectionBook> Ratings { get; set; }

        public List<Chapter> Chapters { get; set; }

        public List<BookInType> BookInTypes { get; set; }

        public List<BookInWriter> BookInWriters { get; set; }

        public List<UserCollectionBook> UserCollectionBooks { get; set; }

        public List<Comment> Comments { get; set; }

    }
}
