using NUnit.Framework;
using Unclazz.Jp1ajs2.Unitdef;

namespace Test.Unclazz.Jp1ajs2.Unitdef
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
            Assert.That(() => immutable2.Add("bar"), Throws.InstanceOf<System.NotSupportedException>());
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
            Assert.That(() => mutable2.Values[0], Throws.InstanceOf<System.ArgumentOutOfRangeException>());
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
        public void Add_WhenSameKeyAlreadyExists_ThrowsException()
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
        public void Add_WhenValueIsNull_ThrowsException_Case1()
        {
            // Arrange
            var immutable1 = Tuple.Empty;
            var mutable1 = immutable1.AsMutable();

            // Act
            mutable1.Add("nokey");
            mutable1.Add("key1", "haskey1");
            mutable1.Add("key2", "haskey2");

            // Assert
            Assert.That(() => mutable1.Add(null as string), Throws.ArgumentNullException);
        }
        [Test]
        public void Add_WhenValueIsNull_ThrowsException_Case2()
        {
            // Arrange
            var immutable1 = Tuple.Empty;
            var mutable1 = immutable1.AsMutable();

            // Act
            mutable1.Add("nokey");
            mutable1.Add("key1", "haskey1");
            mutable1.Add("key2", "haskey2");

            // Assert
            Assert.That(() => mutable1.Add("key3", null as string), Throws.ArgumentNullException);
        }
        [Test]
        public void Add_WhenValueIsNull_DoesNotThrowException_Case1()
        {
            // Arrange
            var immutable1 = Tuple.Empty;
            var mutable1 = immutable1.AsMutable();

            // Act
            mutable1.Add("nokey");
            mutable1.Add("key1", "haskey1");
            mutable1.Add("key2", "haskey2");

            // Assert
            mutable1.Add(string.Empty);
            Assert.That(mutable1[3], Is.EqualTo(string.Empty));
        }
        [Test]
        public void Add_WhenValueIsNull_DoesNotThrowException_Case2()
        {
            // Arrange
            var immutable1 = Tuple.Empty;
            var mutable1 = immutable1.AsMutable();

            // Act
            mutable1.Add("nokey");
            mutable1.Add("key1", "haskey1");
            mutable1.Add("key2", "haskey2");

            // Assert
            mutable1.Add("key3", string.Empty);
            Assert.That(mutable1["key3"], Is.EqualTo(string.Empty));
        }
        [Test]
        public void Insert_AddsNewEntryAtSpecifiedPositionInTuple_Case1()
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
        public void Insert_AddsNewEntryAtSpecifiedPositionInTuple_Case2()
        {
            // Arrange
            var immutable1 = Tuple.Empty;
            var mutable1 = immutable1.AsMutable();

            // Act
            mutable1.Add("nokey");
            mutable1.Add("key1", "haskey");
            mutable1.Insert(0, "key2", "atPosition0");
            mutable1.Insert(3, "atPosition3");

            // Assert
            Assert.That(mutable1.Count, Is.EqualTo(4));
            Assert.That(mutable1.Values[0], Is.EqualTo("atPosition0"));
            Assert.That(mutable1.Values[1], Is.EqualTo("nokey"));
            Assert.That(mutable1.Values[2], Is.EqualTo("haskey"));
            Assert.That(mutable1.Values[3], Is.EqualTo("atPosition3"));
            Assert.That(mutable1.ToString(), Is.EqualTo("(key2=atPosition0,nokey,key1=haskey,atPosition3)"));
        }
        [Test]
        public void Insert_WhenSameKeyAlreadyExists_ThrowsException()
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
        public void Insert_WhenValueIsNull_ThrowsException_Case1()
        {
            // Arrange
            var immutable1 = Tuple.Empty;
            var mutable1 = immutable1.AsMutable();

            // Act
            mutable1.Add("nokey");
            mutable1.Add("key1", "haskey1");
            mutable1.Add("key2", "haskey2");

            // Assert
            Assert.That(() => mutable1.Insert(0, null as string), Throws.ArgumentNullException);
        }
        [Test]
        public void Insert_WhenValueIsNull_ThrowsException_Case2()
        {
            // Arrange
            var immutable1 = Tuple.Empty;
            var mutable1 = immutable1.AsMutable();

            // Act
            mutable1.Add("nokey");
            mutable1.Add("key1", "haskey1");
            mutable1.Add("key2", "haskey2");

            // Assert
            Assert.That(() => mutable1.Insert(1, "key3", null as string), Throws.ArgumentNullException);
        }
        [Test]
        public void Insert_WhenValueIsNull_DoesNotThrowException_Case1()
        {
            // Arrange
            var immutable1 = Tuple.Empty;
            var mutable1 = immutable1.AsMutable();

            // Act
            mutable1.Add("nokey");
            mutable1.Add("key1", "haskey1");
            mutable1.Add("key2", "haskey2");

            // Assert
            mutable1.Insert(0, string.Empty);
            Assert.That(mutable1[0], Is.EqualTo(string.Empty));
        }
        [Test]
        public void Insert_WhenValueIsNull_DoesNotThrowException_Case2()
        {
            // Arrange
            var immutable1 = Tuple.Empty;
            var mutable1 = immutable1.AsMutable();

            // Act
            mutable1.Add("nokey");
            mutable1.Add("key1", "haskey1");
            mutable1.Add("key2", "haskey2");

            // Assert
            mutable1.Insert(1, "key3", string.Empty);
            Assert.That(mutable1["key3"], Is.EqualTo(string.Empty));
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
            Assert.That(mutable1.ToString(), Is.EqualTo("()"));
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
        [Test]
        public void RemoveAt_RemovesSpecifiedEntry_Case3()
        {
            // Arrange
            var immutable1 = Tuple.Empty;
            var mutable1 = immutable1.AsMutable();

            // Act
            mutable1.Add("nokey");
            mutable1.Add("key1", "haskey1");
            mutable1.Add("key2", "haskey2");
            mutable1.RemoveAt(0);
            mutable1.RemoveAt(0);
            mutable1.RemoveAt(0);

            // Assert
            Assert.That(mutable1.Count, Is.EqualTo(0));
            Assert.That(mutable1.ToString(), Is.EqualTo("()"));
        }
    }
}
