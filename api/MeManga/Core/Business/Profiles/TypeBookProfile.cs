using AutoMapper;
using MeManga.Core.Business.Models.TypeBooks;
using MeManga.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeManga.Core.Business.Profiles
{
    public class TypeBookProfile : Profile
    {
        public TypeBookProfile()
        {
            CreateMap<TypeBook, TypeBookManageModel>().ReverseMap();
        }
    }
}
