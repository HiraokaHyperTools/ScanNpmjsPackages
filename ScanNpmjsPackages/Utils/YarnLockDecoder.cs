using ScanNpmjsPackages.Models;
using ScanNpmjsPackages.Models.YarnBased;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScanNpmjsPackages.Utils
{
    internal static class YarnLockDecoder
    {
        internal static YarnUse[] Decode(string[] lines)
        {
            var list = new List<YarnUse>();
            YarnUse use = null;

            foreach (var line in lines)
            {
                if (line.StartsWith("#"))
                {
                    continue;
                }
                if (line.Length == 0)
                {
                    if (use != null)
                    {
                        Complete(use);
                    }
                    use = null;
                    continue;
                }
                if (line.StartsWith(" ") || line.StartsWith("\t"))
                {
                    if (use == null)
                    {
                        continue;
                    }

                    var pair = line.Trim().Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
                    if (pair.Length != 2)
                    {
                        Complete(use);
                        use = null;
                        continue;
                    }
                    use.props[pair[0].Trim('"')] = pair[1].Trim('"');
                    continue;
                }
                use = new YarnUse
                {
                    refs = line,
                    name = GetName(line),
                };
                list.Add(use);
            }
            if (use != null)
            {
                Complete(use);
            }

            return list.ToArray();
        }

        private static void Complete(YarnUse use)
        {
            use.props.TryGetValue("version", out string version);
            use.version = version;
        }

        private static string GetName(string line)
        {
            line = line.Replace("\"", "").Split(',')[0].Trim().TrimEnd(':');

            if (line.StartsWith("@"))
            {
                return line.Substring(0, 1) + line.Substring(1).Split('@')[0];
            }
            return line.Split('@')[0];
        }
    }
}
