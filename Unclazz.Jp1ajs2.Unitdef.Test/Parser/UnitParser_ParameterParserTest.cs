using NUnit.Framework;
using Unclazz.Jp1ajs2.Unitdef.Parser;
using Unclazz.Parsec;

namespace Unclazz.Jp1ajs2.Unitdef.Test.Parser
{
    [TestFixture]
    public class UnitParser_ParameterParserTest
    {
        [Test]
        public void Parse_Case01()
        {
            // Arrange
            var p = new UnitParser.ParameterParser();
            var i = Reader.From("param=foo;...");
            //                   0123456789012

            // Act
            var r = p.Parse(i);

            // Assert
            Assert.That(r.Successful, Is.True);
            Assert.That(r.Capture.Name, Is.EqualTo("param"));
            Assert.That(r.Capture.Values.Count, Is.EqualTo(1));
            Assert.That(r.Capture.Values[0].StringValue, Is.EqualTo("foo"));
            Assert.That(i.Position.Index, Is.EqualTo(10));
        }
        [Test]
        public void Parse_Case02()
        {
            // Arrange
            var p = new UnitParser.ParameterParser();
            var i = Reader.From("param=foo,bar,baz;...");
            //                   012345678901234567890

            // Act
            var r = p.Parse(i);

            // Assert
            Assert.That(r.Successful, Is.True);
            Assert.That(r.Capture.Name, Is.EqualTo("param"));
            Assert.That(r.Capture.Values.Count, Is.EqualTo(3));
            Assert.That(r.Capture.Values[0].StringValue, Is.EqualTo("foo"));
            Assert.That(r.Capture.Values[1].StringValue, Is.EqualTo("bar"));
            Assert.That(r.Capture.Values[2].StringValue, Is.EqualTo("baz"));
            Assert.That(i.Position.Index, Is.EqualTo(18));
        }
        [Test]
        public void Parse_Case03()
        {
            // Arrange
            var p = new UnitParser.ParameterParser();
            var i = Reader.From("param=(f=foo,b=bar,123),\"bar\",baz;...");

            // Act
            var r = p.Parse(i);

            // Assert
            Assert.That(r.Successful, Is.True);
            Assert.That(r.Capture.Name, Is.EqualTo("param"));
            Assert.That(r.Capture.Values.Count, Is.EqualTo(3));
            Assert.That(r.Capture.Values[0].StringValue, Is.EqualTo("(f=foo,b=bar,123)"));
            Assert.That(r.Capture.Values[0].Type, Is.EqualTo(ParameterValueType.Tuple));
            Assert.That(r.Capture.Values[1].StringValue, Is.EqualTo("bar"));
            Assert.That(r.Capture.Values[1].Type, Is.EqualTo(ParameterValueType.QuotedString));
            Assert.That(r.Capture.Values[2].StringValue, Is.EqualTo("baz"));
            Assert.That(r.Capture.Values[2].Type, Is.EqualTo(ParameterValueType.RawString));
        }
        [Test]
        public void Parse_Case04()
        {
            // Arrange
            var p = new UnitParser.ParameterParser();
            var i = Reader.From("param=baz,(f=foo,b=bar,123),\"bar\";...");

            // Act
            var r = p.Parse(i);

            // Assert
            Assert.That(r.Successful, Is.True);
            Assert.That(r.Capture.Name, Is.EqualTo("param"));
            Assert.That(r.Capture.Values.Count, Is.EqualTo(3));
            Assert.That(r.Capture.Values[0].StringValue, Is.EqualTo("baz"));
            Assert.That(r.Capture.Values[0].Type, Is.EqualTo(ParameterValueType.RawString));
            Assert.That(r.Capture.Values[1].StringValue, Is.EqualTo("(f=foo,b=bar,123)"));
            Assert.That(r.Capture.Values[1].Type, Is.EqualTo(ParameterValueType.Tuple));
            Assert.That(r.Capture.Values[2].StringValue, Is.EqualTo("bar"));
            Assert.That(r.Capture.Values[2].Type, Is.EqualTo(ParameterValueType.QuotedString));
        }
        [Test]
        public void Parse_Case05()
        {
            // Arrange
            var p = new UnitParser.ParameterParser();
            var i = Reader.From("param=\"bar\",baz,(f=foo,b=bar,123);...");

            // Act
            var r = p.Parse(i);

            // Assert
            Assert.That(r.Successful, Is.True);
            Assert.That(r.Capture.Name, Is.EqualTo("param"));
            Assert.That(r.Capture.Values.Count, Is.EqualTo(3));
            Assert.That(r.Capture.Values[0].StringValue, Is.EqualTo("bar"));
            Assert.That(r.Capture.Values[0].Type, Is.EqualTo(ParameterValueType.QuotedString));
            Assert.That(r.Capture.Values[1].StringValue, Is.EqualTo("baz"));
            Assert.That(r.Capture.Values[1].Type, Is.EqualTo(ParameterValueType.RawString));
            Assert.That(r.Capture.Values[2].StringValue, Is.EqualTo("(f=foo,b=bar,123)"));
            Assert.That(r.Capture.Values[2].Type, Is.EqualTo(ParameterValueType.Tuple));
        }
        [Test]
        public void Parse_Case11()
        {
            // Arrange
            var p = new UnitParser.ParameterParser();
            var i = Reader.From("param=;...");
            //                   0123456789012

            // Act
            var r = p.Parse(i);

            // Assert
            Assert.That(r.Successful, Is.True);
            Assert.That(r.Capture.Name, Is.EqualTo("param"));
            Assert.That(r.Capture.Values.Count, Is.EqualTo(1));
            Assert.That(r.Capture.Values[0].StringValue, Is.EqualTo(string.Empty));
        }
        [Test]
        public void Parse_Case12()
        {
            // Arrange
            var p = new UnitParser.ParameterParser();
            var i = Reader.From("param=,,;...");

            // Act
            var r = p.Parse(i);

            // Assert
            Assert.That(r.Successful, Is.True);
            Assert.That(r.Capture.Name, Is.EqualTo("param"));
            Assert.That(r.Capture.Values.Count, Is.EqualTo(3));
            Assert.That(r.Capture.Values[0].StringValue, Is.EqualTo(string.Empty));
            Assert.That(r.Capture.Values[1].StringValue, Is.EqualTo(string.Empty));
            Assert.That(r.Capture.Values[2].StringValue, Is.EqualTo(string.Empty));
        }
        [Test]
        public void Parse_Case21()
        {
            // Arrange
            var p = new UnitParser.ParameterParser();
            var i = Reader.From("unit=foo,bar,baz;...");

            // Act
            var r = p.Parse(i);

            // Assert
            Assert.That(r.Successful, Is.False);
        }
    }
}
