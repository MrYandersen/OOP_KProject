using System.Windows.Media;

using Model.Entities;

namespace KProject.Application.Filters.ExactTokens
{
    class YearOfIssueExactToken : ExactFilterToken<Vehicle>
    {
        public override Brush TokenColor => Brushes.Wheat;

        public YearOfIssueExactToken(int value) : base(value)
        { }

        public override bool Check(Vehicle obj)
        {
            return obj.YearOfIssue == (int)FilteringValue;
        }

        public override string ToString()
        {
            return $"Year of issue: {(int)FilteringValue}";
        }
    }
}
