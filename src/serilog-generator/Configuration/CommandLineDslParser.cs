using System;
using System.Collections.Generic;
using System.Linq;
using Sprache;

namespace Serilog.Generator.Configuration
{
    public static class CommandLineDslParser
    {
        static readonly Parser<string> Key =
            Parse.Letter.Or(Parse.Char('-')).Or(Parse.Char('.')).Or(Parse.Char(':')).Many().Text();

        static readonly Parser<string> Value =
            from eq in Parse.Char('=')
            from open in Parse.Char('"')
            from val in Parse.CharExcept('"').Many().Text()
            from close in Parse.Char('"')
            select val;

        static readonly Parser<KeyValuePair<string, string>> Directive =
            from arg in Parse.String("--")
            from key in Key
            from val in Value.Optional()
            select new KeyValuePair<string, string>(key, val.GetOrDefault());


        public static IEnumerable<KeyValuePair<string, string>> ParseCommandLine(string commandLine)
        {
            var firstArg = commandLine.IndexOf("--", StringComparison.InvariantCulture);

            if (firstArg == -1)
                return new KeyValuePair<string, string>[0];

            var argsOnly = commandLine.Substring(firstArg);
            return Directive.Token().Many().End().Parse(argsOnly).ToArray();
        }
    }
}
