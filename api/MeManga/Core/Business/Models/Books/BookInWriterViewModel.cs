using MeManga.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MeManga.Core.Business.Models.Books
{
    public class BookInWriterViewModel
    {
        public BookInWriterViewModel()
        {

        }

        public BookInWriterViewModel(Book book) : this()
        {
            if (book != null)
            {
                Id = book.Id;
                Name = book.Name;
                Avatar = book.Avatar;
                Overall = book.Overall;
                ChapterNumber = book.Chapters != null ? book.Chapters.Count : 0;
            }
        }

        public Guid Id { get; set; }

        [StringLength(512)]
        public string Name { get; set; }

        public string Avatar { get; set; }

        public string Overall { get; set; }

        public int ChapterNumber { get; set; }
    }
}
