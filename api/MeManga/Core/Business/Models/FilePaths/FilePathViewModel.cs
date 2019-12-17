using MeManga.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeManga.Core.Business.Models.FilePaths
{
    public class FilePathViewModel
    {
        public FilePathViewModel()
        {

        }

        public FilePathViewModel(FilePath filePath)
        {
            if (filePath != null)
            {
                Id = filePath.Id;
                PageNumber = filePath.PageNumber;
                Path = filePath.Path;
                ChapterId = filePath.ChapterId;
            }
        }

        public Guid Id { get; set; }

        public int PageNumber { get; set; }

        public string Path { get; set; }

        public Guid ChapterId { get; set; }
    }
}
