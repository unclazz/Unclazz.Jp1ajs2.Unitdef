using System;
using System.Linq;
using NUnit.Framework;
using Unclazz.Jp1ajs2.Unitdef;

namespace Test.Unclazz.Jp1ajs2.Unitdef
{
    [TestFixture]
    public class NonNullCollectionTest
    {
        [Test]
        public void ctor_WhenArgumentIsNull_ThrowsException()
        {
            // Arrange
            // Act
            // Assert
            Assert.That(() => new NonNullCollection<IUnit>(null), Throws.ArgumentNullException);
        }
        [Test]
        public void ctor_WhenArgumentContainsNull_ThrowsException()
        {
            // Arrange
            // Act
            // Assert
            Assert.That(() => new NonNullCollection<IUnit>(new IUnit[] {
                MutableUnit.Create("foo"), null, MutableUnit.Create("bar")
            }), Throws.ArgumentException);
            Assert.That(() => new NonNullCollection<IUnit>(new IUnit[] {
                null, MutableUnit.Create("foo"), MutableUnit.Create("bar")
            }), Throws.ArgumentException);
            Assert.That(() => new NonNullCollection<IUnit>(new IUnit[] {
                MutableUnit.Create("foo"), MutableUnit.Create("bar"), null
            }), Throws.ArgumentException);
        }

        [Test]
        public void Add_OfAnInstanceInitializedWithReadOnlyList_ThrowsException()
        {
            // Arrange
            var c0 = new NonNullCollection<IUnit>(new IUnit[] {
                MutableUnit.Create("foo"), MutableUnit.Create("bar")
            }.ToList().AsReadOnly());

            // Act
            // Assert
            Assert.That(() => {
                c0.Add(MutableUnit.Create("baz"));

            }, Throws.InstanceOf<NotSupportedException>());

        }
        [Test]
        public void Clear_OfAnInstanceInitializedWithReadOnlyList_ThrowsException()
        {
            // Arrange
            var c0 = new NonNullCollection<IUnit>(new IUnit[] {
                MutableUnit.Create("foo"), MutableUnit.Create("bar")
            }.ToList().AsReadOnly());

            // Act
            // Assert
            Assert.That(() => {
                c0.Clear();

            }, Throws.InstanceOf<NotSupportedException>());

        }
        [Test]
        public void Insert_OfAnInstanceInitializedWithReadOnlyList_ThrowsException()
        {
            // Arrange
            var c0 = new NonNullCollection<IUnit>(new IUnit[] {
                MutableUnit.Create("foo"), MutableUnit.Create("bar")
            }.ToList().AsReadOnly());

            // Act
            // Assert
            Assert.That(() => {
                c0.Insert(1, MutableUnit.Create("baz"));

            }, Throws.InstanceOf<NotSupportedException>());

        }
        [Test]
        public void Remove_OfAnInstanceInitializedWithReadOnlyList_ThrowsException()
        {
            // Arrange
            var c0 = new NonNullCollection<IUnit>(new IUnit[] {
                MutableUnit.Create("foo"), MutableUnit.Create("bar")
            }.ToList().AsReadOnly());

            // Act
            // Assert
            Assert.That(() => {
                c0.Remove(MutableUnit.Create("baz"));

            }, Throws.InstanceOf<NotSupportedException>());

        }
        [Test]
        public void RemoveAt_OfAnInstanceInitializedWithReadOnlyList_ThrowsException()
        {
            // Arrange
            var c0 = new NonNullCollection<IUnit>(new IUnit[] {
                MutableUnit.Create("foo"), MutableUnit.Create("bar")
            }.ToList().AsReadOnly());

            // Act
            // Assert
            Assert.That(() => {
                c0.RemoveAt(1);

            }, Throws.InstanceOf<NotSupportedException>());

        }
        [Test]
        public void RemoveAll_OfAnInstanceInitializedWithReadOnlyList_ThrowsException()
        {
            // Arrange
            var c0 = new NonNullCollection<IUnit>(new IUnit[] {
                MutableUnit.Create("foo"), MutableUnit.Create("bar")
            }.ToList().AsReadOnly());

            // Act
            // Assert
            Assert.That(() => {
                c0.RemoveAll("g");

            }, Throws.InstanceOf<NotSupportedException>());
            Assert.That(() => {
                c0.RemoveAll(x => x.Name == "foo");

            }, Throws.InstanceOf<NotSupportedException>());

        }

        [Test]
        public void Add_OfAnInstanceInitializedWithNormalList_DoesNotThrowException()
        {
            // Arrange
            var c0 = new NonNullCollection<IUnit>(new IUnit[] {
                MutableUnit.Create("foo"), MutableUnit.Create("bar")
            }.ToList());

            // Act
            c0.Add(MutableUnit.Create("baz"));

            // Assert
            Assert.That(c0.Count, Is.EqualTo(3));

        }
        [Test]
        public void Clear_OfAnInstanceInitializedWithNormalList_DoesNotThrowException()
        {
            // Arrange
            var c0 = new NonNullCollection<IUnit>(new IUnit[] {
                MutableUnit.Create("foo"), MutableUnit.Create("bar")
            }.ToList());

            // Act
            c0.Clear();

            // Assert
            Assert.That(c0.Count, Is.EqualTo(0));
        }
        [Test]
        public void Insert_OfAnInstanceInitializedWithNormalList_DoesNotThrowException()
        {
            // Arrange
            var c0 = new NonNullCollection<IUnit>(new IUnit[] {
                MutableUnit.Create("foo"), MutableUnit.Create("bar")
            }.ToList());

            // Act
            c0.Insert(1, MutableUnit.Create("baz"));

            // Assert
            Assert.That(c0.Count, Is.EqualTo(3));
        }
        [Test]
        public void Remove_OfAnInstanceInitializedWithNormalList_DoesNotThrowException()
        {
            // Arrange
            var u0 = MutableUnit.Create("foo");
            var c0 = new NonNullCollection<IUnit>(new IUnit[] {
                u0, MutableUnit.Create("bar")
            }.ToList());

            // Act
            c0.Remove(u0);

            // Assert
            Assert.That(c0.Count, Is.EqualTo(1));
        }
        [Test]
        public void RemoveAt_OfAnInstanceInitializedWithNormalList_DoesNotThrowException()
        {
            // Arrange
            var c0 = new NonNullCollection<IUnit>(new IUnit[] {
                MutableUnit.Create("foo"), MutableUnit.Create("bar")
            }.ToList());

            // Act
            c0.RemoveAt(1);

            // Assert
            Assert.That(c0.Count, Is.EqualTo(1));
        }
        [Test]
        public void RemoveAll_OfAnInstanceInitializedWithNormalList_DoesNotThrowException()
        {
            // Arrange
            var c0 = new NonNullCollection<IUnit>(new IUnit[] {
                MutableUnit.Create("foo"), MutableUnit.Create("bar")
            }.ToList());

            // Act
            c0.RemoveAll(x => x.Name == "bar");
            c0.RemoveAll("g");

            // Assert
            Assert.That(c0.Count, Is.EqualTo(0));
        }
    }
}
