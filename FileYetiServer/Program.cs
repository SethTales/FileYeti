using System;
using System.Net;
using System.Net.Sockets;
using FileYetiServer.Data.Repositories;
using FileYetiServer.Handlers;
using FileYetiServer.Services;

namespace FileYetiServer
{
    public class Program
    {
        public static void Main()
        {
            var transferJobRepository = new TransferJobRepository();
            var storageRoot = "C:/temp";
            var diskRepository = new DiskRepository(storageRoot);

            Int32 port = 13000;
            IPAddress localAddr = IPAddress.Any;
            var server = new TcpListener(localAddr, port);
            TcpClient client = new TcpClient();

            var handlerFactory = new HandlerFactory(transferJobRepository, diskRepository, server);
            var listenerService = new ListenerService(server, client, handlerFactory);

            listenerService.Listen();
        }
    }
}
