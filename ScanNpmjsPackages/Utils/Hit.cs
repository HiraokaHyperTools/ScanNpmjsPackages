using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScanNpmjsPackages.Utils
{
    internal class Hit
    {
        public string File { get; set; }
        public PackageRef[] Matches { get; set; }
    }
}
