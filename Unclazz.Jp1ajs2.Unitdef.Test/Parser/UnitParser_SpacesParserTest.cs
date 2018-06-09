using NUnit.Framework;
using Unclazz.Jp1ajs2.Unitdef.Parser;
using Unclazz.Parsec;

namespace Unclazz.Jp1ajs2.Unitdef.Test.Parser
{
    [TestFixture]
    public class UnitParser_SpacesParserTest
    {
        [Test]
        public void Parse_Case01()
        {
            // Arrange
            var p = new UnitParser.SpacesParser();
            var i = Reader.From("/*foo*/...");

            // Act
            var r = p.Parse(i);

            // Assert
            Assert.That(r.Successful, Is.True);
            Assert.That(i.Position.Index, Is.EqualTo(7));
        }
        [Test]
        public void Parse_Case02()
        {
            // Arrange
            var p = new UnitParser.SpacesParser();
            var i = Reader.From("  /*foo*/...");

            // Act
            var r = p.Parse(i);

            // Assert
            Assert.That(r.Successful, Is.True);
            Assert.That(i.Position.Index, Is.EqualTo(9));
        }
        [Test]
        public void Parse_Case03()
        {
            // Arrange
            var p = new UnitParser.SpacesParser();
            var i = Reader.From("\r\n/*foo*/...");

            // Act
            var r = p.Parse(i);

            // Assert
            Assert.That(r.Successful, Is.True);
            Assert.That(i.Position.Index, Is.EqualTo(9));
        }
        [Test]
        public void Parse_Case11()
        {
            // Arrange
            var p = new UnitParser.SpacesParser();
            var i = Reader.From("/*foo*//*foo*/...");
            //                   01234567890123456

            // Act
            var r = p.Parse(i);

            // Assert
            Assert.That(r.Successful, Is.True);
            Assert.That(i.Position.Index, Is.EqualTo(14));
        }
        [Test]
        public void Parse_Case12()
        {
            // Arrange
            var p = new UnitParser.SpacesParser();
            var i = Reader.From("  /*foo*/  /*foo*/...");
            //                   012345678901234567890

            // Act
            var r = p.Parse(i);

            // Assert
            Assert.That(r.Successful, Is.True);
            Assert.That(i.Position.Index, Is.EqualTo(18));
        }
        [Test]
        public void Parse_Case13()
        {
            // Arrange
            var p = new UnitParser.SpacesParser();
            var i = Reader.From("\r\n/*foo*/\r\n/*foo*/...");
            //                    0 12345678 9 01234567890

            // Act
            var r = p.Parse(i);

            // Assert
            Assert.That(r.Successful, Is.True);
            Assert.That(i.Position.Index, Is.EqualTo(18));
        }
    }
}
