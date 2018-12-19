namespace KProject.Application.Filters
{
    public abstract class ExactFilterToken<T> : FilterToken<T>
    {
        public object FilteringValue { get; set; }

        protected ExactFilterToken(object value)
        {
            FilteringValue = value;
        }
    }
}
