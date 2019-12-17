using MeManga.Core.Business.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeManga.Core.Business.Models.Writers
{
    public class WriterRequestListViewModel : RequestListViewModel
    {
        public WriterRequestListViewModel() : base()
        {

        }

        public string Query { get; set; }

        public bool? IsActive { get; set; }
    }
}
