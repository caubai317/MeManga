using MeManga.Core.Business.Models.Base;
using MeManga.Core.Entities;

namespace MeManga.Core.Business.Models.Roles
{
    public class RoleViewModel : ApiBaseModel
    {
        public RoleViewModel() : base()
        {

        }

        public RoleViewModel(Role role)
        {
            if (role != null)
            {
                Id = role.Id;
                Name = role.Name;
            }
        }
    }
}
