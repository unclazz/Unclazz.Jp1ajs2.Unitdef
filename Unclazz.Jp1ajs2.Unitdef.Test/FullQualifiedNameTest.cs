using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unclazz.Jp1ajs2.Unitdef.Test
{
    [TestFixture]
    public class FullQualifiedNameTest
    {
        [Test]
        public void FromRoot_WhenSpecifiedNonEmptyString_ReturnsInstance()
        {
            // Arrange

            // Act
            IFullQualifiedName fqn = FullQualifiedName.FromFragments("XXXX0000");

            // Assert
            Assert.AreEqual(fqn.SuperUnitName, null);
            Assert.AreEqual(fqn.Fragments[0], "XXXX0000");
            Assert.AreEqual(fqn.Fragments.Count, 1);
        }

        [Test]
        public void FromRoot_WhenSpecifiedNull_ThrowsException()
        {
            // Arrange

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                FullQualifiedName.FromFragments(null);
            });
        }

        [Test]
        public void FromRoot_WhenSpecifiedEmptyString_ThrowsException()
        {
            // Arrange

            // Act

            // Assert
            Assert.Throws<ArgumentException>(() =>
            {
                FullQualifiedName.FromFragments("");
            });
        }

        [Test]
        public void GetSubUnitName_WhenSpecifiedNonEmptyString_ReturnsNewInstance()
        {
            // Arrange
            IFullQualifiedName fqn = FullQualifiedName.FromFragments("XXXX0000");

            // Act
            IFullQualifiedName fqn2 = fqn.GetSubUnitName("XXXX1000");

            // Assert
            Assert.AreNotSame(fqn, fqn2);
            Assert.AreEqual(fqn2.SuperUnitName, fqn);
            Assert.AreEqual(fqn2.Fragments[0], "XXXX0000");
            Assert.AreEqual(fqn2.Fragments[1], "XXXX1000");
            Assert.AreEqual(fqn2.Fragments.Count, 2);
        }

        [Test]
        public void GetSubUnitName_WhenSpecifiedNull_ThrowsException()
        {
            // Arrange
            IFullQualifiedName fqn = FullQualifiedName.FromFragments("XXXX0000");

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                fqn.GetSubUnitName(null);
            });
        }

        [Test]
        public void GetSubUnitName_WhenSpecifiedEmptyString_ThrowsException()
        {
            // Arrange
            IFullQualifiedName fqn = FullQualifiedName.FromFragments("XXXX0000");

            // Act

            // Assert
            Assert.Throws<ArgumentException>(() =>
            {
                fqn.GetSubUnitName("");
            });
        }

        [Test]
        public void Equals_When2InstanceRepresentSameSequence_ReturnsTrue()
        {
            // Arrange
            IFullQualifiedName fqn0 = FullQualifiedName.FromFragments("XXXX0000");
            IFullQualifiedName fqn1 = fqn0.GetSubUnitName("XXXX1000");
            IFullQualifiedName fqn2 = fqn0.GetSubUnitName("XXXX1100");
            IFullQualifiedName fqn3 = fqn0.GetSubUnitName("XXXX1000");

            // Act
            bool b0 = fqn0.Equals(null);
            bool b1 = fqn0.Equals(fqn1);
            bool b2 = fqn1.Equals(fqn2);
            bool b3 = fqn1.Equals(fqn3);
            bool b4 = fqn1.Equals(null);

            // Assert
            Assert.AreEqual(false, b0);
            Assert.AreEqual(false, b1);
            Assert.AreEqual(false, b2);
            Assert.AreEqual(true, b3);
            Assert.AreEqual(false, b4);
        }
    }
}
