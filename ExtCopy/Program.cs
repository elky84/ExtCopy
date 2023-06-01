// See https://aka.ms/new-console-template for more information

using CommandLine;
using ExtCopy;

{
    try
    {
        Parser.Default.ParseArguments<Options>(args)
            .WithParsed<Options>(opts => Run.Execute(opts))
            .WithNotParsed<Options>((errs) => Options.HandleParseError(errs));
    }
    catch (Exception exception)
    {
        Console.WriteLine($"Unhandled exception. <Reason:{exception.Message}> <StackTrace:{exception.StackTrace}>");
        Environment.Exit(ErrorCode.Exception);
    }
}