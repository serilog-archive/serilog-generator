using System.Linq;
using NUnit.Framework;
using Serilog.Generator.CommandLine;
using Serilog.Generator.Configuration;

namespace Serilog.Generator.Tests.Configuration
{
    [TestFixture]
    public class CommandLineDslParserTests
    {
        [Test]
        public void ParsesDirectives()
        {
            var d = CommandLineDslParser.ParseCommandLine("--using=\"C:\\foo.bar.dll\" --write-to:Foo.bar=\"baz\"").ToArray();

            Assert.AreEqual(2, d.Length);

            var u = d[0];
            Assert.AreEqual("using", u.Operator);
            Assert.IsNull(u.Key1);
            Assert.IsNull(u.Key2);
            Assert.AreEqual("C:\\foo.bar.dll", u.Value);

            var w = d[1];
            Assert.AreEqual("write-to", w.Operator);
            Assert.AreEqual("Foo", w.Key1);
            Assert.AreEqual("bar", w.Key2);
            Assert.AreEqual("baz", w.Value);

        }
    }
}
