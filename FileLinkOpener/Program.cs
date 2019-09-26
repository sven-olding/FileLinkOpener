using NDesk.Options;
using System;
using System.Collections.Generic;

namespace FileLinkOpener
{
    internal class Program
    {
        private const string URI_SCHEME_PREFIX = "flo://";

        private const string APPLICATION_TITLE = "FileLinkOpener";

        private static void Main(string[] args)
        {
            bool showHelp = false;
            bool verbose = false;

            var p = new OptionSet()
            {
                {"v", "verbose output", v => verbose = v != null },
                {"h|help|?", "show help message", v => showHelp = v != null}
            };

            List<string> extra;
            try
            {
                extra = p.Parse(args);
            }
            catch (OptionException e)
            {
                Console.WriteLine(APPLICATION_TITLE + ":");
                Console.WriteLine(e.Message);
                if (verbose)
                {
                    Console.WriteLine(e.StackTrace);
                }
                Console.WriteLine("try flo --help for more information");
                return;
            }

            if (showHelp)
            {
                ShowHelp(p);
                return;
            }

            if (verbose)
            {
                extra.ForEach(s => Console.WriteLine(s));
            }
        }

        private static void ShowHelp(OptionSet p)
        {
            Console.WriteLine(APPLICATION_TITLE + " usage: flo [OPTIONS] path");
            Console.WriteLine();
            Console.WriteLine("Options:");
            p.WriteOptionDescriptions(Console.Out);
        }
    }
}
