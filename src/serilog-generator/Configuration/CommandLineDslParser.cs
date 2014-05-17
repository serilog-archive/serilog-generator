using System;
using System.Collections.Generic;
using System.Linq;
using Serilog.Generator.CommandLine;
using Sprache;

namespace Serilog.Generator.Configuration
{
    public static class CommandLineDslParser
    {
        static readonly Parser<string> Operator =
            Parse.String("write-to").Text()
            .XOr(Parse.String("using").Text());

        static readonly Parser<string> Identifier =
            from first in Parse.Letter.Or(Parse.Char('_'))
            from rest in Parse.LetterOrDigit.Or(Parse.Char('_')).Many()
            select new string(new[] { first }.Concat(rest).ToArray());

        static readonly Parser<Tuple<string, string>> Arguments =
            from init in Parse.Char(':')
            from first in Identifier
            from second in Parse.Char('.').Then(_ => Identifier).Optional()
            select Tuple.Create(first, second.IsDefined ? second.Get() : null);

        static readonly Parser<Directive> Directive =
            from arg in Parse.String("--")
            from op in Operator
            from args in Arguments.Optional()
            from eq in Parse.Char('=')
            from open in Parse.Char('"')
            from val in Parse.CharExcept('"').Many().Text()
            from close in Parse.Char('"')
            select args.IsDefined ?
                new Directive(op, args.Get().Item1, args.Get().Item2, val) :
                new Directive(op, null, null, val);


        public static IEnumerable<Directive> ParseCommandLine(string commandLine)
        {
            var firstArg = commandLine.IndexOf("--", StringComparison.InvariantCulture);

            if (firstArg == -1)
                return new Directive[0];

            var argsOnly = commandLine.Substring(firstArg);
            return Directive.Token().Many().End().Parse(argsOnly).ToArray();
        }
    }
}
