using MeManga.Core.Business.Models.Books;
using MeManga.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeManga.Core.Business.Models.Chapters
{
    public class ChapterViewModel
    {
        public ChapterViewModel()
        {

        }

        public ChapterViewModel(Chapter chapter) : this()
        {
            if (chapter != null)
            {
                Id = chapter.Id;
                Name = chapter.Name;
                Book = chapter.BookId != null ? new BookViewModel(chapter.Book) : null; 
            }
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public BookViewModel Book { get; set; }
    }
}
