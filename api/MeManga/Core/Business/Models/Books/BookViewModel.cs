using MeManga.Core.Business.Models.Base;
using MeManga.Core.Business.Models.TypeBooks;
using MeManga.Core.Business.Models.Writers;
using MeManga.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MeManga.Core.Business.Models.Books
{
    public class BookViewModel
    {
        public BookViewModel()
        {

        }

        public BookViewModel(Book book) : this()
        {
            if (book != null)
            {
                Id = book.Id;
                Name = book.Name;
                Avatar = book.Avatar;
                Overall = book.Overall;
                Writers = book.BookInWriters != null ? book.BookInWriters.Select(y => new WriterInBookViewModel(y.Writer)).ToArray() : null;
                TypeBooks = book.BookInTypes != null ? book.BookInTypes.Select(y => new TypeBookViewModel(y.TypeBook)).ToArray() : null;
                TranslatorId = book.TranslatorId;
                ChapterNumber = book.Chapters != null ? book.Chapters.Count : 0;
            }
        }

        public Guid Id { get; set; }

        [StringLength(512)]
        public string Name { get; set; }

        public string Avatar { get; set; }

        public string Overall { get; set; }

        public WriterInBookViewModel[] Writers { get; set; }

        public TypeBookViewModel[] TypeBooks { get; set; }

        public Guid? TranslatorId { get; set; }

        public int ChapterNumber { get; set; }
    }
}
