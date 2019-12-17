using MeManga.Core.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeManga.Core.Entities
{
    [Table("User")]
    public class User : BaseEntity
    {
        #region Constructors
        public User() : base()
        {

        }

        public User(string email)
        {

        }

        #endregion

        #region Properties

        public int? Age
        {
            get
            {
                if (Birthday.HasValue)
                {
                    return (int)(DateTime.Now - Birthday.Value).TotalDays / 365;
                }
                return null;
            }
        }

        #region Base Properties

        [StringLength(255)]
        public string Email { get; set; }

        [StringLength(512)]
        public string Password { get; set; }

        [StringLength(512)]
        public string PasswordSalt { get; set; }

        [StringLength(512)]
        public string Name { get; set; }

        public DateTime? Birthday { get; set; }

        public UserEnums.Gender Gender { get; set; }

        public string About { get; set; }

        public string Mobile { get; set; }

        public string AvatarUrl { get; set; }

        [StringLength(512)]
        public string Google { get; set; }

        #endregion
        public Guid RoleId { get; set; }

        public Role Role { get; set; }

        public List<Comment> Comments { get; set; }

        public List<BookInType> BookInTypes { get; set; }

        public List<UserCollectionBook> UserCollectionBooks { get; set; }

        public List<Book> BookTranslates { get; set; }

        #endregion
    }
}