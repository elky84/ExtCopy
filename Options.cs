using CommandLine;
using System;
using System.Collections.Generic;

namespace RepeatedCopy
{

    public partial class Options
    {
        [Option('i', "input", Required = true, HelpText = "Input excel file directory.")]
        public string Input { get; set; } = "";

        [Option('o', "output", Required = true, HelpText = "Input output directory")]
        public string Output { get; set; } = "output";

        [Option('c', "cleanup", Required = false, HelpText = "cleanup output directory")]
        public bool CleanUp { get; set; }

        [Option('r', "repeat", Required = false, HelpText = "repeat condition")]
        public bool Repeat { get; set; }

        [Option('t', "time", Required = false, HelpText = "time interval (unit seconds)")]
        public int TimeInterval { get; set; } = 0;

        public static void HandleParseError(IEnumerable<Error> errs)
        {
            foreach (var err in errs)
            {
                Console.WriteLine(err.ToString());
            }
        }
    }
}