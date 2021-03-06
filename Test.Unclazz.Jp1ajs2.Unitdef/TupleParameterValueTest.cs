﻿using NUnit.Framework;
using System.Linq;
using Unclazz.Jp1ajs2.Unitdef;

namespace Test.Unclazz.Jp1ajs2.Unitdef
{
    [TestFixture]
    public class TupleParameterValueTest
    {
        [Test]
        public void OfValue_WhenNotNullValueSpecified_ReturnsInstance()
        {
            // Arrange
            ITuple t0 = Tuple.FromCollection(new ITupleEntry[]{
                TupleEntry.OfValue("v0"), TupleEntry.OfPair("k1","v1")}.ToList());
            IParameterValue pv0 = TupleParameterValue.OfValue(t0);
            IParameterValue pv1 = TupleParameterValue.OfValue(Tuple.Empty);

            // Act

            // Assert
            Assert.That(pv0.StringValue, Is.EqualTo("(v0,k1=v1)"));
            Assert.That(pv1.StringValue, Is.EqualTo("()"));
        }

        [Test]
        public void OfValue_WhenNullValueSpecified_ThrowsException()
        {
            // Arrange

            // Act

            // Assert
            Assert.Throws<System.ArgumentNullException>(() => {
                TupleParameterValue.OfValue(null);
            });
        }
    }
}
