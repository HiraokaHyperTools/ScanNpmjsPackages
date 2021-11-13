using NUnit.Framework;
using ScanNpmjsPackages.Models;
using ScanNpmjsPackages.Models.NpmBased;
using ScanNpmjsPackages.Utils;
using System;
using System.IO;
using System.Text.Json;

namespace Test
{
    public class Class1
    {
        [Test]
        public void PackageLockJson()
        {
            var packageLock = JsonSerializer.Deserialize<PackageLock>(
                File.ReadAllText(@"H:\Proj\ScanNpmjsPackages\Bad1\package-lock.json")
            );

            foreach (var pair in packageLock.dependencies)
            {
                Console.WriteLine($"# {pair.Key} {pair.Value.version}");
            }
        }

        [Test]
        public void YarnLock()
        {
            var uses = YarnLockDecoder.Decode(
                File.ReadAllLines(@"H:\Proj\ScanNpmjsPackages\Bad1\yarn.lock")
            );

            foreach (var use in uses)
            {
                Console.WriteLine($"# {use.name} {use.version}");
            }
        }
    }
}
