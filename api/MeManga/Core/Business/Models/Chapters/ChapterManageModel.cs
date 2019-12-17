using MeManga.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeManga.Core.Business.Models.Chapters
{
    public class ChapterManageModel
    {
        public string Name { get; set; }

        public Guid BookId { get; set; }

        public void SetDataToModel(Chapter chapter)
        {
            chapter.Name = Name;
            chapter.BookId = BookId;
        }

    }
}
