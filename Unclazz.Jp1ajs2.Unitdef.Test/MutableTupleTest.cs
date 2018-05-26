using System;
using NUnit.Framework;

namespace Unclazz.Jp1ajs2.Unitdef.Test
{
    [TestFixture]
    public class MutableTupleTest
    {
        [Test]
        public void AsImmutable_ReturnsNewImmutableInstance()
        {
            // Arrange
            var immutable1 = Tuple.Empty;
            var mutable1 = immutable1.AsMutable();
            mutable1.Add("foo");
            var immutable2 = mutable1.AsImmutable();

            // Act
            // Assert
            Assert.That(() => immutable2.Add("bar"), Throws.InstanceOf<NotSupportedException>());
            mutable1.Add("baz");
            Assert.That(mutable1.Count, Is.EqualTo(2));
        }
        [Test]
        public void AsMutable_ReturnsNewMutableInstance_Case1()
        {
            // Arrange
            var immutable1 = Tuple.Empty;
            var mutable1 = immutable1.AsMutable();

            // Act
            mutable1.Add("hello");

            // Assert
            Assert.That(mutable1.Count, Is.EqualTo(1));
            Assert.That(mutable1.Values[0], Is.EqualTo("hello"));
        }
        [Test]
        public void AsMutable_ReturnsNewMutableInstance_Case2()
        {
            // Arrange
            var immutable1 = Tuple.Empty;
            var mutable1 = immutable1.AsMutable();
            var mutable2 = mutable1.AsMutable();

            // Act
            mutable1.Add("hello");

            // Assert
            Assert.That(mutable1.Count, Is.EqualTo(1));
            Assert.That(mutable1.Values[0], Is.EqualTo("hello"));
            Assert.That(mutable2.Count, Is.EqualTo(0));
            Assert.That(() => mutable2.Values[0], Throws.InstanceOf<ArgumentOutOfRangeException>());
        }
        [Test]
        public void Add_AddsNewEntryOnTuple()
        {
            // Arrange
            var immutable1 = Tuple.Empty;
            var mutable1 = immutable1.AsMutable();

            // Act
            mutable1.Add("nokey");
            mutable1.Add("key1", "haskey1");
            mutable1.Add("key2", "haskey2");

            // Assert
            Assert.That(mutable1.Count, Is.EqualTo(3));
            Assert.That(mutable1.Values[0], Is.EqualTo("nokey"));
            Assert.That(mutable1.Values[1], Is.EqualTo("haskey1"));
            Assert.That(mutable1.Values[2], Is.EqualTo("haskey2"));
            Assert.That(mutable1.ToString(), Is.EqualTo("(nokey,key1=haskey1,key2=haskey2)"));
        }
        [Test]
        public void Add_WhenSameKeyAlreadyExists_ThrowsArgumentException()
        {
            // Arrange
            var immutable1 = Tuple.Empty;
            var mutable1 = immutable1.AsMutable();

            // Act
            mutable1.Add("nokey");
            mutable1.Add("key1", "haskey1");
            mutable1.Add("key2", "haskey2");

            // Assert
            Assert.That(() => mutable1.Add("key1", "haskey1b"), Throws.ArgumentException);
            Assert.That(mutable1.Count, Is.EqualTo(3));
        }
        [Test]
        public void Insert_AddsNewEntryAtSpecifiedPositionInTuple()
        {
            // Arrange
            var immutable1 = Tuple.Empty;
            var mutable1 = immutable1.AsMutable();

            // Act
            mutable1.Add("nokey");
            mutable1.Add("key1", "haskey1");
            mutable1.Insert(1, "key2", "haskey2");

            // Assert
            Assert.That(mutable1.Count, Is.EqualTo(3));
            Assert.That(mutable1.Values[0], Is.EqualTo("nokey"));
            Assert.That(mutable1.Values[2], Is.EqualTo("haskey1"));
            Assert.That(mutable1.Values[1], Is.EqualTo("haskey2"));
            Assert.That(mutable1.ToString(), Is.EqualTo("(nokey,key2=haskey2,key1=haskey1)"));
        }
        [Test]
        public void Insert_WhenSameKeyAlreadyExists_ThrowsArgumentException()
        {
            // Arrange
            var immutable1 = Tuple.Empty;
            var mutable1 = immutable1.AsMutable();

            // Act
            mutable1.Add("nokey");
            mutable1.Add("key1", "haskey1");
            mutable1.Add("key2", "haskey2");

            // Assert
            Assert.That(() => mutable1.Insert(1, "key1", "haskey1b"), Throws.ArgumentException);
            Assert.That(mutable1.Count, Is.EqualTo(3));
        }
        [Test]
        public void Clear_RemovesAllEntries()
        {
            // Arrange
            var immutable1 = Tuple.Empty;
            var mutable1 = immutable1.AsMutable();

            // Act
            mutable1.Add("nokey");
            mutable1.Add("key1", "haskey1");
            mutable1.Add("key2", "haskey2");
            mutable1.Clear();

            // Assert
            Assert.That(mutable1.Count, Is.EqualTo(0));
        }
        [Test]
        public void RemoveAt_RemovesSpecifiedEntry_Case1()
        {
            // Arrange
            var immutable1 = Tuple.Empty;
            var mutable1 = immutable1.AsMutable();

            // Act
            mutable1.Add("nokey");
            mutable1.Add("key1", "haskey1");
            mutable1.Add("key2", "haskey2");
            mutable1.RemoveAt(0);

            // Assert
            Assert.That(mutable1.Count, Is.EqualTo(2));
            Assert.That(mutable1.ToString(), Is.EqualTo("(key1=haskey1,key2=haskey2)"));
        }
        [Test]
        public void RemoveAt_RemovesSpecifiedEntry_Case2()
        {
            // Arrange
            var immutable1 = Tuple.Empty;
            var mutable1 = immutable1.AsMutable();

            // Act
            mutable1.Add("nokey");
            mutable1.Add("key1", "haskey1");
            mutable1.Add("key2", "haskey2");
            mutable1.RemoveAt(2);

            // Assert
            Assert.That(mutable1.Count, Is.EqualTo(2));
            Assert.That(mutable1.ToString(), Is.EqualTo("(nokey,key1=haskey1)"));
        }
    }
}
