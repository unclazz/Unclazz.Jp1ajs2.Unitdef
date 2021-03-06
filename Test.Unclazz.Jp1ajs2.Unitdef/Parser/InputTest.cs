﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unclazz.Jp1ajs2.Unitdef.Parser;

namespace Unclazz.Jp1ajs2.Unitdef.Test
{
    [TestFixture]
    public class InputTest
    {
        private string GetTestProjectDirectory()
        {
            string binDebug = NUnit.Framework.TestContext
                .CurrentContext.TestDirectory;
            return System.IO.Directory.GetParent(binDebug).Parent.FullName ;
        }

        private string JoinPathSegments(string s0, string s1)
        {
            return System.IO.Path.Combine(s0, s1);
        }

        [Test]
        public void FromFile_WhenValidPathSpecified_ReturnsInstance()
        {
            // Arrange
            Input i = Input.FromFile(JoinPathSegments
                (GetTestProjectDirectory(), "InputTest.txt"), Encoding.UTF8);

            // Act
            StringBuilder sb = new StringBuilder();
            using (i)
            {
                while (!i.EndOfFile)
                {
                    sb.Append(i.Current);
                    i.GoNext();
                }
            }

            // Assert
            Assert.AreEqual("abc\r\nあいうえお\r\nghi", sb.ToString());
        }

        [Test]
        public void FromFile_WhenInvalidPathSpecified_ThrowsException()
        {
            // Arrange
            string invalidPath = JoinPathSegments
                (GetTestProjectDirectory(), "InputTest2.txt");

            // Act

            // Assert
            Assert.Throws<FileNotFoundException>(() => {
				Input.FromFile(invalidPath, Encoding.UTF8);
            });
        }

        [Test]
        public void FromFile_WhenInvalidEncodingSpecified_ThrowsException()
        {
            // Arrange
            string validPath = JoinPathSegments
                (GetTestProjectDirectory(), "InputTest.txt");
            Encoding invalidEncoding = Encoding.GetEncoding("Shift_JIS");
            Input i = Input.FromFile(validPath, invalidEncoding);

            // Act
            StringBuilder sb = new StringBuilder();
            using (i)
            {
                while (!i.EndOfFile)
                {
                    sb.Append(i.Current);
                    i.GoNext();
                }
            }

            // Assert
            Assert.AreNotEqual("abc\r\nあいうえお\r\nghi", sb.ToString());
        }

        [Test]
        public void LineNumber_Always_ReturnsCurrentLineNumber()
        {
            // Arrange
            Input i = Input.FromString("abc\r\ndef\nghi");

            // Act
            int n0 = i.LineNumber;
            i.GoNext();
            i.GoNext();
            i.GoNext();
            i.GoNext();
            int n1 = i.LineNumber;
            i.GoNext();
            int n2 = i.LineNumber;

            // Assert
            Assert.AreEqual(1, n0);
            Assert.AreEqual(1, n1);
            Assert.AreEqual(2, n2);
        }

        [Test]
        public void ColumnNumber_Always_ReturnsCurrentColumnNumber()
        {
            // Arrange
            Input i = Input.FromString("abc\r\ndef\nghi");

            // Act
            int n0 = i.ColumnNumber;
            i.GoNext();
            int n1 = i.ColumnNumber;
            i.GoNext();
            i.GoNext();
            i.GoNext();
            int n2 = i.ColumnNumber;
            i.GoNext();
            int n3 = i.ColumnNumber;

            // Assert
            Assert.AreEqual(1, n0);
            Assert.AreEqual(2, n1);
            Assert.AreEqual(5, n2);
            Assert.AreEqual(1, n3);
        }

        [Test]
        public void EndOfFile_WhenCurrentPositionHasReachedEOF_ReturnsTrue()
        {
            // Arrange
            Input i = Input.FromString("abc\nd");

            // Act
            bool b0 = i.EndOfFile;
            i.GoNext();
            i.GoNext();
            i.GoNext();
            bool b1 = i.EndOfFile;
            i.GoNext();
            bool b2 = i.EndOfFile;
            i.GoNext();
            bool b3 = i.EndOfFile;

            // Assert
            Assert.AreEqual(false, b0);
            Assert.AreEqual(false, b1);
            Assert.AreEqual(false, b2);
            Assert.AreEqual(true, b3);
        }

        [Test]
        public void EndOfLine_WhenCurrentPositionAtEOL_ReturnsTrue()
        {
            // Arrange
            Input i = Input.FromString("abc\nde");

            // Act
            bool b0 = i.EndOfLine;//a
            i.GoNext();//b
            i.GoNext();//c
            bool b1 = i.EndOfLine;
            i.GoNext();//\n
            bool b2 = i.EndOfLine;
            i.GoNext();//d
            i.GoNext();//e
            bool b3 = i.EndOfLine;
            i.GoNext();//EOL
            bool b4 = i.EndOfLine;

            // Assert
            Assert.AreEqual(false, b0);
            Assert.AreEqual(false, b1);
            Assert.AreEqual(true, b2);
            Assert.AreEqual(false, b3);
            Assert.AreEqual(true, b4);
        }

        [Test]
        public void RestOfLine_Always_ReturnsSubSequenceOfLineStartsWithCurrentPositionEndsWithEOL()
        {
            // Arrange
            Input i = Input.FromString("abc\r\ndef\nghi");

            // Act
            string s0 = i.RestOfLine;
            i.GoNext();
            string s1 = i.RestOfLine;
            i.GoNext();
            i.GoNext();
            i.GoNext();
            string s2 = i.RestOfLine;
            i.GoNext();
            string s3 = i.RestOfLine;

            // Assert
            Assert.AreEqual("abc\r\n", s0);
            Assert.AreEqual("bc\r\n", s1);
            Assert.AreEqual("\n", s2);
            Assert.AreEqual("def\n", s3);
        }
    }
}
