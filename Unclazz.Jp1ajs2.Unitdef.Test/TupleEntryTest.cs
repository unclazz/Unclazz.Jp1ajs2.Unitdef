using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unclazz.Jp1ajs2.Unitdef.Test
{
    [TestFixture]
    public class TupleEntryTest
    {
        [Test]
        public void OfPair_WhenNonEmptyStringSpecified_ReturnsInstance()
        {
            // Arrange

            // Act
            ITupleEntry e0 = TupleEntry.OfPair("k0", "v0");

            // Assert
            Assert.AreEqual(true, e0.HasKey);
            Assert.AreEqual("k0", e0.Key);
            Assert.AreEqual("v0", e0.Value);
        }

        [Test]
        public void OfPair_WhenEmptyStringKeySpecified_ThrowsException()
        {
            // Arrange

            // Act

            // Assert
            Assert.Throws<ArgumentException>(() =>
            {
                ITupleEntry e0 = TupleEntry.OfPair("", "v0");
            });
        }

        [Test]
        public void OfPair_WhenNullKeySpecified_ReturnsInstanceHasNotKey()
        {
            // Arrange

            // Act
            ITupleEntry e0 = TupleEntry.OfPair(null, "v0");

            // Assert
            Assert.AreEqual(false, e0.HasKey);
            Assert.AreEqual(null, e0.Key);
            Assert.AreEqual("v0", e0.Value);
        }

        [Test]
        public void OfValue_WhenNonEmptyStringSpecified_ReturnsInstanceHasNotKey()
        {
            // Arrange

            // Act
            ITupleEntry e0 = TupleEntry.OfValue("v0");

            // Assert
            Assert.AreEqual(false, e0.HasKey);
            Assert.AreEqual(null, e0.Key);
            Assert.AreEqual("v0", e0.Value);
        }

        [Test]
        public void OfValue_WhenNullSpecified_ThrowsException()
        {
            // Arrange

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                ITupleEntry e0 = TupleEntry.OfValue(null);
            });
        }
    }
}
