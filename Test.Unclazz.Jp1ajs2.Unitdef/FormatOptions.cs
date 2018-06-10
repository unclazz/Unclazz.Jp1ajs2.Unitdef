using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Unclazz.Jp1ajs2.Unitdef;

namespace Test.Unclazz.Jp1ajs2.Unitdef
{
    [TestFixture]
    public class FormatOptionsTest
    {
        [Test]
        public void NewLine_WhenValueIsNull_ThrowsException()
        {
            Assert.That(() => 
            {
                var o = new FormatOptions { NewLine = null };
                Assert.Fail("FormatOptions.NewLine == {0}", o.NewLine);

            }, Throws.ArgumentNullException);
        }
        [Test]
        public void NewLine_WhenValueIsEmpty_DoesNotThrowException()
        {
            var o = new FormatOptions { NewLine = string.Empty };
            Assert.That(o.NewLine, Is.EqualTo(string.Empty));
        }
        [Test]
        public void TabSize_WhenValueIsLessThan0_ThrowsException()
        {
            Assert.That(() =>
            {
                var o = new FormatOptions { TabSize = -1 };
                Assert.Fail("FormatOptions.TabSize == {0}", o.TabSize);

            }, Throws.InstanceOf<ArgumentOutOfRangeException>());
        }
        [Test]
        public void TabSize_WhenValueIsZero_DoesNotThrowException()
        {
            var o = new FormatOptions { TabSize = 0 };
            Assert.That(o.TabSize, Is.EqualTo(0));
        }
    }
}
