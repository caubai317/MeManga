using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeManga.Core.Entities.Enums
{
    public class QuestionEnums
    {
        public enum Status
        {
            Pending = 1,
            Reject = 2,
            Approve = 3
        }

        public enum Level
        {
            Fresher = 1,
            Junior = 2,
            Senior = 3
        }

        public enum Type
        {
            BackEnd = 1,
            FrondEnd = 2,
            Design = 3
        }
    }
}
