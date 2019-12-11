using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeManga.Core.Entities
{
    [Table("UserCollectionBook")]
    public class UserCollectionBook : BaseEntity
    {
        public UserCollectionBook() : base()
        {

        }

        public bool Favorite { get; set; }

        public int Rating { get; set; }

        public Guid BookId { get; set; }

        public Book Book { get; set; }

        public Guid UserId { get; set; }

        public User User { get; set; }
    }
}