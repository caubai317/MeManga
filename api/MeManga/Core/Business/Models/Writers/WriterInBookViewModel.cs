using MeManga.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeManga.Core.Business.Models.Writers
{
    public class WriterInBookViewModel
    {
        public WriterInBookViewModel()
        {

        }

        public WriterInBookViewModel(Writer writer)
        {
            if (writer != null)
            {
                Name = writer.Name;
                Information = writer.Information;
            }
        }

        public string Name { get; set; }

        public string Information { get; set; }
    }
}
