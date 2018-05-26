using System.Linq;
using NUnit.Framework;

namespace Unclazz.Jp1ajs2.Unitdef.Test
{
    [TestFixture]
    public class UnitTest
    {
        static readonly IUnit immutableUnit0 = Unit.FromString
               ("unit=XXXX0000,,,;" +
                "{ty=g;" +
                "unit=XXXX1000,,,;{ty=pj;sc=xxx;}" +
                "unit=XXXX2000,,,;{ty=j;sc=xxx;}" +
                "}");

        [Test]
        public void AsMutable_ReturnsMutableUnit()
        {
            // Arrange
            var immutable1 = immutableUnit0;

            // Act
            var mutable1 = immutable1.AsMutable();

            // Assert
            Assert.That(immutable1.Type, Is.EqualTo(UnitType.JobGroup));
            Assert.That(immutable1.SubUnits[0].Type, Is.EqualTo(UnitType.PcJob));
            Assert.That(immutable1.SubUnits[1].Type, Is.EqualTo(UnitType.UnixJob));
            Assert.That(mutable1.Type, Is.EqualTo(UnitType.JobGroup));
            Assert.That(mutable1.SubUnits[0].Parameters.First(p => p.Name == "ty").Values[0].StringValue,
                        Is.EqualTo("pj"));
            Assert.That(mutable1.SubUnits[0].Type, Is.EqualTo(UnitType.PcJob));
            Assert.That(mutable1.SubUnits[1].Type, Is.EqualTo(UnitType.UnixJob));

            Assert.That(mutable1, Is.InstanceOf<MutableUnit>());
            Assert.That(mutable1.Parameters[0], Is.InstanceOf<MutableParameter>());
            Assert.That(mutable1.SubUnits[0], Is.InstanceOf<MutableUnit>());
            Assert.That(mutable1.SubUnits[0].Parameters[0], Is.InstanceOf<MutableParameter>());
        }
    }
}
