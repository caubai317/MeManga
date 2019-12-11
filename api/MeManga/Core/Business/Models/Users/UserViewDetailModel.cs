using MeManga.Core.Business.Models.Base;
using MeManga.Core.Business.Models.Roles;
using MeManga.Core.Entities;
using MeManga.Core.Entities.Enums;
using System;
using System.Linq;

namespace MeManga.Core.Business.Models.Users
{
    public class UserViewDetailModel
    {
        public UserViewDetailModel()
        {

        }

        public UserViewDetailModel(User user) : this()
        {
            if (user != null)
            {
                Id = user.Id;
                Name = user.Name;
                Mobile = user.Mobile;
                Avatar = user.AvatarUrl;
                DateOfBirth = user.Birthday;
                JoinDate = user.CreatedOn;
                Gender = user.Gender;
                Role = user.Role != null ? (new RoleViewModel(user.Role)) : null;
            }
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Avatar { get; set; }
        public string Jobs { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? JoinDate { get; set; }
        public UserEnums.Gender Gender { get; set; }
        public RoleViewModel Role { get; set; }
    }
}
