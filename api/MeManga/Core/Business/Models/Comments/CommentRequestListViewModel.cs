﻿using MeManga.Core.Business.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeManga.Core.Business.Models.Comments
{
    public class CommentRequestListViewModel : RequestListViewModel
    {
        public CommentRequestListViewModel()
            : base()
        {

        }
        public string Query { get; set; }

        public bool? IsActive { get; set; }
    }
}
