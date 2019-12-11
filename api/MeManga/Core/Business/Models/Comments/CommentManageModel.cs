using MeManga.Core.Business.IoC;
using MeManga.Core.Common.Constants;
using MeManga.Core.DataAccess.Repositories.Base;
using MeManga.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MeManga.Core.Business.Models.Comments
{
    public class CommentManageModel : IValidatableObject
    {
        public CommentManageModel()
        {

        }

        public void SetDataToModel(Comment comment)
        {
            comment.Content = Content;
        }
       
        [Required]
        public string Content { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(Content.Trim() == null)
            {
                yield return new ValidationResult(CommentMessageConstants.INVALID_CONTENT, new string[] { "Content" });
            }
        }
    }
}
