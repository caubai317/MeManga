using MeManga.Core.Business.Models.Base;

namespace MeManga.Core.Business.Models.Users
{
    public class UserRequestListViewModel : RequestListViewModel
    {
        public UserRequestListViewModel() : base()
        {

        }

        public string Query { get; set; }
        public bool? IsActive { get; set; }
    }
}
