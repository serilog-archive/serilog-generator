using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Serilog.Configuration;
using Serilog.Generator.CommandLine;
using Serilog.Sinks.RollingFile;

namespace Serilog.Generator.Configuration
{
    static class CommandLineReader
    {
        public static LoggerConfiguration ReadCommandLine(this LoggerConfiguration loggerConfiguration)
        {
            var directives = CommandLineDslParser.ParseCommandLine(Environment.CommandLine);

            var sinkDirectives = (from wt in directives
                                  where wt.Operator == "write-to"
                                  let call = new
                                  {
                                      Method = wt.Key1,
                                      Argument = wt.Key2,
                                      wt.Value
                                  }
                                  group call by call.Method).ToList();

            if (sinkDirectives.Any())
            {
                var extensionMethods = FindExtensionMethods(directives);

                var sinkConfigurationMethods = extensionMethods
                    .Where(m => m.GetParameters()[0].ParameterType == typeof(LoggerSinkConfiguration))
                    .ToList();

                foreach (var sinkDirective in sinkDirectives)
                {
                    // Let's not recreate the binder; simple as possible here.

                    var target = sinkConfigurationMethods
                        .Where(m => m.Name == sinkDirective.Key &&
                            m.GetParameters().Skip(1).All(p => p.HasDefaultValue || sinkDirective.Any(s => s.Argument == p.Name)))
                        .OrderByDescending(m => m.GetParameters().Length)
                        .FirstOrDefault();

                    if (target != null)
                    {
                        var config = loggerConfiguration.WriteTo;

                        var call = (from p in target.GetParameters().Skip(1)
                                    let directive = sinkDirective.FirstOrDefault(s => s.Argument == p.Name)
                                    select directive == null ? p.DefaultValue : ConvertToType(directive.Value, p.ParameterType)).ToList();

                        call.Insert(0, config);

                        target.Invoke(null, call.ToArray());
                    }
                }
            }

            return loggerConfiguration;
        }

        static object ConvertToType(string value, Type toType)
        {
            if (toType.IsGenericType && toType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                // unwrap Nullable<> type since we're not handling null situations
                toType = (new NullableConverter(toType)).UnderlyingType;
            }

            var extendedTypeConversions = new Dictionary<Type, Func<string, object>>
            {
                { typeof(Uri), s => new Uri(s) },
                { typeof(TimeSpan), s => TimeSpan.Parse(s) }
            };

            var convertor = extendedTypeConversions
                .Where(t => t.Key.IsAssignableFrom(toType))
                .Select(t => t.Value)
                .FirstOrDefault();

            return convertor == null ? Convert.ChangeType(value, toType) : convertor(value);
        }

        static IList<MethodInfo> FindExtensionMethods(IEnumerable<Directive> directives)
        {
            var extensionAssemblies = new List<Assembly> { typeof(ILogger).Assembly, typeof(RollingFileSink).Assembly };
            foreach (var usingDirective in directives.Where(d => d.Operator.Equals("using")))
            {
                Assembly assembly;
                if (File.Exists(usingDirective.Value))
                {
                    assembly = Assembly.LoadFrom(usingDirective.Value);
                }
                else
                {
                    assembly = Assembly.Load(usingDirective.Value);
                }
                extensionAssemblies.Add(assembly);
            }

            return extensionAssemblies
                .SelectMany(a => a.ExportedTypes.Where(t => t.IsSealed && t.IsAbstract && !t.IsNested))
                .SelectMany(t => t.GetMethods())
                .Where(m => m.IsStatic && m.IsPublic && m.IsDefined(typeof(ExtensionAttribute), false))
                .ToList();
        }
    }
}
