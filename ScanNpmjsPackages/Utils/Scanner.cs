using NLog;
using ScanNpmjsPackages.Models;
using ScanNpmjsPackages.Models.CompromisedBased;
using ScanNpmjsPackages.Models.NpmBased;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ScanNpmjsPackages.Utils
{
    class Scanner
    {
        private readonly Logger log;
        private readonly Compromised compromised;

        public Scanner(Compromised compromised)
        {
            this.log = LogManager.GetCurrentClassLogger();
            this.compromised = compromised;
        }

        internal IEnumerable<Hit> Scan(string rootDir)
        {
            var list = new Stack<string>();
            list.Push(rootDir);

            while (list.Any())
            {
                var which = list.Pop();

                if (Directory.Exists(which))
                {
                    foreach (var hit in ScanDir(which))
                    {
                        yield return hit;
                    }

                    try
                    {
                        foreach (var subDir in Directory.GetDirectories(which).Reverse())
                        {
                            list.Push(subDir);
                        }
                    }
                    catch
                    {
                        // ignore
                    }
                }
            }
        }

        private IEnumerable<Hit> ScanDir(string dir)
        {
            {
                var yarnLockFile = Path.Combine(dir, "yarn.lock");
                if (File.Exists(yarnLockFile))
                {
                    log.Debug("Scanning npm: {0}", yarnLockFile);
                    var uses = YarnLockDecoder.Decode(File.ReadAllLines(yarnLockFile, Encoding.Latin1))
                            .Select(it => new PackageRef { id = $"{it.name}@{it.version}" })
                            .ToArray();
                    var match = Filter(uses);
                    if (match.Any())
                    {
                        yield return new Hit { File = yarnLockFile, Matches = match.ToArray(), };
                    }
                }
            }

            {
                var packageLockJsonFile = Path.Combine(dir, "package-lock.json");
                if (File.Exists(packageLockJsonFile))
                {
                    log.Debug("Scanning yarn: {0}", packageLockJsonFile);
                    var uses = JsonSerializer.Deserialize<PackageLock>(File.ReadAllText(packageLockJsonFile))
                            .dependencies
                            .Select(it => new PackageRef { id = $"{it.Key}@{it.Value.version}" })
                            .ToArray();
                    var match = Filter(uses);
                    if (match.Any())
                    {
                        yield return new Hit { File = packageLockJsonFile, Matches = match.ToArray(), };
                    }
                }
            }
        }

        private IEnumerable<PackageRef> Filter(IEnumerable<PackageRef> list)
        {
            var targets = compromised.Package
                .Select(it => it.id)
                .ToArray();
            return list
                .Where(it => targets.Contains(it.id));
        }
    }
}
