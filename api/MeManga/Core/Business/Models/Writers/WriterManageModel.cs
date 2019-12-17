using MeManga.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeManga.Core.Business.Models.Writers
{
    public class WriterManageModel
    {
        public string Name { get; set; }

        public string Information { get; set; }

        public void SetDataToModel(Writer writer)
        {
            writer.Name = Name;
            writer.Information = Information;
        }
    }
}
