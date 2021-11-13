using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScanNpmjsPackages.Models.YarnBased
{
    public class YarnUse
    {
        internal string refs { get; set; }
        internal Dictionary<string, string> props { get; set; } = new Dictionary<string, string>();

        public string name { get; set; }
        public string version { get; set; }
    }
}
