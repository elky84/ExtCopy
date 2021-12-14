using CommandLine;
using System;
using System.Collections.Generic;

namespace RepeatedCopy
{

    public partial class Options
    {
        [Option('d', "directory", Required = false, HelpText = "Input excel file directory.")]
        public string Input { get; set; } = "";

        [Option('o', "output", Required = false, HelpText = "Input output directory")]
        public string Output { get; set; } = "output";

        [Option('c', "cleanup", Required = false, HelpText = "cleanup output directory")]
        public bool CleanUp { get; set; }

        public static void HandleParseError(IEnumerable<Error> errs)
        {
            foreach (var err in errs)
            {
                Console.WriteLine(err.ToString());
            }
        }
    }
}