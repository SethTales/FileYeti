using System;
using System.Net.Sockets;
using System.Text;
using FileYetiServer.Handlers;
using FileYetiServer.Models;
using Newtonsoft.Json;

namespace FileYetiServer.Services
{
    public interface IListenerService
    {
        void Listen();
    }

    public class ListenerService : IListenerService
    {
        private readonly TcpListener _server;
        private TcpClient _client;
        private readonly HandlerFactory _handlerFactory;

        public ListenerService(TcpListener server, TcpClient client, HandlerFactory handlerFactory)
        {
            _server = server;
            _client = client;
            _handlerFactory = handlerFactory;
        }

        public void Listen()
        {          
            _server.Start();
            while (true)
            {
                Console.Write("Waiting for a connection... ");
                _client = _server.AcceptTcpClient();
                Console.WriteLine("Connected!");

                try
                {
                    while (_client.Connected)
                    {
                        NetworkStream stream = _client.GetStream();

                        var requestHeaders = ReadRequestHeaders(stream);
                        ICommandHandler handler = _handlerFactory.SelectHandler(requestHeaders.CommandType);
                        handler.Handle(stream, requestHeaders);
                    }
                }
                catch (SocketException e)
                {
                    Console.WriteLine("SocketException: {0}", e);
                }
            }
        }

        private RequestHeaders ReadRequestHeaders(NetworkStream stream)
        {
            //get request headers, which are the first 256 bytes of each request
            byte[] requestHeaderBytes = new byte[256];
            stream.Read(requestHeaderBytes, 0, 256);
            var requestHeaders = JsonConvert.DeserializeObject<RequestHeaders>(Encoding.ASCII.GetString(requestHeaderBytes, 0,
                    requestHeaderBytes.Length));
            requestHeaders.ReceiptTimeStamp = DateTime.UtcNow;
            return requestHeaders;
        }
    }
}
