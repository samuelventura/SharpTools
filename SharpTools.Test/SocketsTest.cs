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
            var listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();
            var ep = listener.LocalEndpoint as IPEndPoint;
            listener.Stop();
            var ex = Assert.Throws<Exception>(() => { Sockets.ConnectWithTimeout(ep.Address.ToString(), ep.Port, 1); });
            Assert.AreEqual(string.Format("Timeout connecting to 127.0.0.1:{0}", ep.Port), ex.Message);
        }
    }
}