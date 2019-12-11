using MeManga.Core.Business.Models.Users;
using MeManga.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeManga.Core.Business.Models.Comments
{
    public class CommentViewModel
    {
        public CommentViewModel()
        {

        }

        public CommentViewModel(Comment comment) : this()
        {
            if (comment != null)
            {
                id = comment.Id;
                Content = comment.Content;
                Username = comment.User != null ? comment.User.Name : string.Empty;
                DateComment = comment.CreatedOn;
            }
        }

        public Guid id { get; set; }

        public string Content { get; set; }

        public string Username { get; set; }

        public DateTime? DateComment { get; set; }
    }
}
