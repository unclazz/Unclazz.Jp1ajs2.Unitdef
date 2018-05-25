﻿using System;
using System.Linq;
using NUnit.Framework;

namespace Unclazz.Jp1ajs2.Unitdef.Test
{
    [TestFixture]
    public class MutableUnitTest
    {
        static readonly IUnit immutableUnit0 = Unit.FromString
               ("unit=XXXX0000,,,;" +
                "{ty=g;" +
                "unit=XXXX1000,,,;{ty=pj;sc=xxx;}" +
                "unit=XXXX2000,,,;{ty=j;sc=xxx;}" +
                "}");

        [Test]
        public void set_Name_ReplaceUnitName()
        {
            // Arrange
            var name1 = "ZZZZ1111";
            var mutable1 = immutableUnit0.AsMutable();

            // Act
            mutable1.Name = name1;

            // Assert
            Assert.That(mutable1.Name, Is.EqualTo(name1));
            Assert.That(mutable1.Attributes.UnitName, Is.EqualTo(name1));
            Assert.That(mutable1.FullQualifiedName.BaseName, Is.EqualTo(name1));
        }

        [Test]
        public void set_Name_WhenValueIsEmpty_ThrowsException()
        {
            // Arrange
            var name1 = string.Empty;
            var mutable1 = immutableUnit0.AsMutable();

            // Act
            // Assert
            Assert.That(() => mutable1.Name = name1, Throws.ArgumentException);

        }

        [Test]
        public void set_Name_WhenValueIsNull_ThrowsException()
        {
            // Arrange
            string name1 = null;
            var mutable1 = immutableUnit0.AsMutable();

            // Act
            // Assert
            Assert.That(() => mutable1.Name = name1, Throws.ArgumentNullException);

        }

        [Test]
        public void set_Type_ReplaceUnitType()
        {
            // Arrange
            var type1 = "n";
            var mutable1 = immutableUnit0.AsMutable();

            // Act
            mutable1.Type = UnitType.FromName(type1);

            // Assert
            Assert.That(mutable1.Type, Is.EqualTo(UnitType.Jobnet));
            Assert.That(mutable1.Parameters
                        .First(p => p.Name == "ty").Values[0].StringValue,
                        Is.EqualTo(type1));
        }
    }
}