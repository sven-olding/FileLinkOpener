using NDesk.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace FileLinkOpener
{
    internal class Program
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(APPLICATION_TITLE);

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
                log.Error(e.Message);
                if (verbose)
                {
                    log.Debug(e.StackTrace);
                }
                Console.WriteLine("try flo --help for more information");
                return;
            }

            if (showHelp)
            {
                ShowHelp(p);
                return;
            }

            extra.ForEach(s =>
            {
                if (verbose)
                {
                    log.Debug("path param: " + s);
                }
                try
                {
                    Process.Start(s);
                }
                catch (Exception e)
                {
                    log.Error(e.Message, e);
                }
            });

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
