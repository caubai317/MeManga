using MeManga.Core.Entities;
using MeManga.Core.Entities.Enums;
using System;

namespace MeManga.Core.Business.Models.Users
{
    public class UserUpdateProfileModel
    {
        public string Name { get; set; }

        public string Mobile { get; set; }

        public string Avatar { get; set; }

        public string Jobs { get; set; }

        public DateTime? Birthday { get; set; }

        public UserEnums.Gender Gender { get; set; }

        public string IdentityNumber { get; set; }

        public string Nationality { get; set; }

        public Guid? BankingId { get; set; }

        public string BankingNumber { get; set; }

        public string[] InvestTypes { get; set; }

        public string[] InvestExchanges { get; set; }


        public User GetUserFromModel(User user)
        {
            user.Name = Name;
            user.Mobile = Mobile;
            user.AvatarUrl = Avatar;
            user.Birthday = Birthday;
            user.Gender = Gender;

            return user;
        }
    }
}
