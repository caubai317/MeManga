using MeManga.Core.Business.Models.Roles;
using MeManga.Core.Entities;
using MeManga.Core.Entities.Enums;
using System;
using System.Linq;

namespace MeManga.Core.Business.Models.Users
{
    public class UserViewModel
    {
        public UserViewModel()
        {

        }

        public UserViewModel(User user) : this()
        {
            if (user != null)
            {
                Id = user.Id;
                Name = user.Name;
                Mobile = user.Mobile;
                Avatar = user.AvatarUrl;
                Birthday = user.Birthday;
                JoinDate = user.CreatedOn;
                Gender = user.Gender;
                Role = user.Role != null ? (new RoleViewModel(user.Role)) : null ;
                //Roles = user.UserInRoles != null ? user.UserInRoles.Select(y => new RoleViewModel(y.Role)).ToArray() : null;
            }
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Mobile { get; set; }

        public string Avatar { get; set; }

        public string Jobs { get; set; }

        public DateTime? Birthday { get; set; }

        public DateTime? JoinDate { get; set; }

        public UserEnums.Gender Gender { get; set; }

        public RoleViewModel Role { get; set; }
    }
}
