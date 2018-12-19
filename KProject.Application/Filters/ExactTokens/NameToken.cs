using System.Windows.Media;

using Model.Entities;

namespace KProject.Application.Filters.ExactTokens
{
    class NameToken : ExactFilterToken<Vehicle>
    {
        public override Brush TokenColor => Brushes.Bisque;
        public bool IgnoreCase { get; set; }

        public NameToken(string value, bool ignoreCase) : base(value)
        {
            IgnoreCase = ignoreCase;
        }

        public override bool Check(Vehicle obj)
        {
            return string.Compare(obj.Name, FilteringValue as string, IgnoreCase) == 0;
        }

        public override string ToString()
        {
            return $"Name: {FilteringValue}";
        }
    }
}
