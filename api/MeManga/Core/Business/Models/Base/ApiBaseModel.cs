using System;

namespace MeManga.Core.Business.Models.Base
{
    public class ApiBaseModel
    {
        public ApiBaseModel()
        {

        }
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
