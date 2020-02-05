using FileYetiClient.Adapters.Handlers;

namespace FileYetiClient.Adapters
{
    public interface IAdapterFactory
    {
        IReaderAdapter BuildReaderAdapter(string sourcePath);
        ITcpClientAdapter BuildClientAdapter(string hostName, int port, int chunkSizeBytes, int headerSize);
    }
    public class AdapterFactory : IAdapterFactory
    {
        public IReaderAdapter BuildReaderAdapter(string sourcePath)
        {
            return new ReaderAdapter(sourcePath);
        }

        public ITcpClientAdapter BuildClientAdapter(string hostName, int port, int chunkSizeBytes, int headerSize)
        {
            return new TcpClientAdapter(hostName, port, chunkSizeBytes, new RequestHandler(headerSize), new ResponseHandler());
        }

    }
}
