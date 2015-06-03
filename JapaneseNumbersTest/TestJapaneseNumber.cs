using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JapaneseNumbers;

namespace JapaneseNumbersTest
{
    [TestClass]
    public class TestJapaneseNumber
    {
        [TestMethod]
        public void TestFormat()
        {
            Assert.AreEqual("ichi", JapaneseNumber.Format(1));
            Assert.AreEqual("rei", JapaneseNumber.Format(0));
            Assert.AreEqual("juu", JapaneseNumber.Format(10));
            Assert.AreEqual("hyaku", JapaneseNumber.Format(100));
            Assert.AreEqual("sen", JapaneseNumber.Format(1000));
            Assert.AreEqual("juu ichi", JapaneseNumber.Format(11));
            Assert.AreEqual("hyaku ichi", JapaneseNumber.Format(101));
            Assert.AreEqual("sen ichi", JapaneseNumber.Format(1001));
            Assert.AreEqual("sen hyaku ichi", JapaneseNumber.Format(1101));
            Assert.AreEqual("sen ni hyaku ichi", JapaneseNumber.Format(1201));
            Assert.AreEqual("sen ni hyaku", JapaneseNumber.Format(1200));
            Assert.AreEqual("Error", JapaneseNumber.Format(10000));
            Assert.AreNotEqual("shi hyaku", JapaneseNumber.Format(400));
            Assert.AreEqual("yon hyaku", JapaneseNumber.Format(400));
        }

        [TestMethod]
        public void TestFormatSafe()
        {
            Assert.AreEqual("juu", JapaneseNumber.Format(11, true));
            Assert.AreEqual("", JapaneseNumber.Format(1, true));
        }

        [TestMethod]
        public void TestMatch()
        {
            Assert.IsTrue(new JapaneseNumber(false, 1).Match("ichi"));
            Assert.IsTrue(new JapaneseNumber(false, 11).Match("juu ichi"));
            Assert.IsTrue(new JapaneseNumber(true, 11).Match("11"));
            Assert.IsTrue(new JapaneseNumber(false, 14).Match("juu yon"));
            Assert.IsTrue(new JapaneseNumber(false, 0).Match("maru"));
            Assert.IsTrue(new JapaneseNumber(false, 0).Match("zero"));
        }
    }
}
