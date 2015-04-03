using System.Linq;
using NUnit.Framework;
using Serilog.Generator.Configuration;

namespace Serilog.Generator.Tests.Configuration
{
    [TestFixture]
    public class CommandLineDslParserTests
    {
        [Test]
        public void ParsesDirectives()
        {
            var d = CommandLineDslParser.ParseCommandLine("--using=\"C:\\foo.bar.dll\" --write-to:Foo.bar=\"baz\" --write-to:Quux").ToArray();

            Assert.AreEqual(3, d.Length);

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

            var wn = d[2];
            Assert.AreEqual("write-to", wn.Operator);
            Assert.AreEqual("Quux", wn.Key1);
            Assert.IsNull(wn.Key2);
            Assert.IsNull(wn.Value);
        }
    }
}
