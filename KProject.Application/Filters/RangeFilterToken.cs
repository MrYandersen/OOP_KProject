using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KProject.Application.Filters
{
    public abstract class RangeFilterToken<T> : FilterToken<T>
    {
        public virtual double MinValue { get; set; }
        public virtual double MaxValue { get; set; }
    }
}
