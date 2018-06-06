using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unclazz.Jp1ajs2.Unitdef;

namespace Unclazz.Jp1ajs2.Unitdef.Test
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
            Assert.Throws<ArgumentNullException>(() => {
                TupleParameterValue.OfValue(null);
            });
        }
    }
}
