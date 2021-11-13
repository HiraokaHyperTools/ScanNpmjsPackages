using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScanNpmjsPackages.Models.NpmBased
{
    public class PackageLock
    {
        public string name { get; set; }
        public Dictionary<string, Dependency> dependencies { get; set; } = new Dictionary<string, Dependency>();
    }
}
