using System.Windows.Media;

namespace KProject.Application.Filters
{
    public abstract class FilterToken<T>
    {
        public virtual Brush TokenColor => Brushes.White;

        public abstract bool Check(T obj); 
    }
}
