using System;
using System.IO;
using System.Linq;
using System.Text;
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

        static IUnit immutableUnit1;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var path = Path.Combine(NUnit.Framework.TestContext.
                       CurrentContext.TestDirectory, "TestUnits.txt");
            using (var s = new FileStream(path, FileMode.Open))
            {
                immutableUnit1 = Unit.FromStream(s, Encoding.UTF8);
            }
        }

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

        [Test]
        public void Children_Case01_Matched2()
        {
            var root = immutableUnit1;
            var childrenNames = root.Children().Select(x => x.Name).ToArray();

            Assert.That(childrenNames.Length, Is.EqualTo(2));
            Assert.That(childrenNames[0], Is.EqualTo("XXXX1000"));
            Assert.That(childrenNames[1], Is.EqualTo("XXXX2000"));
        }
        [Test]
        public void Children_Case02_Matched1()
        {
            var root = immutableUnit1;
            var childrenNames = root.Children(UnitType.Jobnet).Select(x => x.Name).ToArray();

            Assert.That(childrenNames.Length, Is.EqualTo(1));
            Assert.That(childrenNames[0], Is.EqualTo("XXXX2000"));
        }
        [Test]
        public void Children_Case03_Matched0()
        {
            var root = immutableUnit1;
            var childrenNames = root.Children(UnitType.PcJob).Select(x => x.Name).ToArray();

            Assert.That(childrenNames.Length, Is.EqualTo(0));
        }
        [Test]
        public void ItSelfAndChildren_Case01_Matched3()
        {
            var root = immutableUnit1;
            var childrenNames = root.ItSelfAndChildren().Select(x => x.Name).ToArray();

            var i = 0;

            Assert.That(childrenNames[i++], Is.EqualTo("XXXX0000"));
            Assert.That(childrenNames[i++], Is.EqualTo("XXXX1000"));
            Assert.That(childrenNames[i++], Is.EqualTo("XXXX2000"));

            Assert.That(childrenNames.Length, Is.EqualTo(i));
        }
        [Test]
        public void ItSelfAndChildren_Case02_Matched1()
        {
            var root = immutableUnit1;
            var childrenNames = root.ItSelfAndChildren(UnitType.Jobnet).Select(x => x.Name).ToArray();

            Assert.That(childrenNames.Length, Is.EqualTo(1));
            Assert.That(childrenNames[0], Is.EqualTo("XXXX2000"));
        }
        [Test]
        public void ItSelfAndChildren_Case03_Matched0()
        {
            var root = immutableUnit1;
            var childrenNames = root.ItSelfAndChildren(UnitType.MailSendJob).Select(x => x.Name).ToArray();

            Assert.That(childrenNames.Length, Is.EqualTo(0));
        }
        [Test]
        public void Descendants_Case01_Matched2()
        {
            var root = immutableUnit1;
            var childrenNames = root.Descendants().Select(x => x.Name).ToArray();

            var i = 0;

            Assert.That(childrenNames[i++], Is.EqualTo("XXXX1000"));
            Assert.That(childrenNames[i++], Is.EqualTo("XXXX2000"));
            Assert.That(childrenNames[i++], Is.EqualTo("XXXX1100"));
            Assert.That(childrenNames[i++], Is.EqualTo("XXXX1200"));
            Assert.That(childrenNames[i++], Is.EqualTo("XXXX1300"));
            Assert.That(childrenNames[i++], Is.EqualTo("XXXX2100"));
            Assert.That(childrenNames[i++], Is.EqualTo("XXXX2200"));
            Assert.That(childrenNames[i++], Is.EqualTo("XXXX2300"));
            Assert.That(childrenNames[i++], Is.EqualTo("XXXX1110"));
            Assert.That(childrenNames[i++], Is.EqualTo("XXXX1120"));
            Assert.That(childrenNames[i++], Is.EqualTo("XXXX2110"));
            Assert.That(childrenNames[i++], Is.EqualTo("XXXX2120"));

            Assert.That(childrenNames.Length, Is.EqualTo(i));
        }
        [Test]
        public void Descendants_Case02_Matched6()
        {
            var root = immutableUnit1;
            var childrenNames = root.Descendants(UnitType.PcJob).Select(x => x.Name).ToArray();

            var i = 0;

            Assert.That(childrenNames[i++], Is.EqualTo("XXXX1200"));
            Assert.That(childrenNames[i++], Is.EqualTo("XXXX1300"));
            Assert.That(childrenNames[i++], Is.EqualTo("XXXX2200"));
            Assert.That(childrenNames[i++], Is.EqualTo("XXXX2300"));
            Assert.That(childrenNames[i++], Is.EqualTo("XXXX1110"));
            Assert.That(childrenNames[i++], Is.EqualTo("XXXX2110"));

            Assert.That(childrenNames.Length, Is.EqualTo(i));
        }
        [Test]
        public void Descendants_Case03_Matched0()
        {
            var root = immutableUnit1;
            var childrenNames = root.Descendants(UnitType.FileWatchJob).Select(x => x.Name).ToArray();

            Assert.That(childrenNames.Length, Is.EqualTo(0));
        }
        [Test]
        public void TheirChildren_Case01_Matched6()
        {
            var root = immutableUnit1;
            var childrenNames = root.Children().TheirChildren().Select(x => x.Name).ToArray();

            var i = 0;

            Assert.That(childrenNames[i++], Is.EqualTo("XXXX1100"));
            Assert.That(childrenNames[i++], Is.EqualTo("XXXX1200"));
            Assert.That(childrenNames[i++], Is.EqualTo("XXXX1300"));
            Assert.That(childrenNames[i++], Is.EqualTo("XXXX2100"));
            Assert.That(childrenNames[i++], Is.EqualTo("XXXX2200"));
            Assert.That(childrenNames[i++], Is.EqualTo("XXXX2300"));

            Assert.That(childrenNames.Length, Is.EqualTo(i));
        }
        [Test]
        public void TheirChildren_Case02_Matched3()
        {
            var root = immutableUnit1;
            var childrenNames = root.Children(UnitType.Jobnet).TheirChildren().Select(x => x.Name).ToArray();

            var i = 0;

            Assert.That(childrenNames[i++], Is.EqualTo("XXXX2100"));
            Assert.That(childrenNames[i++], Is.EqualTo("XXXX2200"));
            Assert.That(childrenNames[i++], Is.EqualTo("XXXX2300"));

            Assert.That(childrenNames.Length, Is.EqualTo(i));
        }
        [Test]
        public void TheirChildren_Case03_Matched0()
        {
            var root = immutableUnit1;
            var childrenNames = root.Children(UnitType.PcJob).TheirChildren().Select(x => x.Name).ToArray();

            Assert.That(childrenNames.Length, Is.EqualTo(0));
        }
        [Test]
        public void TheirDescendants_Case01_Matched2()
        {
            var root = immutableUnit1;
            var childrenNames = root.Children().TheirDescendants().Select(x => x.Name).ToArray();

            var i = 0;

            Assert.That(childrenNames[i++], Is.EqualTo("XXXX1100"));
            Assert.That(childrenNames[i++], Is.EqualTo("XXXX1200"));
            Assert.That(childrenNames[i++], Is.EqualTo("XXXX1300"));
            Assert.That(childrenNames[i++], Is.EqualTo("XXXX1110"));
            Assert.That(childrenNames[i++], Is.EqualTo("XXXX1120"));
            Assert.That(childrenNames[i++], Is.EqualTo("XXXX2100"));
            Assert.That(childrenNames[i++], Is.EqualTo("XXXX2200"));
            Assert.That(childrenNames[i++], Is.EqualTo("XXXX2300"));
            Assert.That(childrenNames[i++], Is.EqualTo("XXXX2110"));
            Assert.That(childrenNames[i++], Is.EqualTo("XXXX2120"));

            Assert.That(childrenNames.Length, Is.EqualTo(i));
        }
        [Test]
        public void TheirDescendants_Case02_Matched6()
        {
            var root = immutableUnit1;
            var childrenNames = root.Children().TheirDescendants(UnitType.PcJob).Select(x => x.Name).ToArray();

            var i = 0;

            Assert.That(childrenNames[i++], Is.EqualTo("XXXX1200"));
            Assert.That(childrenNames[i++], Is.EqualTo("XXXX1300"));
            Assert.That(childrenNames[i++], Is.EqualTo("XXXX1110"));
            Assert.That(childrenNames[i++], Is.EqualTo("XXXX2200"));
            Assert.That(childrenNames[i++], Is.EqualTo("XXXX2300"));
            Assert.That(childrenNames[i++], Is.EqualTo("XXXX2110"));

            Assert.That(childrenNames.Length, Is.EqualTo(i));
        }
    }
}
