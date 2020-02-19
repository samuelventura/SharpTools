using System;
using System.Collections.Generic;
using Xunit;

namespace SharpTools
{
    public class CatcherTest
    {
        [Fact]
        public void StaticIgnoreTest()
        {
            Catcher.Try(
                () =>
                {
                    throw new Exception("Error on Action!");
                }
            );
        }

        [Fact]
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

        [Fact]
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

            Assert.Equal(1, exceptions.Count);
            Assert.Equal("Error on Action!", exceptions[0].Message);
        }

        [Fact]
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

            Assert.Equal(1, exceptions.Count);
            Assert.Equal("Error on Action!", exceptions[0].Message);
        }
    }
}