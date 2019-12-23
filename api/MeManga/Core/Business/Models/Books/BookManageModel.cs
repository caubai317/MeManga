using MeManga.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeManga.Core.Business.Models.Books
{
    public class BookManageModel
    {
        public string Name { get; set; }

        public string Avatar { get; set; }

        public string Overall { get; set; }

        public Guid[] WriterIds { get; set; }

        public Guid[] TypeIds { get; set; }

        public Guid? TranslatorId { get; set; }

        public void SetDataToModel(Book book)
        {
            book.Name = Name;
            book.Avatar = Avatar;
            book.Overall = Overall;
            //book.WriterId = WriterId;
            book.TranslatorId = TranslatorId;
        }

    }
}
