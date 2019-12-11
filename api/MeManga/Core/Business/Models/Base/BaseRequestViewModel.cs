namespace MeManga.Core.Business.Models.Base
{
    public class BaseRequestViewModel
    {
        public BaseRequestViewModel()
        {
        }

        public int? Skip { get; set; }
        public int? Take { get; set; }
    }
}
