using System.Net.Sockets;

namespace SharpTools
{
    public static class Sockets
    {
        public static TcpClient ConnectWithTimeout(string ip, int port, int timeout)
        {
            var client = new TcpClient();
            var result = client.BeginConnect(ip, port, null, null);
            if (!result.AsyncWaitHandle.WaitOne(timeout, true))
            {
                Disposer.Dispose(client);
                Thrower.Throw("Timeout connecting to {0}:{1}", ip, port);
            }
            client.EndConnect(result);
            return client;
        }
    }
}
