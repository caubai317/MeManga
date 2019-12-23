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
                Id = typeBook.Id;
                Name = typeBook.Name;
            }            
        }

        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
