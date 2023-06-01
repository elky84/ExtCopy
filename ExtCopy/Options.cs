using CommandLine;

namespace ExtCopy
{

    // ReSharper disable once ClassNeverInstantiated.Global
    public partial class Options
    {
        [Option('i', "input", Required = true, HelpText = "Input directory.")]
        public string Input { get; set; } = "";

        [Option('o', "output", Required = true, HelpText = "Output directory")]
        public string Output { get; set; } = "output";

        [Option('c', "cleanup", Required = false, HelpText = "cleanup output directory")]
        public bool CleanUp { get; set; }

        [Option('r', "repeat", Required = false, HelpText = "repeat condition")]
        public bool Repeat { get; set; }

        [Option('p', "pattern", Required = false, HelpText = "regex pattern")]
        public string Pattern { get; set; } = "";

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
