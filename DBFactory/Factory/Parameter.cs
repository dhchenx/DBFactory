using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBFactory.Factory
{
    // Author: Donghua Chen
    public class Parameter
    {
        public string Name = string.Empty;
        public object Value = null;
        public Parameter(string name, object value)
        {
            this.Name = name;
            this.Value = value;
        }
    }
}
