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
                log.Error(e.Message, e);
                Console.WriteLine("try flo --help for more information");
                return;
            }

            if (verbose)
                EnableVerboseLogging();

            if (showHelp)
            {
                ShowHelp(p);
                return;
            }

            extra.ForEach(s =>
            {
                log.Debug("path param: " + s);

                try
                {
                    int idx = s.IndexOf(URI_SCHEME_PREFIX);
                    log.Debug(idx);
                    String path = s;
                    if (idx > -1)
                    {
                        path = path.Substring(idx + URI_SCHEME_PREFIX.Length);
                    }
                    log.Debug("path: " + path);
                    Process.Start(path);
                }
                catch (Exception e)
                {
                    log.Error(e.Message, e);
                }
            });

        }

        private static void EnableVerboseLogging()
        {
            log4net.Repository.ILoggerRepository[] repositories = log4net.LogManager.GetAllRepositories();

            //Configure all loggers to be at the debug level.
            foreach (log4net.Repository.ILoggerRepository repository in repositories)
            {
                repository.Threshold = repository.LevelMap["DEBUG"];
                log4net.Repository.Hierarchy.Hierarchy hier = (log4net.Repository.Hierarchy.Hierarchy)repository;
                log4net.Core.ILogger[] loggers = hier.GetCurrentLoggers();
                foreach (log4net.Core.ILogger logger in loggers)
                {
                    ((log4net.Repository.Hierarchy.Logger)logger).Level = hier.LevelMap["DEBUG"];
                }
            }

            //Configure the root logger.
            log4net.Repository.Hierarchy.Hierarchy h = (log4net.Repository.Hierarchy.Hierarchy)log4net.LogManager.GetRepository();
            log4net.Repository.Hierarchy.Logger rootLogger = h.Root;
            rootLogger.Level = h.LevelMap["DEBUG"];
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
