using NUnit.Framework;
using Unclazz.Jp1ajs2.Unitdef.Parser;
using Unclazz.Parsec;

namespace Unclazz.Jp1ajs2.Unitdef.Test.Parser
{
    [TestFixture]
    public class UnitParser2_SharpEscapeParserTest
    {
        [Test]
        public void Parse_Case01()
        {
            // Arrange
            var p = new UnitParser2.SharpEscapeParser();
            var i = Reader.From("#\"...");

            // Act
            var r = p.Parse(i);

            // Assert
            Assert.That(r.Successful, Is.True);
            Assert.That(r.Capture, Is.EqualTo('"'));
            Assert.That(i.Position.Index, Is.EqualTo(2));
        }
        [Test]
        public void Parse_Case02()
        {
            // Arrange
            var p = new UnitParser2.SharpEscapeParser();
            var i = Reader.From("##...");

            // Act
            var r = p.Parse(i);

            // Assert
            Assert.That(r.Successful, Is.True);
            Assert.That(r.Capture, Is.EqualTo('#'));
            Assert.That(i.Position.Index, Is.EqualTo(2));
        }
        [Test]
        public void Parse_Case03()
        {
            // Arrange
            var p = new UnitParser2.SharpEscapeParser();
            var i = Reader.From("#x...");

            // Act
            var r = p.Parse(i);

            // Assert
            Assert.That(r.Successful, Is.False);
        }

    }
}
