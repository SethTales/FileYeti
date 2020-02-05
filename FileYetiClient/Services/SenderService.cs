using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using FileYetiClient.Adapters;

namespace FileYetiClient.Services
{
    public class SenderService
    {
        private const int HeaderSize = 256;

        private readonly IAdapterFactory _adapterFactory;
        private readonly int _chunkSizeBytes;
        private readonly string _sourcePath;

        public SenderService(IAdapterFactory adapterFactory, int chunkSizeBytes, string sourcePath)
        {
            _adapterFactory = adapterFactory;
            _chunkSizeBytes = chunkSizeBytes;
            _sourcePath = sourcePath;
        }

        public void Send()
        {
            try
            {
                using (var clientAdapter =
                    _adapterFactory.BuildClientAdapter("Lenovo-34070", 13000, _chunkSizeBytes, HeaderSize))
                {
                    using (var reader = _adapterFactory.BuildReaderAdapter(_sourcePath))
                    {
                        var numberOfChunks = (int)reader.StreamLength / _chunkSizeBytes + 1;
                        var fileName = _sourcePath.Split(Path.DirectorySeparatorChar).Last();
                        var initiateJobResponse = clientAdapter.SendInitiateJobRequest(fileName, _chunkSizeBytes, numberOfChunks);
                        var jobGuid = initiateJobResponse.JobGuid;

                        for (int i = 0; i < numberOfChunks; i++)
                        {
                            byte[] chunkBytes = new byte[_chunkSizeBytes];

                            if (i == numberOfChunks - 1)
                            {
                                reader.
                            }

                            reader.Read(chunkBytes, 0, chunkBytes.Length);


                            var updateJobResponse = clientAdapter.SendUploadChunkRequest(fileName, jobGuid, i + 1,
                                chunkBytes);

                        }
                        clientAdapter.SendCompleteJobRequest(jobGuid);
                    }

                }

            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
        }
    }
}
