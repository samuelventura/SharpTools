using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace SharpTools
{
    [TestFixture]
    public class CatcherTest
    {
        [Test]
        public void StaticIgnoreTest()
        {
            Catcher.Try(
                () =>
                {
                    throw new Exception("Error on Action!");
                }
            );
        }

        [Test]
        public void InstanceIgnoreTest()
        {
            var catcher = new Catcher();
            catcher.Run(
                () =>
                {
                    throw new Exception("Error on Action!");
                }
            );
        }

        [Test]
        public void StaticHandleTest()
        {
            var exceptions = new List<Exception>();

            Catcher.Try(
                () =>
                {
                    throw new Exception("Error on Action!");
                },
                (ex) =>
                {
                    exceptions.Add(ex);
                    throw new Exception("Error on Handler!");
                }
            );

            Assert.AreEqual(1, exceptions.Count);
            Assert.AreEqual("Error on Action!", exceptions[0].Message);
        }

        public void InstanceHandleTest()
        {
            var exceptions = new List<Exception>();

            var catcher = new Catcher(
                (ex) =>
                {
                    exceptions.Add(ex);
                    throw new Exception("Error on Handler!");
                }
            );
            catcher.Run(
                () =>
                {
                    throw new Exception("Error on Action!");
                }
            );

            Assert.AreEqual(1, exceptions.Count);
            Assert.AreEqual("Error on Action!", exceptions[0].Message);
        }
    }
}