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

        public Guid? WriterId { get; set; }
    }
}
