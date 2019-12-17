using MeManga.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeManga.Core.Business.Models.TypeBooks
{
    public class TypeBookViewModel
    {
        public TypeBookViewModel()
        {

        }

        public TypeBookViewModel(TypeBook typeBook) : this()
        {
            if(typeBook != null)
            {
                Name = typeBook.Name;
            }            
        }

        public string Name { get; set; }
    }
}
