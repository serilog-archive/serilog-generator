using Serilog.Configuration;

namespace Serilog.Generator.Configuration
{
    static class CommandLineReader
    {
        public static LoggerConfiguration CommandLine(this LoggerSettingsConfiguration settingsConfiguration)
        {
            return settingsConfiguration.Settings(new CommandLineSettings());
        }
    }
}
