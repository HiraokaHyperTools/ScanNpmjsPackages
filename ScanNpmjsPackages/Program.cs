using CommandLine;
using NLog;
using ScanNpmjsPackages.Models;
using ScanNpmjsPackages.Models.CompromisedBased;
using ScanNpmjsPackages.Utils;
using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

[assembly: InternalsVisibleTo("Test")]

namespace ScanNpmjsPackages
{
    internal class Program
    {
        class Opts
        {
            [Option('d', "root-dir", Required = true)]
            public string RootDir { get; set; }
        }

        static int Main(string[] args) => Parser.Default.ParseArguments<Opts>(args)
            .MapResult(
                (Opts o) =>
                {
                    var compromised = (Compromised)new XmlSerializer(typeof(Compromised)).Deserialize(
                        new MemoryStream(
                            File.ReadAllBytes(
                                Path.Combine(
                                    AppDomain.CurrentDomain.BaseDirectory,
                                    "Compromised.xml"
                                )
                            )
                        )
                    );

                    var log = LogManager.GetLogger("Hit");

                    var scanner = new Scanner(compromised);
                    foreach (var hit in scanner.Scan(o.RootDir))
                    {
                        log.Warn(hit.File + "\t" + string.Join(" ", hit.Matches.Select(it => it.id).Distinct().AsEnumerable()));
                    }
                    return 0;
                },
                ex => 1
            );
    }
}
