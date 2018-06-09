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
            var i = Reader.From("unit=foo,bar,baz,123;...");
            //                   012345678901234567890123

            // Act
            var r = p.Parse(i);

            // Assert
            Assert.That(r.Successful, Is.True);
            Assert.That(r.Capture.UnitName, Is.EqualTo("foo"));
            Assert.That(r.Capture.PermissionMode, Is.EqualTo("bar"));
            Assert.That(r.Capture.Jp1UserName, Is.EqualTo("baz"));
            Assert.That(r.Capture.ResourceGroupName, Is.EqualTo("123"));
            Assert.That(i.Position.Index, Is.EqualTo(21));
        }
        [Test]
        public void Parse_Case02()
        {
            // Arrange
            var p = new UnitParser2.AttributesParser();
            var i = Reader.From("unit=foo,,,;...");
            //                   012345678901234567890123

            // Act
            var r = p.Parse(i);

            // Assert
            Assert.That(r.Successful, Is.True);
            Assert.That(r.Capture.UnitName, Is.EqualTo("foo"));
            Assert.That(r.Capture.PermissionMode, Is.EqualTo(string.Empty));
            Assert.That(r.Capture.Jp1UserName, Is.EqualTo(string.Empty));
            Assert.That(r.Capture.ResourceGroupName, Is.EqualTo(string.Empty));
            Assert.That(i.Position.Index, Is.EqualTo(12));
        }
        [Test]
        public void Parse_Case03()
        {
            // Arrange
            var p = new UnitParser2.AttributesParser();
            var i = Reader.From("unit=foo,,;...");

            // Act
            var r = p.Parse(i);

            // Assert
            Assert.That(r.Successful, Is.True);
            Assert.That(r.Capture.UnitName, Is.EqualTo("foo"));
            Assert.That(r.Capture.PermissionMode, Is.EqualTo(string.Empty));
            Assert.That(r.Capture.Jp1UserName, Is.EqualTo(string.Empty));
            Assert.That(r.Capture.ResourceGroupName, Is.EqualTo(string.Empty));
        }
        [Test]
        public void Parse_Case04()
        {
            // Arrange
            var p = new UnitParser2.AttributesParser();
            var i = Reader.From("unit=foo,;...");

            // Act
            var r = p.Parse(i);

            // Assert
            Assert.That(r.Successful, Is.True);
            Assert.That(r.Capture.UnitName, Is.EqualTo("foo"));
            Assert.That(r.Capture.PermissionMode, Is.EqualTo(string.Empty));
            Assert.That(r.Capture.Jp1UserName, Is.EqualTo(string.Empty));
            Assert.That(r.Capture.ResourceGroupName, Is.EqualTo(string.Empty));
        }
        [Test]
        public void Parse_Case05()
        {
            // Arrange
            var p = new UnitParser2.AttributesParser();
            var i = Reader.From("unit=foo;...");

            // Act
            var r = p.Parse(i);

            // Assert
            Assert.That(r.Successful, Is.True);
            Assert.That(r.Capture.UnitName, Is.EqualTo("foo"));
            Assert.That(r.Capture.PermissionMode, Is.EqualTo(string.Empty));
            Assert.That(r.Capture.Jp1UserName, Is.EqualTo(string.Empty));
            Assert.That(r.Capture.ResourceGroupName, Is.EqualTo(string.Empty));
        }
        [Test]
        public void Parse_Case11()
        {
            // Arrange
            var p = new UnitParser2.AttributesParser();
            var i = Reader.From("unit=,,,;...");

            // Act
            var r = p.Parse(i);

            // Assert
            Assert.That(r.Successful, Is.False);
        }
        [Test]
        public void Parse_Case12()
        {
            // Arrange
            var p = new UnitParser2.AttributesParser();
            var i = Reader.From("unit=,bar,baz,123;...");

            // Act
            var r = p.Parse(i);

            // Assert
            Assert.That(r.Successful, Is.False);
        }
        [Test]
        public void Parse_Case13()
        {
            // Arrange
            var p = new UnitParser2.AttributesParser();
            var i = Reader.From("unit=,bar,baz,123,456;...");

            // Act
            var r = p.Parse(i);

            // Assert
            Assert.That(r.Successful, Is.False);
        }
	}
}
