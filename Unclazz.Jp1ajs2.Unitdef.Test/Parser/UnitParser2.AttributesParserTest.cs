using System;
using NUnit.Framework;
using Unclazz.Jp1ajs2.Unitdef.Parser;
using Unclazz.Parsec;

namespace Unclazz.Jp1ajs2.Unitdef.Test.Parser
{
    [TestFixture]
	public class UnitParser2_AttributesParserTest
	{
        [Test]
        public void Parse_Case01()
        {
            // Arrange
            var p = new UnitParser2.AttributesParser();
            var i = Reader.From("unit=foo,bar,baz,123;...", logAppender: Console.WriteLine);

            // Act
            var r = p.Parse(i);

            // Assert
            Assert.That(r.Successful, Is.True);
            Assert.That(r.Capture.UnitName, Is.EqualTo("foo"));
        }
	}
}
