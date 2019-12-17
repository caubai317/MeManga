using MeManga.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeManga.Core.Business.Models.FilePaths
{
    public class FilePathManageModel
    {
        public int PageNumber { get; set; }

        public string Path { get; set; }

        public Guid ChapterId { get; set; }

        public void SetDataToModel(FilePath filePath)
        {
            filePath.Path = Path;
            filePath.PageNumber = PageNumber;
            filePath.ChapterId = ChapterId;
        }
    }
}
