using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Unclazz.Jp1ajs2.Unitdef;

namespace Test.Unclazz.Jp1ajs2.Unitdef
{
    [TestFixture]
    public class UnitTest
    {
        static readonly string crlf = Environment.NewLine;

        static readonly string unitdef0 =
            "unit=XXXX0000,,,;" + crlf +
            "{" + crlf +
            "\tty=g;" + crlf +
            "\tunit=XXXX1000,,,;" + crlf +
            "\t{" + crlf +
            "\t\tty=pj;" + crlf +
            "\t\tsc=xxx;" + crlf +
            "\t}" + crlf +
            "\tunit=XXXX2000,,,;" + crlf +
            "\t{" + crlf +
            "\t\tty=j;" + crlf +
            "\t\tsc=xxx;" + crlf +
            "\t}" + crlf +
            "}";

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

        [Test]
        public void WriteTo_Case01_Default()
        {
            var w = new StringWriter();
            var u = immutableUnit0;

            u.WriteTo(w);

            Assert.That(w.ToString(), Is.EqualTo(unitdef0));
        }

        [Test]
        public void WriteTo_Case02_Cr()
        {
            var w = new StringWriter();
            var u = immutableUnit0;

            u.WriteTo(w, new FormatOptions { NewLine = "\r" });

            Assert.That(w.ToString(), Is.EqualTo(unitdef0.Replace(crlf, "\r")));
        }

        [Test]
        public void WriteTo_Case03_CrLf()
        {
            var w = new StringWriter();
            var u = immutableUnit0;

            u.WriteTo(w, new FormatOptions { NewLine = "\r\n" });

            Assert.That(w.ToString(), Is.EqualTo(unitdef0.Replace(crlf, "\r\n")));
        }

        [Test]
        public void WriteTo_Case04_SoftTab()
        {
            var w = new StringWriter();
            var u = immutableUnit0;

            u.WriteTo(w, new FormatOptions { SoftTab = true });

            Assert.That(w.ToString(), Is.EqualTo(unitdef0.Replace("\t", "    ")));
        }

        [Test]
        public void WriteTo_Case05_SoftTab_TabSize()
        {
            var w = new StringWriter();
            var u = immutableUnit0;

            u.WriteTo(w, new FormatOptions { SoftTab = true, TabSize = 2 });

            Assert.That(w.ToString(), Is.EqualTo(unitdef0.Replace("\t", "  ")));
        }

        [Test]
        public void WriteTo_Case11_Default()
        {
            var w = new StringWriter();
            var u = immutableUnit0.AsMutable();

            u.WriteTo(w);

            Assert.That(w.ToString(), Is.EqualTo(unitdef0));
        }

        [Test]
        public void WriteTo_Case12_Cr()
        {
            var w = new StringWriter();
            var u = immutableUnit0.AsMutable();

            u.WriteTo(w, new FormatOptions { NewLine = "\r" });

            Assert.That(w.ToString(), Is.EqualTo(unitdef0.Replace(crlf, "\r")));
        }

        [Test]
        public void WriteTo_Case13_CrLf()
        {
            var w = new StringWriter();
            var u = immutableUnit0.AsMutable();

            u.WriteTo(w, new FormatOptions { NewLine = "\r\n" });

            Assert.That(w.ToString(), Is.EqualTo(unitdef0.Replace(crlf, "\r\n")));
        }

        [Test]
        public void WriteTo_Case14_SoftTab()
        {
            var w = new StringWriter();
            var u = immutableUnit0.AsMutable();

            u.WriteTo(w, new FormatOptions { SoftTab = true });

            Assert.That(w.ToString(), Is.EqualTo(unitdef0.Replace("\t", "    ")));
        }

        [Test]
        public void WriteTo_Case15_SoftTab_TabSize()
        {
            var w = new StringWriter();
            var u = immutableUnit0.AsMutable();

            u.WriteTo(w, new FormatOptions { SoftTab = true, TabSize = 2 });

            Assert.That(w.ToString(), Is.EqualTo(unitdef0.Replace("\t", "  ")));
        }
    }
}
