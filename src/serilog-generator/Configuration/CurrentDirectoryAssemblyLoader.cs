using System;
using System.IO;
using System.Reflection;

namespace Serilog.Generator.Configuration
{
    static class CurrentDirectoryAssemblyLoader
    {
        public static void Install()
        {
            AppDomain.CurrentDomain.AssemblyResolve += (s, e) =>
            {
                var assemblyPath = Path.Combine(Environment.CurrentDirectory, e.Name + ".dll");
                return !File.Exists(assemblyPath) ? null : Assembly.LoadFrom(assemblyPath);
            };
        }
    }
}