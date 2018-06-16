using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Unclazz.Jp1ajs2.Unitdef.Parser;
using Unclazz.Parsec;

namespace Unclazz.Jp1ajs2.Unitdef.Test.Parser
{
    [TestFixture]
    public class UnitParser2Test
    {
        private readonly Parser<Seq<IUnit>> p = new UnitParser2().Repeat(min: 1);

        [Test]
        public void Parse_1Unit_NoAttrs_NoParams_NoSubUnits()
        {
            // Arrange
            Reader i = Reader.From("unit=XXXX0000,,,;{ty=g;}");

            // Act
            IList<IUnit> us = p.Parse(i).Capture.ToList();

            // Assert
            Assert.AreEqual(1, us.Count);
            Assert.AreEqual("XXXX0000", us[0].Name);
            Assert.AreEqual(string.Empty, us[0].Attributes.Jp1UserName);
            Assert.AreEqual(UnitType.JobGroup, us[0].Type);
            Assert.AreEqual("ty", us[0].Parameters[0].Name);
            Assert.AreEqual("g", us[0].Parameters[0].Values[0].StringValue);
            Assert.AreEqual(0, us[0].SubUnits.Count);
        }

        [Test]
        public void Parse_1Unit_WithComment()
        {
            // Arrange
            Reader i = Reader.From("/* xxx */" +
                "unit=XXXX0000,,,;/* xxx */{/* xxx */" +
                "ty=g;/* xxx */}/* xxx */");

            // Act
            IList<IUnit> us = p.Parse(i).Capture.ToList();

            // Assert
            Assert.AreEqual(1, us.Count);
            Assert.AreEqual("XXXX0000", us[0].Name);
            Assert.AreEqual(string.Empty, us[0].Attributes.Jp1UserName);
            Assert.AreEqual(UnitType.JobGroup, us[0].Type);
            Assert.AreEqual("ty", us[0].Parameters[0].Name);
            Assert.AreEqual("g", us[0].Parameters[0].Values[0].StringValue);
            Assert.AreEqual(0, us[0].SubUnits.Count);
        }

        [Test]
        public void Parse_1Unit_WithSpaceAndComment()
        {
            // Arrange
            Reader i = Reader.From(" /* xxx */ " +
                "unit=XXXX0000,,,; /* xxx */ { /* xxx */ " +
                "ty=g; /* xxx */ }  /* xxx */ ");

            // Act
            IList<IUnit> us = p.Parse(i).Capture.ToList();

            // Assert
            Assert.AreEqual(1, us.Count);
            Assert.AreEqual("XXXX0000", us[0].Name);
            Assert.AreEqual(string.Empty, us[0].Attributes.Jp1UserName);
            Assert.AreEqual(UnitType.JobGroup, us[0].Type);
            Assert.AreEqual("ty", us[0].Parameters[0].Name);
            Assert.AreEqual("g", us[0].Parameters[0].Values[0].StringValue);
            Assert.AreEqual(0, us[0].SubUnits.Count);
        }

        [Test]
        public void Parse_2Unit_NoAttrs_NoParams_NoSubUnits()
        {
            // Arrange
            Reader i = Reader.From(
                "unit=XXXX0000,,,;{ty=g;}" +
                "unit=XXXX1000,,,;{ty=pj;}");

            // Act
            IList<IUnit> us = p.Parse(i).Capture.ToList();

            // Assert
            Assert.AreEqual(2, us.Count);
            Assert.AreEqual("XXXX0000", us[0].Name);
            Assert.AreEqual(UnitType.JobGroup, us[0].Type);
            Assert.AreEqual("XXXX1000", us[1].Name);
            Assert.AreEqual(UnitType.PcJob, us[1].Type);
        }

        [Test]
        public void Parse_1Unit_Attrs_Params_NoSubUnits()
        {
            // Arrange
            Reader i = Reader.From("unit=XXXX0000,a1,a2,a3;" +
                "{ty=g;cm=\"this is comment. ###\"\";" +
                "}");

            // Act
            IList<IUnit> us = p.Parse(i).Capture.ToList();

            // Assert
            Assert.AreEqual("XXXX0000", us[0].Attributes.UnitName);
            Assert.AreEqual("a1", us[0].Attributes.PermissionMode);
            Assert.AreEqual("a2", us[0].Attributes.Jp1UserName);
            Assert.AreEqual("a3", us[0].Attributes.ResourceGroupName);
            Assert.AreEqual(UnitType.JobGroup, us[0].Type);
            Assert.AreEqual("this is comment. #\"", us[0].Comment);
        }

        [Test]
        public void Parse_1Unit_NoAttrs_NoParams_SubUnits()
        {
            // Arrange
            Reader i = Reader.From("unit=XXXX0000,,,;" +
                "{ty=g;" +
                "unit=XXXX1000,,,;{ty=pj;sc=xxx;}" +
                "unit=XXXX2000,,,;{ty=j;sc=xxx;}" +
                "}");

            // Act
            var u = p.Parse(i).Capture.ToList()[0];
            IList<IUnit> us = u.SubUnits;

            // Assert
            Assert.AreEqual("/XXXX0000", u.FullName.ToString());
            Assert.AreEqual("/XXXX0000/XXXX1000", us[0].FullName.ToString());
            Assert.AreEqual("XXXX1000", us[0].Name);
            Assert.AreEqual(UnitType.PcJob, us[0].Type);
            Assert.AreEqual("/XXXX0000/XXXX2000", us[1].FullName.ToString());
            Assert.AreEqual("XXXX2000", us[1].Name);
            Assert.AreEqual(UnitType.UnixJob, us[1].Type);
        }
    }
}
