using System;
using NUnit.Framework;
using Unclazz.Jp1ajs2.Unitdef.Parser;
using Unclazz.Parsec;

namespace Unclazz.Jp1ajs2.Unitdef.Test.Parser
{
    [TestFixture]
	public class UnitParser2_CommentParserTest
	{
        [Test]
        public void Parse_Case01()
        {
            // Arrange
            var p = new UnitParser2.CommentParser();
            var i = Reader.From("/**/...");

            // Act
            var r = p.Parse(i);

            // Assert
            Assert.That(r.Successful, Is.True);
            Assert.That(i.Position.Index, Is.EqualTo(4));
        }
        [Test]
        public void Parse_Case02()
        {
            // Arrange
            var p = new UnitParser2.CommentParser();
            var i = Reader.From("/*foo*/...");

            // Act
            var r = p.Parse(i);

            // Assert
            Assert.That(r.Successful, Is.True);
            Assert.That(i.Position.Index, Is.EqualTo(7));
        }
        [Test]
        public void Parse_Case03()
        {
            // Arrange
            var p = new UnitParser2.CommentParser();
            var i = Reader.From("/*\r\n*/...");

            // Act
            var r = p.Parse(i);

            // Assert
            Assert.That(r.Successful, Is.True);
            Assert.That(i.Position.Index, Is.EqualTo(6));
            Assert.That(i.Position.Column, Is.EqualTo(3));
            Assert.That(i.Position.Line, Is.EqualTo(2));
        }
        [Test]
        public void Parse_Case11()
        {
            // Arrange
            var p = new UnitParser2.CommentParser();
            var i = Reader.From("/**...");

            // Act
            var r = p.Parse(i);

            // Assert
            Assert.That(r.Successful, Is.False);
        }
	}
}
