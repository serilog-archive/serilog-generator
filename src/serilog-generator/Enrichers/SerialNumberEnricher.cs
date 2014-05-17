using System.Threading;
using Serilog.Core;

namespace Serilog.Generator.Enrichers
{
    class SerialNumberEnricher : ILogEventEnricher
    {
        int last;

        public void Enrich(Events.LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("SerialNumber", Interlocked.Increment(ref last)));
        }
    }
}
