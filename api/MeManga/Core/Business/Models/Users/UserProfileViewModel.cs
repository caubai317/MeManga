using MeManga.Core.Entities;
using MeManga.Core.Entities.Enums;
using System;

namespace MeManga.Core.Business.Models.Users
{
    public class UserProfileViewModel
    {
        public UserProfileViewModel()
        {

        }

        public UserProfileViewModel(User user) : this()
        {
            if (user != null)
            {
                Id = user.Id;
                Name = user.Name;
                Avatar = user.AvatarUrl;
                Mobile = user.Mobile;
                DateOfBirth = user.Birthday;
                Email = user.Email;
                Gender = user.Gender;
            }
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Email { get; set; }
        public UserEnums.Gender Gender { get; set; }
    }
}
