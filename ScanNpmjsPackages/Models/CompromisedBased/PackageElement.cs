using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ScanNpmjsPackages.Models.CompromisedBased
{
    public class PackageElement
    {
        [XmlAttribute] public string id;
    }
}
