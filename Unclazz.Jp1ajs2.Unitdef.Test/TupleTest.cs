using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unclazz.Jp1ajs2.Unitdef.Test
{
    [TestFixture]
    public class TupleTest
    {
        private ITuple Empty()
        {
            return Tuple.Empty;
        }

        private ITuple _2EntriesHaveKey()
        {
            IList<ITupleEntry> col = new List<ITupleEntry>();
            col.Add(TupleEntry.OfPair("k0", "v0"));
            col.Add(TupleEntry.OfPair("k1", "v1"));
            return Tuple.FromCollection(col);
        }

        private ITuple _2EntriesHaveNotKey()
        {
            IList<ITupleEntry> col = new List<ITupleEntry>();
            col.Add(TupleEntry.OfPair(null, "v0"));
            col.Add(TupleEntry.OfValue("v1"));
            return Tuple.FromCollection(col);
        }

        private ITuple _2EntriesHaveKeyAnd2EntriesHaveNotKey()
        {
            IList<ITupleEntry> col = new List<ITupleEntry>();
            col.Add(TupleEntry.OfPair("k0", "v0"));
            col.Add(TupleEntry.OfValue("v1"));
            col.Add(TupleEntry.OfPair("k2", "v2"));
            col.Add(TupleEntry.OfPair(null, "v3"));
            return Tuple.FromCollection(col);
        }

        [Test]
        public void Count_ReturnsNumberOfEntryInTuple()
        {
            // Arrange
            ITuple t0 = Empty();
            ITuple t1 = _2EntriesHaveKey();

            // Act

            // Assert
            Assert.That(t0.Count, Is.EqualTo(0));
            Assert.That(t0.Entries.Count, Is.EqualTo(0));
            Assert.That(t1.Count, Is.EqualTo(2));
            Assert.That(t1.Entries.Count, Is.EqualTo(2));
        }

        [Test]
        public void Keys_ReturnsSetOfKeysInTuple()
        {
            // Arrange
            ITuple t0 = Empty();
            ITuple t1 = _2EntriesHaveKey();
            ITuple t2 = _2EntriesHaveNotKey();

            // Act

            // Assert
            Assert.That(t0.Keys.Count, Is.EqualTo(0));
            Assert.That(t0.Keys.Contains("k1"), Is.False);
            Assert.That(t0.Keys.Contains("k2"), Is.False);
            Assert.That(t1.Keys.Count, Is.EqualTo(2));
            Assert.That(t1.Keys.Contains("k1"), Is.True);
            Assert.That(t1.Keys.Contains("k2"), Is.False);
            Assert.That(t2.Keys.Count, Is.EqualTo(0));
        }

        [Test]
        public void Values_ReturnsListOfValuesInTuple()
        {
            // Arrange
            ITuple t0 = Empty();
            ITuple t1 = _2EntriesHaveKey();
            ITuple t2 = _2EntriesHaveNotKey();
            ITuple t3 = _2EntriesHaveKeyAnd2EntriesHaveNotKey();

            // Act
            
            // Assert
            Assert.That(t0.Values.Count, Is.EqualTo(0));
            Assert.That(t1.Values.Count, Is.EqualTo(2));
            Assert.That(t1.Values.Contains("v1"), Is.True);
            Assert.That(t1.Values.Contains("v2"), Is.False);
            Assert.That(t2.Values.Count, Is.EqualTo(2));
            Assert.That(t2.Values.Contains("v1"), Is.True);
            Assert.That(t2.Values.Contains("v2"), Is.False);
            Assert.That(t3.Values.Contains("v2"), Is.True);
        }
    }
}
