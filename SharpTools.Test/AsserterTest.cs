using System;
using System.Collections.Generic;
using Xunit;

namespace SharpTools
{
    public class AsserterTest
    {
        [Fact]
        public void AssertTest()
        {
            var exceptions = new List<Exception>();

            try { Asserter.IsTrue(true, "IsTrue={0}", true); } catch (Exception ex) { exceptions.Add(ex); }
            try { Asserter.IsTrue(false, "IsTrue={0}", false); } catch (Exception ex) { exceptions.Add(ex); }
            try { Asserter.IsFalse(true, "IsFalse={0}", true); } catch (Exception ex) { exceptions.Add(ex); }
            try { Asserter.IsFalse(false, "IsFalse={0}", false); } catch (Exception ex) { exceptions.Add(ex); }

            Assert.Equal(2, exceptions.Count);
            Assert.Equal("IsTrue=False", exceptions[0].Message);
            Assert.Equal("IsFalse=True", exceptions[1].Message);
        }
    }
}