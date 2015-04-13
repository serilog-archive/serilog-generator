using Serilog.Configuration;

namespace Serilog.Generator.Configuration
{
    static class CommandLineReader
    {
        /// <summary>
        /// Supports command-line syntax like: --quiet --write-to:Console.outputTemplate="{Message}{NewLine}"
        /// </summary>
        /// <param name="settingsConfiguration"></param>
        /// <returns></returns>
        public static LoggerConfiguration CommandLine(this LoggerSettingsConfiguration settingsConfiguration)
        {
            return settingsConfiguration.Settings(new CommandLineSettings());
        }
    }
}
