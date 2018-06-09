using NUnit.Framework;
using Unclazz.Jp1ajs2.Unitdef.Parser;
using Unclazz.Parsec;

namespace Unclazz.Jp1ajs2.Unitdef.Test.Parser
{
    [TestFixture]
    public class UnitParser2_ParameterValueParserTest
    {
        [Test]
        public void Parse_Case01()
        {
            // Arrange
            var p = new UnitParser2.ParameterValueParser();
            var i = Reader.From("abc123,...");

            // Act
            var r = p.Parse(i);

            // Assert
            Assert.That(r.Successful, Is.True);
            Assert.That(r.Capture.TupleValue, Is.Null);
            Assert.That(r.Capture.StringValue, Is.EqualTo("abc123"));
            Assert.That(r.Capture.Type, Is.EqualTo(ParameterValueType.RawString));
            Assert.That(i.Position.Index, Is.EqualTo(6));
        }
        [Test]
        public void Parse_Case11()
        {
            // Arrange
            var p = new UnitParser2.ParameterValueParser();
            var i = Reader.From("\"abc123\",...");

            // Act
            var r = p.Parse(i);

            // Assert
            Assert.That(r.Successful, Is.True);
            Assert.That(r.Capture.TupleValue, Is.Null);
            Assert.That(r.Capture.StringValue, Is.EqualTo("abc123"));
            Assert.That(r.Capture.Type, Is.EqualTo(ParameterValueType.QuotedString));
            Assert.That(i.Position.Index, Is.EqualTo(8));
        }
        [Test]
        public void Parse_Case12()
        {
            // Arrange
            var p = new UnitParser2.ParameterValueParser();
            var i = Reader.From("\"\",...");

            // Act
            var r = p.Parse(i);

            // Assert
            Assert.That(r.Successful, Is.True);
            Assert.That(r.Capture.TupleValue, Is.Null);
            Assert.That(r.Capture.StringValue, Is.EqualTo(string.Empty));
            Assert.That(r.Capture.Type, Is.EqualTo(ParameterValueType.QuotedString));
            Assert.That(i.Position.Index, Is.EqualTo(2));
        }
        [Test]
        public void Parse_Case13()
        {
            // Arrange
            var p = new UnitParser2.ParameterValueParser();
            var i = Reader.From("\"abc#\"123##\",...");

            // Act
            var r = p.Parse(i);

            // Assert
            Assert.That(r.Successful, Is.True);
            Assert.That(r.Capture.TupleValue, Is.Null);
            Assert.That(r.Capture.StringValue, Is.EqualTo("abc\"123#"));
            Assert.That(r.Capture.Type, Is.EqualTo(ParameterValueType.QuotedString));
            Assert.That(i.Position.Index, Is.EqualTo(12));
        }
        [Test]
        public void Parse_Case14()
        {
            // Arrange
            var p = new UnitParser2.ParameterValueParser();
            var i = Reader.From("\"abc#\"123#\",...");

            // Act
            var r = p.Parse(i);

            // Assert
            Assert.That(r.Successful, Is.True);
            Assert.That(r.Capture.Type, Is.EqualTo(ParameterValueType.RawString));
            Assert.That(r.Capture.StringValue, Is.EqualTo(string.Empty));
        }
        [Test]
        public void Parse_Case21()
        {
            // Arrange
            var p = new UnitParser2.ParameterValueParser();
            var i = Reader.From("(f=foo,b=bar,123),...");

            // Act
            var r = p.Parse(i);

            // Assert
            Assert.That(r.Successful, Is.True);
            Assert.That(r.Capture.TupleValue.Count, Is.EqualTo(3));
            Assert.That(r.Capture.TupleValue[0], Is.EqualTo("foo"));
            Assert.That(r.Capture.TupleValue[1], Is.EqualTo("bar"));
            Assert.That(r.Capture.TupleValue[2], Is.EqualTo("123"));
            Assert.That(r.Capture.TupleValue["f"], Is.EqualTo("foo"));
            Assert.That(r.Capture.TupleValue["b"], Is.EqualTo("bar"));
            Assert.That(r.Capture.StringValue, Is.EqualTo("(f=foo,b=bar,123)"));
            Assert.That(r.Capture.Type, Is.EqualTo(ParameterValueType.Tuple));
            Assert.That(i.Position.Index, Is.EqualTo(17));
        }
        [Test]
        public void Parse_Case22()
        {
            // Arrange
            var p = new UnitParser2.ParameterValueParser();
            var i = Reader.From("(123),...");

            // Act
            var r = p.Parse(i);

            // Assert
            Assert.That(r.Successful, Is.True);
            Assert.That(r.Capture.TupleValue.Count, Is.EqualTo(1));
            Assert.That(r.Capture.TupleValue[0], Is.EqualTo("123"));
            Assert.That(r.Capture.StringValue, Is.EqualTo("(123)"));
            Assert.That(r.Capture.Type, Is.EqualTo(ParameterValueType.Tuple));
            Assert.That(i.Position.Index, Is.EqualTo(5));
        }
        [Test]
        public void Parse_Case23()
        {
            // Arrange
            var p = new UnitParser2.ParameterValueParser();
            var i = Reader.From("(,b=bar,123),...");

            // Act
            var r = p.Parse(i);

            // Assert
            Assert.That(r.Successful, Is.True);
            Assert.That(r.Capture.TupleValue.Count, Is.EqualTo(3));
            Assert.That(r.Capture.TupleValue[0], Is.EqualTo(""));
            Assert.That(r.Capture.TupleValue[1], Is.EqualTo("bar"));
            Assert.That(r.Capture.TupleValue[2], Is.EqualTo("123"));
            Assert.That(r.Capture.TupleValue["b"], Is.EqualTo("bar"));
            Assert.That(r.Capture.StringValue, Is.EqualTo("(,b=bar,123)"));
            Assert.That(r.Capture.Type, Is.EqualTo(ParameterValueType.Tuple));
        }
        [Test]
        public void Parse_Case24()
        {
            // Arrange
            var p = new UnitParser2.ParameterValueParser();
            var i = Reader.From("(f=foo,b=bar,),...");

            // Act
            var r = p.Parse(i);

            // Assert
            Assert.That(r.Successful, Is.True);
            Assert.That(r.Capture.TupleValue.Count, Is.EqualTo(3));
            Assert.That(r.Capture.TupleValue[0], Is.EqualTo("foo"));
            Assert.That(r.Capture.TupleValue[1], Is.EqualTo("bar"));
            Assert.That(r.Capture.TupleValue[2], Is.EqualTo(""));
            Assert.That(r.Capture.TupleValue["f"], Is.EqualTo("foo"));
            Assert.That(r.Capture.TupleValue["b"], Is.EqualTo("bar"));
            Assert.That(r.Capture.StringValue, Is.EqualTo("(f=foo,b=bar,)"));
            Assert.That(r.Capture.Type, Is.EqualTo(ParameterValueType.Tuple));
        }
        [Test]
        public void Parse_Case25()
        {
            // Arrange
            var p = new UnitParser2.ParameterValueParser();
            var i = Reader.From("(),...");

            // Act
            var r = p.Parse(i);

            // Assert
            Assert.That(r.Successful, Is.True);
            Assert.That(r.Capture.TupleValue.Count, Is.EqualTo(1));
            Assert.That(r.Capture.Type, Is.EqualTo(ParameterValueType.Tuple));
            Assert.That(r.Capture.StringValue, Is.EqualTo("()"));
        }
    }
}
