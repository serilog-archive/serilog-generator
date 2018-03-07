using System;
using System.Linq;
using Serilog.Configuration;

namespace Serilog.Generator.Configuration
{
    class CommandLineSettings : ILoggerSettings
    {
        public void Configure(LoggerConfiguration configuration)
        {
            var directives = CommandLineDslParser.ParseCommandLine(Environment.CommandLine)
                .ToDictionary(kv => kv.Key, kv => kv.Value);

            if (!directives.ContainsKey("quiet"))
                configuration.WriteTo.Console();

            configuration.ReadFrom.KeyValuePairs(directives);
        }
    }
}