using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Entities;

namespace KProject.Application.Filters.RangeTokens
{
    class YearOfIssueRangeToken<T> : RangeFilterToken<Vehicle>
    {
        public YearOfIssueRangeToken()
        {
            MinValue = int.MinValue;
            MaxValue = int.MaxValue;
        }

        public override bool Check(Vehicle obj)
        {
            return obj.YearOfIssue >= MinValue && obj.YearOfIssue <= MaxValue;
        }
    }
}
