using System.Linq;
using Xunit;
using Serilog.Generator.Configuration;

namespace Serilog.Generator.Tests.Configuration
{
   
    public class CommandLineDslParserTests
    {
        [Fact]
        public void ParsesDirectives()
        {
            var d = CommandLineDslParser.ParseCommandLine("--using=\"C:\\foo.bar.dll\" --write-to:Foo.bar=\"baz\" --write-to:Quux").ToArray();
            
            Assert.Equal(3, d.Length);

            var u = d[0];
            Assert.Equal("using", u.Key);
            Assert.Equal("C:\\foo.bar.dll", u.Value);

            var w = d[1];
            Assert.Equal("write-to:Foo.bar", w.Key);
            Assert.Equal("baz", w.Value);

            var wn = d[2];
            Assert.Equal("write-to:Quux", wn.Key);
            Assert.Null(wn.Value);
        }

        [Fact]
        public void UseCase()
        {

            var d = CommandLineDslParser.ParseCommandLine("dotnet src/serilog-generator/out/serilog-generator.dll --using=\"Serilog.Sinks.File\" --write-to:File.path=\"test.txt\"").ToArray();
            
            Assert.Equal(2, d.Length);

            var u = d[0];
            Assert.Equal("using", u.Key);
            Assert.Equal("Serilog.Sinks.File", u.Value);

            var w = d[1];
            Assert.Equal("write-to:File.path", w.Key);
            Assert.Equal("test.txt", w.Value);
 
        }
    }
}
