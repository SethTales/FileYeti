using FileYetiServer.Data;
using FileYetiServer.Services;

namespace FileYetiServer
{
    public class MyTcpListener
    {
        public static void Main()
        {
            var transferJobRepository = new TransferJobRepository();
            var diskRepository = new DiskRepository();
            var storageRoot = "C:/temp";
            var listenerService = new ListenerService(transferJobRepository, diskRepository, storageRoot);

            listenerService.Listen();
        }
    }
}
