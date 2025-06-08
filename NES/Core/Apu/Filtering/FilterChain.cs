﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEStor.Core.Apu.Filtering
{
    public class FilterChain: Filter
    {
        public FilterChain()
        {
            Filters = new List<Filter>();
        }

        public List<Filter> Filters { get; private set; }

        public float Apply(float value)
        {
            foreach (Filter filter in Filters)
                value = filter.Apply(value);
            return value;
        }
    }
}
