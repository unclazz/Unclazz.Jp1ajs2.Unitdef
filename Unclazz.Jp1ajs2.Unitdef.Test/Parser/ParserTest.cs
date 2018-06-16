using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unclazz.Jp1ajs2.Unitdef.Parser;

namespace Unclazz.Jp1ajs2.Unitdef.Test.Parser
{
    [TestFixture]
    public class ParserTest
    {
        private readonly UnitParser p = UnitParser.Instance;

        [Test]
        public void Parse_1Unit_NoAttrs_NoParams_NoSubUnits()
        {
            // Arrange
            Input i = Input.FromString("unit=XXXX0000,,,;{ty=g;}");

            // Act
            IList<IUnit> us = p.Parse(i);

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
            Input i = Input.FromString("/* xxx */" +
                "unit=XXXX0000,,,;/* xxx */{/* xxx */" +
                "ty=g;/* xxx */}/* xxx */");

            // Act
            IList<IUnit> us = p.Parse(i);

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
            Input i = Input.FromString(" /* xxx */ " +
                "unit=XXXX0000,,,; /* xxx */ { /* xxx */ " +
                "ty=g; /* xxx */ }  /* xxx */ ");

            // Act
            IList<IUnit> us = p.Parse(i);

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
            Input i = Input.FromString(
                "unit=XXXX0000,,,;{ty=g;}" +
                "unit=XXXX1000,,,;{ty=pj;}");

            // Act
            IList<IUnit> us = p.Parse(i);

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
            Input i = Input.FromString("unit=XXXX0000,a1,a2,a3;" +
                "{ty=g;cm=\"this is comment. ###\"\";" +
                "}");

            // Act
            IList<IUnit> us = p.Parse(i);

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
            Input i = Input.FromString("unit=XXXX0000,,,;" +
                "{ty=g;" +
                "unit=XXXX1000,,,;{ty=pj;sc=xxx;}" +
                "unit=XXXX2000,,,;{ty=j;sc=xxx;}" +
                "}");

            // Act
            IList<IUnit> us = p.Parse(i)[0].SubUnits;

            // Assert
            Assert.AreEqual("XXXX1000", us[0].Name);
            Assert.AreEqual(UnitType.PcJob, us[0].Type);
            Assert.AreEqual("XXXX2000", us[1].Name);
            Assert.AreEqual(UnitType.UnixJob, us[1].Type);
        }
    }
}
