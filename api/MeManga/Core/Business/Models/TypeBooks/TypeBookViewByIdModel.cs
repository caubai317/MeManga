using MeManga.Core.Business.Models.Books;
using MeManga.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeManga.Core.Business.Models.TypeBooks
{
    public class TypeBookViewByIdModel
    {
        public TypeBookViewByIdModel()
        {

        }

        public TypeBookViewByIdModel(TypeBook typeBook) : this()
        {
            if (typeBook != null)
            {
                Id = typeBook.Id;
                Name = typeBook.Name;
                Books = typeBook.BookInTypes != null ? typeBook.BookInTypes.Select(y => new BookViewModel(y.Book)).ToArray() : null;
            }
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public BookViewModel[] Books { get; set; }
    }
}
