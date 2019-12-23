using MeManga.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeManga.Core.Business.Models.TypeBooks
{
    public class TypeBookManageModel
    {
        public string Name { get; set; }

        public void SetDataToModel(TypeBook typeBook)
        {
            typeBook.Name = Name;
        }
    }
}
