﻿using MeManga.Core.Business.Models.Base;
using MeManga.Core.Business.Models.Books;
using MeManga.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeManga.Core.Business.Models.Writers
{
    public class WriterViewModel 
    {
        public WriterViewModel()
        {

        }

        public WriterViewModel(Writer writer)
        {
            if (writer != null)
            {
                Id = writer.Id;
                Name = writer.Name;
                Information = writer.Information;
            }
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Information { get; set; }
    }
}
