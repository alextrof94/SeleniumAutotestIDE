using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumAutotest
{
    [Serializable]
    internal class Parameter
    {
        public string Name { get; set; }
        public string Pattern { get; set; }
        public string Value { get; set; }
    }
}
