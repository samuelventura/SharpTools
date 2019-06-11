using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace SharpTools
{
    [TestFixture]
    public class AsserterTest
    {
        [Test]
        public void AssertTest()
        {
            var exceptions = new List<Exception>();

            try { Asserter.IsTrue(true, "IsTrue={0}", true); } catch (Exception ex) { exceptions.Add(ex); }
            try { Asserter.IsTrue(false, "IsTrue={0}", false); } catch (Exception ex) { exceptions.Add(ex); }
            try { Asserter.IsFalse(true, "IsFalse={0}", true); } catch (Exception ex) { exceptions.Add(ex); }
            try { Asserter.IsFalse(false, "IsFalse={0}", false); } catch (Exception ex) { exceptions.Add(ex); }

            Assert.AreEqual(2, exceptions.Count);
            Assert.AreEqual("IsTrue=False", exceptions[0].Message);
            Assert.AreEqual("IsFalse=True", exceptions[1].Message);
        }
    }
}