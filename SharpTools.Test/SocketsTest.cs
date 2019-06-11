using System;
using System.Net.Sockets;
using System.Net;
using NUnit.Framework;

namespace SharpTools
{
    [TestFixture]
    public class SocketsTest
    {
        [Test]
        public void ConnectTest()
        {
            var listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();
            var ep = listener.LocalEndpoint as IPEndPoint;
            var socket = Sockets.ConnectWithTimeout(ep.Address.ToString(), ep.Port, 1);
            listener.Stop();
            socket.Close();
        }

        [Test]
        public void TimeoutTest()
        {
            //Windows Timeout connecting to 127.0.0.1:XXXX
            //MacOS System.Net.Internals.SocketExceptionFactory+ExtendedSocketException (61): Connection refused [::ffff:127.0.0.1]:49653
            //Better to point to a local-only endpoint
            var ex = Assert.Throws<Exception>(() => { Sockets.ConnectWithTimeout("10.77.0.99", 9999, 1); });
            Assert.AreEqual("Timeout connecting to 10.77.0.99:9999", ex.Message);
        }
    }
}