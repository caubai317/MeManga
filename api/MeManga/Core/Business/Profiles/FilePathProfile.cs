using AutoMapper;
using MeManga.Core.Business.Models.FilePaths;
using MeManga.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeManga.Core.Business.Profiles
{
    public class FilePathProfile : Profile
    {
        public FilePathProfile()
        {
            CreateMap<FilePath, FilePathManageModel>().ReverseMap();
        }
    }
}
