using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScanNpmjsPackages.Utils
{
    internal class PackageRef
    {
        public string id;

        public override string ToString() => $"{id}";
    }
}
